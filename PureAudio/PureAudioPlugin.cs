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
using System.Text.RegularExpressions;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Un4seen.Bass;
using Un4seen.Bass.AddOn.Cd;
using MediaPortal.Configuration;
using MediaPortal.GUI.Library;
using MediaPortal.Services;
using Action = MediaPortal.GUI.Library.Action;

namespace MediaPortal.Player.PureAudio
{
  public partial class PureAudioPlugin : IExternalPlayer
  {
    #region Constants

    private const string VizWindowName = "PureAudioVisualizationWindow";

    #endregion

    #region Static interface

    public static g_Player.ShowFullScreenWindowHandler FullScreenHandler = new g_Player.ShowFullScreenWindowHandler(ActivateFullScreen);
    private static g_Player.ShowFullScreenWindowHandler _SavedFullScreenHandler;

    private static bool ActivateFullScreen()
    {
      if (g_Player.Player != null && g_Player.Player.GetType().FullName == "MediaPortal.Player.PureAudio.PureAudioPlugin")
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

    public delegate void PlaybackStateChangedDelegate(object sender, PlayState oldState, PlayState newState);
    public event PlaybackStateChangedDelegate PlaybackStateChanged;

    public delegate void InternetStreamSongChangedDelegate(object sender);
    public event InternetStreamSongChangedDelegate InternetStreamSongChanged;

    private bool _Initialized = false;
    private bool _NotifyPlayingFlag = true;
    private bool _DebugMode = false;
    private bool _SettingsLoaded = false;
    private bool _UseForCDDA = false;
    private bool _UseForWebStream = false;
    private bool _UseForLastFMWebStream = false;

    private string _CurrentFilePath = String.Empty;
    private string[] _SupportedExtensions = null;

    private PlayState _State = PlayState.Init;
    private WebStreamInfo _WebStreamInfo = new WebStreamInfo();
    private BASSPlayer _BassPlayer = null;
    private BaseVisualizationWindow _VizWindow = null;
    private WaitCursor _WaitCursor = null;

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
      get
      {
        // Get all Informational Version attributes on this assembly
        object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyInformationalVersionAttribute), false);

        // If there aren't any Informational Version attributes, return the assembly version
        if (attributes.Length == 0)
          return Assembly.GetExecutingAssembly().GetName().Version.ToString();

        // If there is a Informational Version attribute, return its value
        return ((AssemblyInformationalVersionAttribute)attributes[0]).InformationalVersion;
      }
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
      LoadSettings();
      return _SupportedExtensions;
    }

    public override bool SupportsFile(string filename)
    {
      LoadSettings();

      FileType fileType = Utils.GetFileType(filename);
      bool supported = false;

      if (fileType.FileMainType == FileMainType.AudioFile)
      {
        string ext = Path.GetExtension(filename).ToLower();
        supported = SupportsExtension(ext);
      }
      else if (fileType.FileMainType == FileMainType.WebStream && fileType.FileSubType == FileSubType.None)
      {
        if (_UseForWebStream)
        {
          Uri uri = new Uri(filename);
          string ext = Path.GetExtension(uri.AbsolutePath).ToLower();
          supported = (ext == "" || SupportsExtension(ext));
        }
      }
      else if (fileType.FileMainType == FileMainType.WebStream && fileType.FileSubType == FileSubType.LastFmWebStream)
      {
        supported = _UseForLastFMWebStream;
      }
      else if (fileType.FileMainType == FileMainType.CDTrack)
      {
        supported = _UseForCDDA;
      }
      else
      {
        supported = (fileType.FileMainType != FileMainType.Unknown);
      }

      Log.Debug("PureAudio: SupportsFile(\"{0}\"): {1}", filename, supported ? "Supported" : "Not supported");

      return supported;
    }

    public override void ShowPlugin()
    {
      ConfigurationForm confForm = new ConfigurationForm(this);
      confForm.ShowDialog();
    }

    #endregion

    #region IPlayer Interface

    public override bool Initializing
    {
      get { return (_State == PlayState.Init); }
    }

    public override int PlaybackType
    {
      get
      {
        // Remark: Gets called frequently
        if (_Initialized)
          return (int)_BassPlayer.PlayBackMode;
        else
          return 0;
      }
    }

    public override double Duration
    {
      // Remark: Gets called frequently
      get
      {
        if (_Initialized)
          return _BassPlayer.Duration;
        else
          return 0;
      }
    }

    public override double CurrentPosition
    {
      // Remark: Gets called frequently
      get
      {
        if (_Initialized)
          return _BassPlayer.CurrentPosition;
        else
          return 0;
      }
    }

    public override string CurrentFile
    {
      get { return _CurrentFilePath; }
    }

    public override int CurrentAudioStream
    {
      get
      {
        if (_Initialized)
          return _BassPlayer.GetCurrentVizStream();
        else
          return 0;
      }
      set
      {
      }
    }

    public override bool IsRadio
    {
      get
      {
        if (_Initialized)
          return
            (_BassPlayer.CurrentFileType.FileMainType == FileMainType.WebStream &&
            _BassPlayer.CurrentFileType.FileSubType != FileSubType.ASXWebStream);
        else
          return false;
      }
    }

    public override bool HasVideo
    {
      get
      {
        // return false because MP is doing a lot of unwanted things when set to true
        // This means however that we have to do some things ourselfes, like 
        // making sure the videowindow is properly positioned and scaled.
        return false;
      }
    }

    public override bool Playing
    {
      get { return (_State == PlayState.Playing || _State == PlayState.Paused); }
    }

    public override bool Paused
    {
      get { return (_State == PlayState.Paused); }
    }

    public override bool Stopped
    {
      get { return (_State == PlayState.Init); }
    }

    public override bool Ended
    {
      get { return _State == PlayState.Ended; }
    }

    public override bool Play(string filePath)
    {
      Log.Debug("PureAudio: Play(\"{0}\") called...", filePath);

      bool result = true;
      try
      {
        // Fix:
        // g_Player sets GUIGraphicsContext.IsFullScreenVideo to false on Stop().
        GUIGraphicsContext.IsFullScreenVideo =
          GUIGraphicsContext.IsFullScreenVideo ||
          (GUIWindowManager.ActiveWindow == (int)GUIWindow.Window.WINDOW_FULLSCREEN_MUSIC);

        result = Initialize();
        if (result)
        {
          if (filePath.Equals(_CurrentFilePath, StringComparison.InvariantCultureIgnoreCase) && Paused)
          {
            // Selected file is equal to current stream and is paused
            // The Pause() method will resume
            Pause();
          }
          else
          {
            SetState(PlayState.Init);
            _WebStreamInfo.Reset();
            _CurrentFilePath = filePath;

            GUIMessage msg = new GUIMessage(GUIMessage.MessageType.GUI_MSG_PLAYBACK_STARTED, 0, 0, 0, 0, 0, null);
            msg.Label = _CurrentFilePath;
            GUIWindowManager.SendThreadMessage(msg);

            AddVizWindowToForm();
            _NotifyPlayingFlag = true;

            FileType fileType = Utils.GetFileType(filePath);
            if (fileType.FileMainType == FileMainType.WebStream && fileType.FileSubType == FileSubType.LastFmWebStream)
            {
              // Last.fm streams: wait till playback has started in the background
              // Not doing so will confuse the last.fm radio plugin.
              result = _BassPlayer.Play(filePath, true);
            }
            else
            {
              // Start actual playback in background
              _BassPlayer.Play(filePath);
            }

            if (result)
            {
              SetState(PlayState.Playing);
            }
          }
        }
      }
      catch (Exception ex)
      {
        result = false;
        Log.Error("PureAudio: Play() caused an exception: {0}.", ex);
      }
      return result;
    }

    public override void Pause()
    {
      Log.Debug("PureAudio: Pause() called...");
      if (_Initialized)
      {
        if (_State == PlayState.Playing)
        {
          _BassPlayer.Pause(true);
          SetState(PlayState.Paused);
        }
        else if (_State == PlayState.Paused)
        {
          _BassPlayer.Pause(false);
          SetState(PlayState.Playing);
        }
      }
    }

    public override void Stop()
    {
      Log.Debug("PureAudio: Stop() called...");
      if (_Initialized)
      {
        bool IsLastFM =
          (_BassPlayer.CurrentFileType.FileMainType == FileMainType.WebStream && _BassPlayer.CurrentFileType.FileSubType == FileSubType.LastFmWebStream);

        _BassPlayer.Stop(IsLastFM || IsStoppedByShuttingDown());

        _CurrentFilePath = "";
        SetState(PlayState.Init);
      }
    }

    public override void Process()
    {
    }

    #endregion

    #region Public Instance Interface

    public PureAudioPlugin()
    {
      Log.Write += new Log.LogDelegate(Log_Write);
    }

    public override void Dispose()
    {
      // This method is called on each Stop(), allthough the player object 
      // is kept alive in the player factory.
      // This means we cannot use this as a real Dispose.

      if (!Stopped)
        Stop();
    }

    #endregion

    #region Private Instance Interface

    private bool IsStream
    {
      get { return (_BassPlayer.CurrentFileType.FileMainType == FileMainType.WebStream); }
    }

    private bool Initialize()
    {
      Log.Debug("PureAudio: Initialize() called...");

      bool result = true;
      if (!_Initialized)
      {
        Log.Info("PureAudio: Plugin version: {0}", VersionNumber);
        Log.Info("PureAudio: Initializing player ...");

        _BassPlayer = new BASSPlayer();

        LoadSettings();
        _BassPlayer.DebugMode = _DebugMode;

        _BassPlayer.Ended += new BASSPlayer.EndedDelegate(_BassPlayer_Ended);
        _BassPlayer.Stopped += new BASSPlayer.StoppedDelegate(_BassPlayer_Stopped);
        _BassPlayer.StreamTagsChanged += new BASSPlayer.StreamTagsChangedDelegate(_BassPlayer_StreamTagsChanged);
        _BassPlayer.MetaStreamTagsChanged += new BASSPlayer.MetaStreamTagsChangedDelegate(_BassPlayer_MetaStreamTagsChanged);
        _BassPlayer.SessionStopped += new BASSPlayer.SessionStoppedDelegate(_BassPlayer_SessionStopped);
        _BassPlayer.SessionStarted += new BASSPlayer.SessionStartedDelegate(_BassPlayer_SessionStarted);
        _BassPlayer.MonitorProcess += new BASSPlayer.MonitorProcessDelegate(_BassPlayer_MonitorProcess);
        _BassPlayer.WaitCursorRequested += new BASSPlayer.WaitCursorRequestedDelegate(_BassPlayer_WaitCursorRequested);

        result = _BassPlayer.Initialize();
        if (result)
        {
          _SavedFullScreenHandler = g_Player.ShowFullScreenWindowVideo;
          g_Player.ShowFullScreenWindowVideo = new g_Player.ShowFullScreenWindowHandler(FullScreenHandler);

          GUIGraphicsContext.form.Disposed += new EventHandler(OnAppFormDisposed);
          GUIGraphicsContext.OnNewAction += new OnActionHandler(OnNewAction);

          VisualizationFactory vizFactory = VisualizationFactory.Create(_BassPlayer.Profile);
          _VizWindow = vizFactory.GetVisualizationWindow();

          // VizTest
          if (_VizWindow != null)
          {
            _VizWindow.Name = VizWindowName;
            _VizWindow.Visible = false;

          }

          Log.Info("PureAudio: Initializing complete.");

          g_Player.PlayBackEnded += new g_Player.EndedHandler(g_Player_PlayBackEnded);

          _Initialized = true;
        }
      }
      return result;
    }

    void g_Player_PlayBackEnded(g_Player.MediaType type, string filename)
    {
    }

    private void LoadSettings()
    {
      if (!_SettingsLoaded)
      {
        string section = ConfigProfile.ConfigSection;
        using (MediaPortal.Profile.Settings xmlreader = new MediaPortal.Profile.Settings(Config.GetFile(Config.Dir.Config, ConfigProfile.ConfigFile)))
        {
          string ext = xmlreader.GetValueAsString(section, ConfigProfile.PropNames.Extensions, ConfigProfile.Defaults.Extensions);
          _SupportedExtensions = ext.Split(new string[] { "," }, StringSplitOptions.None);

          _UseForCDDA = xmlreader.GetValueAsBool(section, ConfigProfile.PropNames.UseForCDDA, ConfigProfile.Defaults.UseForCDDA);
          _UseForWebStream = xmlreader.GetValueAsBool(section, ConfigProfile.PropNames.UseForWebStream, ConfigProfile.Defaults.UseForWebStream);
          _UseForLastFMWebStream = xmlreader.GetValueAsBool(section, ConfigProfile.PropNames.UseForLastFMWebStream, ConfigProfile.Defaults.UseForLastFMWebStream);

        }
        using (MediaPortal.Profile.Settings xmlreader = new MediaPortal.Profile.Settings(Config.GetFile(Config.Dir.Config, "MediaPortal.xml")))
        {
          Level logLevel = (Level)Enum.Parse(typeof(Level), xmlreader.GetValueAsString("general", "loglevel", "3"));
          _DebugMode = logLevel == Level.Debug;
        }
        _SettingsLoaded = true;
      }
    }

    private void SetState(PlayState state)
    {
      if (state != _State)
      {
        PlayState prevState = _State;
        _State = state;

        if (PlaybackStateChanged != null)
          PlaybackStateChanged(this, prevState, _State);
      }
    }

    private void GetStreamTags(string[] tags)
    {
      Log.Debug("GetStreamTags() called...");

      string album = null;
      string genre = null;

      if (tags != null)
      {
        foreach (string item in tags)
        {
          if (item.StartsWith("icy-name:", StringComparison.InvariantCultureIgnoreCase))
            album = item.Substring(9);

          if (item.StartsWith("icy-genre:", StringComparison.InvariantCultureIgnoreCase))
            genre = item.Substring(10);

          Log.Debug("Connection Information: {0}", item);
        }
      }

      _WebStreamInfo.Album = album;
      _WebStreamInfo.Genre = genre;

      if (_WebStreamInfo.Genre != null)
        GUIPropertyManager.SetProperty("#Play.Current.Genre", _WebStreamInfo.Genre);

      if (_WebStreamInfo.Album != null)
        GUIPropertyManager.SetProperty("#Play.Current.Album", _WebStreamInfo.Album);
    }

    private void GetStreamMetaTags(string tags)
    {
      string title = null;
      string artist = null;

      if (tags != null)
      {
        Log.Debug("Title sent via Stream: {0}", tags);
        Regex r = new Regex("StreamTitle='(.+?)';StreamUrl=", RegexOptions.IgnoreCase);
        Match m = r.Match(tags);
        if (m.Success)
        {
          Group g1 = m.Groups[1];
          CaptureCollection captures = g1.Captures;
          Capture c = captures[0];
          Regex r1 = new Regex("( - )");
          string[] s = r1.Split(c.ToString());
          if (s.Length > 2)
          {
            artist = s[0];
            title = s[2];
          }
          else
            title = c.ToString();
        }
      }

      _WebStreamInfo.Title = title;
      _WebStreamInfo.Artist = artist;

      if (_WebStreamInfo.Artist != null)
        GUIPropertyManager.SetProperty("#Play.Current.Artist", _WebStreamInfo.Artist);

      if (_WebStreamInfo.Title != null)
        GUIPropertyManager.SetProperty("#Play.Current.Title", _WebStreamInfo.Title);

      if (InternetStreamSongChanged != null)
        InternetStreamSongChanged(this);
    }

    private void AddVizWindowToForm()
    {
      if (_VizWindow != null)
      {
        bool present = false;
        Control.ControlCollection controls = GUIGraphicsContext.form.Controls;
        foreach (Control control in controls)
        {
          if (control == _VizWindow)
          {
            present = true;
            break;
          }
        }

        if (!present)
        {
          GUIGraphicsContext.form.SuspendLayout();
          GUIGraphicsContext.form.Controls.Add(_VizWindow);
          GUIGraphicsContext.form.ResumeLayout();
        }
      }
    }

    private void RemoveVizWindow()
    {
      if (_VizWindow != null)
      {
        Control.ControlCollection controls = GUIGraphicsContext.form.Controls;
        foreach (Control control in controls)
        {
          if (control == _VizWindow)
          {
            GUIGraphicsContext.form.SuspendLayout();
            GUIGraphicsContext.form.Controls.Remove(control);
            GUIGraphicsContext.form.ResumeLayout();
            break;
          }
        }
      }
    }

    private bool IsStoppedByShuttingDown()
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

    private bool SupportsExtension(string ext)
    {
      bool supported = false;
      for (int i = 0; i < _SupportedExtensions.Length; i++)
      {
        if (_SupportedExtensions[i].Equals(ext))
        {
          supported = true;
          break;
        }
      }
      return supported;
    }

    public void RealDispose()
    {
      // See Dispose()

      if (_BassPlayer != null)
        _BassPlayer.Dispose();

      if (_VizWindow != null)
        _VizWindow.Dispose();
    }

    #endregion

    #region Event Handlers

    void _BassPlayer_SessionStarted()
    {
      if (_VizWindow != null)
        _VizWindow.BassStream = _BassPlayer.GetCurrentVizStream();
    }

    void _BassPlayer_SessionStopped()
    {
      // Test for Playing because before we get here another Play() call may be pending.
      // Happens when skipping in fullscreen mode or manually starting another track.
      if (!Playing)
      {
        RemoveVizWindow();

        // Todo: _BackGroundPlayer.CurrentFileType is not a reliable check
        // If we did a playback of a Audio CD, release the CD, as we might have problems with other CD related functions
        if (_BassPlayer.CurrentFileType.FileMainType == FileMainType.CDTrack)
        {
          int driveCount = BassCd.BASS_CD_GetDriveCount();
          for (int i = 0; i < driveCount; i++)
          {
            BassCd.BASS_CD_Release(i);
          }
        }
      }
    }

    void _BassPlayer_MetaStreamTagsChanged(object sender, string tags)
    {
      GetStreamMetaTags(tags);
    }

    void _BassPlayer_StreamTagsChanged(object sender, string[] tags)
    {
      GetStreamTags(tags);
    }

    void _BassPlayer_Ended()
    {
      SetState(PlayState.Ended);
    }

    void _BassPlayer_Stopped()
    {
      SetState(PlayState.Init);
    }

    void _BassPlayer_MonitorProcess()
    {
      if (Playing)
      {
        if (IsStream)
        {
          // This is to solve a problem in MP where the Playing Now screen will 
          // reset the properties to empty when playing a webstream from a 
          // single-entry pls.  There is no current playlistitem in that case.
          if (_WebStreamInfo.Genre != null)
            GUIPropertyManager.SetProperty("#Play.Current.Genre", _WebStreamInfo.Genre);

          if (_WebStreamInfo.Album != null)
            GUIPropertyManager.SetProperty("#Play.Current.Album", _WebStreamInfo.Album);

          if (_WebStreamInfo.Artist != null)
            GUIPropertyManager.SetProperty("#Play.Current.Artist", _WebStreamInfo.Artist);

          if (_WebStreamInfo.Title != null)
            GUIPropertyManager.SetProperty("#Play.Current.Title", _WebStreamInfo.Title);
        }

        if (_VizWindow != null)
        {
          if (GUIGraphicsContext.BlankScreen)
          {
            if (_VizWindow.Visible)
              _VizWindow.Visible = false;
          }
          else
          {
            // Fix:
            // g_Player sets GUIGraphicsContext.IsFullScreenVideo to false on Stop().
            GUIGraphicsContext.IsFullScreenVideo =
              GUIGraphicsContext.IsFullScreenVideo ||
              (GUIWindowManager.ActiveWindow == (int)GUIWindow.Window.WINDOW_FULLSCREEN_MUSIC);

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

            if (location != _VizWindow.Location)
              _VizWindow.Location = location;

            if (size != _VizWindow.Size)
              _VizWindow.Size = size;

            if (visible != _VizWindow.Visible)
              _VizWindow.Visible = visible;

          }
        }

        if (_NotifyPlayingFlag && CurrentPosition >= 10.0)
        {
          _NotifyPlayingFlag = false;
          GUIMessage msg = new GUIMessage(GUIMessage.MessageType.GUI_MSG_PLAYING_10SEC, 0, 0, 0, 0, 0, null);
          msg.Label = CurrentFile;
          GUIWindowManager.SendThreadMessage(msg);
        }
      }
      else
      {
        if (_VizWindow != null && _VizWindow.Visible)
          _VizWindow.Visible = false;
      }
    }

    void _BassPlayer_WaitCursorRequested(object sender, BASSPlayer.WaitCursorRequest request)
    {
      switch (request)
      {
        case BASSPlayer.WaitCursorRequest.On:
          if (_WaitCursor == null)
          {
            _WaitCursor = new WaitCursor();
          }
          break;

        case BASSPlayer.WaitCursorRequest.Off:
          if (_WaitCursor != null)
          {
            _WaitCursor.Dispose();
            _WaitCursor = null;
          }
          break;
      }
    }

    void OnAppFormDisposed(object sender, EventArgs e)
    {
      RealDispose();
    }

    void OnNewAction(Action action)
    {
      //Log.Debug("BASS: Action: {0}", action.wID);

      switch (action.wID)
      {
        case Action.ActionType.ACTION_FORWARD:
        case Action.ActionType.ACTION_MUSIC_FORWARD:
          if (!IsStream)
            _BassPlayer.Forward();

          break;

        case Action.ActionType.ACTION_REWIND:
        case Action.ActionType.ACTION_MUSIC_REWIND:
          if (!IsStream)
            _BassPlayer.Rewind();

          break;

        case Action.ActionType.ACTION_TOGGLE_MUSIC_GAP:
          _BassPlayer.TogglePlayBackMode();

          break;
      }
    }

    void Log_Write(Log.LogType logType, string format, params object[] arg)
    {
      switch (logType)
      {
        case Log.LogType.Debug:
          {
            MediaPortal.GUI.Library.Log.Debug(format, arg);
            break;
          }
        case Log.LogType.Info:
          {
            MediaPortal.GUI.Library.Log.Info(format, arg);
            break;
          }
        case Log.LogType.Error:
          {
            MediaPortal.GUI.Library.Log.Error(format, arg);
            break;
          }
        case Log.LogType.Warn:
          {
            MediaPortal.GUI.Library.Log.Warn(format, arg);
            break;
          }
      }
    }

    #endregion
  }
}