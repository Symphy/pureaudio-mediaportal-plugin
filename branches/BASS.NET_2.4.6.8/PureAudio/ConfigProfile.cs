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
using System.IO;
using System.Collections.Generic;
using System.Windows.Forms;
using MediaPortal.Configuration;
using MediaPortal.GUI.Library;

namespace MediaPortal.Player.PureAudio
{
  public class ConfigProfile
  {
    public const string ConfigFile = "PureAudio.xml";
    public const string ConfigSection = "PureAudio";

    public static class PropNames
    {
      public const string ASIODevice = "ASIODevice";
      public const string ASIOFirstChan = "ASIOFirstChan";
      public const string ASIOLastChan = "ASIOLastChan";
      public const string BASSPlayBackBufferSize = "BASSPlayBackBufferSize";
      public const string DefaultPlayBackMode = "DefaultPlayBackMode";
      public const string DirectSoundDevice = "SoundDevice";
      public const string DoSoftStop = "DoSoftStop";
      public const string Extensions = "Extensions";
      public const string FiveDotOneUpMix = "FiveDotOneUpMix";
      public const string ForceMaxASIORate = "ForceMaxASIORate";
      public const string ForceMinASIORate = "ForceMinASIORate";
      public const string GapLength = "GapLength";
      public const string MonoUpMix = "MonoUpMix";
      public const string PlayBackBufferSize = "Delay";
      public const string QuadraphonicUpMix = "QuadraphonicUpMix";
      public const string SeekIncrement = "SeekIncrement";
      public const string SoftStopDuration = "SoftStopDuration";
      public const string SonVizFFTAmp = "SonVizFFTAmp";
      public const string SonVizFFTSkipCount = "SonVizFFTSkipCount";
      public const string SonVizSlowFade = "SonVizSlowFade";
      public const string SonVizWaveSkipCount = "SonVizWaveSkipCount";
      public const string StereoUpMix = "StereoUpMix";
      public const string UseASIO = "UseASIO";
      public const string UseForCDDA = "UseForCDDA";
      public const string UseForWebStream = "UseForWebStream";
      public const string UseForLastFMWebStream = "UseForLastFMWebStream";
      public const string UseMaxASIOBufferSize = "UseMaxASIOBufferSize";
      public const string UseOverSampling = "UseOverSampling";
      public const string UseReplayGain = "UseReplayGain";
      public const string UseRGAlbumGain = "UseRGAlbumGain";
      public const string UseVizAGC = "UseVizAGC";
      public const string VizLatencyCorrection = "VizLatencyCorrection";
      public const string VizType = "VizType";
      public const string WMPVizClsId = "WMPVizClsId";
      public const string WMPVizPreset = "WMPVizPreset";
      public const string WMPVizFps = "WMPVizFps";
      public const string WMPVizFFTFallBack = "WMPVizFFTFallBack";
      public const string WMPVizFFTMinimum = "WMPVizFFTMinimum";
      public const string WMPVizFFTHalf = "WMPVizFFTHalf";
    }

    public static class Defaults
    {
      public const string ASIODevice = "";
      public const int ASIOFirstChan = -1;
      public const int ASIOLastChan = -1;
      public const double BassDSPGainDb = 0;
      public const int BassDSPDynAmpPreset = (int)MediaPortal.Player.PureAudio.BassDSPDynAmpPreset.None;
      public const double BassDSPCompThresholdDb = 0;
      public const int BASSPlayBackBufferSize = 200; //ms
      public const int DefaultPlayBackMode = 0;
      public const string DirectSoundDevice = "Default Sound Device";
      public const bool DoSoftStop = true;
      public const int FiveDotOneUpMix = (int)MediaPortal.Player.PureAudio.FiveDotOneUpMix.None;
      public const int ForceMaxASIORate = 0; // kHz
      public const int ForceMinASIORate = 0; // kHz
      public const int GapLength = 1000; //ms
      public const int MonoUpMix = (int)MediaPortal.Player.PureAudio.MonoUpMix.Stereo;
      public const int PlayBackBufferSize = 2000; //ms
      public const int QuadraphonicUpMix = (int)MediaPortal.Player.PureAudio.QuadraphonicUpMix.None;
      public const int SeekIncrement = 20; // sec
      public const int SoftStopDuration = 500; //ms
      public const int SonVizFFTAmp = 48;
      public const int SonVizFFTSkipCount = 0;
      public const int SonVizSlowFade = 24;
      public const int SonVizWaveSkipCount = 0;
      public const int StereoUpMix = (int)MediaPortal.Player.PureAudio.StereoUpMix.None;
      public const bool UseASIO = false;
      public const bool UseBassDSPGain = false;
      public const bool UseBassDSPDynAmp = false;
      public const bool UseBassDSPComp = false;
      public const bool UseMaxASIOBufferSize = true;
      public const bool UseForCDDA = true;
      public const bool UseForWebStream = true;
      public const bool UseForLastFMWebStream = false;
      public const bool UseOverSampling = false;
      public const bool UseReplayGain = false;
      public const bool UseRGAlbumGain = false;
      public const bool UseVizAGC = true;
      public const int VizLatencyCorrection = 0; //ms
      public const int VizType = (int)VisualizationType.None;
      public const string WMPVizClsId = "";
      public const int WMPVizPreset = 0;
      public const int WMPVizFps = 30;
      public const int WMPVizFFTFallBack = 0; //ms;
      public const bool WMPVizFFTHalf = false;
      public const int WMPVizFFTMinimum = -90; // dB

      public const string Extensions =
        ".asx,.dts," +
        // Bass
        // .mpg,.mpeg,
        ".mod,.mo3,.s3m,.xm,.it,.mtm,.umx,.mdz,.s3z,.itz,.xmz," +
        ".mp3,.ogg,.wav,.mp2,.mp1,.aiff,.m2a,.mpa,.m1a,.swa,.aif,.mp3pro," +
        // BassCD
        ".cda," +
        // BassAac
        ".aac,.mp4,.m4a,.m4b," +
        // BassAc3
        ".ac3," +
        // BassAlac
        // .mov,
        ".m4a,.aac,.mp4," +
        // BassApe
        ".ape,.apl," +
        // BassFlac
        ".flac," +
        // BassMidi
        ".midi,.mid,.rmi,.kar," +
        // BassMpc
        ".mpc,.mpp,.mp+," +
        // BassOfr
        ".ofr,.ofs," +
        // BassSpx
        ".spx," +
        // BassTta
        ".tta," +
        // BassWma
        // .wmv,
        ".wma," +
        // BassWv
        ".wv";
    }

    private VSTPluginCollection _VSTPlugins = new VSTPluginCollection();
    private WADSPPluginCollection _WADSPPlugins = new WADSPPluginCollection();

    public string ASIODevice { get; set; }
    public int ASIOFirstChan { get; set; }
    public int ASIOLastChan { get; set; }
    public double BassDSPGainDb { get; set; }
    public BassDSPDynAmpPreset BassDSPDynAmpPreset { get; set; }
    public double BassDSPCompThresholdDb { get; set; }
    public int BASSPlayBackBufferSize { get; set; }
    public PlayBackMode DefaultPlayBackMode { get; set; }
    public string DirectSoundDevice { get; set; }
    public bool DoSoftStop { get; set; }
    public string Extensions { get; set; }
    public FiveDotOneUpMix FiveDotOneUpMix { get; set; }
    public int ForceMaxASIORate { get; set; }
    public int ForceMinASIORate { get; set; }
    public int GapLength { get; set; }
    public MonoUpMix MonoUpMix { get; set; }
    public int PlayBackBufferSize { get; set; }
    public QuadraphonicUpMix QuadraphonicUpMix { get; set; }
    public int SeekIncrement { get; set; }
    public int SoftStopDuration { get; set; }
    public int SonVizFFTAmp { get; set; }
    public int SonVizFFTSkipCount { get; set; }
    public int SonVizSlowFade { get; set; }
    public int SonVizWaveSkipCount { get; set; }
    public StereoUpMix StereoUpMix { get; set; }
    public bool UseASIO { get; set; }
    public bool UseBassDSPGain { get; set; }
    public bool UseBassDSPDynAmp { get; set; }
    public bool UseBassDSPComp { get; set; }
    public bool UseForCDDA { get; set; }
    public bool UseForWebStream { get; set; }
    public bool UseForLastFMWebStream { get; set; }
    public bool UseMaxASIOBufferSize { get; set; }
    public bool UseOverSampling { get; set; }
    public bool UseReplayGain { get; set; }
    public bool UseRGAlbumGain { get; set; }
    public bool UseVizAGC { get; set; }
    public int VizLatencyCorrection { get; set; }
    public int VizType { get; set; }
    public string WMPVizClsId { get; set; }
    public int WMPVizPreset { get; set; }
    public int WMPVizFps { get; set; }
    public VSTPluginCollection VSTPlugins
    {
      get { return _VSTPlugins; }
    }
    public WADSPPluginCollection WADSPPlugins
    {
      get { return _WADSPPlugins; }
    }
    public int WMPVizFFTFallBack { get; set; }
    public bool WMPVizFFTHalf { get; set; }
    public int WMPVizFFTMinimum { get; set; }

    public ConfigProfile()
    {
      ASIODevice = Defaults.ASIODevice;
      ASIOFirstChan = Defaults.ASIOFirstChan;
      ASIOLastChan = Defaults.ASIOLastChan;
      BASSPlayBackBufferSize = Defaults.BASSPlayBackBufferSize; //msec
      BassDSPGainDb = Defaults.BassDSPGainDb;
      BassDSPDynAmpPreset = (BassDSPDynAmpPreset)Defaults.BassDSPDynAmpPreset;
      BassDSPCompThresholdDb = Defaults.BassDSPCompThresholdDb;
      DefaultPlayBackMode = (PlayBackMode)Defaults.DefaultPlayBackMode;
      DirectSoundDevice = Defaults.DirectSoundDevice;
      DoSoftStop = Defaults.DoSoftStop;
      Extensions = Defaults.Extensions;
      FiveDotOneUpMix = (FiveDotOneUpMix)Defaults.FiveDotOneUpMix;
      ForceMaxASIORate = Defaults.ForceMaxASIORate;
      ForceMinASIORate = Defaults.ForceMinASIORate;
      GapLength = Defaults.GapLength; //msec
      MonoUpMix = (MonoUpMix)Defaults.MonoUpMix;
      PlayBackBufferSize = Defaults.PlayBackBufferSize; //msec
      QuadraphonicUpMix = (QuadraphonicUpMix)Defaults.QuadraphonicUpMix;
      SeekIncrement = Defaults.SeekIncrement; //sec
      SoftStopDuration = Defaults.SoftStopDuration;  //msec
      SonVizFFTAmp = Defaults.SonVizFFTAmp;
      SonVizFFTSkipCount = Defaults.SonVizFFTSkipCount;
      SonVizSlowFade = Defaults.SonVizSlowFade;
      SonVizWaveSkipCount = Defaults.SonVizWaveSkipCount;
      StereoUpMix = (StereoUpMix)Defaults.StereoUpMix;
      UseASIO = Defaults.UseASIO;
      UseBassDSPGain = Defaults.UseBassDSPGain;
      UseBassDSPDynAmp = Defaults.UseBassDSPDynAmp;
      UseBassDSPComp = Defaults.UseBassDSPComp;
      UseForCDDA = Defaults.UseForCDDA;
      UseForWebStream = Defaults.UseForWebStream;
      UseForLastFMWebStream = Defaults.UseForLastFMWebStream;
      UseMaxASIOBufferSize = Defaults.UseMaxASIOBufferSize;
      UseOverSampling = Defaults.UseOverSampling;
      UseReplayGain = Defaults.UseReplayGain;
      UseRGAlbumGain = Defaults.UseRGAlbumGain;
      UseVizAGC = Defaults.UseVizAGC;
      VizLatencyCorrection = Defaults.VizLatencyCorrection;

      VizType = Defaults.VizType;
      WMPVizClsId = Defaults.WMPVizClsId;
      WMPVizPreset = Defaults.WMPVizPreset;
      WMPVizFps = Defaults.WMPVizFps;
      WMPVizFFTFallBack = Defaults.WMPVizFFTFallBack;
      WMPVizFFTHalf = Defaults.WMPVizFFTHalf;
      WMPVizFFTMinimum = Defaults.WMPVizFFTMinimum;
    }

    public void LoadSettings()
    {
      //using (Profile.Settings xmlreader = new Profile.MPSettings())
      using (MediaPortal.Profile.Settings xmlreader = new MediaPortal.Profile.Settings(Config.GetFile(Config.Dir.Config, ConfigFile)))
      {
        string section = ConfigSection;

        ASIODevice =
          xmlreader.GetValueAsString(section, PropNames.ASIODevice, Defaults.ASIODevice);

        ASIOFirstChan =
          xmlreader.GetValueAsInt(section, PropNames.ASIOFirstChan, Defaults.ASIOFirstChan);

        ASIOLastChan =
          xmlreader.GetValueAsInt(section, PropNames.ASIOLastChan, Defaults.ASIOLastChan);

        BASSPlayBackBufferSize =
          xmlreader.GetValueAsInt(section, PropNames.BASSPlayBackBufferSize, Defaults.BASSPlayBackBufferSize);

        DefaultPlayBackMode = (PlayBackMode)
          xmlreader.GetValueAsInt(section, PropNames.DefaultPlayBackMode, Defaults.DefaultPlayBackMode);

        DoSoftStop =
          xmlreader.GetValueAsBool(section, PropNames.DoSoftStop, Defaults.DoSoftStop);

        Extensions =
          xmlreader.GetValueAsString(section, PropNames.Extensions, Defaults.Extensions);

        FiveDotOneUpMix = (FiveDotOneUpMix)
          xmlreader.GetValueAsInt(section, PropNames.FiveDotOneUpMix, Defaults.FiveDotOneUpMix);

        GapLength =
          xmlreader.GetValueAsInt(section, PropNames.GapLength, Defaults.GapLength);

        ForceMaxASIORate =
          xmlreader.GetValueAsInt(section, PropNames.ForceMaxASIORate, Defaults.ForceMaxASIORate);

        ForceMinASIORate =
          xmlreader.GetValueAsInt(section, PropNames.ForceMinASIORate, Defaults.ForceMinASIORate);

        MonoUpMix = (MonoUpMix)
          xmlreader.GetValueAsInt(section, PropNames.MonoUpMix, Defaults.MonoUpMix);

        PlayBackBufferSize =
          xmlreader.GetValueAsInt(section, PropNames.PlayBackBufferSize, Defaults.PlayBackBufferSize);

        QuadraphonicUpMix = (QuadraphonicUpMix)
          xmlreader.GetValueAsInt(section, PropNames.QuadraphonicUpMix, Defaults.QuadraphonicUpMix);

        SeekIncrement =
          xmlreader.GetValueAsInt(section, PropNames.SeekIncrement, Defaults.SeekIncrement);

        SoftStopDuration =
          xmlreader.GetValueAsInt(section, PropNames.SoftStopDuration, Defaults.SoftStopDuration);

        SonVizFFTAmp =
          xmlreader.GetValueAsInt(section, PropNames.SonVizFFTAmp, Defaults.SonVizFFTAmp);

        SonVizFFTSkipCount =
          xmlreader.GetValueAsInt(section, PropNames.SonVizFFTSkipCount, Defaults.SonVizFFTSkipCount);

        SonVizSlowFade =
          xmlreader.GetValueAsInt(section, PropNames.SonVizSlowFade, Defaults.SonVizSlowFade);

        SonVizWaveSkipCount =
          xmlreader.GetValueAsInt(section, PropNames.SonVizWaveSkipCount, Defaults.SonVizWaveSkipCount);

        DirectSoundDevice =
          xmlreader.GetValueAsString(section, PropNames.DirectSoundDevice, Defaults.DirectSoundDevice);

        StereoUpMix = (StereoUpMix)
          xmlreader.GetValueAsInt(section, PropNames.StereoUpMix, Defaults.StereoUpMix);

        UseASIO =
          xmlreader.GetValueAsBool(section, PropNames.UseASIO, Defaults.UseASIO);

        UseForCDDA =
          xmlreader.GetValueAsBool(section, PropNames.UseForCDDA, Defaults.UseForCDDA);

        UseForWebStream =
          xmlreader.GetValueAsBool(section, PropNames.UseForWebStream, Defaults.UseForWebStream);

        UseForLastFMWebStream =
          xmlreader.GetValueAsBool(section, PropNames.UseForLastFMWebStream, Defaults.UseForLastFMWebStream);

        UseMaxASIOBufferSize =
          xmlreader.GetValueAsBool(section, PropNames.UseMaxASIOBufferSize, Defaults.UseMaxASIOBufferSize);

        UseOverSampling =
          xmlreader.GetValueAsBool(section, PropNames.UseOverSampling, Defaults.UseOverSampling);

        UseReplayGain =
          xmlreader.GetValueAsBool(section, PropNames.UseReplayGain, Defaults.UseReplayGain);

        UseRGAlbumGain =
          xmlreader.GetValueAsBool(section, PropNames.UseRGAlbumGain, Defaults.UseRGAlbumGain);

        UseVizAGC =
          xmlreader.GetValueAsBool(section, PropNames.UseVizAGC, Defaults.UseVizAGC);

        VizLatencyCorrection =
          xmlreader.GetValueAsInt(section, PropNames.VizLatencyCorrection, Defaults.VizLatencyCorrection);

        VizType =
          xmlreader.GetValueAsInt(section, PropNames.VizType, Defaults.VizType);

        WMPVizClsId =
          xmlreader.GetValueAsString(section, PropNames.WMPVizClsId, Defaults.WMPVizClsId);

        WMPVizPreset =
          xmlreader.GetValueAsInt(section, PropNames.WMPVizPreset, Defaults.WMPVizPreset);

        WMPVizFps =
          xmlreader.GetValueAsInt(section, PropNames.WMPVizFps, Defaults.WMPVizFps);

        //WMPVizFFTFallBack =
        //  xmlreader.GetValueAsInt(section, PropNames.WMPVizFFTFallBack, Defaults.WMPVizFFTFallBack);

        //WMPVizFFTHalf =
        //  xmlreader.GetValueAsBool(section, PropNames.WMPVizFFTHalf, Defaults.WMPVizFFTHalf);

        //WMPVizFFTMinimum =
        //  xmlreader.GetValueAsInt(section, PropNames.WMPVizFFTMinimum, Defaults.WMPVizFFTMinimum);
      }
      LoadVSTSettings();
      LoadWADSPSettings();
    }

    private void LoadVSTSettings()
    {
      _VSTPlugins.Clear();
      string vstPluginDir = Path.Combine(Application.StartupPath, @"musicplayer\plugins\dsp");
      foreach (MediaPortal.Player.DSP.VSTPlugin plugin in MediaPortal.Player.DSP.Settings.Instance.VSTPlugins)
      {
        string pluginName = plugin.PluginDll;
        string dllFile = Path.Combine(vstPluginDir, plugin.PluginDll);

        VSTPlugin vstPlugin = new VSTPlugin(pluginName, dllFile);
        foreach (MediaPortal.Player.DSP.VSTPluginParm parameter in plugin.Parameter)
        {
          float value;
          if (float.TryParse(parameter.Value, out value))
            vstPlugin.Parameters.Add(parameter.Index, new VSTParameter(parameter.Index, parameter.Name, value));
        }
        _VSTPlugins.Add(pluginName, vstPlugin);
      }
    }

    private void LoadWADSPSettings()
    {
      _WADSPPlugins.Clear();
      string pluginDir = Path.Combine(Application.StartupPath, @"musicplayer\plugins\dsp");
      foreach (MediaPortal.Player.DSP.WinAmpPlugin plugin in MediaPortal.Player.DSP.Settings.Instance.WinAmpPlugins)
      {
        string pluginName = plugin.PluginDll;
        string dllFile = Path.Combine(pluginDir, plugin.PluginDll);

        WADSPPlugin waDSPPlugin = new WADSPPlugin(pluginName, dllFile);
        _WADSPPlugins.Add(pluginName, waDSPPlugin);
      }
    }

    public void SaveSettings()
    {
      //using (Profile.Settings xmlWriter = new Profile.MPSettings())
      using (MediaPortal.Profile.Settings xmlWriter = new MediaPortal.Profile.Settings(Config.GetFile(Config.Dir.Config, ConfigFile)))
      {
        string section = ConfigSection;

        xmlWriter.SetValue(section, PropNames.ASIODevice, ASIODevice);
        xmlWriter.SetValue(section, PropNames.ASIOFirstChan, ASIOFirstChan);
        xmlWriter.SetValue(section, PropNames.ASIOLastChan, ASIOLastChan);
        xmlWriter.SetValue(section, PropNames.BASSPlayBackBufferSize, BASSPlayBackBufferSize);
        xmlWriter.SetValue(section, PropNames.DefaultPlayBackMode, (int)DefaultPlayBackMode);
        xmlWriter.SetValueAsBool(section, PropNames.DoSoftStop, DoSoftStop);
        xmlWriter.SetValue(section, PropNames.Extensions, Extensions);
        xmlWriter.SetValue(section, PropNames.FiveDotOneUpMix, (int)FiveDotOneUpMix);
        xmlWriter.SetValue(section, PropNames.GapLength, GapLength);
        xmlWriter.SetValue(section, PropNames.ForceMaxASIORate, ForceMaxASIORate);
        xmlWriter.SetValue(section, PropNames.ForceMinASIORate, ForceMinASIORate);
        xmlWriter.SetValue(section, PropNames.MonoUpMix, (int)MonoUpMix);
        xmlWriter.SetValue(section, PropNames.PlayBackBufferSize, PlayBackBufferSize);
        xmlWriter.SetValue(section, PropNames.QuadraphonicUpMix, (int)QuadraphonicUpMix);
        xmlWriter.SetValue(section, PropNames.SeekIncrement, SeekIncrement);
        xmlWriter.SetValue(section, PropNames.SoftStopDuration, SoftStopDuration);
        xmlWriter.SetValue(section, PropNames.SonVizFFTAmp, SonVizFFTAmp);
        xmlWriter.SetValue(section, PropNames.SonVizFFTSkipCount, SonVizFFTSkipCount);
        xmlWriter.SetValue(section, PropNames.SonVizSlowFade, SonVizSlowFade);
        xmlWriter.SetValue(section, PropNames.SonVizWaveSkipCount, SonVizWaveSkipCount);
        xmlWriter.SetValue(section, PropNames.DirectSoundDevice, DirectSoundDevice);
        xmlWriter.SetValue(section, PropNames.StereoUpMix, (int)StereoUpMix);
        xmlWriter.SetValueAsBool(section, PropNames.UseASIO, UseASIO);
        xmlWriter.SetValueAsBool(section, PropNames.UseForCDDA, UseForCDDA);
        xmlWriter.SetValueAsBool(section, PropNames.UseForWebStream, UseForWebStream);
        xmlWriter.SetValueAsBool(section, PropNames.UseForLastFMWebStream, UseForLastFMWebStream);
        xmlWriter.SetValueAsBool(section, PropNames.UseMaxASIOBufferSize, UseMaxASIOBufferSize);
        xmlWriter.SetValueAsBool(section, PropNames.UseOverSampling, UseOverSampling);
        xmlWriter.SetValueAsBool(section, PropNames.UseReplayGain, UseReplayGain);
        xmlWriter.SetValueAsBool(section, PropNames.UseRGAlbumGain, UseRGAlbumGain);
        xmlWriter.SetValueAsBool(section, PropNames.UseVizAGC, UseVizAGC);
        xmlWriter.SetValue(section, PropNames.VizLatencyCorrection, VizLatencyCorrection);
        xmlWriter.SetValue(section, PropNames.VizType, VizType);
        xmlWriter.SetValue(section, PropNames.WMPVizClsId, WMPVizClsId);
        xmlWriter.SetValue(section, PropNames.WMPVizPreset, WMPVizPreset);
        xmlWriter.SetValue(section, PropNames.WMPVizFps, WMPVizFps);
        //xmlWriter.SetValue(section, PropNames.WMPVizFFTFallBack, WMPVizFFTFallBack);
        //xmlWriter.SetValueAsBool(section, PropNames.WMPVizFFTHalf, WMPVizFFTHalf);
        //xmlWriter.SetValue(section, PropNames.WMPVizFFTMinimum, WMPVizFFTMinimum);

        MediaPortal.Profile.Settings.SaveCache();
      }
    }

    public class VSTPluginCollection : Dictionary<string, VSTPlugin>
    {
    }

    public class VSTPlugin
    {
      private string _Name = null;
      private string _DllFile = null;
      private VSTParameterCollection _Parameters = new VSTParameterCollection();

      public string Name
      {
        get
        {
          return _Name;
        }
      }

      public string DllFile
      {
        get
        {
          return _DllFile;
        }
      }

      public VSTParameterCollection Parameters
      {
        get
        {
          return _Parameters;
        }
      }

      public VSTPlugin(string name, string dllFile)
      {
        _Name = name;
        _DllFile = dllFile;
      }
    }

    public class VSTParameterCollection : Dictionary<int, VSTParameter>
    {
    }

    public class VSTParameter
    {
      int _Index = 0;
      string _Name = "";
      float _Value = 0;

      public int Index
      {
        get
        {
          return _Index;
        }
      }

      public string Name
      {
        get
        {
          return _Name;
        }
      }

      public float Value
      {
        get
        {
          return _Value;
        }
      }

      public VSTParameter(int index, string name, float value)
      {
        _Index = index;
        _Name = name;
        _Value = value;
      }
    }

    public class WADSPPluginCollection : Dictionary<string, WADSPPlugin>
    {
    }

    public class WADSPPlugin
    {
      private string _Name = null;
      private string _DllFile = null;

      public string Name
      {
        get
        {
          return _Name;
        }
      }

      public string DllFile
      {
        get
        {
          return _DllFile;
        }
      }

      public WADSPPlugin(string name, string dllFile)
      {
        _Name = name;
        _DllFile = dllFile;
      }
    }
  }
}
