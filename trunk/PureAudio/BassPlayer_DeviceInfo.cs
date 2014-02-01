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
using Un4seen.BassWasapi;
using MediaPortal.Player.PureAudio.Asio;

namespace MediaPortal.Player.PureAudio
{
	public partial class BASSPlayer
	{
    public class WASAPIDeviceInfo : DeviceInfo
    {
      private int _Channels = 8;
      private int _MinRate = 8000;
      private int _MaxRate = 192000;
      private int _Latency = 50;

      public override int Channels
      {
        get { return _Channels; }
      }

      public override int MinRate
      {
        get { return _MinRate; }
      }

      public override int MaxRate
      {
        get { return _MaxRate; }
      }

      public override int Latency
      {
        get { return _Latency; }
      }

      public WASAPIDeviceInfo(BASS_WASAPI_DEVICEINFO deviceInfo, bool exclusive, int minRate, int maxRate, int channels)
      {
        if (!exclusive)
        {
          _MinRate = deviceInfo.mixfreq;
          _MaxRate = deviceInfo.mixfreq;
          _Channels = deviceInfo.mixchans;
        }
        else
        {
          _MinRate = minRate;
          _MaxRate = maxRate;
          _Channels = channels;
        }
      }
    }
    
    public class AsioDeviceInfo : DeviceInfo
		{
			private int _Channels = 0;
			private int _MinRate = 8000;
			private int _MaxRate = 192000;
			private int _Latency = 0;

			public override int Channels
			{
				get{return _Channels;}
			}

			public override int MinRate
			{
				get{return _MinRate;}
			}

			public override int MaxRate
			{
				get{return _MaxRate;}
			}

			public override int Latency
			{
				get{return _Latency;}
			}

			public AsioDeviceInfo(AsioEngine asioEngine, int minRate, int maxRate, int latency)
			{
				_Channels = asioEngine.Driver.OutputChannels.Length;
				_MinRate = minRate;
				_MaxRate = maxRate;
				_Latency = latency;
			}
		}

		public class DSDeviceInfo : DeviceInfo
		{
			private BASS_INFO _DeviceInfo = null;

			public BASS_INFO DeviceInfo
			{
				get
				{
					return _DeviceInfo;
				}
			}

			public override int MinRate
			{
				get
				{
					return _DeviceInfo.minrate;
				}
			}

			public override int MaxRate
			{
				get
				{
					return _DeviceInfo.maxrate;
				}
			}

			public override int Channels
			{
				get
				{
					return _DeviceInfo.speakers;
				}
			}

			public override int Latency
			{
				get
				{
					return _DeviceInfo.latency;
				}
			}

			public DSDeviceInfo(BASS_INFO deviceInfo)
			{
				_DeviceInfo = deviceInfo;
			}
		}

		public abstract class DeviceInfo
		{
			public abstract int Channels { get;}
			public abstract int MinRate { get;}
			public abstract int MaxRate { get;}
			public abstract int Latency { get;}
    
      public override string ToString()
      {
        return String.Format(
           "outputs = {0}, minrate = {1}, maxrate = {2}, latency = {3}",
           Channels,
          MinRate,
          MaxRate,
          Latency);
      }
    }
	}
}
