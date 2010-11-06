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
using Un4seen.Bass.Misc;
using MediaPortal.GUI.Library;

namespace MediaPortal.Player.PureAudio
{
	public class DSP_VizAGC : BaseDSP, IDisposable
	{
		private Thread _MonitorThread;
		private AutoResetEvent _WakeUpMonitorThread;
		private bool _MonitorThreadAbort = false;
		private float _PeakLevel = 0f;
		private float _TargetGain = 1f;
		private float _Gain = 1f;
		private bool _IsStarted = false;

		public float Gain
		{
			get
			{
				return _Gain;
			}
		}

		public DSP_VizAGC()
			: base()
		{
			Initialize();
		}
		
		public DSP_VizAGC(int channel, int priority)
			: base()
		{
			Initialize();
			this.ChannelHandle = channel;
			this.DSPPriority = priority;
		}

		~DSP_VizAGC()
		{
			Dispose();
		}

		public override void OnStarted()
		{
			base.OnStarted();
			_IsStarted = true;

			if (!_MonitorThread.IsAlive)
			{
				_MonitorThreadAbort = false;
				_MonitorThread.Start();
			}
			else
			{
				_WakeUpMonitorThread.Set();
			}
		}

		public override void OnStopped()
		{
			base.OnStopped();
			_IsStarted = false;
		}
		
		public override void OnChannelChanged()
		{
			// override this method if you need to react on channel changes
			// e.g. usefull, if an internal buffer needs to be reset etc.
		}

		public override string ToString()
		{
			return "DSP_VizAGC";
		}

		void IDisposable.Dispose()
		{
			if (_MonitorThread.IsAlive)
			{
				_MonitorThreadAbort = true;
				_WakeUpMonitorThread.Set();
				_MonitorThread.Join();
			}
		}

		private void Initialize()
		{
			_WakeUpMonitorThread = new AutoResetEvent(false);
			_MonitorThread = new Thread(new ThreadStart(MonitorThread));
			_MonitorThread.IsBackground = true;
		}

		private void MonitorThread()
		{
			while (!_MonitorThreadAbort)
			{
				// Calculate new targetgain from peaklevel
				if (_PeakLevel > 0)
				{
					_TargetGain = Math.Min(1f / _PeakLevel, 10);
					_PeakLevel = 0;
				}
				
				// Adjust gain
				if (_TargetGain > _Gain)
					_Gain += (_TargetGain - _Gain) / 30;
				else
					_Gain = _TargetGain;
				
				if (_IsStarted)
					_WakeUpMonitorThread.WaitOne(100, false);
				else
					_WakeUpMonitorThread.WaitOne();
			}
		}

    unsafe public override void DSPCallback(int handle, int channel, IntPtr buffer, int length, IntPtr user)
		{
			if (IsBypassed)
				return;

			if (ChannelBitwidth == 32)
			{
				float* data = (float*)buffer;

				// Determine peaklevel
				for (int a = 0; a < length / 4; a++)
				{
					if (data[a] > _PeakLevel)
						_PeakLevel = data[a];
				}

				// Apply gain
				for (int a = 0; a < length / 4; a++)
				{
					data[a] *= _Gain;
				}
			}
			RaiseNotification();
		}
	}
}
