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
    static private partial class ChannelAssignmentHelper
    {
      private class ChannelAssignmentInfoDictionary : Dictionary<ChannelAssignmentDef, IChannelAssignmentInfo>
      {
        public ChannelAssignmentInfoDictionary()
        {
          this.Add(ChannelAssignmentDef._1_0, new ChannelAssignmentInfos._1_0());
          this.Add(ChannelAssignmentDef._2_0, new ChannelAssignmentInfos._2_0());
          this.Add(ChannelAssignmentDef._4_0, new ChannelAssignmentInfos._4_0());
          this.Add(ChannelAssignmentDef._4_1, new ChannelAssignmentInfos._4_1());
          this.Add(ChannelAssignmentDef._5_0, new ChannelAssignmentInfos._5_0());
          this.Add(ChannelAssignmentDef._5_1, new ChannelAssignmentInfos._5_1());
          this.Add(ChannelAssignmentDef._7_1, new ChannelAssignmentInfos._7_1());
        }
      }
    }
  }
}
