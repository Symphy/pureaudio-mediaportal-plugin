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

namespace MediaPortal.Player.PureAudio
{
  public partial class BassPlayer
  {
    static private partial class ChannelAssignmentHelper
    {
      /// <summary>
      /// Contains definitions for all supported channel assignments
      /// </summary>
      static private class ChannelAssignmentInfos
      {
        /// <summary>
        /// 1.0 Mono
        /// </summary>
        public class _1_0 : IChannelAssignmentInfo
        {
          #region IChannelAssignmentInfo Members

          public ChannelAssignmentDef AssignmentDef
          {
            get { return ChannelAssignmentDef._1_0; }
          }

          public string Name
          {
            get { return "Mono"; }
          }

          private static List<ChannelName> _Channels = new List<ChannelName>(new ChannelName[]
          {
            ChannelName.C
          });

          public List<ChannelName> Channels
          {
            get { return _Channels; }
          }

          public bool Input
          {
            get { return true; }
          }

          public bool Output
          {
            get { return false; }
          }

          #endregion
        }

        /// <summary>
        /// 2.0 Stereo
        /// </summary>
        public class _2_0 : IChannelAssignmentInfo
        {
          #region IChannelAssignmentInfo Members

          public ChannelAssignmentDef AssignmentDef
          {
            get { return ChannelAssignmentDef._2_0; }
          }

          public string Name
          {
            get { return "Stereo"; }
          }

          private static List<ChannelName> _Channels = new List<ChannelName>(new ChannelName[]
          {
            ChannelName.Lf,
            ChannelName.Rf
          });

          public List<ChannelName> Channels
          {
            get { return _Channels; }
          }

          public bool Input
          {
            get { return true; }
          }

          public bool Output
          {
            get { return true; }
          }

          #endregion
        }

        /// <summary>
        /// 4.0 Quadrophonic
        /// </summary>
        public class _4_0 : IChannelAssignmentInfo
        {
          #region IChannelAssignmentInfo Members

          public ChannelAssignmentDef AssignmentDef
          {
            get { return ChannelAssignmentDef._4_0; }
          }

          public string Name
          {
            get { return "4.0"; }
          }

          private static List<ChannelName> _Channels = new List<ChannelName>(new ChannelName[]
          {
            ChannelName.Lf,
            ChannelName.Rf,
            ChannelName.Ls,
            ChannelName.Rs
          });

          public List<ChannelName> Channels
          {
            get { return _Channels; }
          }

          public bool Input
          {
            get { return true; }
          }

          public bool Output
          {
            get { return true; }
          }

          #endregion
        }

        /// <summary>
        /// 4.1
        /// </summary>
        public class _4_1 : IChannelAssignmentInfo
        {
          #region IChannelAssignmentInfo Members

          public ChannelAssignmentDef AssignmentDef
          {
            get { return ChannelAssignmentDef._4_1; }
          }

          public string Name
          {
            get { return "4.1"; }
          }

          private static List<ChannelName> _Channels = new List<ChannelName>(new ChannelName[]
          {
            ChannelName.Lf,
            ChannelName.Rf,
            ChannelName.LFE,
            ChannelName.Ls,
            ChannelName.Rs
          });

          public List<ChannelName> Channels
          {
            get { return _Channels; }
          }

          public bool Input
          {
            get { return true; }
          }

          public bool Output
          {
            get { return false; }
          }

          #endregion
        }

        /// <summary>
        /// 5.0
        /// </summary>
        public class _5_0 : IChannelAssignmentInfo
        {
          #region IChannelAssignmentInfo Members

          public ChannelAssignmentDef AssignmentDef
          {
            get { return ChannelAssignmentDef._5_0; }
          }

          public string Name
          {
            get { return "5.0"; }
          }

          private static List<ChannelName> _Channels = new List<ChannelName>(new ChannelName[]
          {
            ChannelName.Lf,
            ChannelName.Rf,
            ChannelName.C,
            ChannelName.Ls,
            ChannelName.Rs
          });

          public List<ChannelName> Channels
          {
            get { return _Channels; }
          }

          public bool Input
          {
            get { return true; }
          }

          public bool Output
          {
            get { return false; }
          }

          #endregion
        }

        /// <summary>
        /// 5.1 Side speaker
        /// </summary>
        public class _5_1 : IChannelAssignmentInfo
        {
          #region IChannelAssignmentInfo Members

          public ChannelAssignmentDef AssignmentDef
          {
            get { return ChannelAssignmentDef._5_1; }
          }

          public string Name
          {
            get { return "5.1"; }
          }

          private static List<ChannelName> _Channels = new List<ChannelName>(new ChannelName[]
          {
            ChannelName.Lf,
            ChannelName.Rf,
            ChannelName.C,
            ChannelName.LFE,
            ChannelName.Ls,
            ChannelName.Rs
          });

          public List<ChannelName> Channels
          {
            get { return _Channels; }
          }

          public bool Input
          {
            get { return true; }
          }

          public bool Output
          {
            get { return true; }
          }

          #endregion
        }

        /// <summary>
        /// 7.1 Home theater
        /// </summary>
        public class _7_1 : IChannelAssignmentInfo
        {
          #region IChannelAssignmentInfo Members

          public ChannelAssignmentDef AssignmentDef
          {
            get { return ChannelAssignmentDef._7_1; }
          }

          public string Name
          {
            get { return "7.1"; }
          }

          private static List<ChannelName> _Channels = new List<ChannelName>(new ChannelName[]
          {
            ChannelName.Lf,
            ChannelName.Rf,
            ChannelName.C,
            ChannelName.LFE,
            ChannelName.Ls,
            ChannelName.Rs,
            ChannelName.Lrs,
            ChannelName.Rrs
          });

          public List<ChannelName> Channels
          {
            get { return _Channels; }
          }

          public bool Input
          {
            get { return true; }
          }

          public bool Output
          {
            get { return true; }
          }

          #endregion
        }

        public class Custom : IChannelAssignmentInfo
        {

          #region Fields
          
          private bool _Input;
          private bool _Output;
          private List<ChannelName> _Channels;
          
          #endregion
          
          #region IChannelAssignmentInfo Members

          public ChannelAssignmentDef AssignmentDef
          {
            get { return ChannelAssignmentDef.Custom; }
          }

          public string Name
          {
            get { return "Custom"; }
          }

          public List<ChannelName> Channels
          {
            get { return _Channels; }
          }

          public bool Input
          {
            get { return _Input; }
          }

          public bool Output
          {
            get { return _Output; }
          }

          #endregion

          #region Public members

          public Custom(bool input, bool output, List<ChannelName> channels)
          {
            _Input = input;
            _Output = output;
            channels = _Channels;
          }
          
          #endregion

        }
      }
    }
  }
}