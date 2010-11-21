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
  public partial class PureAudioPlayer
  {
    static private partial class ChannelAssignmentHelper
    {
      /// <summary>
      /// WAVEFORMATEXTENSIBLE masks
      /// </summary>
      static private class WFEChannelMasks
      {
        /// <summary>
        /// 1.0 Mono
        /// </summary>
        public const int _1_0 =
          WFEChannels.SPEAKER_FRONT_CENTER;

        /// <summary>
        /// 2.0 Stereo
        /// </summary>
        public const int _2_0 =
          WFEChannels.SPEAKER_FRONT_LEFT |
          WFEChannels.SPEAKER_FRONT_RIGHT;

        /// <summary>
        /// 4.0 Quadrophonic
        /// </summary>
        public const int _4_0_Q =
          WFEChannels.SPEAKER_FRONT_LEFT |
          WFEChannels.SPEAKER_FRONT_RIGHT |
          WFEChannels.SPEAKER_BACK_LEFT |
          WFEChannels.SPEAKER_BACK_RIGHT;

        /// <summary>
        /// 4.0 Surround
        /// </summary>
        public const int _4_0_S =
          WFEChannels.SPEAKER_FRONT_LEFT |
          WFEChannels.SPEAKER_FRONT_RIGHT |
          WFEChannels.SPEAKER_FRONT_CENTER |
          WFEChannels.SPEAKER_BACK_CENTER;

        /// <summary>
        /// 4.1
        /// </summary>
        public const int _4_1 =
          WFEChannels.SPEAKER_FRONT_LEFT |
          WFEChannels.SPEAKER_FRONT_RIGHT |
          WFEChannels.SPEAKER_LOW_FREQUENCY |
          WFEChannels.SPEAKER_BACK_LEFT |
          WFEChannels.SPEAKER_BACK_RIGHT;

        /// <summary>
        /// 5.0
        /// </summary>
        public const int _5_0 =
          WFEChannels.SPEAKER_FRONT_LEFT |
          WFEChannels.SPEAKER_FRONT_RIGHT |
          WFEChannels.SPEAKER_FRONT_CENTER |
          WFEChannels.SPEAKER_BACK_LEFT |
          WFEChannels.SPEAKER_BACK_RIGHT;

        /// <summary>
        /// 5.1 Back speaker
        /// </summary>
        public const int _5_1_B =
          WFEChannels.SPEAKER_FRONT_LEFT |
          WFEChannels.SPEAKER_FRONT_RIGHT |
          WFEChannels.SPEAKER_FRONT_CENTER |
          WFEChannels.SPEAKER_LOW_FREQUENCY |
          WFEChannels.SPEAKER_BACK_LEFT |
          WFEChannels.SPEAKER_BACK_RIGHT;

        /// <summary>
        /// 5.1 Side speaker
        /// </summary>
        public const int _5_1_S =
          WFEChannels.SPEAKER_FRONT_LEFT |
          WFEChannels.SPEAKER_FRONT_RIGHT |
          WFEChannels.SPEAKER_FRONT_CENTER |
          WFEChannels.SPEAKER_LOW_FREQUENCY |
          WFEChannels.SPEAKER_SIDE_LEFT |
          WFEChannels.SPEAKER_SIDE_RIGHT;

        /// <summary>
        /// 7.1 surround
        /// </summary>
        public const int _7_1 =
          WFEChannels.SPEAKER_FRONT_LEFT |
          WFEChannels.SPEAKER_FRONT_RIGHT |
          WFEChannels.SPEAKER_FRONT_CENTER |
          WFEChannels.SPEAKER_LOW_FREQUENCY |
          WFEChannels.SPEAKER_BACK_LEFT |
          WFEChannels.SPEAKER_BACK_RIGHT |
          WFEChannels.SPEAKER_SIDE_LEFT |
          WFEChannels.SPEAKER_SIDE_RIGHT;
      }
      
      /// <summary>
      /// WAVEFORMATEXTENSIBLE channels
      /// </summary>
      static private class WFEChannels
      {
        public const int SPEAKER_FRONT_LEFT = 0x1;
        public const int SPEAKER_FRONT_RIGHT = 0x2;
        public const int SPEAKER_FRONT_CENTER = 0x4;
        public const int SPEAKER_LOW_FREQUENCY = 0x8;
        public const int SPEAKER_BACK_LEFT = 0x10;
        public const int SPEAKER_BACK_RIGHT = 0x20;
        public const int SPEAKER_FRONT_LEFT_OF_CENTER = 0x40;
        public const int SPEAKER_FRONT_RIGHT_OF_CENTER = 0x80;
        public const int SPEAKER_BACK_CENTER = 0x100;
        public const int SPEAKER_SIDE_LEFT = 0x200;
        public const int SPEAKER_SIDE_RIGHT = 0x400;
        public const int SPEAKER_TOP_CENTER = 0x800;
        public const int SPEAKER_TOP_FRONT_LEFT = 0x1000;
        public const int SPEAKER_TOP_FRONT_CENTER = 0x2000;
        public const int SPEAKER_TOP_FRONT_RIGHT = 0x4000;
        public const int SPEAKER_TOP_BACK_LEFT = 0x8000;
        public const int SPEAKER_TOP_BACK_CENTER = 0x10000;
        public const int SPEAKER_TOP_BACK_RIGHT = 0x20000;
      }
    }
  }
}