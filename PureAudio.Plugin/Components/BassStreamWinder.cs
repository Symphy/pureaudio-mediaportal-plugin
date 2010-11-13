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
using Un4seen.Bass.Misc;

namespace MediaPortal.Player.PureAudio
{
  public partial class BassPlayer
  {
    private class BassStreamWinder : IDisposable
    {
      #region Static members

      /// <summary>
      /// Creates and initializes an new instance.
      /// </summary>
      /// <param name="stream">Reference to BassStream object.</param>
      /// <returns>The new instance.</returns>
      public static BassStreamWinder Create(BassStream stream)
      {
        BassStreamWinder bassStreamWinder = new BassStreamWinder(stream);
        bassStreamWinder.Initialize();
        return bassStreamWinder;
      }

      #endregion

      #region Fields

      BassStream _Stream;
      int _Speed;

      private Thread _WindThread;
      private bool _WindThreadAbortFlag;
      private AutoResetEvent _WindThreadNotify;
      private TimeSpan _Interval;
      private DSP_Gain _GainDSP;

      #endregion

      #region Public members

      public int PlaybackSpeed
      {
        get { return _Speed; }
        set
        {
          _Speed = value;
          _WindThreadNotify.Set();
        }
      }

      #endregion

      #region Private members

      private BassStreamWinder(BassStream stream)
      {
        _Stream = stream;
      }

      /// <summary>
      /// Initializes a new instance.
      /// </summary>
      private void Initialize()
      {
        _GainDSP = new DSP_Gain();
        _GainDSP.ChannelHandle = _Stream.Handle;
        _GainDSP.Gain_dBV = -10;

        _Interval = TimeSpan.FromMilliseconds(50);
        _Speed = 1;

        _WindThreadAbortFlag = false;
        _WindThreadNotify = new AutoResetEvent(false);
        _WindThread = new Thread(new ThreadStart(ThreadWind));
        _WindThread.IsBackground = true;
        _WindThread.Name = "PureAudio.BassStreamWinder";
        _WindThread.Start();
      }

      /// <summary>
      /// Terminates and waits for the controller thread.
      /// </summary>
      public void TerminateThread()
      {
        if (_WindThread.IsAlive)
        {
          Log.Debug("BassStreamWinder: Stopping thread");

          _WindThreadAbortFlag = true;
          _WindThreadNotify.Set();
          _WindThread.Join();
        }
      }

      private void ThreadWind()
      {
        try
        {
          while (!_WindThreadAbortFlag)
          {
            if (_Speed == 1)
            {
              _GainDSP.Stop();
              _WindThreadNotify.WaitOne();
            }
            else
            {
              _GainDSP.Start();

              TimeSpan newPosition = _Stream.GetPosition();

              if (_Speed < 1)
                newPosition = newPosition.Add(-_Interval);

              newPosition = newPosition.Add(TimeSpan.FromMilliseconds(_Interval.TotalMilliseconds * _Speed));

              if (newPosition >= TimeSpan.Zero && newPosition < _Stream.Length)
                _Stream.SetPosition(newPosition);

              _WindThreadNotify.WaitOne((int)_Interval.TotalMilliseconds, false);
            }
          }
        }
        catch (Exception e)
        {
          throw new BassPlayerException("Exception in winder thread.", e);
        }
      }

      #endregion


      #region IDisposable Members

      public void Dispose()
      {
        Log.Debug("BassStreamWinder.Dispose()");

        TerminateThread();

        _GainDSP.Dispose();
        _GainDSP = null;
      }

      #endregion
    }
  }
}