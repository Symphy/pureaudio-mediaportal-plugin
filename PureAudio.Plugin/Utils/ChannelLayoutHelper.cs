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

namespace MediaPortal.Plugins.PureAudio
{
  public partial class BassPlayer
  {
    /// <summary>
    /// Provides helperfunctions dealing with channelassignments and mappings.
    /// </summary>
    static private partial class ChannelAssignmentHelper
    {
      private static ChannelAssignmentInfoDictionary _ChannelAssignmentInfoDictionary = new ChannelAssignmentInfoDictionary();

      /// <summary>
      /// Gets a channelassignmentinfo object for the given channelassignment
      /// </summary>
      /// <param name="channelAssignment"></param>
      /// <returns></returns>
      public static IChannelAssignmentInfo GetChannelAssignmentInfo(ChannelAssignmentDef channelAssignmentDef)
      {
        return _ChannelAssignmentInfoDictionary[channelAssignmentDef];
      }

      /// <summary>
      /// Gets a channelassignmentinfo object for a given number of channels.
      /// </summary>
      /// <param name="channels"></param>
      /// <returns></returns>
      public static IChannelAssignmentInfo GuessChannelAssignmentInfo(int channels)
      {
        if (channels == 5)
          return _ChannelAssignmentInfoDictionary[ChannelAssignmentDef._5_0];

        foreach (IChannelAssignmentInfo info in _ChannelAssignmentInfoDictionary.Values)
        {
          if (info.Channels.Count == channels)
            return info;
        }
        return _ChannelAssignmentInfoDictionary[ChannelAssignmentDef._2_0];
      }

      /// <summary>
      /// Gets a channelassignmentinfo object based on a WAVEFORMATEXTENSIBLE_CHANNEL_MASK
      /// </summary>
      /// <param name="wfeMask"></param>
      /// <returns></returns>
      public static IChannelAssignmentInfo GetChannelAssignmentInfo(int wfeMask)
      {
        IChannelAssignmentInfo assignmentInfo = null;
        switch (wfeMask)
        {
          case WFEChannelMasks._1_0:
            assignmentInfo = _ChannelAssignmentInfoDictionary[ChannelAssignmentDef._1_0];
            break;

          case WFEChannelMasks._2_0:
            assignmentInfo = _ChannelAssignmentInfoDictionary[ChannelAssignmentDef._2_0];
            break;

          case WFEChannelMasks._4_0_Q:
            assignmentInfo = _ChannelAssignmentInfoDictionary[ChannelAssignmentDef._4_0];
            break;

          case WFEChannelMasks._4_1:
            assignmentInfo = _ChannelAssignmentInfoDictionary[ChannelAssignmentDef._4_1];
            break;

          case WFEChannelMasks._5_0:
            assignmentInfo = _ChannelAssignmentInfoDictionary[ChannelAssignmentDef._5_0];
            break;

          case WFEChannelMasks._5_1_B:
            assignmentInfo = _ChannelAssignmentInfoDictionary[ChannelAssignmentDef._5_1];
            break;

          case WFEChannelMasks._5_1_S:
            assignmentInfo = _ChannelAssignmentInfoDictionary[ChannelAssignmentDef._5_1];
            break;

          case WFEChannelMasks._7_1:
            assignmentInfo = _ChannelAssignmentInfoDictionary[ChannelAssignmentDef._7_1];
            break;

          default:
            assignmentInfo = null;
            break;
        }
        
        if (assignmentInfo == null)
        {
          List<ChannelName> channels = new List<ChannelName>();

          if (WFEMaskContains(wfeMask, WFEChannels.SPEAKER_FRONT_LEFT))
            channels.Add(ChannelName.Lf);

          if (WFEMaskContains(wfeMask, WFEChannels.SPEAKER_FRONT_RIGHT))
            channels.Add(ChannelName.Rf);

          if (WFEMaskContains(wfeMask, WFEChannels.SPEAKER_FRONT_CENTER))
            channels.Add(ChannelName.C);

          if (WFEMaskContains(wfeMask, WFEChannels.SPEAKER_LOW_FREQUENCY))
            channels.Add(ChannelName.LFE);

          if (WFEMaskContains(wfeMask, WFEChannels.SPEAKER_BACK_LEFT))
            channels.Add(ChannelName.Ls);

          if (WFEMaskContains(wfeMask, WFEChannels.SPEAKER_BACK_RIGHT))
            channels.Add(ChannelName.Rs);

          if (WFEMaskContains(wfeMask, WFEChannels.SPEAKER_SIDE_LEFT))
            channels.Add(ChannelName.Lrs);

          if (WFEMaskContains(wfeMask, WFEChannels.SPEAKER_SIDE_RIGHT))
            channels.Add(ChannelName.Rrs);

          assignmentInfo = new ChannelAssignmentInfos.Custom(true, false, channels);
        }
        return assignmentInfo;
      }
      
      private static bool WFEMaskContains(int wfeMask, int wfeChannel)
      {
        return (wfeMask & wfeChannel) == wfeChannel;
      }
    }
  }
}
