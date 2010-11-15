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
using System.Runtime.InteropServices;
using Un4seen.Bass;

namespace MediaPortal.Plugins.PureAudio
{
  public partial class BassPlayer
  {
    private partial class OutputDeviceManager
    {
      private partial class OutputDeviceFactory
      {
        /// <summary>
        /// Represents the user-selected DirectX outputdevice.
        /// </summary>
        private class DirectXOutputDevice : IOutputDevice
        {
          #region Static members

          /// <summary>
          /// Creates and initializes an new instance.
          /// </summary>
          /// <param name="player">Reference to containing IPlayer object.</param>
          /// <returns>The new instance.</returns>
          public static DirectXOutputDevice Create(BassPlayerSettings settings)
          {
            DirectXOutputDevice outputDevice = new DirectXOutputDevice(settings);
            outputDevice.Initialize();
            return outputDevice;
          }

          Dictionary<int, DeviceInfo> _DeviceInfos = new Dictionary<int, DeviceInfo>();

          #endregion

          #region Events

          public event EventHandler OutputStreamEnded;

          #endregion

          #region Fields

          private BassPlayerSettings _Settings;
          private BassStream _InputStream;
          private BassStream _OutputStream;
          private STREAMPROC _StreamWriteProcDelegate;
          private DeviceState _DeviceState;
          private int _DeviceNo;
          private BassStreamFader _Fader;
          private bool _OutputStreamEnded;
          private Silence _Silence;
          private float[] _ReadData = new float[1];
          private float _MaxFloatSampleValue = 8388607.0f / 8388608.0f; //24 bit clipping value

          #endregion

          #region IDisposable Members

          public void Dispose()
          {
            Stop();

            Log.Debug("Disposing output stream");

            _OutputStream.Dispose();
            _OutputStream = null;

            Log.Debug("Resetting global Bass environment");

            if (!Bass.BASS_SetConfig(BASSConfig.BASS_CONFIG_UPDATEPERIOD, 0))
              throw new BassLibraryException("BASS_SetConfig");

            if (!Bass.BASS_SetDevice(BassConstants.BassNoSoundDevice))
              throw new BassLibraryException("BASS_SetDevice");
          }

          #endregion

          #region IOutputDevice Members

          public BassStream InputStream
          {
            get { return _InputStream; }
          }

          public DeviceState DeviceState
          {
            get { return _DeviceState; }
          }

          public string Name
          {
            get { return _DeviceInfos[_DeviceNo]._Name; }
          }

          public string Driver
          {
            get { return _DeviceInfos[_DeviceNo]._Driver; }
          }

          public int Channels
          {
            get { return _DeviceInfos[_DeviceNo]._Channels; }
          }

          public int MinRate
          {
            get { return _DeviceInfos[_DeviceNo]._MinRate; }
          }

          public int MaxRate
          {
            get { return _DeviceInfos[_DeviceNo]._MaxRate; }
          }

          public TimeSpan Latency
          {
            get { return _DeviceInfos[_DeviceNo]._Latency + _Settings.DirectSoundBufferSize; }
          }

          public void SetInputStream(BassStream stream)
          {
            if (_DeviceState != DeviceState.Stopped)
              throw new BassPlayerException("Device state is not 'DeviceState.Stopped'.");

            _InputStream = stream;

            Log.Debug("Creating output stream");

            // BASS builtin conversion from float to 16 bit is not corrent, breaking bitperfectness.
            // Therefor, create a 16 bit stream. Conversion from float to 16 bit is done 
            // in OutputStreamWriteProc()
            int handle = Bass.BASS_StreamCreate(
                _InputStream.SampleRate,
                _InputStream.Channels,
                BASSFlag.BASS_DEFAULT,
                _StreamWriteProcDelegate,
                IntPtr.Zero);

            if (handle == BassConstants.BassInvalidHandle)
              throw new BassLibraryException("BASS_StreamCreate");

            _OutputStream = BassStream.Create(handle);

            _Fader = new BassStreamFader(_OutputStream, _Settings.FadeDuration);

            ResetState();
          }

          public void PrepareFadeIn()
          {
            _Fader.PrepareFadeIn();
          }

          public void FadeIn()
          {
            _Fader.FadeIn();
          }

          public void FadeOut()
          {
            if (!_OutputStreamEnded)
            {
              Log.Debug("Fading out");
              _Fader.FadeOut();
            }
          }

          public void Start()
          {
            if (_DeviceState != DeviceState.Started)
            {
              Log.Debug("Starting output");

              if (!Bass.BASS_ChannelPlay(_OutputStream.Handle, false))
                throw new BassLibraryException("BASS_ChannelPlay");

              _DeviceState = DeviceState.Started;
            }
          }

          public void Stop()
          {
            if (_DeviceState != DeviceState.Stopped)
            {
              Log.Debug("Stopping output");

              if (!Bass.BASS_ChannelStop(_OutputStream.Handle))
                throw new BassLibraryException("BASS_ChannelStop");

              _DeviceState = DeviceState.Stopped;
            }
          }

          public void ClearBuffers()
          {
            Bass.BASS_ChannelSetPosition(_OutputStream.Handle, 0L);
          }

          #endregion

          #region Public members

          #endregion

          #region Private members

          private DirectXOutputDevice(BassPlayerSettings settings)
          {
            _Settings = settings;
          }

          /// <summary>
          /// Initializes a new instance.
          /// </summary>
          private void Initialize()
          {
            _DeviceState = DeviceState.Stopped;
            _StreamWriteProcDelegate = new STREAMPROC(OutputStreamWriteProc);
            _Silence = new Silence();

            _DeviceNo = GetDeviceNo();

            BASSInit flags = BASSInit.BASS_DEVICE_DEFAULT;

            // Because all deviceinfo is saved in a static dictionary,
            // we need to determine the latency only once.
            if (!_DeviceInfos.ContainsKey(_DeviceNo))
              flags |= BASSInit.BASS_DEVICE_LATENCY;

            bool result = Bass.BASS_Init(
               _DeviceNo,
               44100, //Only relevant for -> pre-XP (VxD drivers)
               flags,
               IntPtr.Zero,
               Guid.Empty);

            if (!result)
            {
              if (Bass.BASS_ErrorGetCode() == BASSError.BASS_ERROR_ALREADY)
              {
                if (!Bass.BASS_SetDevice(_DeviceNo))
                  throw new BassLibraryException("BASS_SetDevice");
                result = true;
              }
            }

            if (!result)
              throw new BassLibraryException("BASS_Init");

            CollectDeviceInfo(_DeviceNo);

            int ms = Convert.ToInt32(_Settings.DirectSoundBufferSize.TotalMilliseconds);

            if (!Bass.BASS_SetConfig(BASSConfig.BASS_CONFIG_BUFFER, ms))
              throw new BassLibraryException("BASS_SetConfig");

            if (!Bass.BASS_SetConfig(BASSConfig.BASS_CONFIG_UPDATEPERIOD, ms / 4))
              throw new BassLibraryException("BASS_SetConfig");
          }

          /// <summary>
          /// Retrieves information on a device and adds it to the 
          /// static deviceinfo dictionary do it can be reused later. 
          /// </summary>
          /// <param name="deviceNo">Device number to retrieve information on.</param>
          private void CollectDeviceInfo(int deviceNo)
          {
            // Device info is saved in a dictionary so it can be reused lateron.
            if (!_DeviceInfos.ContainsKey(deviceNo))
            {
              Log.Debug("Collecting device info");

              BASS_DEVICEINFO bassDeviceInfo = Bass.BASS_GetDeviceInfo(deviceNo);
              if (bassDeviceInfo == null)
                throw new BassLibraryException("BASS_GetDeviceInfo");

              BASS_INFO bassInfo = Bass.BASS_GetInfo();
              if (bassInfo == null)
                throw new BassLibraryException("BASS_GetInfo");

              DeviceInfo deviceInfo = new DeviceInfo();
              deviceInfo._Name = bassDeviceInfo.name;
              deviceInfo._Driver = bassDeviceInfo.driver;
              deviceInfo._Channels = bassInfo.speakers;
              deviceInfo._MinRate = bassInfo.minrate;
              deviceInfo._MaxRate = bassInfo.maxrate;
              deviceInfo._Latency = TimeSpan.FromMilliseconds(bassInfo.latency);

              lock (_DeviceInfos)
              {
                _DeviceInfos.Add(deviceNo, deviceInfo);
              }
            }
            Log.Debug("DirectSound Device info: {0}", _DeviceInfos[_DeviceNo].ToString());
          }

          /// <summary>
          /// Gets the device number for the selected DirectSound device.
          /// </summary>
          /// <returns></returns>
          private int GetDeviceNo()
          {
            string deviceName = _Settings.DirectSoundDevice;
            int deviceNo;

            if (String.IsNullOrEmpty(deviceName) || deviceName == BassPlayerSettings.Defaults.DirectSoundDevice)
            {
              Log.Info("Initializing default DirectSound device");
              deviceNo = 1;
            }
            else
            {
              deviceNo = BassConstants.BassDefaultDevice;

              BASS_DEVICEINFO[] deviceDescriptions = Bass.BASS_GetDeviceInfos();
              for (int i = 0; i < deviceDescriptions.Length; i++)
              {
                if (deviceDescriptions[i].name == deviceName)
                {
                  deviceNo = i;
                  break;
                }
              }
              if (deviceNo == BassConstants.BassDefaultDevice)
              {
                Log.Warn("Specified DirectSound device does not exist. Initializing default DirectSound Device");
                deviceNo = 1;
              }
              else
                Log.Info("Initializing DirectSound Device {0}", deviceName);
            }
            return deviceNo;
          }

          /// <summary>
          /// Callback function for the outputstream.
          /// </summary>
          /// <param name="streamHandle">Bass stream handle that requests sample data.</param>
          /// <param name="buffer">Buffer to write the sampledata in.</param>
          /// <param name="requestedBytes">Requested number of bytes.</param>
          /// <param name="userData"></param>
          /// <returns>Number of bytes read.</returns>
          unsafe private int OutputStreamWriteProc(int streamHandle, IntPtr buffer, int requestedBytes, IntPtr userData)
          {
            short* output = (short*)buffer;
            int requestedSamples = requestedBytes / BassConstants.ShortBytes;

            // Make sure the temp buffer is large enough
            if (_ReadData.Length < requestedSamples)
              Array.Resize<float>(ref _ReadData, requestedSamples);

            // Read float data
            int samplesRead = _InputStream.Read(_ReadData, requestedSamples);
            if (samplesRead > 0)
            {
              // Convert to 16 bit
              for (int i = 0; i < samplesRead; i++)
              {
                float floatValue = _ReadData[i];

                if (floatValue > _MaxFloatSampleValue)
                  floatValue = _MaxFloatSampleValue;

                else if (floatValue < -1.0f)
                  floatValue = -1.0f;

                output[i] = (short)(floatValue * 32768.0f);
              }
            }
            else if (samplesRead == 0)
            {
              // We're done!

              // Play silence until playback has stopped to avoid any buffer underruns.
              int read = _Silence.Write(buffer, requestedBytes);

              // Set a flag so we call HandleOutputStreamEnded() only once.
              if (!_OutputStreamEnded)
              {
                _OutputStreamEnded = true;

                // Let the world know that we can stop now.
                if (OutputStreamEnded != null)
                  OutputStreamEnded(this, new EventArgs());
              }
            }
            return samplesRead * BassConstants.ShortBytes;
          }

          /// <summary>
          /// Resets all stored state.
          /// </summary>
          private void ResetState()
          {
            _OutputStreamEnded = false;
          }

          #endregion

        }
      }
    }
  }
}