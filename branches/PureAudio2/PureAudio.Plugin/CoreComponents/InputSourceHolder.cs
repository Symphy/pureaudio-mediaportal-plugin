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
using Un4seen.Bass;
using Un4seen.Bass.AddOn.Mix;
using Un4seen.Bass.Misc;

namespace MediaPortal.Plugins.PureAudio
{
  public partial class Player
  {
    private partial class InputSourceHolder : IDisposable
    {
      #region Static members

      /// <summary>
      /// Creates and initializes an new instance.
      /// </summary>
      /// <param name="mediaItem">The mediaItem to be handled by the instance.</param>
      /// <returns>The new instance.</returns>
      public static InputSourceHolder Create(PureAudioSettings settings, IInputSource inputSource)
      {
        InputSourceHolder instance = new InputSourceHolder(settings, inputSource);
        instance.Initialize();
        return instance;
      }

      #endregion

      #region Fields

      private IInputSource _InputSource;
      private PureAudioSettings _Settings;
      private BassStream _OutputStream;
      private DSP_Gain _ScalingGainDSP;

      #endregion

      #region IDisposable Members

      public void Dispose()
      {
        if (_ScalingGainDSP != null)
        {
          _ScalingGainDSP.Stop();
          _ScalingGainDSP.Dispose();
        }

        if (_OutputStream != null && !_OutputStream.Equals(_InputSource.OutputStream))
          _OutputStream.Dispose();

        if (_InputSource != null)
          _InputSource.Dispose();

        _ScalingGainDSP = null;
        _OutputStream = null;
        _InputSource = null;
        _Settings = null;
      }

      #endregion

      #region Public Members

      public IInputSource InputSource
      {
        get { return _InputSource; }
      }

      public BassStream OutputStream
      {
        get { return _OutputStream; }
      }

      #endregion

      #region Private Members

      private InputSourceHolder(PureAudioSettings settings, IInputSource inputSource)
      {
        _Settings = settings;
        _InputSource = inputSource;
      }

      /// <summary>
      /// Initializes a new instance.
      /// </summary>
      private void Initialize()
      {
        Log.Debug("InputSourceHolder.Initialize()");

        IChannelAssignmentInfo targetChannelAssignmentInfo =
          ChannelAssignmentHelper.GetChannelAssignmentInfo(_Settings.ChannelAssignmentDef);

        MixingMatrix mixingMatrix = MixingMatrix.Create(_InputSource.ChannelAssignmentInfo, targetChannelAssignmentInfo, _Settings.EnableUpmixing);

        if (mixingMatrix.IsPassThrough)
        {
          _OutputStream = _InputSource.OutputStream;
        }
        else
        {
          int mixerStreamHandle = BassMix.BASS_Mixer_StreamCreate(
            _InputSource.OutputStream.SampleRate,
            targetChannelAssignmentInfo.Channels.Count,
            BASSFlag.BASS_SAMPLE_FLOAT | BASSFlag.BASS_STREAM_DECODE);

          if (mixerStreamHandle == BassConstants.BassInvalidHandle)
            throw new BassLibraryException("BASS_Mixer_StreamCreate");

          _OutputStream = BassStream.Create(mixerStreamHandle);

          if (!BassMix.BASS_Mixer_StreamAddChannel(
              _OutputStream.Handle,
              _InputSource.OutputStream.Handle,
              BASSFlag.BASS_MIXER_MATRIX | BASSFlag.BASS_MIXER_DOWNMIX | BASSFlag.BASS_MIXER_NORAMPIN))
            throw new BassLibraryException("BASS_Mixer_StreamAddChannel");

          if (!BassMix.BASS_Mixer_ChannelSetMatrix(_InputSource.OutputStream.Handle, mixingMatrix.BassMixMatrix))
            throw new BassLibraryException("BASS_Mixer_ChannelSetMatrix");

          if (mixingMatrix.Scaling != 0f)
          {
            _ScalingGainDSP = new DSP_Gain();
            _ScalingGainDSP.ChannelHandle = _OutputStream.Handle;
            _ScalingGainDSP.Gain_dBV = mixingMatrix.Scaling;
            _ScalingGainDSP.Start();
          }

          if (Log.IsDebugLevel)
          {
            float[,] currentMatrix = new float[targetChannelAssignmentInfo.Channels.Count, _InputSource.OutputStream.Channels];
            if (BassMix.BASS_Mixer_ChannelGetMatrix(_InputSource.OutputStream.Handle, currentMatrix))
            {
              string header = "";
              foreach (ChannelName cname in _InputSource.ChannelAssignmentInfo.Channels)
              {
                header += cname.ToString().PadLeft(7);
              }
              Log.Debug("matrix: {0} | {1}", String.Empty.PadRight(3), header);
              Log.Debug("matrix: {0}", String.Empty.PadRight(50, '-'));

              for (int i = 0; i < currentMatrix.GetLength(0); i++)
              {
                string line = "";
                for (int j = 0; j < currentMatrix.GetLength(1); j++)
                {
                  line += GetLevelDescr(currentMatrix[i, j]).PadLeft(7);
                }
                Log.Debug("matrix: {0} | {1}", targetChannelAssignmentInfo.Channels[i].ToString().PadRight(3), line);
              }
            }
            else
            {
              Log.Debug("Error retrieving mixingmatrix");
            }
          }
        }
      }

      private string GetLevelDescr(float mixingLevel)
      {
        return mixingLevel > 0 ? Math.Round((20d * Math.Log10(mixingLevel)), 1).ToString() + "dB" : "";
      }

      #endregion

    }
  }
}
