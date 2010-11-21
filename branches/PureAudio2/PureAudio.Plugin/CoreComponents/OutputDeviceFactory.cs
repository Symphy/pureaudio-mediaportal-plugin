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
    private partial class OutputDeviceManager
    {
      /// <summary>
      /// 
      /// </summary>
      private partial class OutputDeviceFactory : IDisposable
      {
        #region Fields

        PureAudioPlayer _Player;

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
        }

        #endregion

        #region Public members

        public OutputDeviceFactory(PureAudioPlayer player)
        {
          _Player = player;
        }

        /// <summary>
        /// Creates an IOutputDevice object based on usersettings.
        /// </summary>
        /// <returns></returns>
        public IOutputDevice CreateOutputDevice()
        {
          IOutputDevice outputDevice;
          switch (_Player._Settings.OutputMode)
          {
            case OutputMode.DirectSound:
              outputDevice = DirectXOutputDevice.Create(_Player._Settings);
              break;

            case OutputMode.ASIO:
              outputDevice = ASIOOutputDevice.Create(_Player._Settings);
              break;

            default:
              throw new BassPlayerException(String.Format("Unknown constant OutputMode.{0}", _Player._Settings.OutputMode));
          }
          return outputDevice;
        }

        #endregion
      }
    }
  }
}