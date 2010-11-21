#region Copyright (C) 2005-2010 Team MediaPortal

// Copyright (C) 2005-2010 Team MediaPortal
// http://www.team-mediaportal.com
// 
// MediaPortal is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 2 of the License, or
// (at your option) any later version.
// 
// MediaPortal is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with MediaPortal. If not, see <http://www.gnu.org/licenses/>.

#endregion

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using MediaPortal.GUI.Library;
using MediaPortal.Player;

namespace MediaPortal.Plugins.PureAudio
{
  public partial class PureAudioPlayer : IExternalPlayer, IDisposable
  {
    private const string VisualizationName = "PureAudioVisualizationWindow";

    #region static members

    public static g_Player.ShowFullScreenWindowHandler FullScreenHandler = new g_Player.ShowFullScreenWindowHandler(ActivateFullScreen);
    private static g_Player.ShowFullScreenWindowHandler _SavedFullScreenHandler;

    private static bool ActivateFullScreen()
    {
      if (g_Player.Player != null && g_Player.Player.GetType().FullName == "MediaPortal.Plugins.PureAudio.PureAudioPlayer")
      {
        if (GUIWindowManager.ActiveWindow != (int)GUIWindow.Window.WINDOW_FULLSCREEN_MUSIC)
        {
          GUIWindowManager.ActivateWindow((int)GUIWindow.Window.WINDOW_FULLSCREEN_MUSIC);
          GUIGraphicsContext.IsFullScreenVideo = true;
        }
        return true;
      }
      else
        return _SavedFullScreenHandler.Invoke();
    }

    #endregion

    #region Fields

    private BassLibraryManager _BassLibraryManager;
    private BassPlayerSettings _Settings;
    private Controller _Controller;
    private Monitor _Monitor;
    private InputSourceFactory _InputSourceFactory;
    private InputSourceQueue _InputSourceQueue;
    private InputSourceManager _InputSourceManager;
    private VSTProcessor _VSTProcessor;
    private WinAmpDSPProcessor _WinAmpDSPProcessor;
    private Resampler _Resampler;
    private PlaybackBuffer _PlaybackBuffer;
    private OutputDeviceManager _OutputDeviceManager;
    private PlaybackSession _PlaybackSession;
    private BaseVisualizationWindow _VisualizationWindow;
    private WaitCursor _WaitCursor;

    private bool _SettingsLoaded = false;
    private bool _Initialized = false;

    private MediaItemType _CurrentMediaItemType;
    private string _CurrentMediaItemURI;

    #endregion

    #region IExternalPlayer Interface

    public override string Description()
    {
      // Get all Description attributes on this assembly
      object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
      // If there aren't any Description attributes, return an empty string
      if (attributes.Length == 0)
        return "";
      // If there is a Description attribute, return its value
      return ((AssemblyDescriptionAttribute)attributes[0]).Description;
    }

    public override string PlayerName
    {
      get
      {
        // Get all Title attributes on this assembly
        object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
        // If there is at least one Title attribute
        if (attributes.Length > 0)
        {
          // Select the first one
          AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
          // If it is not an empty string, return it
          if (titleAttribute.Title != "")
            return titleAttribute.Title;
        }
        // If there was no Title attribute, or if the Title attribute was the empty string, return the .exe name
        return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
      }
    }

    public override string VersionNumber
    {
      get { return Assembly.GetExecutingAssembly().GetName().Version.ToString(); }
    }

    public override string AuthorName
    {
      get
      {
        // Get all Company attributes on this assembly
        object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
        // If there aren't any Company attributes, return an empty string
        if (attributes.Length == 0)
          return "";
        // If there is a Company attribute, return its value
        return ((AssemblyCompanyAttribute)attributes[0]).Company;
      }
    }

    public override string[] GetAllSupportedExtensions()
    {
      // Is obsolete
      return new string[0];
    }

    public override bool SupportsFile(string filename)
    {
      LoadSettings();

      bool supported = _InputSourceFactory.IsSupported(new MediaItem(new Uri(filename)));
      Log.Debug("SupportsFile(\"{0}\"): {1}", filename, supported ? "Supported" : "Not supported");

      return supported;
    }

    public override void ShowPlugin()
    {
      Configuration.ConfigurationForm confForm = new Configuration.ConfigurationForm();
      confForm.ShowDialog();
    }

    #endregion

    #region IPlayer interface

    public override int PlaybackType
    {
      get { return _Initialized ? (int)_Controller.PlaybackMode : 0; }
    }

    public override double Duration
    {
      get { return _Initialized ? _Monitor.Duration.TotalSeconds : 0; }
    }

    public override double CurrentPosition
    {
      get { return _Initialized ? _Monitor.CurrentPosition.TotalSeconds : 0; }
    }

    public override string CurrentFile
    {
      get { return _CurrentMediaItemURI; }
    }

    public override bool IsRadio
    {
      get { return _CurrentMediaItemType.MainType == MediaItemMainType.WebStream; }
    }

    public override bool IsCDA
    {
      get { return _CurrentMediaItemType.MainType == MediaItemMainType.CDTrack; }
    }

    public override bool HasVideo
    {
      // return false because MP is doing a lot of unwanted things when set to true
      // This means however that we have to do some things ourselfes, like 
      // making sure the videowindow is properly positioned and scaled.
      get { return false; }
    }

    public override bool Initializing
    {
      get { return (_Controller.ExternalState == PlaybackState.Init); }
    }

    public override bool Playing
    {
      get { return (_Controller.ExternalState == PlaybackState.Playing || _Controller.ExternalState == PlaybackState.Paused); }
    }

    public override bool Paused
    {
      get { return (_Controller.ExternalState == PlaybackState.Paused); }
    }

    public override bool Stopped
    {
      get { return (_Controller.ExternalState == PlaybackState.Stopped); }
    }

    public override bool Ended
    {
      get { return _Controller.ExternalState == PlaybackState.Ended; }
    }

    public override bool Play(string filePath)
    {
      Log.Debug("Play(\"{0}\") called...", filePath);

      LoadSettings();

      MediaItem mediaItem = new MediaItem(new Uri(filePath));

      // We need the uri and the its MediaItemType immediatly;
      // This info might be needed by the outside world before the 
      // asynchronous playback has actually started.
      // For example Audioscrobbler
      _CurrentMediaItemType = _InputSourceFactory.GetMediaItemType(mediaItem);
      _CurrentMediaItemURI = filePath;

      _Controller.Play(mediaItem, false);

      return true;
    }

    public override void Pause()
    {
      Log.Debug("Pause() called...");
      if (_Initialized)
      {
        if (_Controller.ExternalState == PlaybackState.Playing)
          _Controller.Pause();

        else if (_Controller.ExternalState == PlaybackState.Paused)
          _Controller.Resume();
      }
    }

    public override void Stop()
    {
      Log.Debug("Stop() called...");
      if (_Initialized)
      {
        _Controller.Stop(IsShuttingDown());
      }
    }

    public override void Process()
    {
    }

    public override void Dispose()
    {
      // This method is called on each Stop(), allthough the player object 
      // is kept alive in the player factory.
      // This means we cannot use this as a real Dispose.

      if (!Stopped)
        Stop();
    }

    // Not used by MP for music
    // public override int Volume

    #endregion

    #region IDisposable Members

    public void RealDispose()
    {
      // See Dispose()

      Log.Debug("BassPlayer.Dispose()");

      _Controller.TerminateThread();
      _Monitor.TerminateThread();

      _OutputDeviceManager.Dispose();
      _PlaybackBuffer.Dispose();
      _Resampler.Dispose();
      _WinAmpDSPProcessor.Dispose();
      _VSTProcessor.Dispose();
      _InputSourceManager.Dispose();
      _InputSourceQueue.Dispose();
      _InputSourceFactory.Dispose();
      _Monitor.Dispose();
      _Controller.Dispose();

      _BassLibraryManager.Dispose();

      if (_VisualizationWindow != null)
        _VisualizationWindow.Dispose();
    }

    #endregion

    #region Public members

    public PureAudioPlayer()
    {
      Initialize();
    }

    #endregion

    #region Private members

    private void Initialize()
    {
      Log.Debug("Initialize() called...");

      if (!_Initialized)
      {
        Log.Info("Plugin version: {0}", VersionNumber);
        Log.Info("Initializing player ...");

        _Settings = new BassPlayerSettings();
        LoadSettings();

        _BassLibraryManager = BassLibraryManager.Create();
        _InputSourceFactory = new InputSourceFactory(this);

        _Controller = Controller.Create(this);
        _Controller.WaitStarted += new Controller.WaitStartedDelegate(_Controller_WaitStarted);
        _Controller.WaitEnded += new Controller.WaitEndedDelegate(_Controller_WaitEnded);

        _Monitor = Monitor.Create(this);
        _InputSourceQueue = new InputSourceQueue();
        _InputSourceManager = InputSourceManager.Create(this);
        _VSTProcessor = VSTProcessor.Create(this);
        _WinAmpDSPProcessor = WinAmpDSPProcessor.Create(this);
        _Resampler = Resampler.Create(this);
        _PlaybackBuffer = PlaybackBuffer.Create(this);
        _OutputDeviceManager = OutputDeviceManager.Create(this);

        _Controller.WireUp();

        _Monitor.MonitorProcess += new Monitor.MonitorProcessDelegate(_Monitor_MonitorProcess);
        _Controller.SessionStarted += new Controller.SessionStartedDelegate(_Controller_SessionStarted);
        _Controller.SessionStopped += new Controller.SessionStoppedDelegate(_Controller_SessionStopped);

        _SavedFullScreenHandler = g_Player.ShowFullScreenWindowVideo;
        g_Player.ShowFullScreenWindowVideo = new g_Player.ShowFullScreenWindowHandler(FullScreenHandler);

        VisualizationFactory visualizationFactory = VisualizationFactory.Create(_Settings);
        _VisualizationWindow = visualizationFactory.GetVisualizationWindow();
        if (_VisualizationWindow != null)
        {
          _VisualizationWindow.Name = VisualizationName;
          _VisualizationWindow.Visible = false;
        }

        GUIGraphicsContext.form.Disposed += new EventHandler(OnAppFormDisposed);
        GUIGraphicsContext.OnNewAction += new OnActionHandler(OnNewAction);

        _Initialized = true;
      }
    }

    private bool IsShuttingDown()
    {
      // Not really neat, but works for now

      StackTrace stackTrace = new StackTrace(false);
      bool checkFlag = false;
      bool shuttingDown = false;
      for (int i = 0; i < stackTrace.FrameCount; i++)
      {
        StackFrame StackFrame = stackTrace.GetFrame(i);
        MethodBase methodBase = StackFrame.GetMethod();

        string method = methodBase.ReflectedType.FullName + "." + methodBase.Name;
        if (checkFlag)
        {
          shuttingDown = (method == "MediaPortal.D3DApp.D3DApp_Closing");
          break;
        }
        else if (method == "MediaPortal.Player.g_Player.Stop")
          checkFlag = true;
      }
      return shuttingDown;
    }

    private void LoadSettings()
    {
      if (!_SettingsLoaded)
      {
        _Settings.LoadSettings();
        _SettingsLoaded = true;
      }
    }
    private void AttachVisualizationWindow()
    {
      if (_VisualizationWindow != null)
      {
        bool present = false;
        Control.ControlCollection controls = GUIGraphicsContext.form.Controls;
        foreach (Control control in controls)
        {
          if (control == _VisualizationWindow)
          {
            present = true;
            break;
          }
        }

        if (!present)
        {
          GUIGraphicsContext.form.SuspendLayout();
          GUIGraphicsContext.form.Controls.Add(_VisualizationWindow);
          GUIGraphicsContext.form.ResumeLayout();
        }
      }
    }

    private void DetachVisualizationWindow()
    {
      if (_VisualizationWindow != null)
      {
        Control.ControlCollection controls = GUIGraphicsContext.form.Controls;
        foreach (Control control in controls)
        {
          if (control == _VisualizationWindow)
          {
            GUIGraphicsContext.form.SuspendLayout();
            GUIGraphicsContext.form.Controls.Remove(control);
            GUIGraphicsContext.form.ResumeLayout();
            break;
          }
        }
      }
    }

    private void PositionVisualizationWindow()
    {
      if (_VisualizationWindow != null)
      {
        if (GUIGraphicsContext.BlankScreen)
        {
          if (_VisualizationWindow.Visible)
            _VisualizationWindow.Visible = false;
        }
        else
        {

          //// Fix:
          //// g_Player sets GUIGraphicsContext.IsFullScreenVideo to false on Stop().
          //GUIGraphicsContext.IsFullScreenVideo =
          //  GUIGraphicsContext.IsFullScreenVideo ||
          //  (GUIWindowManager.ActiveWindow == (int)GUIWindow.Window.WINDOW_FULLSCREEN_MUSIC);

          Point location;
          Size size;
          bool visible;

          if (GUIGraphicsContext.IsFullScreenVideo)
          {
            location = new Point(GUIGraphicsContext.OverScanLeft, GUIGraphicsContext.OverScanTop);
            size = new Size(GUIGraphicsContext.OverScanWidth, GUIGraphicsContext.OverScanHeight);
            visible = true;
          }
          else
          {
            location = new Point(1, 1);
            size = new Size(1, 1);
            visible = false;
          }

          if (location != _VisualizationWindow.Location)
            _VisualizationWindow.Location = location;

          if (size != _VisualizationWindow.Size)
            _VisualizationWindow.Size = size;

          if (visible != _VisualizationWindow.Visible)
            _VisualizationWindow.Visible = visible;

        }
      }
    }

    #endregion

    #region Eventhandlers

    void OnNewAction(Action action)
    {
      Log.Debug("Action: {0}", action.wID);

      switch (action.wID)
      {
        case Action.ActionType.ACTION_FORWARD:
        case Action.ActionType.ACTION_MUSIC_FORWARD:
          _Controller.Wind();

          break;

        case Action.ActionType.ACTION_REWIND:
        case Action.ActionType.ACTION_MUSIC_REWIND:
          _Controller.Rewind();

          break;

        case Action.ActionType.ACTION_TOGGLE_MUSIC_GAP:
          _Controller.TogglePlayBackMode();
          break;

        case Action.ActionType.ACTION_VOLUME_UP:
          break;

        case Action.ActionType.ACTION_VOLUME_DOWN:
          break;

        case Action.ActionType.ACTION_VOLUME_MUTE:
          break;
      }
    }

    void Application_ApplicationExit(object sender, EventArgs e)
    {
      RealDispose();
    }

    void OnAppFormDisposed(object sender, EventArgs e)
    {
      RealDispose();
    }

    void _Monitor_MonitorProcess()
    {
      PositionVisualizationWindow();
    }

    void _Controller_WaitStarted()
    {
      if (_WaitCursor == null)
        _WaitCursor = new WaitCursor();
    }

    void _Controller_WaitEnded()
    {
      if (_WaitCursor != null)
      {
        _WaitCursor.Dispose();
        _WaitCursor = null;
      }
    }

    void _Controller_SessionStarted()
    {
      AttachVisualizationWindow();

      if (_VisualizationWindow != null)
        _VisualizationWindow.BassStream = _PlaybackBuffer.VizStream.Handle;
    }

    void _Controller_SessionStopped()
    {
      DetachVisualizationWindow();
    }

    #endregion

  }
}
