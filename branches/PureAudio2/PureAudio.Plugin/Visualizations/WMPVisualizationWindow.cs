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
using System.Timers;
using System.Threading;
using MediaPortal.Plugins.PureAudio.WMPEffectsInterop;
using MediaPortal.GUI.Library;
using Un4seen.Bass;

namespace MediaPortal.Plugins.PureAudio
{
  public class WMPVisualizationWindow : BaseVisualizationWindow
  {
    public const int MaxSamples = 1024;
    public const int MaxChannels = 2;

    private float[] _WaveDataBuffer = new float[MaxSamples * 2];
    private float[] _FFTDataBuffer = new float[MaxSamples * 2];

    private float[,] _WaveData = new float[2, 1024];
    private float[,] _FFTData = new float[2, 1024];

    private delegate void RenderDelegate();

    private WMPEffect _WMPEffect;
    private BassPlayerSettings _Settings;
    private RenderDelegate _RenderDelegate;
    private Thread _RenderThread;
    private AutoResetEvent _WakeupRenderThread;
    private System.Timers.Timer _Timer;

    private bool CanRender
    {
      get
      {
        return Visible && _WMPEffect != null;
      }
    }

    public WMPVisualizationWindow(BassPlayerSettings settings)
    {
      this._Settings = settings;

      _RenderDelegate = new RenderDelegate(Render);

      _WMPEffect = WMPEffect.SelectEffect(_Settings.WMPEffectClsId, Handle);

      if (_WMPEffect != null && _Settings.WMPEffectPreset < _WMPEffect.Presets.Length)
        _WMPEffect.SetCurrentPreset(_WMPEffect.Presets[0]);

      _WakeupRenderThread = new AutoResetEvent(false);
      _RenderThread = new Thread(new ThreadStart(RenderThread));
      _RenderThread.IsBackground = true;
      _RenderThread.Start();

      _Timer = new System.Timers.Timer(1000 / _Settings.WMPEffectFps);
      _Timer.Elapsed += new ElapsedEventHandler(_Timer_Elapsed);
      _Timer.Start();

    }

    void _Timer_Elapsed(object sender, ElapsedEventArgs e)
    {
      if (CanRender)
        _WakeupRenderThread.Set();
      else
        _Timer.Stop();
    }

    protected override void SetVisibleCore(bool value)
    {
      base.SetVisibleCore(value);

      if (CanRender)
        _Timer.Start();
    }

    private void RenderThread()
    {
      try
      {
        while (true)
        {
          if (CanRender)
          {
            PrepareRenderData();
            Invoke(_RenderDelegate);
          }
          _WakeupRenderThread.WaitOne();
        }
      }
      catch (Exception e)
      {
        Log.Error("PureAudio: Exception in WMPViz thread: {0}", e.ToString());
      }
    }

    private void PrepareRenderData()
    {
      if (BassStream.HasValue)
      {
        // Assume 2 channels floating point
        int read = Bass.BASS_ChannelGetData(BassStream.Value, _WaveDataBuffer, _WaveDataBuffer.Length * 4);
        if (read > 0)
        {
          int audioDataChan = 0;
          int audioDataIndex = 0;
          for (int index = 0; index < _WaveDataBuffer.Length; index++)
          {
            _WaveData[audioDataChan, audioDataIndex] = _WaveDataBuffer[index];

            if (audioDataChan == 0)
              audioDataChan = 1;
            else
            {
              audioDataChan = 0;
              audioDataIndex++;
            }
          }
        }

        read = Bass.BASS_ChannelGetData(BassStream.Value, _FFTDataBuffer, (int)(BASSData.BASS_DATA_FFT2048 | BASSData.BASS_DATA_FFT_INDIVIDUAL));
        if (read > 0)
        {
          int fftDataChan = 0;
          int fftDataIndex = 0;
          
          // Start at index = 2; to skip the first fft samples being the DC components.
          // This will cause the last samples in _FFTData to be always empty, but who cares at 22050 Hz...
          for (int index = 2; index < _FFTDataBuffer.Length; index++)
          {
            _FFTData[fftDataChan, fftDataIndex] = _FFTDataBuffer[index];

            if (fftDataChan == 0)
              fftDataChan = 1;
            else
            {
              fftDataChan = 0;
              fftDataIndex++;
            }
          }
        }
      }
      else
      {
        Array.Clear(_WaveData, 0, _FFTData.Length);
        Array.Clear(_FFTData, 0, _FFTData.Length);
      }
    }

    private void Render()
    {
      _WMPEffect.Render(_WaveData, _FFTData, Handle, ClientRectangle);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing)
      {
        if (_WMPEffect != null)
          _WMPEffect.Release();
      }
      base.Dispose(disposing);
    }
  }
}
