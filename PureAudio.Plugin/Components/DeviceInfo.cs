﻿#region Copyright (C) 2005-2010 Team MediaPortal

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
    private partial class OutputDeviceManager
    {
      private partial class OutputDeviceFactory
      {
        /// <summary>
        /// Contains information about an outputdevice.
        /// </summary>
        private struct DeviceInfo
        {
          #region Fields

          public string _Name;
          public string _Driver;
          public int _Channels;
          public int _MinRate;
          public int _MaxRate;
          public TimeSpan _Latency;

          #endregion

          #region Public members

          public override string ToString()
          {
            return
                String.Format(
                    "Name=\"{0}\", Driver=\"{1}\", Channels={2}, MinRate={3}, MaxRate={4}, Latency={5}ms",
                    _Name,
                    _Driver,
                    _Channels,
                    _MinRate,
                    _MaxRate,
                    _Latency.TotalMilliseconds);
          }

          #endregion
        }
      }
    }
  }
}