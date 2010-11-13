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
using MediaPortal.Core;

namespace MediaPortal.Player.PureAudio
{
  public partial class BassPlayer
  {
    /// <summary>
    /// Player controller.
    /// </summary>
    private partial class Controller : IDisposable
    {
      #region Static members

      /// <summary>
      /// Creates and initializes an new instance.
      /// </summary>
      /// <param name="player">Reference to containing IPlayer object.</param>
      /// <returns>The new instance.</returns>
      public static Controller Create(BassPlayer player)
      {
        Controller controller = new Controller(player);
        controller.Initialize();
        return controller;
      }

      #endregion

      #region Fields

      // Reference to the containin IPlayer object.
      private BassPlayer _Player;

      // The current external playback state.
      private PlaybackState _ExternalState;

      // The current internal playback state.
      private InternalPlayBackState _InternalState;

      // The current playback mode.
      private PlaybackMode _PlaybackMode;

      // Mainthread that controls the player.
      private Thread _MainThread;
      private bool _MainThreadAbortFlag;
      private AutoResetEvent _MainThreadNotify;

      // Work item queue
      private WorkItemQueue _WorkItemQueue;

      #endregion

      #region Delegates

      // Delegates used in conjunction with workitems.
      private delegate void PlayWorkItemDelegate(IMediaItem mediaItem);
      private delegate void SeekWorkItemDelegate(TimeSpan interval);
      private delegate void WorkItemDelegate();

      #endregion

      #region IDisposable Members

      public void Dispose()
      {
        Log.Debug("Controller.Dispose()");

        TerminateThread();
      }

      #endregion

      #region Public members

      public delegate void WaitStartedDelegate();
      public event WaitStartedDelegate WaitStarted;

      public delegate void WaitEndedDelegate();
      public event WaitEndedDelegate WaitEnded;

      /// <summary>
      /// Returns the current external playback state. 
      /// </summary>
      /// <remarks>
      /// Because the player operates asynchronous, its internal state 
      /// can differ from its external state as seen by MP.
      /// </remarks>
      public PlaybackState ExternalState
      {
        get { return _ExternalState; }
      }

      /// <summary>
      /// Gets the current internal playback state. 
      /// </summary>
      /// <remarks>
      /// Because the player operates asynchronous, its internal state 
      /// can differ from its external state as seen by MP.
      /// </remarks>
      public InternalPlayBackState InternalState
      {
        get { return _InternalState; }
      }

      /// <summary>
      /// Gets the current playbackmode.
      /// </summary>
      public PlaybackMode PlaybackMode
      {
        get { return _PlaybackMode; }
      }

      /// <summary>
      /// Subscribe to events
      /// </summary>
      public void WireUp()
      {
        _Player._OutputDeviceManager.OutputStreamEnded += new EventHandler(_OutputDeviceManager_OutputStreamEnded);
      }

      /// <summary>
      /// Enqueues a play workitem for the given mediaitem.
      /// </summary>
      /// <param name="mediaItem"></param>
      /// <remarks>
      /// The workitem will actually be executed on the controller's mainthread.
      /// </remarks>
      public void Play(IMediaItem mediaItem, bool wait)
      {
        if (_ExternalState == PlaybackState.Stopped || _ExternalState == PlaybackState.Ended)
        {
          WorkItem workItem = EnQueueWorkItem(new WorkItem(new PlayWorkItemDelegate(InternalPlay), mediaItem));
          if (wait)
            workItem.WaitHandle.WaitOne();

          _ExternalState = PlaybackState.Playing;
        }
      }

      /// <summary>
      /// Enqueues a pause workitem.
      /// </summary>
      /// <remarks>
      /// The workitem will actually be executed on the controller's mainthread.
      /// </remarks>
      public void Pause()
      {
        if (_ExternalState == PlaybackState.Playing)
        {
          EnQueueWorkItem(new WorkItem(new WorkItemDelegate(InternalPause)));
          _ExternalState = PlaybackState.Paused;
        }
      }

      /// <summary>
      /// Enqueues a resume workitem.
      /// </summary>
      /// <remarks>
      /// The workitem will actually be executed on the controller's mainthread.
      /// </remarks>
      public void Resume()
      {
        if (_ExternalState == PlaybackState.Paused)
        {
          EnQueueWorkItem(new WorkItem(new WorkItemDelegate(InternalResume)));
          _ExternalState = PlaybackState.Playing;
        }
      }

      /// <summary>
      /// Enqueues a wind workitem.
      /// </summary>
      /// <remarks>
      /// The workitem will actually be executed on the controller's mainthread.
      /// </remarks>
      public void Wind()
      {
        if (_ExternalState == PlaybackState.Playing)
        {
          EnQueueWorkItem(new WorkItem(new WorkItemDelegate(InternalWind)));
        }
      }

      /// <summary>
      /// Enqueues a wind workitem.
      /// </summary>
      /// <remarks>
      /// The workitem will actually be executed on the controller's mainthread.
      /// </remarks>
      public void Rewind()
      {
        if (_ExternalState == PlaybackState.Playing)
        {
          EnQueueWorkItem(new WorkItem(new WorkItemDelegate(InternalRewind)));
        }
      }

      /// <summary>
      /// Enqueues a stop workitem.
      /// </summary>
      /// <remarks>
      /// The workitem will actually be executed on the controller's mainthread.
      /// </remarks>
      /// <param name="wait"></param>
      public void Stop(bool wait)
      {
        // Make sure nothing happens here if _ExternalState == PlaybackState.Ended!

        if (_ExternalState == PlaybackState.Playing || _ExternalState == PlaybackState.Paused)
        {
          WorkItem workItem = EnQueueWorkItem(new WorkItem(new WorkItemDelegate(InternalStop)));
          if (wait)
            workItem.WaitHandle.WaitOne();

          _ExternalState = PlaybackState.Stopped;
        }
      }

      /// <summary>
      /// Enqueues a workitem to change the current playbackmode.
      /// </summary>
      /// <remarks>
      /// The workitem will actually be executed on the controller's mainthread.
      /// </remarks>
      public void TogglePlayBackMode()
      {
        WorkItem workItem = EnQueueWorkItem(new WorkItem(new WorkItemDelegate(InternalTogglePlayBackMode)));
        workItem.WaitHandle.WaitOne();
      }

      /// <summary>
      /// Handle the event when a new mediaitem is required to prepare crossfading or gapless playback.
      /// </summary>
      public void HandleNextMediaItemSyncPoint()
      {
        EnQueueWorkItem(new WorkItem(new WorkItemDelegate(RequestNextMediaItem)));
      }

      /// <summary>
      /// Start crossfading.
      /// </summary>
      public void HandleCrossFadeSyncPoint()
      {
        EnQueueWorkItem(new WorkItem(new WorkItemDelegate(StartCrossFade)));
      }

      /// <summary>
      /// Raises the WaitStarted event.
      /// </summary>
      public void OnWaitStarted()
      {
        if (WaitStarted != null)
          WaitStarted();
      }

      /// <summary>
      /// Raises the WaitEnded event.
      /// </summary>
      public void OnWaitEnded()
      {
        if (WaitEnded != null)
          WaitEnded();
      }

      #endregion

      #region Private members

      private Controller(BassPlayer player)
      {
        _Player = player;
      }

      /// <summary>
      /// Initializes a new instance.
      /// </summary>
      private void Initialize()
      {
        _ExternalState = PlaybackState.Ended;
        _InternalState = InternalPlayBackState.Stopped;
        _MainThreadAbortFlag = false;

        _WorkItemQueue = new WorkItemQueue();

        _MainThreadNotify = new AutoResetEvent(false);
        _MainThread = new Thread(new ThreadStart(ThreadMain));
        _MainThread.Name = "PureAudio.Controller";
        _MainThread.Start();
      }

      /// <summary>
      /// Enqueues a workitem and notifies the controller mainthread something needs to be done.
      /// </summary>
      /// <param name="workItem">The workitem to enqueue.</param>
      /// <returns>The enqueued workitem.</returns>
      private WorkItem EnQueueWorkItem(WorkItem workItem)
      {
        _WorkItemQueue.Enqueue(workItem);
        _MainThreadNotify.Set();

        return workItem;
      }

      /// <summary>
      /// Enqueues the given mediaitem for playback and starts playback if nessecary.
      /// </summary>
      /// <param name="mediaItem">The mediaitem to play.</param>
      private void InternalPlay(IMediaItem mediaItem)
      {
        Log.Info("Preparing for playback: \"{0}\"", mediaItem.ContentUri);
        InputSourceHolder inputSource = _Player._InputSourceFactory.CreateInputSource(mediaItem);
        _Player._InputSourceQueue.Enqueue(inputSource);

        if (_InternalState != InternalPlayBackState.Playing && _InternalState != InternalPlayBackState.Paused)
        {
          _InternalState = InternalPlayBackState.Initializing;

          _Player._PlaybackSession = PlaybackSession.Create(_Player, inputSource.OutputStream.Channels, inputSource.OutputStream.SampleRate, inputSource.OutputStream.IsPassThrough);

          _InternalState = InternalPlayBackState.Playing;
        }
      }

      /// <summary>
      /// Pauses playback.
      /// </summary>
      private void InternalPause()
      {
        if (_InternalState == InternalPlayBackState.Playing)
        {
          _InternalState = InternalPlayBackState.Pausing;
          _Player._InputSourceManager.PlaybackSpeed = 1;
          _Player._OutputDeviceManager.StopDevice();
          _InternalState = InternalPlayBackState.Paused;
        }
      }

      /// <summary>
      /// Resumes playback from pause.
      /// </summary>
      private void InternalResume()
      {
        if (_InternalState == InternalPlayBackState.Paused)
        {
          _InternalState = InternalPlayBackState.Resuming;
          _Player._InputSourceManager.PlaybackSpeed = 1;
          _Player._OutputDeviceManager.StartDevice(true);
          _InternalState = InternalPlayBackState.Playing;
        }
      }

      /// <summary>
      /// Start winding or increases winding speed.
      /// </summary>
      private void InternalWind()
      {
        if (_InternalState == InternalPlayBackState.Playing)
        {
          int speed = _Player._InputSourceManager.PlaybackSpeed;
          if (speed < InternalSettings.MaxWindingSpeed)
          {
            if (speed < -InternalSettings.WindingSpeedIncrement)
              speed /= InternalSettings.WindingSpeedIncrement;
            else if (speed == -InternalSettings.WindingSpeedIncrement)
              speed /= -InternalSettings.WindingSpeedIncrement;
            else
              speed *= InternalSettings.WindingSpeedIncrement;
          }
          _Player._InputSourceManager.PlaybackSpeed = speed;
        }
      }

      /// <summary>
      /// Start rewinding or decreases winding speed.
      /// </summary>
      private void InternalRewind()
      {
        if (_InternalState == InternalPlayBackState.Playing)
        {
          int speed = _Player._InputSourceManager.PlaybackSpeed;
          if (speed > -InternalSettings.MaxWindingSpeed)
          {
            if (speed > 1)
              speed /= InternalSettings.WindingSpeedIncrement;
            else if (speed == 1)
              speed *= -InternalSettings.WindingSpeedIncrement;
            else
              speed *= InternalSettings.WindingSpeedIncrement;
          }

          _Player._InputSourceManager.PlaybackSpeed = speed;

        }
      }

      /// <summary>
      /// Stops playback.
      /// </summary>
      private void InternalStop()
      {
        if (_InternalState == InternalPlayBackState.Playing || _InternalState == InternalPlayBackState.Paused)
        {
          _InternalState = InternalPlayBackState.Stopping;

          _Player._PlaybackSession.End();
          _Player._PlaybackSession = null;

          _InternalState = InternalPlayBackState.Stopped;
        }
      }

      /// <summary>
      /// Toggles the current playback mode.
      /// </summary>
      private void InternalTogglePlayBackMode()
      {
      }

      /// <summary>
      /// Request MP to give us the next track on the playlist.
      /// </summary>
      private void RequestNextMediaItem()
      {
        if (_ExternalState == PlaybackState.Playing)
        {
          // Just make MP come up with the next item on its playlist. 
          _ExternalState = PlaybackState.Ended;
        }
      }

      /// <summary>
      /// Take actions required when its time to crossfade.
      /// </summary>
      private void StartCrossFade()
      {
        if (_PlaybackMode == PlaybackMode.CrossFading)
        {
          // Todo: tell _Player._InputSourceManager to start crossfade.
        }
      }

      /// <summary>
      /// Main controllerthread loop.
      /// </summary>
      private void ThreadMain()
      {
        try
        {
          while (!_MainThreadAbortFlag)
          {
            if (_WorkItemQueue.Count == 0)
              _MainThreadNotify.WaitOne();

            while (_WorkItemQueue.Count > 0)
            {
              WorkItem workItem = _WorkItemQueue.Dequeue();
              workItem.Invoke();
            }
          }
          // Make sure player is stopped
          InternalStop();
        }
        catch (Exception e)
        {
          throw new BassPlayerException("Exception in controller mainthread.", e);
        }
      }

      /// <summary>
      /// Terminates and waits for the controller thread.
      /// </summary>
      public void TerminateThread()
      {
        if (_MainThread.IsAlive)
        {
          Log.Debug("Stopping controller thread.");

          _MainThreadAbortFlag = true;
          _MainThreadNotify.Set();
          _MainThread.Join();
        }
      }

      #endregion

      #region Eventhandlers
      
      void _OutputDeviceManager_OutputStreamEnded(object sender, EventArgs e)
      {
        // When we get here an ended msg is already send to MP (through HandleNextMediaItemSyncPoint).
        // All we have to do is stop playback that only runs in the background.
        EnQueueWorkItem(new WorkItem(new WorkItemDelegate(InternalStop)));
      }
      
      #endregion

    }
  }
}
