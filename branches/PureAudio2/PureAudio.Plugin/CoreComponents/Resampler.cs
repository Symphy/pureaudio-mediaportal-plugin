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
using Un4seen.Bass;
using Un4seen.Bass.AddOn.Mix;

namespace MediaPortal.Plugins.PureAudio
{
  public partial class BassPlayer
  {
    /// <summary>
    /// Performs upmixing and downmixing.
    /// </summary>
    private class Resampler : IDisposable
    {
      #region Static members

      /// <summary>
      /// Creates and initializes an new instance.
      /// </summary>
      /// <param name="player">Reference to containing IPlayer object.</param>
      /// <returns>The new instance.</returns>
      public static Resampler Create(BassPlayer player)
      {
        Resampler resampler = new Resampler(player);
        resampler.Initialize();
        return resampler;
      }

      #endregion

      #region Fields

      private BassPlayer _Player;
      private BassStream _InputStream;
      private BassStream _OutputStream;
      private bool _Initialized;

      #endregion

      #region IDisposable Members

      public void Dispose()
      {
      }

      #endregion

      #region Public members

      /// <summary>
      /// Gets the current inputstream as set with SetInputStream.
      /// </summary>
      public BassStream InputStream
      {
        get { return _InputStream; }
      }

      /// <summary>
      /// Gets the output Bass stream.
      /// </summary>
      public BassStream OutputStream
      {
        get { return _OutputStream; }
      }

      /// <summary>
      /// Sets the Bass inputstream.
      /// </summary>
      /// <param name="stream"></param>
      public void SetInputStream(BassStream stream)
      {
        Log.Debug("Resampler.SetInputStream()");

        ResetInputStream();

        _InputStream = stream;

        int maxRate = _Player._OutputDeviceManager.OutputDevice.MaxRate;
        int minRate = _Player._OutputDeviceManager.OutputDevice.MinRate;

        int inputRate = _InputStream.SampleRate;
        int outputRate = inputRate;

        if (_Player._Settings.EnableOversampling)
        {
          int factor = 2;
          while (outputRate * factor <= maxRate)
          {
            factor++;
          }
          outputRate *= factor;
        }

        outputRate = Math.Max(outputRate, minRate);
        outputRate = Math.Min(outputRate, maxRate);

        if (outputRate != inputRate)
        {
          Log.Info("Resampling {0} -> {1}...", inputRate, outputRate);

          BASSFlag streamFlags =
              BASSFlag.BASS_SAMPLE_FLOAT |
              BASSFlag.BASS_STREAM_DECODE |
              BASSFlag.BASS_MIXER_NORAMPIN;

          int handle = BassMix.BASS_Mixer_StreamCreate(outputRate, _InputStream.Channels, streamFlags);
          if (handle == BassConstants.BassInvalidHandle)
            throw new BassLibraryException("BASS_Mixer_StreamCreate");

          _OutputStream = BassStream.Create(handle);

          if (!BassMix.BASS_Mixer_StreamAddChannel(_OutputStream.Handle, _InputStream.Handle, BASSFlag.BASS_MIXER_NORAMPIN))
            throw new BassLibraryException("BASS_Mixer_StreamAddChannel");
        }
        else
        {
          // No resampling required; just pass on inputstream
          _OutputStream = _InputStream;
        }
        _Initialized = true;
      }

      /// <summary>
      /// Resets the instance to its uninitialized state.
      /// </summary>
      public void ResetInputStream()
      {
        if (_Initialized)
        {
          _Initialized = false;

          if (!_OutputStream.Equals(_InputStream))
            _OutputStream.Dispose();
          
          _OutputStream = null;
          _InputStream = null;
        }
      }

      #endregion

      #region Private members

      private Resampler(BassPlayer player)
      {
        _Player = player;
      }

      /// <summary>
      /// Initializes a new instance.
      /// </summary>
      private void Initialize()
      {
      }

      #endregion
    }
  }
}