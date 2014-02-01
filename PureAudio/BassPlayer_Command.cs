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

namespace MediaPortal.Player.PureAudio
{
	public partial class BASSPlayer
	{
		private class Command
		{
			private Delegate _method;
			private object _arg1 = null;
			private object _arg2 = null;
			private object _result = null;
			ManualResetEvent _event = new ManualResetEvent(false);

			public WaitHandle WaitHandle
			{
				get
				{
					return _event;
				}
			}

			public bool Result
			{
				get
				{
					if (_result == null)
						return false;
					else
						return (bool)_result;
				}
			}

			public Command(Delegate method)
			{
				_method = method;
			}

			public Command(Delegate method, object arg)
			{
				_method = method;
				_arg1 = arg;
			}

			public Command(Delegate method, object arg1, object arg2)
			{
				_method = method;
				_arg1 = arg1;
				_arg2 = arg2;
			}

			internal void Invoke()
			{
				if (_arg2 != null)
				{
					_result = _method.DynamicInvoke(new object[2] { _arg1, _arg2 });
				}
				else if (_arg1 != null)
				{
					_result = _method.DynamicInvoke(new object[1] { _arg1 });
				}
				else
				{
					_result = _method.DynamicInvoke(null);
				}
				_event.Set();
			}
		}
	}
}
