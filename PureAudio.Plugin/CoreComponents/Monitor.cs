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
using System.Threading;

namespace MediaPortal.Plugins.PureAudio
{
  public partial class PureAudioPlayer
  {
    /// <summary>
    /// Player monitor. Keeps track of playback position and notifies controller if nessecary.
    /// </summary>
    private class Monitor : IDisposable
    {
      #region Static members

      /// <summary>
      /// Creates and initializes an new instance.
      /// </summary>
      /// <param name="player">Reference to containing IPlayer object.</param>
      /// <returns>The new instance.</returns>
      public static Monitor Create(PureAudioPlayer player)
      {
        Monitor monitor = new Monitor(player);
        monitor.Initialize();
        return monitor;
      }

      #endregion

      #region Fields

      // Reference to the containin IPlayer object.
      private PureAudioPlayer _Player;

      // Monitorthread.
      private Thread _MonitorThread;
      private bool _MonitorThreadAbortFlag;
      private AutoResetEvent _MonitorThreadNotify;

      // Playback progress
      private TimeSpan _Duration;
      private TimeSpan _CurrentPosition;

      private int _PlaybackSpeed;

      #endregion

      #region Public members

      public delegate void MonitorProcessDelegate();
      /// <summary>
      /// Fires periodically after each monitoring interval. Hook for additional functionality.
      /// </summary>
      public event MonitorProcessDelegate MonitorProcess;

      /// <summary>
      /// Returns the duration of the currently played track.
      /// </summary>
      public TimeSpan Duration
      {
        get { return _Duration; }
      }

      /// <summary>
      /// Returns the playback position of the currently played track.
      /// </summary>
      public TimeSpan CurrentPosition
      {
        get { return _CurrentPosition; }
      }

      /// <summary>
      /// Returns the current playbackspeed.
      /// </summary>
      public int PlaybackSpeed
      {
        get { return _PlaybackSpeed; }
      }

      /// <summary>
      /// Terminates and waits for the monitor thread.
      /// </summary>
      public void TerminateThread()
      {
        if (_MonitorThread.IsAlive)
        {
          Log.Debug("Stopping monitor thread.");

          _MonitorThreadAbortFlag = true;
          _MonitorThreadNotify.Set();
          _MonitorThread.Join();
        }
      }

      #endregion

      #region Private members

      private Monitor(PureAudioPlayer player)
      {
        _Player = player;
      }

      /// <summary>
      /// Initializes a new instance.
      /// </summary>
      private void Initialize()
      {
        _MonitorThreadAbortFlag = false;
        _MonitorThreadNotify = new AutoResetEvent(false);
        _MonitorThread = new Thread(new ThreadStart(ThreadMonitor));
        _MonitorThread.IsBackground = true;
        _MonitorThread.Name = "PureAudio.Monitor";
        _MonitorThread.Start();
      }

      /// <summary>
      /// Monitor thread loop.
      /// </summary>
      private void ThreadMonitor()
      {
        try
        {
          while (!_MonitorThreadAbortFlag)
          {
            if (IsPlaying)
            {
              _Duration = _Player._InputSourceManager.CurrentInputSourceHolder.InputSource.OutputStream.Length;
              _CurrentPosition = _Player._InputSourceManager.CurrentInputSourceHolder.InputSource.OutputStream.GetPosition();

              _PlaybackSpeed = _Player._InputSourceManager.PlaybackSpeed;
            }
            else
            {
              _Duration = TimeSpan.Zero;
              _CurrentPosition = TimeSpan.Zero;

              _PlaybackSpeed = 1;
            }

            if (MonitorProcess != null)
              MonitorProcess();

            _MonitorThreadNotify.WaitOne(200, false);
          }
        }
        catch (Exception e)
        {
          throw new BassPlayerException("Exception in monitor thread.", e);
        }
      }

      private bool IsPlaying
      {
        get
        {
          InternalPlayBackState internalState = _Player._Controller.InternalState;
          return
            internalState == InternalPlayBackState.Playing ||
            internalState == InternalPlayBackState.Pausing ||
            internalState == InternalPlayBackState.Paused ||
            internalState == InternalPlayBackState.Resuming;
        }
      }

      #endregion

      #region IDisposable Members

      public void Dispose()
      {
        Log.Debug("PlaybackBuffer.Dispose()");

        TerminateThread();
      }

      #endregion
    }
  }
}
