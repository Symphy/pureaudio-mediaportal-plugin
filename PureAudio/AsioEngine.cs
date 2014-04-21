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
using BlueWave.Interop.Asio;
using Un4seen.Bass;

namespace MediaPortal.Player.PureAudio.Asio
{
  public class AsioEngine
  {
    private AsioDriver _Driver;

    private bool _DriverLoaded;
    private Thread _MainThread;
    private AutoResetEvent _WakeupMainThread;
    private bool _MainThreadAbortFlag = false;
    private delegate bool InitCommandDelegate(int asioDriver, IntPtr sysHandle, bool useMaxBuffer);
    private delegate bool StartCommandDelegate();
    private delegate bool StopCommandDelegate();
    private delegate void CommandDelegate();
    private Queue<Command> _RequestQueue = new Queue<Command>();

    private int _BassStream;
    private BASS_CHANNELINFO _BassChannelInfo;

    private float[] _Buffer = new float[0];

    private int _FirstChannel;
    private int _ChannelCount;

    public AsioDriver Driver
    {
      get { return _Driver; }
    }

    public int BassStream
    {
      get { return _BassStream; }
    }

    public AsioEngine()
    {
      _WakeupMainThread = new AutoResetEvent(false);
      _MainThread = new Thread(new ThreadStart(ThreadMain));
      // ASIO requires STA for proper functioning.
      _MainThread.SetApartmentState(ApartmentState.STA);
      _MainThread.Start();
    }

    private void ThreadMain()
    {
      try
      {
        while (!_MainThreadAbortFlag)
        {
          if (_RequestQueue.Count == 0)
            _WakeupMainThread.WaitOne();

          if (_RequestQueue.Count > 0)
          {
            Command request = _RequestQueue.Dequeue();
            request.Invoke();
          }
        }
      }
      catch (Exception e)
      {
        Log.Error("PureAudio: Exception in ASIO thread: {0}", e.ToString());
      }
    }

    public bool InitDriver(int asioDriver, IntPtr sysHandle, bool useMaxBuffer)
    {
      Command command = EnQueueCommand(new Command(new InitCommandDelegate(InternalInitDriver), asioDriver, sysHandle, useMaxBuffer));
      command.WaitHandle.WaitOne();

      if (command.Result)
        _DriverLoaded = true;

      return command.Result;
    }

    private bool InternalInitDriver(int asioDriver, IntPtr sysHandle, bool useMaxBuffer)
    {
      InternalReleaseDriver();

      InstalledDriver driver = AsioDriver.InstalledDrivers[asioDriver];
      _Driver = AsioDriver.SelectDriver(driver, sysHandle);

      bool result = (_Driver != null);
      if (result)
      {
        result = _Driver.CreateBuffers(useMaxBuffer);
      }

      if (result)
      {
        _Driver.BufferUpdate += new EventHandler(_Driver_BufferUpdate);

        _FirstChannel = 0;
        _ChannelCount = _Driver.OutputChannels.Length;
      }
      return result;
    }

    public void SetChannels(int firstChannel, int channelCount)
    {
      // this should not be called while playing! 

      _FirstChannel = firstChannel;
      _ChannelCount = channelCount;
    }

    public void SetBassStream(int stream)
    {
      _BassChannelInfo = Bass.BASS_ChannelGetInfo(stream);
      _BassStream = stream;
    }

    public bool Start()
    {
      Command command = EnQueueCommand(new Command(new StartCommandDelegate(InternalStart)));
      command.WaitHandle.WaitOne();
      return command.Result;
    }

    private bool InternalStart()
    {
      int bufferSize = _Driver.BufferSize * _ChannelCount;
      if (_Buffer.Length != bufferSize)
        Array.Resize<float>(ref _Buffer, bufferSize);

      SetOptimizeValues();
      
      return _Driver.Start();
    }

    public bool Stop()
    {
      Command command = EnQueueCommand(new Command(new StopCommandDelegate(InternalStop)));
      command.WaitHandle.WaitOne();
      return command.Result;
    }

    private bool InternalStop()
    {
      bool result = _Driver.Stop();
      return result;
    }

    public void ClearOutputBuffers()
    {
      int bufferSize = _Driver.BufferSize;
      //Log.Debug("Clearing asio buffers {0}", bufferSize);
      foreach (Channel channel in _Driver.OutputChannels)
      {
        for (int i = 0; i < bufferSize; i++)
        {
          channel[i] = 0.0f;
        }
      }
    }

    public void ReleaseDriver()
    {
      if (!_DriverLoaded)
        return;

      Command command = EnQueueCommand(new Command(new CommandDelegate(InternalReleaseDriver)));
      command.WaitHandle.WaitOne();

      _DriverLoaded = false;
    }

    private void InternalReleaseDriver()
    {
      if (_Driver != null)
      {
        _Driver.Stop();
        _Driver.DisposeBuffers();
        _Driver.Release();

        _Driver = null;

        _FirstChannel = 0;
        _ChannelCount = 0;
      }
      return;
    }

    private Command EnQueueCommand(Command command)
    {
      _RequestQueue.Enqueue(command);
      _WakeupMainThread.Set();

      return command;
    }

    // Storage of some values to optimize the buffer update callback
    private int _OptBufferByteCount;
    private int _OptLastChannel;
    private int _OptBufferLength;
    private Channel[] _OptChannels;

    private void SetOptimizeValues()
    {
      _OptBufferByteCount = _Buffer.Length * 4;
      _OptLastChannel = _FirstChannel + _ChannelCount - 1;
      _OptBufferLength = _Buffer.Length;
      _OptChannels = _Driver.OutputChannels;
    }
    
    private void _Driver_BufferUpdate(object sender, EventArgs e)
    {
      int samplesRead = Bass.BASS_ChannelGetData(_BassStream, _Buffer, _OptBufferByteCount) / 4;

      int channelIndex = _FirstChannel;
      int channelSample = 0;

      for (int index = 0; index < _OptBufferLength; index++)
      {
        //if (channelIndex <= _OptLastChannel)
          if (index < samplesRead)
            _OptChannels[channelIndex][channelSample] = _Buffer[index];
          else
            _OptChannels[channelIndex][channelSample] = 0.0f;

        channelIndex++;
        if (channelIndex > _OptLastChannel)
        {
          channelIndex = _FirstChannel;
          channelSample++;
        }
      }
    }

    public void Dispose()
    {
      ReleaseDriver();
      StopThreads();
    }

    private void StopThreads()
    {
      Log.Debug("PureAudio: ASIOEngine.StopThreads() called...");

      if (_MainThread.IsAlive)
      {
        _MainThreadAbortFlag = true;
        _WakeupMainThread.Set();
        _MainThread.Join();
      }
    }

    private class Command
    {
      private Delegate _method;
      private object[] _args = null;
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
          return _result == null ? false : (bool)_result;
        }
      }

      public Command(Delegate method, params object[] args)
      {
        _method = method;
        _args = args;
      }

      internal void Invoke()
      {
        _result = _method.DynamicInvoke(_args);
        _event.Set();
      }
    }
  }
}
