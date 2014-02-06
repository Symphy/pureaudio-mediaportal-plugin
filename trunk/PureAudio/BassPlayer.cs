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
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using Un4seen.Bass;
using Un4seen.BassWasapi;
using Un4seen.Bass.AddOn.Cd;
using Un4seen.Bass.AddOn.Mix;
using Un4seen.Bass.AddOn.Vst;
using Un4seen.Bass.AddOn.WaDsp;
using Un4seen.Bass.AddOn.Tags;
using Un4seen.Bass.Misc;
using BlueWave.Interop.Asio;
using MediaPortal.Player.PureAudio.Asio;
using System.IO;

namespace MediaPortal.Player.PureAudio
{
  public partial class BASSPlayer
  {
    #region Constants

    private const int VizLatencyCorrectionRangeMS = 500;
    private const int VizAGCPeakDetectionIntervalMS = 1000;
    private const int VizAGCMaxGaindBV = 20;

    #endregion

    #region Fields

    private Thread _MainThread;
    private Thread _MonitorThread;
    private Thread _BufferUpdateThread;

    private AutoResetEvent _WakeupMainThread;
    private AutoResetEvent _WakeupMonitorThread;
    private AutoResetEvent _WakeupBufferUpdateThread;
    private ManualResetEvent _BufferUpdated;

    private bool _Initialized = false;
    private bool _MainThreadAbortFlag = false;
    private bool _MonitorThreadAbortFlag = false;
    private bool _BufferUpdateThreadAbortFlag = false;

    private delegate void PauseDelegate(bool pause);
    private delegate bool PlayDelegate(string filePath, bool isAsync);
    private delegate void SeekDelegate(int seconds);
    private delegate void StopDelegate();
    private delegate void ToggleModeDelegate();
    private delegate void TogglePauseDelegate();

    private SYNCPROC _MetaTagSyncProcDelegate = null;
    private STREAMPROC _StreamWriteProcDelegate = null;
    private STREAMPROC _VizRawStreamWriteProcDelegate = null;
    private STREAMPROC _DSOutputStreamWriteProcDelegate = null;
    private WASAPIPROC _WasapiProcDelegate = null;

    private int _CurrentStream = 0;
    private int _DataStream = 0;
    private int _MixerStream = 0;
    private int _NextDataStream = 0;
    private int _ASIOOutputStream = 0;
    private int _DSOutputStream = 0;
    private int _WASAPIOutputStream = 0;
    private int _SinkStream = 0;
    private int _VizRawStream = 0;
    private int _VizStream = 0;

    private byte[] _ReadData = new byte[1];
    private byte[] _VizStreamSilence = new byte[1];

    private DSP_Gain _ReplayGainDSP = null;
    private ReplayGainInfo _CurrentReplayGainInfo = new ReplayGainInfo();

    private DSP_VizAGC _VizAGC = null;

    private int _VSTDelayMS = 0;
    private Dictionary<int, BASS_VST_INFO> _VSTHandles = new Dictionary<int, BASS_VST_INFO>();

    private Dictionary<int, string> _WADSPHandles = new Dictionary<int, string>();

    private List<int> _DecoderPluginHandles = new List<int>();
    private bool _DecoderPluginsLoaded = false;

    private float _Duration = 0;
    private float _CurrentPosition = 0;
    private string _CurrentFilePath = String.Empty;
    private FileType _CurrentFileType = new FileType();
    private StreamContentType _CurrentContentType = StreamContentType.PCM;
    private CurrentStreamInfo _DataStreamInfo;

    private bool _BufferEmptySignaled = false;
    private bool _DataStreamEndSignaled = false;
    private bool _GapPlayed = false;
    private bool _InitBASS = true;
    private bool _NewSessionPending = false;
    private bool _PlayingFromBuffer = false;
    private PlayState _State = PlayState.Stopped;

    private int _ASIODeviceNumber = -1;
    private AsioEngine _ASIOEngine = null;

    private int _WASAPIDeviceNumber = -1;

    private AudioRingBuffer _Buffer = null;
    private int _BufferUpdateThreshold = 90;
    private int _BufferUpdateThresholdBytes = 0;
    private bool _DebugMode = false;
    private DeviceInfo _DeviceInfo = null;
    private int _GapByteLength = 0;
    private int _GapBytesLeft = 0;
    private int _LastDeviceInfoDevice = 0;
    private LastErrorInfo _LastError = new LastErrorInfo();
    private long _LastReadMilliSecs = 0;
    private PlayBackMode _PlayBackMode = (PlayBackMode)ConfigProfile.Defaults.DefaultPlayBackMode;
    private string _PrevMetaTag = String.Empty;
    private ConfigProfile _Profile = new ConfigProfile();
    private int _ReadOffsetBytes = 0;
    private int _ReadOffsetMS = 0;
    private Queue<Command> _RequestQueue = new Queue<Command>();
    private Stopwatch _StopWatch = new Stopwatch();
    private int _TotalLatencyMS = 0;
    private int _VizReadOffsetBytes = 0;
    private int _VizReadOffsetMS = 0;
    private MediaPortal.GUI.Library.WaitCursor _WaitCursor = null;

    private int[] SamplingRates = new int[] {
			8000,
			9600,
			11025,
			12000,
			16000,
			22050,
			24000,
			32000,
			44100,
			48000,
			88200,
			96000,
			176400,
			192000
		};

    #endregion

    #region Public interface

    public delegate void StreamTagsChangedDelegate(object sender, string[] tags);
    public event StreamTagsChangedDelegate StreamTagsChanged;

    public delegate void MetaStreamTagsChangedDelegate(object sender, string tags);
    public event MetaStreamTagsChangedDelegate MetaStreamTagsChanged;

    public delegate void EndedDelegate();
    public event EndedDelegate Ended;

    public delegate void StoppedDelegate();
    public event StoppedDelegate Stopped;

    public delegate void SessionStartedDelegate();
    public event SessionStartedDelegate SessionStarted;

    public delegate void SessionStoppedDelegate();
    public event SessionStoppedDelegate SessionStopped;

    public delegate void MonitorProcessDelegate();
    public event MonitorProcessDelegate MonitorProcess;

    public string CurrentFilePath
    {
      get { return _CurrentFilePath; }
    }

    public FileType CurrentFileType
    {
      get { return _CurrentFileType; }
    }

    public float CurrentPosition
    {
      get { return _CurrentPosition; }
    }

    public bool DebugMode
    {
      get { return _DebugMode; }
      set { _DebugMode = value; }
    }

    public LastErrorInfo LastError
    {
      get { return _LastError; }
    }

    public PlayBackMode PlayBackMode
    {
      get { return _PlayBackMode; }
    }

    public ConfigProfile Profile
    {
      get { return _Profile; }
    }

    public AsioEngine AsioEngine
    {
      get { return _ASIOEngine; }
    }

    public float Duration
    {
      get { return _Duration; }
    }

    public BASSPlayer()
    {
      _VizAGC = new DSP_VizAGC();
      _ReplayGainDSP = new DSP_Gain();
      _ASIOEngine = new AsioEngine();

      _StreamWriteProcDelegate = new STREAMPROC(StreamWriteProc);
      _VizRawStreamWriteProcDelegate = new STREAMPROC(VizRawStreamWriteProc);
      _DSOutputStreamWriteProcDelegate = new STREAMPROC(DSOutputStreamWriteProc);
      _MetaTagSyncProcDelegate = new SYNCPROC(StreamMetaTagSyncProc);
      _WasapiProcDelegate = new WASAPIPROC(WasApiProc);

      // Make sure threads are stopped in case Dispose does not get called.
      Application.ApplicationExit += new EventHandler(Application_ApplicationExit);

      _WakeupMainThread = new AutoResetEvent(false);
      _WakeupMonitorThread = new AutoResetEvent(false);
      _WakeupBufferUpdateThread = new AutoResetEvent(false);
      _BufferUpdated = new ManualResetEvent(false);

      _MainThread = new Thread(new ThreadStart(ThreadMain));
      _MonitorThread = new Thread(new ThreadStart(ThreadMonitor));
      _BufferUpdateThread = new Thread(new ThreadStart(ThreadBufferUpdate));

      _MainThread.Name = "PureAudio.Main";
      _MainThread.Start();

      _BufferUpdateThread.Name = "PureAudio.BufferUpdate";
      _BufferUpdateThread.IsBackground = true;
      _BufferUpdateThread.Start();

      _MonitorThread.Name = "PureAudio.Monitor";
      _MonitorThread.IsBackground = true;
      _MonitorThread.Start();
    }

    public bool Play(string filePath)
    {
      return Play(filePath, false);
    }

    public bool Play(string filePath, bool wait)
    {
      Command command = EnQueueCommand(new Command(new PlayDelegate(InternalPlay), filePath, !wait));
      if (wait)
      {
        command.WaitHandle.WaitOne();
        return command.Result;
      }
      else
      {
        return true;
      }
    }

    public void Pause()
    {
      Command command = EnQueueCommand(new Command(new TogglePauseDelegate(InternalPause)));
    }

    public void Pause(bool pause)
    {
      Command command = EnQueueCommand(new Command(new PauseDelegate(InternalPause), pause));
    }

    public void Stop()
    {
      Stop(false);
    }

    public void Stop(bool wait)
    {
      Command command = EnQueueCommand(new Command(new StopDelegate(InternalStop)));
      if (wait)
        command.WaitHandle.WaitOne();
    }

    public void Forward()
    {
      Command command = EnQueueCommand(new Command(new SeekDelegate(InternalSeek), _Profile.SeekIncrement));
    }

    public void Rewind()
    {
      Command command = EnQueueCommand(new Command(new SeekDelegate(InternalSeek), -_Profile.SeekIncrement));
    }

    public void Seek(int seconds)
    {
      Command command = EnQueueCommand(new Command(new SeekDelegate(InternalSeek), seconds));
    }

    public void TogglePlayBackMode()
    {
      Command command = EnQueueCommand(new Command(new ToggleModeDelegate(InternalTogglePlayBackMode)));
      command.WaitHandle.WaitOne();
    }

    public void SetVizLatencyCorrection(int correction)
    {
      _VizReadOffsetMS = VizLatencyCorrectionRangeMS - correction - _VSTDelayMS;

      Log.Debug("Viz-stream pre-reading offset: {0} ms...", _VizReadOffsetMS);

      int channels = _DataStreamInfo.BASSChannelInfo.chans;
      int samplingRate = _DataStreamInfo.BASSChannelInfo.freq;
      _VizReadOffsetBytes = AudioRingBuffer.CalculateLength(samplingRate, channels, _VizReadOffsetMS);
    }

    public bool Initialize()
    {
      bool result = true;
      if (!_Initialized)
      {
        LoadSettings();
        result = InitBASS();
        _Initialized = true;
      }
      return result;
    }

    public int GetCurrentVizStream()
    {
      return _VizStream;
    }

    public void Dispose()
    {
      StopThreads();

      BassWasapi.BASS_WASAPI_Free();

      if (_ASIOEngine != null)
        _ASIOEngine.Dispose();

      BassWaDsp.BASS_WADSP_Free();

      UnLoadAudioDecoderPlugins();

      Bass.BASS_Free();
    }

    #endregion

    private bool CurrentNeedsPassThrough
    {
      get
      {
        return (_CurrentContentType != StreamContentType.PCM);
      }
    }

    private bool CurrentUsesOverSampling
    {
      get
      {
        if (CurrentNeedsPassThrough)
          return false;
        else
          return _Profile.UseOverSampling;
      }
    }

    private bool CurrentDoesSoftStop
    {
      get
      {
        if (CurrentNeedsPassThrough)
          return false;
        else
          return _Profile.DoSoftStop;
      }
    }

    private Command EnQueueCommand(Command command)
    {
      _RequestQueue.Enqueue(command);
      _WakeupMainThread.Set();

      return command;
    }

    private void ThreadMain()
    {
      try
      {
        while (!_MainThreadAbortFlag)
        {
          if (_RequestQueue.Count == 0)
          {
            _WakeupMainThread.WaitOne();
          }

          if (_RequestQueue.Count > 0)
          {
            Command request = _RequestQueue.Dequeue();
            request.Invoke();
          }

          if (_State == PlayState.Playing)
          {
            if (!_BufferEmptySignaled)
            {
              if (!_PlayingFromBuffer)
              {
                if (_DataStreamEndSignaled)
                {
                  Log.Debug("Raising 'Ended' event...");
                  _PlayingFromBuffer = true;

                  if (Ended != null)
                    Ended();
                }
              }
              else
              {
                if (_NextDataStream != 0)
                {
                  Log.Debug("Switching to queued stream...");

                  if (IsNewSessionRequired())
                  {
                    // The next stream has a different sampling rate or number of channels:
                    // the player must be stopped, re-initialized and restarted.

                    _NewSessionPending = true;
                    Log.Debug("New session pending...");
                  }
                  else
                  {
                    _DataStreamEndSignaled = false;
                    _PlayingFromBuffer = false;
                  }

                  // Make sure _DataStream never points to an non-existing stream

                  int oldStream = _DataStream;
                  _DataStream = _NextDataStream;
                  _NextDataStream = 0;
                  Bass.BASS_StreamFree(oldStream);

                  SetReplayGain();
                }
              }
            }
            else //if (_BufferEmptySignaled)
            {
              if (!_NewSessionPending)
              {
                // At this point the buffer is read empty and no new stream 
                // is queued. We can stop now.
                StopSession(true, false);

                if (Stopped != null)
                {
                  Stopped();
                }
              }
              else
              {
                // At this point the buffer is read empty but a new stream 
                // is queued with a different sample rate or number of channels.
                Restart();
              }
            }
          }
        }
      }
      catch (Exception e)
      {
        HandleError(ErrorCode.MiscError, "Exception in mainthread: {0}", e.ToString());
      }
    }

    private void ThreadMonitor()
    {
      try
      {
        while (!_MonitorThreadAbortFlag)
        {
          _WakeupMonitorThread.WaitOne(100, false);

          if (_State != PlayState.Stopped && _DataStream != 0)
          {
            _Duration = (float)Bass.BASS_ChannelBytes2Seconds(
                _DataStream,
                Bass.BASS_ChannelGetLength(_DataStream));

            float position = (float)Bass.BASS_ChannelBytes2Seconds(
                _DataStream,
                Bass.BASS_ChannelGetPosition(_DataStream));

            position -= _TotalLatencyMS / 1000;

            _CurrentPosition = Math.Max(position, 0);
          }
          else
          {
            _Duration = 0;
            _CurrentPosition = 0;
          }

          if (MonitorProcess != null)
            MonitorProcess();
        }
      }
      catch (Exception e)
      {
        HandleError(ErrorCode.MiscError, "Exception in monitorthread: {0}", e.ToString());
      }
    }

    private void ThreadBufferUpdate()
    {
      try
      {
        while (!_BufferUpdateThreadAbortFlag)
        {
          _WakeupBufferUpdateThread.WaitOne();
          if (_State == PlayState.Buffering || _State == PlayState.Playing)
          {
            while (!_DataStreamEndSignaled && _Buffer.Space >= _BufferUpdateThresholdBytes)
            {
              if (UpdateBuffer() == -1)
              {
                Log.Debug("Data stream end signaled...");

                _DataStreamEndSignaled = true;
                _WakeupMainThread.Set();
              }
              else
              {
                Thread.Sleep(10);
              }
            }
            _BufferUpdated.Set();
          }
        }
      }
      catch (Exception e)
      {
        HandleError(ErrorCode.MiscError, "Exception in bufferupdatethread: {0}", e.ToString());
      }
    }

    private bool InternalPlay(string filePath, bool isAsync)
    {
      Log.Debug("BASSPlayer.InternalPlay(\"{0}\", {1}) called...", filePath, isAsync);

      _CurrentFilePath = filePath;
      bool result = InitBASS();

      if (result)
      {
        _CurrentFileType = Utils.GetFileType(filePath);
        Log.Info(String.Format("Determined file type: {0}", _CurrentFileType));

        CreateCurrentStream();
        result = (_CurrentStream != 0);
      }

      if (result)
      {
        GetReplayGainData();
      }

      if (result)
      {
        _CurrentContentType = EncodedStreamHelper.GetStreamContentType(_CurrentStream);
        Log.Info(String.Format("Determined stream content type: {0}", _CurrentContentType));

        if (_CurrentFileType.FileMainType == FileMainType.WebStream)
        {
          // Get the Tags and set the Meta Tag SyncProc
          GetStreamTags(_CurrentStream);
          _PrevMetaTag = "";
          GetStreamMetaTags(Bass.BASS_ChannelGetTags(_CurrentStream, BASSTag.BASS_TAG_META));
          Bass.BASS_ChannelSetSync(_CurrentStream, BASSSync.BASS_SYNC_META, 0, _MetaTagSyncProcDelegate, IntPtr.Zero);
        }

        if (_State == PlayState.Playing || _State == PlayState.Paused)
        {
          if (!_PlayingFromBuffer)
          {
            //Normally we won't get here; MP will call Stop()
            FadeOut();
            result = StopSession(false, true);

            if (result)
            {
              _DataStream = _CurrentStream;
              result = StartSession();
            }
          }
          else
          {
            Log.Debug("Queueing stream...");
            _NextDataStream = _CurrentStream;
          }
        }
        else if (_State == PlayState.Stopped)
        {
          _DataStream = _CurrentStream;
          result = StartSession();
        }
      }

      if (_WaitCursor != null)
      {
        _WaitCursor.Dispose();
        _WaitCursor = null;
      }

      if (isAsync && !result)
      {
        // If there is a problem with the file, and we're not playing a CD
        // make MP try the next item in the playlist
        // When playing CD, most likely cause is CD has been ejected.
        // In other cases, stop alltogether
        if (_LastError.ErrorCode == ErrorCode.FileError && _CurrentFileType.FileMainType != FileMainType.CDTrack)
        {
          if (Ended != null)
            Ended();
        }
        else
        {
          if (_State != PlayState.Stopped)
          {
            FadeOut();
            StopSession();
          }

          if (Stopped != null)
            Stopped();
        }
      }

      return result;
    }

    private void InternalPause()
    {
      InternalPause(_State == PlayState.Playing);
    }

    private void InternalPause(bool pause)
    {
      Log.Debug("BASSPlayer.InternalPause({0}) called...", pause.ToString());

      if (pause && _State == PlayState.Playing)
      {
        _State = PlayState.Paused;
        FadeOut();
        StopBass();

      }
      else if (!pause && _State == PlayState.Paused)
      {
        if (StartBass())
          FadeIn();

        _State = PlayState.Playing;
      }
    }

    private void InternalStop()
    {
      Log.Debug("BASSPlayer.InternalStop() called...");

      if (!_PlayingFromBuffer && _State != PlayState.Stopped)
      {
        FadeOut();
        StopSession();
      }
    }

    private void InternalSeek(int seconds)
    {
      Log.Debug("BASSPlayer.InternalSeek() called...");

      if (_DataStream != 0 && _State == PlayState.Playing && _CurrentFileType.FileMainType != FileMainType.WebStream)
      {
        FadeOut();
        if (StopBass())
        {
          float totalLatency = _TotalLatencyMS / 1000;

          // Determine maximum position we can forward to
          float max = (float)Bass.BASS_ChannelBytes2Seconds(
              _DataStream,
              Bass.BASS_ChannelGetLength(_DataStream));

          max -= totalLatency;

          // For stable operation, limit the maximum position to 5 seconds + buffersize
          // before the end of the track.
          max -= 5;
          if (max < 0)
            max = 0;

          float currentPos = (float)Bass.BASS_ChannelBytes2Seconds(
              _DataStream,
              Bass.BASS_ChannelGetPosition(_DataStream));

          float newPos = currentPos + Convert.ToSingle(seconds) - totalLatency;
          if (newPos > max)
            newPos = max;
          else if (newPos < 0)
            newPos = 0;

          if (newPos != currentPos)
          {
            Bass.BASS_ChannelSetPosition(
                _DataStream,
                newPos);

            // If encoded, position stream at the beginning of the next frame.
            EncodedStreamHelper.Sync(_DataStream, _CurrentContentType);

            // Reset the ringbuffer, to eliminate the delay
            ClearPlayBackBuffer();

            // Clear any VST plugin buffers
            ClearVSTBuffers();

            // Reset the BASS playbackbuffer, to eliminate the delay
            ClearBASSPlayBackBuffer();

            // Re-populate the buffers, so playback can continue.
            PreBuffer();
          }

          if (StartBass())
            FadeIn();
        }
      }
    }

    private void InternalTogglePlayBackMode()
    {
      if (_State == PlayState.Playing)
      {
        switch (_PlayBackMode)
        {
          case PlayBackMode.Normal:
            _PlayBackMode = PlayBackMode.Gapless;
            break;
          case PlayBackMode.Gapless:
            _PlayBackMode = PlayBackMode.Normal;
            break;
          default:
            break;
        }
      }
    }

    private bool Restart()
    {
      return StopSession(false, true) && StartSession();
    }

    private bool StartSession()
    {
      Log.Debug("Starting session...");

      _WakeupMonitorThread.Set();

      bool result = (_State == PlayState.Stopped);
      if (result)
      {
        result = (_DataStream != 0);
        if (!result)
          HandleError(ErrorCode.MiscError, "Cannot start session: No datastream.");
      }

      if (result)
      {
        ResetSessionState();
        _DataStreamInfo.BASSChannelInfo = Bass.BASS_ChannelGetInfo(_DataStream);
        _DataStreamInfo.PassThrough = CurrentNeedsPassThrough;

        if (_DebugMode)
        {
          Log.Debug("Stream info: chans = {0}, freq = {1}, origres = {2}, ctype = {3}", new string[]{
						_DataStreamInfo.BASSChannelInfo.chans.ToString(),
						_DataStreamInfo.BASSChannelInfo.freq.ToString(),
						_DataStreamInfo.BASSChannelInfo.origres.ToString(),
						Enum.GetName(typeof(BASSChannelType), _DataStreamInfo.BASSChannelInfo.ctype)});
        }

        result =
            PrepareDevice() &&
            InitBuffer() &&
            InitSinkStream() &&
            InitMixerStream() &&
            InitVSTPlugins() &&
            InitWADSPPlugins() &&
            InitOutputStream() &&
            InitVizStream() &&
            InitDevice() &&
            SetReplayGain();
      }

      if (result)
      {
        // Make sure there's something in the buffer to start with, or else 
        // the player will stop again immediatly.
        _State = PlayState.Buffering;
        result = PreBuffer();
      }

      if (result)
      {
        // Call SetVizLatencyCorrection() to adjust to DSP latency.
        SetVizLatencyCorrection(_Profile.VizLatencyCorrection);

        CalculateTotalLatency();

        if (_CurrentFileType.FileMainType == FileMainType.WebStream)
          PrepareFadeIn();

        result = StartBass();
      }

      if (result)
      {
        if (_CurrentFileType.FileMainType == FileMainType.WebStream)
          FadeIn();

        _State = PlayState.Playing;

        if (SessionStarted != null)
          SessionStarted();
      }
      return result;
    }

    private bool StopSession()
    {
      return StopSession(false, false);
    }

    private bool StopSession(bool isAutoStop, bool restarting)
    {
      Log.Debug("Stopping session...");

      if (_State == PlayState.Playing)
      {
        if ((_Profile.OutputMode == OutputMode.DirectSound) && isAutoStop)
          // The stop is triggered because there was no new track queued;
          // Keep playing until the BASS buffer is also outputted.
          Thread.Sleep(_Profile.BASSPlayBackBufferSize);

        StopBass();
      }

      _State = PlayState.Stopped;

      switch (_Profile.OutputMode)
      {
        case OutputMode.WASAPI:
          {
            Log.Debug("Stopping session: releasing WASAPI device...");
            BassWasapi.BASS_WASAPI_Free();
            break;
          }
        case OutputMode.ASIO:
          {
            Log.Debug("Stopping session: releasing ASIO device...");
            _ASIOEngine.ReleaseDriver();
            break;
          }
        case OutputMode.DirectSound:
          {
            break;
          }
      }

      if (_WADSPHandles.Count > 0)
      {
        Log.Debug("Stopping session: releasing WinAmp DSP plugins...");
        foreach (int handle in _WADSPHandles.Keys)
        {
          // Some Winamp dsps might raise an exception when closing
          try
          {
            BassWaDsp.BASS_WADSP_Stop(handle);
          }
          catch (Exception e)
          {
            Log.Debug("An exception occurred while stopping WinAmp DSP plugin \"{0}\": {1}", _WADSPHandles[handle], e.ToString());
          }

          try
          {
            BassWaDsp.BASS_WADSP_ChannelRemoveDSP(handle);
          }
          catch (Exception e)
          {
            Log.Debug("An exception occurred while removing WinAmp DSP plugin \"{0}\": {1}", _WADSPHandles[handle], e.ToString());
          }

          try
          {
            BassWaDsp.BASS_WADSP_FreeDSP(handle);
          }
          catch (Exception e)
          {
            Log.Debug("An exception occurred while releasing WinAmp DSP plugin \"{0}\": {1}", _WADSPHandles[handle], e.ToString());
          }
        }
      }

      if (_Profile.UseVizAGC)
        _VizAGC.Stop();

      Log.Debug("Stopping session: freeing _VizFloatStream...");
      FreeStream(ref _VizStream);

      Log.Debug("Stopping session: freeing _VizStream...");
      FreeStream(ref _VizRawStream);

      Log.Debug("Stopping session: freeing _WASAPIOutputStream...");
      FreeStream(ref _WASAPIOutputStream);

      Log.Debug("Stopping session: freeing _ASIOOutputStream...");
      FreeStream(ref _ASIOOutputStream);

      Log.Debug("Stopping session: freeing _DSOutputStream...");
      FreeStream(ref _DSOutputStream);

      Log.Debug("Stopping session: freeing _MixerStream...");
      FreeStream(ref _MixerStream);

      Log.Debug("Stopping session: freeing _SinkStream...");
      FreeStream(ref _SinkStream);

      // Do not free bass if we are going to restart, because that would also free
      // the stream we are about to play. 
      if (!restarting)
      {
        if (_Profile.UseReplayGain)
          _ReplayGainDSP.Stop();

        // Apparently Bass.BASS_Free() does not always free all streams as 
        // stated in documentation!!!
        // When not in ASIO mode and switching webstreams a few times,
        // BASS starts refusing to connect to any stream. 
        // Explicitly Freeing _DataStream solves this problem.

        Log.Debug("Stopping session: freeing _DataStream...");
        FreeStream(ref _DataStream);

        Log.Debug("Stopping session: freeing _NextDataStream...");
        FreeStream(ref _NextDataStream);

        Log.Debug("Stopping session: freeing _CurrentStream...");
        FreeStream(ref _CurrentStream);

        Log.Debug("Stopping session: freeing BASS...");
        if (!Bass.BASS_Free())
          HandleBassError("BASS_Free");

        // Make sure the built-in BASS player knows that BASS is freed,
        // otherwise we will just break it by calling Bass.BASS_Free()
        if (BassMusicPlayer.Initialized)
          BassMusicPlayer.FreeBass();

        _InitBASS = true;

        if (SessionStopped != null)
          SessionStopped();
      }

      return true;
    }

    private void CalculateTotalLatency()
    {
      _TotalLatencyMS = _Buffer.Delay - _ReadOffsetMS + _VSTDelayMS;

      if (_Profile.OutputMode == OutputMode.DirectSound)
        _TotalLatencyMS += _Profile.BASSPlayBackBufferSize;

      Log.Debug("Calculated total latency: {0}ms", _TotalLatencyMS);
    }

    private void ResetSessionState()
    {
      Log.Debug("BASSPlayer.ResetSessionState() called...");

      _State = PlayState.Stopped;
      _NewSessionPending = false;
      _BufferEmptySignaled = false;
      _DataStreamEndSignaled = false;
      _PlayingFromBuffer = false;
      _GapPlayed = false;
      _GapBytesLeft = 0;
    }

    private bool PrepareDevice()
    {
      bool result = false;

      switch (_Profile.OutputMode)
      {
        case OutputMode.WASAPI:
          {
            result = PrepareDevice_WASAPI();
            break;
          }
        case OutputMode.ASIO:
          {
            result = PrepareDevice_ASIO();
            break;
          }
        case OutputMode.DirectSound:
          {
            result = PrepareDevice_DirectSound();
            break;
          }
      }

      if (result && _DebugMode)
      {
        Log.Debug(
            "Device info: " +
            _DeviceInfo.ToString());
      }
      return result;
    }

    private bool PrepareDevice_WASAPI()
    {
      Log.Debug("Initializing WASAPI device: {0}...", _Profile.WASAPIDevice);

      _WASAPIDeviceNumber = -1;
      BASS_WASAPI_DEVICEINFO[] devices = BassWasapi.BASS_WASAPI_GetDeviceInfos();
      for (int i = 0; i < devices.Length; i++)
      {
        if (devices[i].name == _Profile.WASAPIDevice)
        {
          _WASAPIDeviceNumber = i;
          break;
        }
      }

      bool result = (_WASAPIDeviceNumber > -1);
      if (!result)
        HandleError(ErrorCode.MiscError, "Specified WASAPI device not found.");

      if (result)
      {
        string sharedDescr = _Profile.WASAPIExclusive ? "exclusive" : "shared";
        string eventDescr = _Profile.WASAPIEvent ? "pull" : "push";

        Log.Info("Using WASAPI Device {0}, {1}, {2}", _Profile.WASAPIDevice, sharedDescr, eventDescr);
        BASS_WASAPI_DEVICEINFO wasapiDeviceInfo = BassWasapi.BASS_WASAPI_GetDeviceInfo(_WASAPIDeviceNumber);

        _DeviceInfo = new WASAPIDeviceInfo(wasapiDeviceInfo, _Profile.WASAPIExclusive, GetMinExclWASAPIRate(), GetMaxExclWASAPIRate(), GetExclWASAPIChannels());
      }

      return result;
    }

    private int GetExclWASAPIChannels()
    {
      switch (_Profile.WASAPISpeakerLayout)
      {
        case SpeakerLayout.Mono:
          return 1;
        case SpeakerLayout.Stereo:
          return 2;
        case SpeakerLayout.QuadraphonicPhonic:
          return 4;
        case SpeakerLayout.FiveDotOne:
          return 6;
        case SpeakerLayout.SevenDotOne:
          return 8;
        default:
          return 2;
      }
    }

    private int GetMinExclWASAPIRate()
    {
      if (_Profile.ForceMinWASAPIRate == 0)
        return 8000;
      else
        return _Profile.ForceMinWASAPIRate;
    }

    private int GetMaxExclWASAPIRate()
    {
      if (_Profile.ForceMaxWASAPIRate == 0)
        return 192000;
      else
        return _Profile.ForceMaxWASAPIRate;
    }

    private bool PrepareDevice_ASIO()
    {
      Log.Debug("Initializing ASIO device: {0}...", _Profile.ASIODevice);

      bool result = true;
      if (_Profile.ASIODevice == String.Empty)
      {
        Log.Info("Using first available ASIO Device");
        _ASIODeviceNumber = 0;
      }
      else
      {
        _ASIODeviceNumber = -1;

        // Check if the ASIO device read is amongst the one retrieved
        for (int i = 0; i < AsioDriver.InstalledDrivers.Length; i++)
        {
          if (AsioDriver.InstalledDrivers[i].Name == _Profile.ASIODevice)
          {
            _ASIODeviceNumber = i;
            break;
          }
        }
        result = (_ASIODeviceNumber > -1);
        if (!result)
          HandleError(ErrorCode.MiscError, "Specified ASIO device not found.");

        if (result)
          Log.Info("Using ASIO device {0}", _Profile.ASIODevice);
      }

      if (result)
      {
        result = _ASIOEngine.InitDriver(_ASIODeviceNumber, IntPtr.Zero, _Profile.UseMaxASIOBufferSize);
        if (!result)
          HandleAsioEngineError("InitDriver");
      }

      if (result)
      {
        _DeviceInfo = new AsioDeviceInfo(
            _ASIOEngine,
            GetMinASIORate(),
            GetMaxASIORate(),
            GetASIOLatency());

        if (_DebugMode)
        {
          AsioDriver driver = _ASIOEngine.Driver;

          Log.Debug(String.Format(
              "ASIO device info: name = {0}, version = {1}, bufmin = {2}, bufmax = {3}, bufgran = {4}, bufpref = {5}",
              driver.DriverName,
              driver.Version,
              driver.Buffer.MinSize,
              driver.Buffer.MaxSize,
              driver.Buffer.Granularity,
              driver.Buffer.PreferredSize));

          for (int i = 0; i < _ASIOEngine.Driver.OutputChannels.Length; i++)
          {
            Log.Debug(String.Format(
                "ASIO channel info: name = '{0}', sampletype = {1}",
                _ASIOEngine.Driver.OutputChannels[i].Name,
                _ASIOEngine.Driver.OutputChannels[i].SampleType.ToString()));
          }
        }
      }
      return result;
    }

    private int GetMinASIORate()
    {
      int minimumRate;
      if (_Profile.ForceMinASIORate == 0)
      {
        Log.Debug("Auto-detecting minimum supported ASIO samplingrate");

        minimumRate = SamplingRates[0];
        for (int index = 0; index < SamplingRates.Length; index++)
        {
          int rate = SamplingRates[index];
          if (_ASIOEngine.Driver.CanSampleRate(rate))
          {
            minimumRate = rate;
            break;
          }
        }
      }
      else
      {
        minimumRate = _Profile.ForceMinASIORate;
      }
      return minimumRate;
    }

    private int GetMaxASIORate()
    {
      int maximumRate;
      if (_Profile.ForceMaxASIORate == 0)
      {
        Log.Debug("Auto-detecting maximum supported ASIO samplingrate");

        maximumRate = SamplingRates[SamplingRates.Length - 1];
        for (int index = SamplingRates.Length - 1; index >= 0; index--)
        {
          int rate = SamplingRates[index];
          if (_ASIOEngine.Driver.CanSampleRate(rate))
          {
            maximumRate = rate;
            break;
          }
        }
      }
      else
      {
        maximumRate = _Profile.ForceMaxASIORate;
      }
      return maximumRate;
    }

    private int GetASIOLatency()
    {
      int sampleLatency = _ASIOEngine.Driver.Latency.OutputLatency;
      int freq = Convert.ToInt32(_ASIOEngine.Driver.GetSampleRate());
      int latency = ((sampleLatency * 1000) / freq);

      if (latency == 0)
        latency = 50;

      return latency;
    }

    private bool PrepareDevice_DirectSound()
    {
      Log.Debug("Initializing DirectSound device: {0}...", _Profile.DirectSoundDevice);

      int deviceNo;

      if (_Profile.DirectSoundDevice == ConfigProfile.Defaults.DirectSoundDevice || _Profile.DirectSoundDevice == String.Empty)
      {
        Log.Info("Using default DirectSound device");
        deviceNo = -1;
      }
      else
      {
        deviceNo = -1;
        BASS_DEVICEINFO[] devices = Bass.BASS_GetDeviceInfos();
        for (int i = 0; i < devices.Length; i++)
        {
          if (devices[i].name == _Profile.DirectSoundDevice)
          {
            deviceNo = i;
            break;
          }
        }
        if (deviceNo == -1)
          Log.Warn("specified DirectSound device does not exist. Using default DirectSound Device");
        else
          Log.Info("Using DirectSound Device {0}", _Profile.DirectSoundDevice);
      }

      BASSInit flags = BASSInit.BASS_DEVICE_DEFAULT;

      if (deviceNo != _LastDeviceInfoDevice)
        // Only do this the first time this device is initialized; because it's kinda slow.
        flags |= BASSInit.BASS_DEVICE_LATENCY;

      bool result = Bass.BASS_Init(
          deviceNo,
          _DataStreamInfo.BASSChannelInfo.freq,
          flags,
          IntPtr.Zero,
          Guid.Empty);

      if (!result)
      {
        BASSError error = Bass.BASS_ErrorGetCode();
        if (error == BASSError.BASS_ERROR_ALREADY)
          result = true;
        else
          HandleError(ErrorCode.DeviceError, String.Format("BASS_Init() failed: {0}", error));
      }

      if (result)
      {
        // Bass will maximize the value to 100 ms
        Bass.BASS_SetConfig(BASSConfig.BASS_CONFIG_UPDATEPERIOD, _Profile.BASSPlayBackBufferSize / 4);

        if (deviceNo != _LastDeviceInfoDevice)
        {
          // Retrieve deviceinfo only the first time we use this device, because 
          // retrieving latency and minbuf (BASSInit.BASS_DEVICE_LATENCY) 
          // slows down initialization quite a bit.
          _LastDeviceInfoDevice = deviceNo;
          BASS_INFO bassInfo = Bass.BASS_GetInfo();
          _DeviceInfo = new DSDeviceInfo(bassInfo);

          if (_DebugMode)
          {
            Log.Debug(String.Format(
                "DirectX device info: dsver = {0}, minbuf = {1}",
                 bassInfo.dsver,
                 bassInfo.minbuf));
          }
        }
      }
      return result;
    }

    private bool InitDevice()
    {
      bool result = false;

      switch (_Profile.OutputMode)
      {
        case OutputMode.WASAPI:
          {
            result = InitDevice_WASAPI();
            break;
          }
        case OutputMode.ASIO:
          {
            result = InitDevice_ASIO();
            break;
          }
        case OutputMode.DirectSound:
          {
            break;
          }
      }
      return result;
    }

    private bool InitBuffer()
    {
      int channels = _DataStreamInfo.BASSChannelInfo.chans;
      int samplingRate = _DataStreamInfo.BASSChannelInfo.freq;

      _ReadOffsetMS = VizLatencyCorrectionRangeMS;
      if (_Profile.OutputMode == OutputMode.DirectSound)
        _ReadOffsetMS += _Profile.BASSPlayBackBufferSize;

      _ReadOffsetBytes = AudioRingBuffer.CalculateLength(samplingRate, channels, _ReadOffsetMS);
      int bufferLength = AudioRingBuffer.CalculateLength(samplingRate, channels, _Profile.PlayBackBufferSize) + _ReadOffsetBytes;

      _GapByteLength = AudioRingBuffer.CalculateLength(samplingRate, channels, _Profile.GapLength);

      if (_Buffer == null || _Buffer.Length != bufferLength)
      {
        Log.Debug("Creating new ringbuffer with {0} ms delay...", _Profile.PlayBackBufferSize);

        _Buffer = new AudioRingBuffer(samplingRate, channels, _Profile.PlayBackBufferSize + _ReadOffsetMS);
        _BufferUpdateThresholdBytes = Convert.ToInt32(((bufferLength - _ReadOffsetBytes) * (100 - _BufferUpdateThreshold)) / 100);

        if (_ReadData.Length < bufferLength)
          Array.Resize<byte>(ref _ReadData, bufferLength);
      }
      else
      {
        Log.Debug("Clearing ringbuffer...");
      }

      ClearPlayBackBuffer();

      return true;
    }

    private void ClearPlayBackBuffer()
    {
      _Buffer.Clear();

      // Fill buffer with space untill _ReadOffsetBytes
      Array.Clear(_ReadData, 0, _ReadOffsetBytes);
      _Buffer.Write(_ReadData, _ReadOffsetBytes);
    }

    private void ClearBASSPlayBackBuffer()
    {
      switch (_Profile.OutputMode)
      {
        case OutputMode.WASAPI:
          {
            Bass.BASS_ChannelSetPosition(_WASAPIOutputStream, 0);
            break;
          }
        case OutputMode.ASIO:
          {
            _ASIOEngine.ClearOutputBuffers();
            break;
          }
        case OutputMode.DirectSound:
          {
            Bass.BASS_ChannelSetPosition(_DSOutputStream, 0);
            break;
          }
      }
    }

    private void ClearVSTBuffers()
    {
      foreach (int handle in _VSTHandles.Keys)
      {
        bool result = BassVst.BASS_VST_Resume(handle);
        if (!result)
          HandleBassError("BASS_VST_Resume");
      }
    }

    private bool InitSinkStream()
    {
      Log.Debug("Creating sinkstream...");

      BASSFlag streamFlags =
          BASSFlag.BASS_STREAM_DECODE |
          BASSFlag.BASS_SAMPLE_FLOAT;

      _SinkStream = Bass.BASS_StreamCreate(
          _DataStreamInfo.BASSChannelInfo.freq,
          _DataStreamInfo.BASSChannelInfo.chans,
          streamFlags,
          _StreamWriteProcDelegate,
          IntPtr.Zero);

      bool result = (_SinkStream != 0);
      if (!result)
        HandleBassError("BASS_StreamCreate");

      return result;
    }

    private bool InitMixerStream()
    {
      Log.Debug("Preparing mixerstream...");

      BASSFlag streamFlags =
          BASSFlag.BASS_MIXER_NONSTOP |
          BASSFlag.BASS_STREAM_DECODE |
          BASSFlag.BASS_SAMPLE_FLOAT;

      int inputChannels = _DataStreamInfo.BASSChannelInfo.chans;
      int outputChannels;

      if (_Profile.OutputMode == OutputMode.ASIO)
      {
        int first = _Profile.ASIOFirstChan;
        int last = _Profile.ASIOLastChan;

        if (first == -1)
          first = 0;

        if (last == -1)
          last = _DeviceInfo.Channels - 1;

        first = Math.Min(first, _DeviceInfo.Channels - 1);
        last = Math.Min(last, _DeviceInfo.Channels - 1);

        outputChannels = last - first + 1;
        outputChannels = Math.Max(outputChannels, 1);
      }
      else
      {
        outputChannels = _DeviceInfo.Channels;
      }

      float[,] mixMatrix = null;

      if (outputChannels > inputChannels)
      {
        // Upmix
        mixMatrix = MixingMatrixHelper.CreateMixingMatrix(inputChannels, _Profile);
        if (mixMatrix != null)
          outputChannels = Math.Min(mixMatrix.GetLength(0), outputChannels);
        else
          outputChannels = inputChannels;
      }
      else if (outputChannels < inputChannels)
      {
        // Downmix: we only support downmix to stereo for now
        // BASS lib has built in downmix functionality
        outputChannels = Math.Min(outputChannels, 2);
      }

      Log.Debug("Using {0} channels...", outputChannels);

      int inputFreq = _DataStreamInfo.BASSChannelInfo.freq;

      int minRate = _DeviceInfo.MinRate;
      int maxRate = _DeviceInfo.MaxRate;

      int outputFreq = inputFreq;

      outputFreq = Math.Max(outputFreq, minRate);
      outputFreq = Math.Min(outputFreq, maxRate);

      if (_DebugMode && outputFreq != inputFreq)
        Log.Debug("Resampling {0} -> {1}...", new object[] { inputFreq, outputFreq });

      Log.Debug("Creating mixerstream...");

      _MixerStream = BassMix.BASS_Mixer_StreamCreate(
          outputFreq,
          outputChannels,
          streamFlags);

      bool result = (_MixerStream != 0);
      if (!result)
        HandleBassError("BASS_Mixer_StreamCreate");

      if (result)
      {
        Log.Debug("Plugging sinkstream into mixer...");

        // Bass 2.3: BASS_MIXER_NORAMPIN is required because when using floating streams it never reaches 100% apparently.
        result = BassMix.BASS_Mixer_StreamAddChannel(_MixerStream, _SinkStream,
            BASSFlag.BASS_MIXER_MATRIX | BASSFlag.BASS_MIXER_DOWNMIX | BASSFlag.BASS_MIXER_NORAMPIN);

        if (!result)
          HandleBassError("BASS_Mixer_StreamAddChannel");
      }

      // If no upmix matrix defined, we let BASS use its default mapping.
      if (result && mixMatrix != null)
      {
        Log.Debug("Setting mixing matrix...");
        result = BassMix.BASS_Mixer_ChannelSetMatrix(_SinkStream, mixMatrix);
        if (!result)
          HandleBassError("BASS_Mixer_ChannelSetMatrix");
      }

      if (result && _DebugMode)
      {
        float[,] matrix = new float[outputChannels, inputChannels];
        BassMix.BASS_Mixer_ChannelGetMatrix(_SinkStream, matrix);
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
          for (int j = 0; j < matrix.GetLength(1); j++)
          {
            Log.Debug(String.Format("Mixing Matrix: matrix[{0}, {1}] = {2}", i, j, matrix[i, j].ToString()));
          }
        }
      }
      return result;
    }

    private bool InitVSTPlugins()
    {
      _VSTHandles.Clear();
      _VSTDelayMS = 0;

      bool result = true;
      if (!CurrentNeedsPassThrough && _Profile.VSTPlugins.Count > 0)
      {
        int delaySamples = 0;

        foreach (KeyValuePair<string, ConfigProfile.VSTPlugin> plugin in _Profile.VSTPlugins)
        {
          Log.Info("Loading VST plugin \"{0}\"...", plugin.Value.DllFile);

          int handle = BassVst.BASS_VST_ChannelSetDSP(_MixerStream, plugin.Value.DllFile, BASSVSTDsp.BASS_VST_DEFAULT, 1);
          result = (handle > 0);
          if (!result)
          {
            HandleBassError("BassVst.BASS_VST_ChannelSetDSP");
            break;
          }

          if (result)
          {
            BASS_VST_INFO vstInfo = new BASS_VST_INFO();
            BassVst.BASS_VST_GetInfo(handle, vstInfo);

            delaySamples += vstInfo.initialDelay;

            Log.Debug("VST plugin info: vendorName={0}, vendorVersion={1}, productName={2}, uniqueID={3}, effectName={4}, effectVersion={5}, effectVstVersion={6}, hostVstVersion={7}, chansIn={8}, chansOut={9}, initialDelay={10}", new object[]{
							vstInfo.vendorName,
							vstInfo.vendorVersion,
							vstInfo.productName,
							vstInfo.uniqueID,
							vstInfo.effectName,
							vstInfo.effectVersion,
							vstInfo.effectVstVersion,
							vstInfo.hostVstVersion,
							vstInfo.chansIn,
							vstInfo.chansOut,
							vstInfo.initialDelay
						});

            _VSTHandles[handle] = vstInfo;
            foreach (KeyValuePair<int, ConfigProfile.VSTParameter> parameter in plugin.Value.Parameters)
            {
              //Log.Debug("   VST parameter: index={0}, name={1}, value={2}", parameter.Value.Index, parameter.Value.Name, parameter.Value.Value);
              BassVst.BASS_VST_SetParam(handle, parameter.Value.Index, parameter.Value.Value);
            }
          }
        }
        BASS_CHANNELINFO channelInfo = Bass.BASS_ChannelGetInfo(_MixerStream);
        _VSTDelayMS = (delaySamples * 1000 / channelInfo.freq);
        Log.Debug("Total calculated VST latency: {0} ms...", _VSTDelayMS);
      }
      return result;
    }

    private bool InitWADSPPlugins()
    {
      _WADSPHandles.Clear();
      bool result = true;
      if (!CurrentNeedsPassThrough && _Profile.WADSPPlugins.Count > 0)
      {
        BassWaDsp.BASS_WADSP_Init(MediaPortal.GUI.Library.GUIGraphicsContext.ActiveForm);

        foreach (KeyValuePair<string, ConfigProfile.WADSPPlugin> plugin in _Profile.WADSPPlugins)
        {
          Log.Info("Loading WinAmp DSP plugin \"{0}\"...", plugin.Value.DllFile);

          int handle = BassWaDsp.BASS_WADSP_Load(plugin.Value.DllFile, 5, 5, 100, 100, null);
          result = (handle > 0);
          if (!result)
          {
            HandleBassError("BassWa.BASS_WADSP_Load");
            break;
          }

          if (result)
          {
            BassWaDsp.BASS_WADSP_ChannelSetDSP(handle, _MixerStream, 1);
            BassWaDsp.BASS_WADSP_Start(handle, 0, 0);
            _WADSPHandles[handle] = plugin.Value.DllFile;
          }
        }
      }
      return result;
    }

    private bool InitOutputStream()
    {
      switch (_Profile.OutputMode)
      {
        case OutputMode.DirectSound:
          return InitOutputStream_DirectSound();
        case OutputMode.WASAPI:
          return InitOutputStream_WASAPI();
        case OutputMode.ASIO:
          return InitOutputStream_ASIO();
        default:
          return false;
      }
    }

    private bool InitOutputStream_DirectSound()
    {
      Log.Debug("Creating DS outputstream...");

      BASSFlag streamFlags = BASSFlag.BASS_DEFAULT;
      BASS_CHANNELINFO info = Bass.BASS_ChannelGetInfo(_MixerStream);

      _DSOutputStream = Bass.BASS_StreamCreate(
        info.freq,
        info.chans,
        streamFlags,
        _DSOutputStreamWriteProcDelegate,
        IntPtr.Zero);

      bool result = (_DSOutputStream != 0);
      if (!result)
        HandleBassError("BassMix.BASS_Mixer_StreamCreate");

      return result;
    }

    private bool InitOutputStream_ASIO()
    {
      Log.Debug("Creating outputmixerstream...");

      BASSFlag streamFlags =
          BASSFlag.BASS_MIXER_NONSTOP |
          BASSFlag.BASS_SAMPLE_FLOAT |
          BASSFlag.BASS_STREAM_DECODE;

      BASS_CHANNELINFO info = Bass.BASS_ChannelGetInfo(_MixerStream);

      int inputFreq = info.freq;
      int outputFreq = inputFreq;
      if (CurrentUsesOverSampling)
      {
        int newFreq = outputFreq * 2;
        if (newFreq <= _DeviceInfo.MaxRate)
          outputFreq = newFreq;
        else
          Log.Info("Oversampling disabled: samplingrate of {0} is not supported by device...", newFreq);
      }

      if (_DebugMode && outputFreq != inputFreq)
        Log.Info("Oversampling {0} -> {1}...", new object[] { inputFreq, outputFreq });

      _ASIOOutputStream = BassMix.BASS_Mixer_StreamCreate(
          outputFreq,
          info.chans,
          streamFlags);

      bool result = (_ASIOOutputStream != 0);
      if (!result)
        HandleBassError("BassMix.BASS_Mixer_StreamCreate");

      if (result)
      {
        Log.Debug("Plugging mixerstream into outputmixer...");

        // Bass 2.3: BASS_MIXER_NORAMPIN is required because when using floating streams it never reaches 100% apparently.
        result = BassMix.BASS_Mixer_StreamAddChannel(
            _ASIOOutputStream,
            _MixerStream,
            BASSFlag.BASS_MIXER_NORAMPIN);

        if (!result)
          HandleBassError("BassMix.BASS_Mixer_StreamAddChannel");
      }
      return result;
    }

    private bool InitOutputStream_WASAPI()
    {
      Log.Debug("Creating WASAPI outputstream...");

      BASSFlag streamFlags =
        BASSFlag.BASS_STREAM_DECODE |
        BASSFlag.BASS_MIXER_NONSTOP |
        BASSFlag.BASS_SAMPLE_FLOAT;

      BASS_CHANNELINFO info = Bass.BASS_ChannelGetInfo(_MixerStream);

      int inputFreq = info.freq;
      int outputFreq = inputFreq;
      if (CurrentUsesOverSampling)
      {
        int newFreq = outputFreq * 2;
        if (newFreq <= _DeviceInfo.MaxRate)
          outputFreq = newFreq;
        else
          Log.Info("Oversampling disabled: samplingrate of {0} is not supported by device...", newFreq);
      }

      if (_DebugMode && outputFreq != inputFreq)
        Log.Info("Oversampling {0} -> {1}...", new object[] { inputFreq, outputFreq });

      _WASAPIOutputStream = BassMix.BASS_Mixer_StreamCreate(
          outputFreq,
          info.chans,
          streamFlags);

      bool result = (_WASAPIOutputStream != 0);
      if (!result)
        HandleBassError("BassMix.BASS_Mixer_StreamCreate");

      if (result)
      {
        Log.Debug("Plugging mixerstream into outputmixer...");

        // Bass 2.3: BASS_MIXER_NORAMPIN is required because when using floating streams it never reaches 100% apparently.
        result = BassMix.BASS_Mixer_StreamAddChannel(
            _WASAPIOutputStream,
            _MixerStream,
            BASSFlag.BASS_MIXER_NORAMPIN);

        if (!result)
          HandleBassError("BassMix.BASS_Mixer_StreamAddChannel");
      }

      return result;
    }

    private bool InitVizStream()
    {
      Log.Debug("Creating visualization stream...");

      BASSFlag streamFlags =
          BASSFlag.BASS_STREAM_DECODE |
          BASSFlag.BASS_SAMPLE_FLOAT;

      _VizRawStream = Bass.BASS_StreamCreate(
          _DataStreamInfo.BASSChannelInfo.freq,
          _DataStreamInfo.BASSChannelInfo.chans,
          streamFlags,
          _VizRawStreamWriteProcDelegate,
          IntPtr.Zero);

      bool result = (_VizRawStream != 0);
      if (!result)
        HandleBassError("Bass.BASS_StreamCreate");

      if (result && _Profile.UseVizAGC)
      {
        // Note:
        // - BASS_FX_DSPDAMP breaks when playing silence.
        // - DSP_PeakLevelMeter breaks when using AC3...
        // So we made our own AGC

        _VizAGC.ChannelHandle = _VizRawStream;
        result = _VizAGC.Start();
        if (!result)
          HandleBassError("_VizAGC.Start");
      }

      if (result)
      {
        streamFlags =
            BASSFlag.BASS_MIXER_NONSTOP |
            BASSFlag.BASS_SAMPLE_FLOAT |
            BASSFlag.BASS_STREAM_DECODE;

        _VizStream = BassMix.BASS_Mixer_StreamCreate(
            44100,
            2,
            streamFlags);

        result = (_VizStream != 0);
        if (!result)
          HandleBassError("BassMix.BASS_Mixer_StreamCreate");
      }

      if (result)
      {
        streamFlags =
            BASSFlag.BASS_MIXER_NORAMPIN |
            BASSFlag.BASS_MIXER_DOWNMIX |
            BASSFlag.BASS_MIXER_MATRIX;

        result = BassMix.BASS_Mixer_StreamAddChannel(
            _VizStream,
            _VizRawStream,
            streamFlags);

        if (!result)
          HandleBassError("BassMix.BASS_Mixer_StreamAddChannel");
      }

      if (result)
      {
        if (_DataStreamInfo.BASSChannelInfo.chans == 1)
        {
          float[,] mixMatrix = new float[2, 1];
          mixMatrix[0, 0] = 1;
          mixMatrix[1, 0] = 1;

          result = BassMix.BASS_Mixer_ChannelSetMatrix(_VizRawStream, mixMatrix);
          if (!result)
            HandleBassError("BassMix.BASS_Mixer_ChannelSetMatrix");
        }
      }
      return result;
    }

    private bool InitDevice_WASAPI()
    {
      if (_Profile.OutputMode == OutputMode.WASAPI)
      {
        Log.Debug("Initializing WASAPI device...");

        BASS_CHANNELINFO info = Bass.BASS_ChannelGetInfo(_WASAPIOutputStream);

        BASSWASAPIInit flags = BASSWASAPIInit.BASS_WASAPI_AUTOFORMAT;

        if (_Profile.WASAPIExclusive)
          flags |= BASSWASAPIInit.BASS_WASAPI_EXCLUSIVE;

        if (_Profile.WASAPIEvent)
          flags |= BASSWASAPIInit.BASS_WASAPI_EVENT;

        bool result = BassWasapi.BASS_WASAPI_Init(_WASAPIDeviceNumber, info.freq, info.chans, flags, 0.01f, 0f, _WasapiProcDelegate, IntPtr.Zero);
        if (!result)
          HandleBassError("BASS_WASAPI_Init");

        return result;
      }
      else
        return true;
    }

    private bool InitDevice_ASIO()
    {
      if (_Profile.OutputMode == OutputMode.ASIO)
      {
        Log.Debug("Initializing ASIO handler...");

        BASS_CHANNELINFO info = Bass.BASS_ChannelGetInfo(_ASIOOutputStream);
        int firstChan = Math.Max(_Profile.ASIOFirstChan, 0);

        _ASIOEngine.SetBassStream(_ASIOOutputStream);

        bool result = _ASIOEngine.Driver.SetSampleRate(info.freq);
        if (!result)
          HandleAsioEngineError("Driver.SetSampleRate");

        if (result)
        {
          Log.Debug("Using asio channels {0} - {1}...", firstChan, info.chans);
          _ASIOEngine.SetChannels(firstChan, info.chans);
        }
        return result;
      }
      else
        return true;
    }

    private int StreamWriteProc(int streamHandle, IntPtr buffer, int requestedBytes, IntPtr userData)
    {
      int read = _Buffer.Read(buffer, requestedBytes, _ReadOffsetBytes);

      _LastReadMilliSecs = _StopWatch.ElapsedMilliseconds;

      if (read > 0)
      {
        if (_Buffer.Space >= _BufferUpdateThresholdBytes)
          _WakeupBufferUpdateThread.Set();

        return read;
      }
      else
      {
        Log.Debug("Buffer-empty signaled...");
        _BufferEmptySignaled = true;
        _WakeupMainThread.Set();

        return (read | (int)BASSStreamProc.BASS_STREAMPROC_END);
      }
    }

    private int VizRawStreamWriteProc(int streamHandle, IntPtr buffer, int requestedBytes, IntPtr userData)
    {
      if (_State == PlayState.Playing)
      {
        // Compensate for bytes being played since the last time the buffer was read.
        int bytes;
        if (_LastReadMilliSecs > 0)
        {
          bytes = (int)(_StopWatch.ElapsedMilliseconds - _LastReadMilliSecs) * _Buffer.BytesPerMilliSec;
          bytes = Math.Max(Math.Min(bytes, _VizReadOffsetBytes), 0);
        }
        else
          bytes = 0;

        int read = _Buffer.ShadowRead(buffer, requestedBytes, _VizReadOffsetBytes - bytes);
        return read;
      }
      else
      {
        if (_VizStreamSilence.Length < requestedBytes)
          Array.Resize<byte>(ref _VizStreamSilence, requestedBytes);

        Marshal.Copy(_VizStreamSilence, 0, buffer, requestedBytes);
        return requestedBytes;
      }
    }

    private float[] _DSOSFloatData = new float[1];
    private short[] _DSOSInt16Data = new short[1];
    float _MaxSampleValue = 8388607.0f / 8388608.0f;

    private int DSOutputStreamWriteProc(int streamHandle, IntPtr buffer, int requestedBytes, IntPtr userData)
    {
      int requestedSamples = requestedBytes / 2;
      if (_DSOSFloatData.Length < requestedSamples)
      {
        Array.Resize<float>(ref _DSOSFloatData, requestedSamples);
        Array.Resize<short>(ref _DSOSInt16Data, requestedSamples);
      }

      int read = Bass.BASS_ChannelGetData(_MixerStream, _DSOSFloatData, requestedSamples * 4);
      int samplesRead = read / 4;

      for (int i = 0; i < samplesRead; i++)
      {
        float floatValue = _DSOSFloatData[i];

        if (floatValue > _MaxSampleValue)
          floatValue = _MaxSampleValue;
        else if (floatValue < -1.0f)
          floatValue = -1.0f;

        _DSOSInt16Data[i] = (short)(floatValue * 32768.0f);
      }
      Marshal.Copy(_DSOSInt16Data, 0, buffer, samplesRead);
      return samplesRead * 2;
    }

    private bool IsNewSessionRequired()
    {
      Log.Debug("BASSPlayer.IsNewSessionRequired() called...");

      BASS_CHANNELINFO CurrentStreamInfo = Bass.BASS_ChannelGetInfo(_CurrentStream);

      return (_DataStreamInfo.PassThrough != CurrentNeedsPassThrough ||
          _DataStreamInfo.BASSChannelInfo == null ||
          _DataStreamInfo.BASSChannelInfo.chans != CurrentStreamInfo.chans ||
          _DataStreamInfo.BASSChannelInfo.freq != CurrentStreamInfo.freq);
    }

    private bool PreBuffer()
    {
      Log.Debug("Filling buffer...");

      bool result = true;
      double now = DateTime.Now.TimeOfDay.TotalSeconds;
      while (result && _Buffer.Space >= _BufferUpdateThresholdBytes && DateTime.Now.TimeOfDay.TotalSeconds < now + 5)
      {
        _BufferUpdated.Reset();
        _WakeupBufferUpdateThread.Set();
        _BufferUpdated.WaitOne();

        result = !_DataStreamEndSignaled;
        if (result)
        {
          if (_VSTHandles.Count > 0)
          {
            BASS_CHANNELINFO mixerStreamInfo = Bass.BASS_ChannelGetInfo(_MixerStream);

            foreach (System.Collections.Generic.KeyValuePair<int, BASS_VST_INFO> keyValue in _VSTHandles)
            {
              int delaySamples = keyValue.Value.initialDelay;
              if (delaySamples > 0)
              {
                int bytes = keyValue.Value.initialDelay * 4 * mixerStreamInfo.chans;
                byte[] buff = new byte[bytes];
                int read = Bass.BASS_ChannelGetData(_MixerStream, buff, bytes);
                if (read == -1)
                {
                  HandleBassError("Bass.BASS_ChannelGetData");
                  result = false;
                }
              }
            }
          }
          if (result && _Profile.OutputMode == OutputMode.DirectSound)
          {
            if (!Bass.BASS_ChannelUpdate(_DSOutputStream, 0))
            {
              HandleBassError("Bass.BASS_ChannelUpdate");
              result = false;
            }
          }
        }
      }
      return result;
    }

    private int UpdateBuffer()
    {
      //Log.Debug("UpdateBuffer() called...");

      if (_GapBytesLeft == 0)
      {
        int channelRead = 0;
        if (_DataStream != 0)
        {
          int requestedBytes = _Buffer.Space;
          if (requestedBytes > 0)
          {
            // According documentation this overload should not be used with 
            // floating point streams... however it does work fine.
            channelRead = Bass.BASS_ChannelGetData(
                _DataStream,
                _ReadData,
                requestedBytes);

            if (channelRead >= 0)
            {
              _Buffer.Write(_ReadData, channelRead);
              _GapPlayed = false;
              _GapBytesLeft = 0;
            }
            else if (channelRead == -1)
            {
              BASSError error = Bass.BASS_ErrorGetCode();
              if (error == BASSError.BASS_ERROR_ENDED)
              {
                if (_PlayBackMode == PlayBackMode.Normal && !_GapPlayed)
                {
                  _GapBytesLeft = _GapByteLength;
                  channelRead = 0;
                }
              }
              else
              {
                HandleError(ErrorCode.MiscError, "BASS_ChannelGetData() failed: {0}.", error);
              }
            }
          }
        }
        return channelRead;
      }
      else //(_GapBytesLeft != 0)
      {
        // write silence to buffer to implement a gap.

        int written = 0;
        int bytes = _Buffer.Space;
        if (bytes >= _BufferUpdateThresholdBytes)
        {
          if (bytes > _GapBytesLeft)
            bytes = _GapBytesLeft;

          Array.Clear(_ReadData, 0, bytes);
          written = _Buffer.Write(_ReadData, bytes);

          _GapBytesLeft -= written;
          if (_GapBytesLeft == 0)
          {
            _GapPlayed = true;
            written = -1;
          }
        }
        return written;
      }
    }

    private int GetFadeStream()
    {
      // Fading on _OutputMixerStream does not work with ASIO, but when using WaveOut
      // is fades actually after the BASS playbackbuffer, which eliminates the delay we would
      // have otherwise.

      switch (_Profile.OutputMode)
      {
        case OutputMode.DirectSound:
          return _DSOutputStream;
        case OutputMode.WASAPI:
          return _MixerStream;
        case OutputMode.ASIO:
          return _MixerStream;
        default:
          return 0;
      }
    }

    private void PrepareFadeIn()
    {
      int stream = GetFadeStream();

      if (CurrentDoesSoftStop)
        Bass.BASS_ChannelSetAttribute(stream, BASSAttribute.BASS_ATTRIB_VOL, 0);
    }

    private void FadeIn()
    {
      int stream = GetFadeStream();

      if (CurrentDoesSoftStop)
        Bass.BASS_ChannelSlideAttribute(stream, BASSAttribute.BASS_ATTRIB_VOL, 1, _Profile.SoftStopDuration);

      else
      {
        // Do nothing; due to a Bass 2.3 bug the volume never reaches 100% again causing 
        // playback to be not bitperfect anymore (below 16bit resolution).
        //Bass.BASS_ChannelSetAttribute(stream, BASSAttribute.BASS_ATTRIB_VOL, 1);
      }
    }

    private void FadeOut()
    {
      int stream = GetFadeStream();

      if (CurrentDoesSoftStop)
      {
        Bass.BASS_ChannelSlideAttribute(stream, BASSAttribute.BASS_ATTRIB_VOL, 0, _Profile.SoftStopDuration);
        while (Bass.BASS_ChannelIsSliding(stream, BASSAttribute.BASS_ATTRIB_VOL))
        {
          Thread.Sleep(10);
        }
      }
      else
      {
        // Do nothing; see FadeIn().
        //Bass.BASS_ChannelSetAttribute(stream, BASSAttribute.BASS_ATTRIB_VOL, 0);
      }

      // Compensate for device latency to make sure playback will 
      // not be stopped until the fadeout has reached the speakers.
      Thread.Sleep(_DeviceInfo.Latency + 30);
    }

    private bool StartBass()
    {
      _StopWatch.Reset();
      _StopWatch.Start();

      bool result = false;
      switch (_Profile.OutputMode)
      {
        case OutputMode.DirectSound:
          {
            Log.Debug("Starting output...");

            result = Bass.BASS_ChannelPlay(_DSOutputStream, false);
            if (!result)
              HandleBassError("Bass.BASS_ChannelPlay");

            break;
          }
        case OutputMode.WASAPI:
          {
            Log.Debug("Starting WASAPI...");

            result = BassWasapi.BASS_WASAPI_Start();
            if (!result)
              HandleBassError("BASS_WASAPI_Start");

            break;
          }
        case OutputMode.ASIO:
          {
            Log.Debug("Starting ASIO...");

            result = _ASIOEngine.Start();
            if (!result)
              HandleAsioEngineError("Start");

            break;
          }
        default:
          break;
      }

      return result;
    }

    private bool StopBass()
    {
      _StopWatch.Stop();

      bool result = false;
      switch (_Profile.OutputMode)
      {
        case OutputMode.DirectSound:
          {
            Log.Debug("Stopping output...");

            result = Bass.BASS_ChannelStop(_DSOutputStream);
            if (!result)
              HandleBassError("Bass.BASS_ChannelStop");

            break;
          }
        case OutputMode.WASAPI:
          {
            Log.Debug("Stopping WASAPI...");

            result = BassWasapi.BASS_WASAPI_Stop(false);
            if (!result)
              HandleBassError("BASS_WASAPI_Stop");

            break;
          }
        case OutputMode.ASIO:
          {
            Log.Debug("stopping ASIO...");

            result = _ASIOEngine.Stop();
            if (!result)
              HandleAsioEngineError("Stop");

            break;
          }
        default:
          break;
      }

      return result;
    }

    private void CreateCurrentStream()
    {
      BASSFlag streamFlags =
          BASSFlag.BASS_STREAM_DECODE |
          BASSFlag.BASS_SAMPLE_FLOAT;

      switch (_CurrentFileType.FileMainType)
      {
        case FileMainType.CDTrack:
          _CurrentStream = BassCd.BASS_CD_StreamCreateFile(_CurrentFilePath, streamFlags);
          break;

        case FileMainType.WebStream:
          if (_WaitCursor == null)
            _WaitCursor = new MediaPortal.GUI.Library.WaitCursor();

          // In case connecting fails: retry connecting for as long as the timeout setting.
          double tryUntil = DateTime.Now.TimeOfDay.TotalMilliseconds +
              Bass.BASS_GetConfig(BASSConfig.BASS_CONFIG_NET_TIMEOUT);

          while (_CurrentStream == 0 && DateTime.Now.TimeOfDay.TotalMilliseconds < tryUntil)
          {
            if (_CurrentFileType.FileSubType == FileSubType.ASXWebStream)
              _CurrentStream = Bass.BASS_StreamCreateFile(_CurrentFilePath, 0, 0, streamFlags);
            else
              _CurrentStream = Bass.BASS_StreamCreateURL(_CurrentFilePath, 0, streamFlags, null, IntPtr.Zero);

            if (_CurrentStream == 0)
              // Connection failed: wait a little while
              Thread.Sleep(100);
          }
          break;

        case FileMainType.MODFile:
          _CurrentStream = Bass.BASS_MusicLoad(_CurrentFilePath, 0, 0,
            BASSFlag.BASS_SAMPLE_FLOAT |
            BASSFlag.BASS_MUSIC_SINCINTER |
            BASSFlag.BASS_MUSIC_RAMP |
            BASSFlag.BASS_MUSIC_PRESCAN |
            BASSFlag.BASS_MUSIC_DECODE, 0);
          break;

        case FileMainType.AudioFile:
          _CurrentStream = Bass.BASS_StreamCreateFile(_CurrentFilePath, 0, 0, streamFlags);
          break;

        case FileMainType.Unknown:
          break;
      }

      if (_CurrentStream != 0)
        Log.Debug(String.Format("Stream created for : {0}", _CurrentFilePath));
      else
        HandleError(ErrorCode.FileError, "Cannot create stream from {0}: {1}.", _CurrentFilePath, Bass.BASS_ErrorGetCode());
    }

    private void GetReplayGainData()
    {
      if (_Profile.UseReplayGain)
      {
        _CurrentReplayGainInfo.AlbumGain = null;
        _CurrentReplayGainInfo.AlbumPeak = null;
        _CurrentReplayGainInfo.TrackGain = null;
        _CurrentReplayGainInfo.TrackPeak = null;

        // Find ID3V2 tags first, if not there, look for native tags.
        string[] id3v2Tags = Bass.BASS_ChannelGetTagsID3V2(_CurrentStream);
        if (id3v2Tags != null)
        {
          foreach (string tag in id3v2Tags)
          {
            //TXXX=replaygain_track_gain:-5.12 dB
            Log.Debug("Found ID3V2 tag: {0}", tag);

            int pos1 = tag.IndexOf("=");
            if (pos1 > -1)
            {
              int pos2 = tag.IndexOf(":", pos1);
              if (pos2 > -1)
              {
                string tagName = tag.Substring(pos1 + 1, pos2 - pos1 - 1).Trim().ToLower();
                string tagValue = tag.Substring(pos2 + 1);
                SetRGTagValue(tagName, tagValue);
              }
            }
          }
        }
        else
        {
          // FLAC native
          // replaygain_track_gain=-5.71 dB

          TAG_INFO tags = new TAG_INFO();
          BassTags.BASS_TAG_GetFromFile(_CurrentStream, tags);
          if (tags.NativeTags != null)
          {
            foreach (string tag in tags.NativeTags)
            {
              Log.Debug("Found native tag: {0}", tag);

              int pos1 = tag.IndexOf("=");
              if (pos1 > -1)
              {
                string tagName = tag.Substring(0, pos1).Trim().ToLower();
                string tagValue = tag.Substring(pos1 + 1);
                SetRGTagValue(tagName, tagValue);
              }
            }
          }
        }

        Log.Info("Replay Gain Data: {0}", _CurrentReplayGainInfo.ToString());
      }
    }

    private void SetRGTagValue(string tagName, string tagValue)
    {
      switch (tagName)
      {
        case "replaygain_track_gain":
          _CurrentReplayGainInfo.TrackGain = ParseTagValue(tagValue);
          break;

        case "replaygain_track_peak":
          _CurrentReplayGainInfo.TrackPeak = ParseTagValue(tagValue);
          break;

        case "replaygain_album_gain":
          _CurrentReplayGainInfo.AlbumGain = ParseTagValue(tagValue);
          break;

        case "replaygain_album_peak":
          _CurrentReplayGainInfo.AlbumPeak = ParseTagValue(tagValue);
          break;
      }
    }

    private float? ParseTagValue(string s)
    {
      // Remove "dB"
      int pos = s.IndexOf(" ");
      if (pos > -1)
        s = s.Substring(0, pos);

      NumberFormatInfo formatInfo = new NumberFormatInfo();
      formatInfo.NumberDecimalSeparator = ".";
      formatInfo.PercentGroupSeparator = ",";
      formatInfo.NegativeSign = "-";

      float f;
      if (float.TryParse(s, NumberStyles.Number, formatInfo, out f))
        return new float?(f);
      else
        return new float?();
    }

    private bool SetReplayGain()
    {
      bool result = true;
      if (_Profile.UseReplayGain)
      {
        float? gain = null;

        if (!CurrentNeedsPassThrough)
        {
          if (_Profile.UseRGAlbumGain && _CurrentReplayGainInfo.AlbumGain.HasValue)
            gain = _CurrentReplayGainInfo.AlbumGain;
          else if (_CurrentReplayGainInfo.TrackGain.HasValue)
            gain = _CurrentReplayGainInfo.TrackGain;
        }

        if (gain.HasValue)
        {
          Log.Info("Setting Replay Gain to {0}dB...", gain.Value);
          _ReplayGainDSP.ChannelHandle = _DataStream;
          _ReplayGainDSP.Gain_dBV = gain.Value;

          result = _ReplayGainDSP.Start();
          if (!result)
            HandleBassError("_ReplayGainDSP.Start");
        }
        else
        {
          _ReplayGainDSP.Stop();
        }
      }
      return result;
    }

    private void GetStreamTags(int stream)
    {
      Log.Debug("BASSPlayer.GetStreamTags() called...");

      string[] tags = Bass.BASS_ChannelGetTagsICY(stream);
      if (StreamTagsChanged != null)
        StreamTagsChanged(this, tags);
    }

    private void StreamMetaTagSyncProc(int handle, int channel, int data, IntPtr user)
    {
      Log.Debug("BASSPlayer.StreamMetaTagSyncProc() called...");

      IntPtr tagPtr = Bass.BASS_ChannelGetTags(channel, BASSTag.BASS_TAG_META);
      if (tagPtr != IntPtr.Zero)
      {
        GetStreamMetaTags(tagPtr);
      }

      //// BASS_SYNC_META delivers a pointer to the metadata in data parameter...
      //if (data != 0)
      //{
      //  GetStreamMetaTags(new IntPtr(data));
      //}
    }

    private void GetStreamMetaTags(IntPtr tagPtr)
    {
      Log.Debug("BASSPlayer.GetStreamMetaTags() called...");

      string tag = String.Empty;
      try
      {
        tag = Marshal.PtrToStringAnsi(tagPtr);
      }
      catch (Exception)
      {
      }

      if (tag == _PrevMetaTag)
        return;

      _PrevMetaTag = tag;

      if (MetaStreamTagsChanged != null)
        MetaStreamTagsChanged(this, tag);
    }

    private void FreeStream(ref int stream)
    {
      if (stream != 0)
      {
        int tmpStream = stream;
        stream = 0;
        Bass.BASS_StreamFree(tmpStream);
      }
    }

    void Application_ApplicationExit(object sender, EventArgs e)
    {
      Dispose();
    }

    private void StopThreads()
    {
      Log.Debug("PureAudio: BASSPlayer.StopThreads() called...");

      if (_MainThread.IsAlive)
      {
        Log.Debug("_MainThread.IsAlive");

        _MainThreadAbortFlag = true;
        _WakeupMainThread.Set();
        _MainThread.Join();
      }

      if (_BufferUpdateThread.IsAlive)
      {
        Log.Debug("_BufferUpdateThread.IsAlive");

        _BufferUpdateThreadAbortFlag = true;
        _WakeupBufferUpdateThread.Set();
        _BufferUpdateThread.Join();
      }

      if (_MonitorThread.IsAlive)
      {
        Log.Debug("_MonitorThread.IsAlive");

        _MonitorThreadAbortFlag = true;
        _WakeupMonitorThread.Set();
        _MonitorThread.Join();
      }
    }

    private void HandleBassError(string methodName)
    {
      HandleError(ErrorCode.MiscError, "PureAudio: {0}() failed: {1}", methodName, Bass.BASS_ErrorGetCode());
    }

    private void HandleAsioEngineError(string methodName)
    {
      HandleError(ErrorCode.MiscError, "PureAudio: _ASIOEngine.{0} failed: {1} ({2}).", methodName, _ASIOEngine.Driver.GetErrorMessage(), _ASIOEngine.Driver.LastASIOError);
    }

    private void HandleError(ErrorCode errorCode, string message, params object[] args)
    {
      _LastError.ErrorCode = errorCode;
      _LastError.Message = String.Format(message, args);
      Log.Error(_LastError.ToString());
    }

    private void LoadAudioDecoderPlugins()
    {
      if (!_DecoderPluginsLoaded)
      {
        string appPath = System.Windows.Forms.Application.StartupPath;
        string decoderFolderPath = Path.Combine(appPath, @"musicplayer\plugins\audio decoders");

        Log.Info("PureAudio: Loading audio decoder add-ins from {0}...", decoderFolderPath);

        if (!Directory.Exists(decoderFolderPath))
        {
          Log.Error(@"PureAudio: Unable to find \musicplayer\plugins\audio decoders folder in MediaPortal.exe path.");
          return;
        }

        DirectoryInfo dirInfo = new DirectoryInfo(decoderFolderPath);
        FileInfo[] decoders = dirInfo.GetFiles();

        int pluginHandle = 0;
        int decoderCount = 0;
        int errorCount = 0;

        foreach (FileInfo file in decoders)
        {
          if (Path.GetExtension(file.FullName).ToLower() != ".dll")
            continue;

          Log.Debug("  PureAudio: Loading: {0}", file.Name);
          pluginHandle = Bass.BASS_PluginLoad(file.FullName);

          if (pluginHandle != 0)
          {
            _DecoderPluginHandles.Add(pluginHandle);
            decoderCount++;
            Log.Debug("  PureAudio: Added: {0}", file.Name);
          }

          else
          {
            BASSError error = Bass.BASS_ErrorGetCode();
            if (error == BASSError.BASS_ERROR_ALREADY)
              Log.Debug("  PureAudio: Already loaded: {0}", file.Name);
            else
            {
              errorCount++;
              Log.Error("  PureAudio: Unable to load: {0}: {1}", file.Name, error);
            }
          }
        }

        if (errorCount == 0)
        {
          if (decoderCount == 0)
            Log.Info("PureAudio: No Audio Decoders loaded; probably already loaded.");
          else
            Log.Info("PureAudio: Loaded {0} Audio Decoders.", decoderCount);
        }

        _DecoderPluginsLoaded = true;
      }
    }

    private void UnLoadAudioDecoderPlugins()
    {
      foreach (int pluginHandle in _DecoderPluginHandles)
        Bass.BASS_PluginFree(pluginHandle);

      _DecoderPluginHandles.Clear();
      _DecoderPluginsLoaded = false;
    }

    private void LoadSettings()
    {
      _Profile.LoadSettings();
      _PlayBackMode = _Profile.DefaultPlayBackMode;
    }

    private bool InitBASS()
    {
      bool result = true;
      if (_InitBASS)
      {
        Log.Debug("PureAudio: Initializing BASS library...");

        // Reset buit-in BASS player to:
        // - make sure it will be properly re-initialized on next use
        // - we have a clean surface to start with
        if (BassMusicPlayer.Initialized)
          BassMusicPlayer.FreeBass();

        result = BassRegistration.BassRegistration.Register();
        if (result)
        {
          // Initialize BASS to "no sound", in case we are going to play over 
          // WaveOut, the selected device will be initialized in InitDSDevice()
          result = Bass.BASS_Init(0, 44100, 0, IntPtr.Zero, Guid.Empty);
          if (!result)
          {
            if (Bass.BASS_ErrorGetCode() == BASSError.BASS_ERROR_ALREADY)
              result = true;
            else
              HandleBassError("Bass.BASS_Init");
          }
        }
        if (result)
        {
          // BASS_CONFIG_UPDATEPERIOD is set in InitDSDevice in case 
          // we are going to playback over DirectSound.
          Bass.BASS_SetConfig(BASSConfig.BASS_CONFIG_UPDATEPERIOD, 0);
          Bass.BASS_SetConfig(BASSConfig.BASS_CONFIG_GVOL_STREAM, 10000);
          Bass.BASS_SetConfig(BASSConfig.BASS_CONFIG_BUFFER, _Profile.BASSPlayBackBufferSize);

          // network buffersize for webstreams should be larger then the playbackbuffer.
          // To insure a stable playback-start.
          int netBufferSize = _Profile.PlayBackBufferSize;
          if (_Profile.OutputMode == OutputMode.DirectSound)
            netBufferSize += _Profile.BASSPlayBackBufferSize;

          // Minimize at default value.
          if (netBufferSize < 5000)
            netBufferSize = 5000;

          Bass.BASS_SetConfig(BASSConfig.BASS_CONFIG_NET_BUFFER, netBufferSize);

          // PreBuffer() takes care of this.
          Bass.BASS_SetConfig(BASSConfig.BASS_CONFIG_NET_PREBUF, 0);

          LoadAudioDecoderPlugins();

          // For Bass Wma: must be set after LoadAudioDecoderPlugins()
          Bass.BASS_SetConfig(BASSConfig.BASS_CONFIG_NET_PLAYLIST, 2);
        }
        _InitBASS = false;
      }
      return result;
    }

    private int WasApiProc(IntPtr buffer, int length, IntPtr user)
    {
      int read = Bass.BASS_ChannelGetData(_WASAPIOutputStream, buffer, length);
      // ignore errors (read = -1)
      return read > 0 ? read : 0;
    }
  }
}
