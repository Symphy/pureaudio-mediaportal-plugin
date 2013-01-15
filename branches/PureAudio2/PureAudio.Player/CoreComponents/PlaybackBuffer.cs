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
using System.Threading;
using Un4seen.Bass;
using Un4seen.Bass.AddOn.Mix;

namespace MediaPortal.Plugins.PureAudio
{
  public partial class Player
  {
    /// <summary>
    /// Buffers the output stream to ensure stable playback. Also provides a synchronized stream for visualization purposes.
    /// </summary>
    private class PlaybackBuffer : IDisposable
    {
      #region Static members

      /// <summary>
      /// Creates and initializes an new instance.
      /// </summary>
      /// <param name="player">Reference to containing IPlayer object.</param>
      /// <returns>The new instance.</returns>
      public static PlaybackBuffer Create(Player player)
      {
        PlaybackBuffer playbackBuffer = new PlaybackBuffer(player);
        playbackBuffer.Initialize();
        return playbackBuffer;
      }

      #endregion

      #region Fields

      private int _BufferUpdateIntervalMS;
      private int _ReadOffsetSamples;
      private int _VizReadOffsetSamples;

      private bool _Initialized;

      private float[] _ReadData = new float[1];

      private AutoResetEvent _BufferUpdated;
      private AutoResetEvent _NotifyBufferUpdateThread;
      private AutoResetEvent _BufferUpdateThreadPaused;

      private BassStream _InputStream;
      private BassStream _OutputStream;
      private BassStream _VizRawStream;
      private BassStream _VizStream;

      private TimeSpan _BufferSize;
      private TimeSpan _ReadOffset;
      private TimeSpan _VizReadOffset;

      private STREAMPROC _StreamWriteProcDelegate;
      private STREAMPROC _VizRawStreamWriteProcDelegate;

      private AudioRingBuffer _Buffer;
      private Player _Player;
      private Silence _Silence;
      private Thread _BufferUpdateThread;

      #endregion

      #region IDisposable Members

      public void Dispose()
      {
        Log.Debug("PlaybackBuffer.Dispose()");

        ResetInputStream();
      }

      #endregion

      #region Public members

      /// <summary>
      /// Gets the current inputstream as set with SetInputStream.
      /// </summary>
      public BassStream InputStream
      {
        get { return _InputStream; }
      }

      /// <summary>
      /// Gets the output Bass stream.
      /// </summary>
      public BassStream OutputStream
      {
        get { return _OutputStream; }
      }

      /// <summary>
      /// Gets the visualization Bass stream.
      /// </summary>
      public BassStream VizStream
      {
        get { return _VizStream; }
      }

      /// <summary>
      /// Sets the Bass inputstream and initializes the playbackbuffer.
      /// </summary>
      /// <param name="stream"></param>
      public void SetInputStream(BassStream stream)
      {
        Log.Debug("PlaybackBuffer.SetInputStream()");

        ResetInputStream();

        _InputStream = stream;
        _ReadOffsetSamples = AudioRingBuffer.CalculateLength(stream.SampleRate, stream.Channels, _ReadOffset);

        Log.Debug("Output stream reading offset: {0} ms", _ReadOffset.TotalMilliseconds);

        UpdateVizLatencyCorrection();

        _Buffer = new AudioRingBuffer(stream.SampleRate, stream.Channels, _BufferSize + _ReadOffset);
        _Buffer.ResetPointers(_ReadOffsetSamples);

        CreateOutputStream();
        CreateVizStream();

        _Initialized = true;

        // ensure prebuffering.
        _BufferUpdated.Reset();
        _NotifyBufferUpdateThread.Set();
        _BufferUpdated.WaitOne();
      }

      /// <summary>
      /// Resets and clears the buffer.
      /// </summary>
      public void ClearBuffer()
      {
        _Buffer.Clear();
        _Buffer.ResetPointers(_ReadOffsetSamples);
      }

      /// <summary>
      /// Resets the instance to its uninitialized state.
      /// </summary>
      public void ResetInputStream()
      {
        Log.Debug("PlaybackBuffer.ResetInputStream()");

        if (_Initialized)
        {
          // Pause the bufferupdate thread.

          _BufferUpdateThreadPaused.Reset();
          _Initialized = false;
          _NotifyBufferUpdateThread.Set();
          _BufferUpdateThreadPaused.WaitOne();

          Log.Debug("Disposing output stream");

          _OutputStream.Dispose();
          _OutputStream = null;

          Log.Debug("Disposing visualization stream");

          _VizStream.Dispose();
          _VizStream = null;

          Log.Debug("Disposing visualization raw stream");

          _VizRawStream.Dispose();
          _VizRawStream = null;

          _Buffer = null;
          _InputStream = null;
          _ReadOffsetSamples = 0;
          _VizReadOffsetSamples = 0;
        }
      }

      #endregion

      #region Private members

      private PlaybackBuffer(Player player)
      {
        _Player = player;
      }

      /// <summary>
      /// Initializes a new instance.
      /// </summary>
      private void Initialize()
      {
        _Silence = new Silence();

        _BufferSize = _Player._Settings.PlaybackBufferSize;
        _BufferUpdateIntervalMS = (int)_BufferSize.TotalMilliseconds / 5;

        _ReadOffset = InternalSettings.VizLatencyCorrectionRange;

        _StreamWriteProcDelegate = new STREAMPROC(OutputStreamWriteProc);
        _VizRawStreamWriteProcDelegate = new STREAMPROC(VizRawStreamWriteProc);

        _NotifyBufferUpdateThread = new AutoResetEvent(false);
        _BufferUpdateThreadPaused = new AutoResetEvent(false);
        _BufferUpdated = new AutoResetEvent(false);

        _BufferUpdateThread = new Thread(new ThreadStart(ThreadBufferUpdate));
        _BufferUpdateThread.IsBackground = true;
        _BufferUpdateThread.Name = "PureAudio.PlaybackBuffer";
        _BufferUpdateThread.Start();
      }

      /// <summary>
      /// Reclculates the visualization stream byte-offset according usersettings.
      /// </summary>
      private void UpdateVizLatencyCorrection()
      {
        Log.Debug("PlaybackBuffer.UpdateVizLatencyCorrection()");

        _VizReadOffset = InternalSettings.VizLatencyCorrectionRange.Add(_Player._Settings.VisualizationLatencyCorrection);
        _VizReadOffsetSamples = AudioRingBuffer.CalculateLength(_InputStream.SampleRate, _InputStream.Channels, _VizReadOffset);

        Log.Debug("Vizstream reading offset: {0} ms", _VizReadOffset.TotalMilliseconds);
      }

      /// <summary>
      /// Creates the outputstream.
      /// </summary>
      private void CreateOutputStream()
      {
        Log.Debug("Creating output stream");

        BASSFlag flags =
          BASSFlag.BASS_SAMPLE_FLOAT |
          BASSFlag.BASS_STREAM_DECODE;

        int handle = Bass.BASS_StreamCreate(
            _InputStream.SampleRate,
            _InputStream.Channels,
            flags,
            _StreamWriteProcDelegate,
            IntPtr.Zero);

        if (handle == BassConstants.BassInvalidHandle)
          throw new BassLibraryException("BASS_StreamCreate");

        _OutputStream = BassStream.Create(handle);
      }

      /// <summary>
      /// Creates the visualization Bass stream.
      /// </summary>
      private void CreateVizStream()
      {
        Log.Debug("Creating visualization raw stream");

        BASSFlag streamFlags =
            BASSFlag.BASS_STREAM_DECODE |
            BASSFlag.BASS_SAMPLE_FLOAT;

        int handle = Bass.BASS_StreamCreate(
            _InputStream.SampleRate,
            _InputStream.Channels,
            streamFlags,
            _VizRawStreamWriteProcDelegate,
            IntPtr.Zero);

        if (handle == BassConstants.BassInvalidHandle)
          throw new BassLibraryException("BASS_StreamCreate");

        _VizRawStream = BassStream.Create(handle);

        // Todo: apply AGC

        Log.Debug("Creating visualizationstream");

        streamFlags =
            BASSFlag.BASS_MIXER_NONSTOP |
            BASSFlag.BASS_SAMPLE_FLOAT |
            BASSFlag.BASS_STREAM_DECODE;

        handle = BassMix.BASS_Mixer_StreamCreate(_InputStream.SampleRate, 2, streamFlags);
        if (handle == BassConstants.BassInvalidHandle)
          throw new BassLibraryException("BASS_StreamCreate");

        _VizStream = BassStream.Create(handle);

        streamFlags =
            BASSFlag.BASS_MIXER_NORAMPIN |
            BASSFlag.BASS_MIXER_DOWNMIX |
            BASSFlag.BASS_MIXER_MATRIX;

        if (!BassMix.BASS_Mixer_StreamAddChannel(_VizStream.Handle, _VizRawStream.Handle, streamFlags))
          throw new BassLibraryException("BASS_Mixer_StreamAddChannel");

        if (_InputStream.Channels == 1)
        {
          float[,] mixMatrix = new float[2, 1];
          mixMatrix[0, 0] = 1;
          mixMatrix[1, 0] = 1;

          if (!BassMix.BASS_Mixer_ChannelSetMatrix(_VizRawStream.Handle, mixMatrix))
            throw new BassLibraryException("BASS_Mixer_ChannelSetMatrix");
        }
      }

      /// <summary>
      /// Callback function for the output stream.
      /// </summary>
      /// <param name="streamHandle">Bass stream handle that requests sample data.</param>
      /// <param name="buffer">Buffer to write the sampledata in.</param>
      /// <param name="requestedBytes">Requested number of bytes.</param>
      /// <param name="userData"></param>
      /// <returns>Number of bytes read.</returns>
      private int OutputStreamWriteProc(int streamHandle, IntPtr buffer, int requestedBytes, IntPtr userData)
      {
        int read = _Buffer.Read(buffer, requestedBytes / BassConstants.FloatBytes, _ReadOffsetSamples);
        return read * BassConstants.FloatBytes;
      }

      /// <summary>
      /// Callback function for the visualizationstream.
      /// </summary>
      /// <param name="streamHandle">Bass stream handle that requests sample data.</param>
      /// <param name="buffer">Buffer to write the sampledata in.</param>
      /// <param name="requestedBytes">Requested number of bytes.</param>
      /// <param name="userData"></param>
      /// <returns>Number of bytes read.</returns>
      private int VizRawStreamWriteProc(int streamHandle, IntPtr buffer, int requestedBytes, IntPtr userData)
      {
        if (_Player._Controller.ExternalState == PlaybackState.Playing)
        {

          int read = _Buffer.Peek(buffer, requestedBytes / BassConstants.FloatBytes, _VizReadOffsetSamples);
          return read * BassConstants.FloatBytes;
        }
        else
        {
          return _Silence.Write(buffer, requestedBytes);
        }
      }

      /// <summary>
      /// Bufferupdate thread loop.
      /// </summary>
      private void ThreadBufferUpdate()
      {
        try
        {
          // This is a background thread
          while (true)
          {
            // Test for _Initialized because _InputStream and _Buffer may get released.
            if (_Initialized)
            {
              int requestedSamples = _Buffer.Space;
              if (requestedSamples > 0)
              {
                if (_ReadData.Length < requestedSamples)
                  Array.Resize<float>(ref _ReadData, requestedSamples);

                int samplesRead = _InputStream.Read(_ReadData, requestedSamples);
                _Buffer.Write(_ReadData, samplesRead);
                
              }
              _BufferUpdated.Set();
              _NotifyBufferUpdateThread.WaitOne(_BufferUpdateIntervalMS, false);
            }
            else
            {
              Log.Debug("Thread paused");
              
              _BufferUpdateThreadPaused.Set();
              _NotifyBufferUpdateThread.WaitOne();
              
              Log.Debug("Thread resumed");
            }
          }
        }
        catch (Exception e)
        {
          throw new BassPlayerException("Exception in bufferupdate thread.", e);
        }
      }

      #endregion
    }
  }
}