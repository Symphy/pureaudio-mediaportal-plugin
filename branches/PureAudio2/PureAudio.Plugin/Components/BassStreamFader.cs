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
using System.Threading;
using Un4seen.Bass;

namespace MediaPortal.Player.PureAudio
{
  /// <summary>
  /// Performs fade-in and fade-out on a Bass stream.
  /// </summary>
  /// <remarks>
  /// Only modifies the volume attribute.
  /// The actual fading must be implemented in code that reads from the stream!
  /// </remarks>
  public partial class BassPlayer
  {
    private partial class BassStreamFader
    {
      #region Fields

      BassStream _Stream;
      TimeSpan _Duration;
      int _DurationMS;

      #endregion

      #region Public members

      public BassStreamFader(BassStream stream, TimeSpan duration)
      {
        _Stream = stream;
        _Duration = duration;
        _DurationMS = Convert.ToInt32(duration.TotalMilliseconds);
      }

      /// <summary>
      /// Prepares for a fadein; sets the volume to zero.
      /// </summary>
      public void PrepareFadeIn()
      {
        SetVolume(0f);
      }

      /// <summary>
      /// Performs a fadein.
      /// </summary>
      /// <remarks>
      /// If the fade duration is set to 0 in usersettings, volume is set to 100% instantly.
      /// </remarks>
      public void FadeIn()
      {
        if (_DurationMS != 0)
        {
          SetVolume((float)InternalSettings.FadingMinLeveldB);
          SlideVolume(0f);
        }
        else
        {
          SetVolume(0f);
        }
      }

      /// <summary>
      /// Performs a fadeout.
      /// </summary>
      /// <remarks>
      /// If the fade duration is set to 0 in usersettings, volume is set to 0% instantly.
      /// </remarks>
      public void FadeOut()
      {
        if (_DurationMS != 0)
        {
          SlideVolume((float)InternalSettings.FadingMinLeveldB);

          while (Bass.BASS_ChannelIsSliding(_Stream.Handle, BASSAttribute.BASS_ATTRIB_VOL))
          {
            Thread.Sleep(10);
          }
        }
        SetVolume(-100f);
      }

      #endregion

      #region Private members

      /// <summary>
      /// Sets the volume to the given value instantly.
      /// </summary>
      /// <param name="volume">-100dB - 0dB</param>
      private void SetVolume(float volume)
      {
        float attributeValue = (100f + volume) / 100f;
        if (!Bass.BASS_ChannelSetAttribute(_Stream.Handle, BASSAttribute.BASS_ATTRIB_VOL, attributeValue))
          throw new BassPlayerException("BASS_ChannelSetAttribute");
      }

      /// <summary>
      /// Slides the volume to the given value over the period of time determined by the fadeduration usersetting.
      /// </summary>
      /// <param name="volume">-100dB - 0dB</param>
      private void SlideVolume(float volume)
      {
        float attributeValue = (100f + volume) / 100f;
        if (!Bass.BASS_ChannelSlideAttribute(_Stream.Handle, BASSAttribute.BASS_ATTRIB_VOL, attributeValue, _DurationMS))
          throw new BassPlayerException("BASS_ChannelSlideAttribute");
      }

      #endregion

    }
  }
}
