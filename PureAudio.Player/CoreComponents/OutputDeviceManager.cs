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

namespace MediaPortal.Plugins.PureAudio
{
  public partial class Player
  {
    /// <summary>
    /// Manages creation and initialization of the outputdevice.
    /// </summary>
    private partial class OutputDeviceManager : IDisposable
    {
      #region Static members

      /// <summary>
      /// Creates and initializes an new instance.
      /// </summary>
      /// <param name="player">Reference to containing IPlayer object.</param>
      /// <returns>The new instance.</returns>
      public static OutputDeviceManager Create(Player player)
      {
        OutputDeviceManager outputDeviceManager = new OutputDeviceManager(player);
        outputDeviceManager.Initialize();
        return outputDeviceManager;
      }

      #endregion

      #region Events

      public event EventHandler OutputStreamEnded;

      #endregion

      #region Fields

      private Player _Player;
      private OutputDeviceFactory _OutputDeviceFactory;
      private IOutputDevice _OutputDevice;
      private bool _StreamInitialized;
      private bool _DeviceInitialized;

      #endregion

      #region IDisposable Members

      public void Dispose()
      {
        Log.Debug("OutputDeviceManager.Dispose()");
        
        if (_OutputDevice != null)
        {
          Log.Debug("Disposing output device");
          
          _OutputDevice.Dispose();
          _OutputDevice = null;

          Log.Debug("Disposing output device factory");
          _OutputDeviceFactory.Dispose();
        }
      }

      #endregion

      #region Public members

      /// <summary>
      /// Gets the current inputstream as set with SetInputStream.
      /// </summary>
      public BassStream InputStream
      {
        get { return OutputDevice.InputStream; }
      }

      /// <summary>
      /// Returns a reference to the currently used IOutputDevice object.
      /// </summary>
      public IOutputDevice OutputDevice
      {
        get { return _OutputDevice; }
      }

      public void InitOutputDevice()
      {
        if (!_DeviceInitialized)
        {
          Log.Debug("Instantiating output device");
          _OutputDevice = _OutputDeviceFactory.CreateOutputDevice();
          _OutputDevice.OutputStreamEnded += new EventHandler(_OutputDevice_OutputStreamEnded);
          
          _DeviceInitialized = true;
        }
      }

      /// <summary>
      /// Sets the Bass inputstream and initializes the outputdevice.
      /// </summary>
      /// <param name="stream"></param>
      public void SetInputStream(BassStream stream)
      {
        Log.Debug("OutputDeviceManager.SetInputStream()");

        if (!_DeviceInitialized)
          throw new BassPlayerException("Outputdevice is not initialized");

        ResetInputStream();
        
        Log.Debug("Calling IOutputDevice.SetInputStream()");
        _OutputDevice.SetInputStream(stream);
        _StreamInitialized = true;
      }

      /// <summary>
      /// Starts playback.
      /// </summary>
      /// <param name="fadeIn"></param>
      public void StartDevice(bool fadeIn)
      {
        Log.Debug("OutputDeviceManager.StartDevice()");

        if (!_StreamInitialized)
          throw new BassPlayerException("OutputDeviceManager not initialized");
        
        if (_OutputDevice.DeviceState == DeviceState.Stopped)
        {
          if (fadeIn)
          {
            Log.Debug("Calling IOutputDevice.PrepareFadeIn()");
            _OutputDevice.PrepareFadeIn();
          }

          Log.Debug("Calling IOutputDevice.Start()");
          _OutputDevice.Start();

          if (fadeIn)
          {
            Log.Debug("Calling IOutputDevice.FadeIn()");
            _OutputDevice.FadeIn();
          }
        }
      }

      /// <summary>
      /// Stops playback.
      /// </summary>
      public void StopDevice()
      {
        Log.Debug("OutputDeviceManager.StopDevice()");

        if (!_StreamInitialized)
          throw new BassPlayerException("OutputDeviceManager not initialized");

        if (_OutputDevice.DeviceState == DeviceState.Started)
        {
          if (!_Player._InputSourceManager.CurrentInputSourceHolder.OutputStream.IsPassThrough)
          {
            Log.Debug("Calling IOutputDevice.FadeOut()");
            _OutputDevice.FadeOut();
          }

          Log.Debug("Calling IOutputDevice.Stop()");
          _OutputDevice.Stop();
        }
      }

      /// <summary>
      /// Resets the devices outputbuffers and fills them with zeros.
      /// </summary>
      public void ClearDeviceBuffers()
      {
        Log.Debug("OutputDeviceManager.ClearDeviceBuffers()");
        
        if (!_StreamInitialized)
          throw new BassPlayerException("OutputDeviceManager not initialized");
        
        _OutputDevice.ClearBuffers();
      }

      /// <summary>
      /// Disposes the currently instantiated outputdevice.
      /// </summary>
      public void ResetOutputDevice()
      {
        Log.Debug("OutputDeviceManager.ResetOutputDevice()");

        if (_DeviceInitialized)
        {
          _DeviceInitialized = false;

          Log.Debug("Stopping output device");

          if (_OutputDevice.DeviceState == DeviceState.Started)
            _OutputDevice.Stop();

          Log.Debug("Disposing output device");

          _OutputDevice.Dispose();
          _OutputDevice = null;
        }
      }

      /// <summary>
      /// Resets the outputdevice manager to its uninitialized state.
      /// </summary>
      public void ResetInputStream()
      {
        Log.Debug("OutputDeviceManager.ResetInputStream()");

        if (_StreamInitialized)
        {
          _StreamInitialized = false;
        }
      }

      #endregion

      #region Private members
      
      private OutputDeviceManager(Player player)
      {
        _Player = player;
      }
      
      /// <summary>
      /// Initializes a new instance.
      /// </summary>
      private void Initialize()
      {
        _OutputDeviceFactory = new OutputDeviceFactory(this._Player);
      }

      #endregion

      #region Eventhandlers

      void _OutputDevice_OutputStreamEnded(object sender, EventArgs e)
      {
        if (OutputStreamEnded != null)
          OutputStreamEnded(this, e);
      }

      #endregion

    }
  }
}