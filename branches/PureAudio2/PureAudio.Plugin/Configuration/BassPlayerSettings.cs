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
using MediaPortal.Configuration;

namespace MediaPortal.Plugins.PureAudio
{
  /// <summary>
  /// Contains the bass player user configuration.
  /// </summary>
  public class BassPlayerSettings
  {
    public const string ConfigFile = "PureAudio.xml";
    public const string ConfigSection = "PureAudio2";

    public static class Constants
    {
      public const int Auto = -1;
    }

    public static class PropNames
    {
      public const string OutputMode = "OutputMode";
      public const string ChannelAssignmentDef = "ChannelAssignmentDef";
      public const string EnableOversampling = "EnableOversampling";
      public const string EnableUpmixing = "EnableUpmixing";

      public const string DirectSoundDevice = "DirectSoundDevice";
      public const string DirectSoundBufferSize = "DirectSoundBufferSize";

      public const string ASIODevice = "ASIODevice";
      public const string ASIOFirstChan = "ASIOFirstChan";
      public const string ASIOLastChan = "ASIOLastChan";
      public const string ASIOUseMaxBufferSize = "ASIOUseMaxBufferSize";
      public const string ASIOMaxRate = "ASIOMaxRate";
      public const string ASIOMinRate = "ASIOMinRate";

      public const string PlaybackMode = "PlaybackMode";
      public const string FadeDuration = "FadeDuration";
      public const string CrossFadeDuration = "CrossFadeDuration";

      public const string PlaybackBufferSize = "PlaybackBufferSize";

      public const string SupportedExtensions = "SupportedExtensions";
      public const string UseForCDDA = "UseForCDDA";
      public const string UseForWebStream = "UseForWebStream";
      public const string UseForLastFMWebStream = "UseForLastFMWebStream";

      public const string VisualizationUseAGC = "VisualizationUseAGC";
      public const string VisualizationLatencyCorrection = "VisualizationLatencyCorrection";
      public const string VisualizationType = "VisualizationType";
      
      public const string WMPEffectClsId = "WMPEffectClsId";
      public const string WMPEffectPreset = "WMPEffectPreset";
      public const string WMPEffectFps = "WMPEffectFps";
    }

    public static class Defaults
    {
      public const OutputMode OutputMode = MediaPortal.Plugins.PureAudio.OutputMode.ASIO;
      public const ChannelAssignmentDef ChannelAssignmentDef = MediaPortal.Plugins.PureAudio.ChannelAssignmentDef._4_0;
      public const bool EnableOversampling = false;
      public const bool EnableUpmixing = false;

      public const string DirectSoundDevice = "";
      public static TimeSpan DirectSoundBufferSize = TimeSpan.FromMilliseconds(200);

      public const string ASIODevice = "";
      public const int ASIOFirstChan = Constants.Auto;
      public const int ASIOLastChan = Constants.Auto;
      public const bool ASIOUseMaxBufferSize = true;
      public const int ASIOMaxRate = Constants.Auto;
      public const int ASIOMinRate = Constants.Auto;

      public const PlaybackMode PlaybackMode = MediaPortal.Plugins.PureAudio.PlaybackMode.Normal;
      public static TimeSpan FadeDuration = TimeSpan.FromMilliseconds(1000);
      public static TimeSpan CrossFadeDuration = TimeSpan.FromMilliseconds(5000);

      public static TimeSpan PlaybackBufferSize = TimeSpan.FromMilliseconds(500);

      public const string SupportedExtensions =
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

      public const bool UseForCDDA = true;
      public const bool UseForWebStream = true;
      public const bool UseForLastFMWebStream = true;

      public const bool VisualizationUseAGC = true;
      public static TimeSpan VisualizationLatencyCorrection = TimeSpan.FromMilliseconds(0);
      public const VisualizationType VisualizationType = MediaPortal.Plugins.PureAudio.VisualizationType.None;
      
      public const string WMPEffectClsId = "";
      public const int WMPEffectPreset = 0;
      public const int WMPEffectFps = 30;
    }

    #region Fields

    #endregion

    #region Public members

    public OutputMode OutputMode { get; set; }
    public ChannelAssignmentDef ChannelAssignmentDef { get; set; }
    public bool EnableOversampling { get; set; }
    public bool EnableUpmixing { get; set; }
    public string DirectSoundDevice { get; set; }
    public TimeSpan DirectSoundBufferSize { get; set; }
    public string ASIODevice { get; set; }
    public int ASIOFirstChan { get; set; }
    public int ASIOLastChan { get; set; }
    public int ASIOMaxRate { get; set; }
    public int ASIOMinRate { get; set; }
    public bool ASIOUseMaxBufferSize { get; set; }
    public TimeSpan PlaybackBufferSize { get; set; }
    public PlaybackMode PlaybackMode { get; set; }
    public TimeSpan FadeDuration { get; set; }
    public TimeSpan CrossFadeDuration { get; set; }
    public string SupportedExtensions { get; set; }
    public bool UseForCDDA { get; set; }
    public bool UseForWebStream { get; set; }
    public bool UseForLastFMWebStream { get; set; }

    public bool VisualizationUseAGC { get; set; }
    public TimeSpan VisualizationLatencyCorrection { get; set; }
    public VisualizationType VisualizationType { get; set; }
    
    public string WMPEffectClsId { get; set; }
    public int WMPEffectPreset { get; set; }
    public int WMPEffectFps { get; set; }

    public BassPlayerSettings()
    {
      OutputMode = Defaults.OutputMode;
      ChannelAssignmentDef = Defaults.ChannelAssignmentDef;
      EnableOversampling = Defaults.EnableOversampling;
      EnableUpmixing = Defaults.EnableUpmixing;

      DirectSoundDevice = Defaults.DirectSoundDevice;
      DirectSoundBufferSize = Defaults.DirectSoundBufferSize;

      ASIODevice = Defaults.ASIODevice;
      ASIOFirstChan = Defaults.ASIOFirstChan;
      ASIOLastChan = Defaults.ASIOLastChan;
      ASIOMaxRate = Defaults.ASIOMaxRate;
      ASIOMinRate = Defaults.ASIOMinRate;
      ASIOUseMaxBufferSize = Defaults.ASIOUseMaxBufferSize;

      PlaybackMode = Defaults.PlaybackMode;
      FadeDuration = Defaults.FadeDuration;
      CrossFadeDuration = Defaults.CrossFadeDuration;

      PlaybackBufferSize = Defaults.PlaybackBufferSize;

      SupportedExtensions = Defaults.SupportedExtensions;
      UseForCDDA = Defaults.UseForCDDA;
      UseForWebStream = Defaults.UseForWebStream;
      UseForLastFMWebStream = Defaults.UseForLastFMWebStream;

      VisualizationUseAGC = Defaults.VisualizationUseAGC;
      VisualizationLatencyCorrection = Defaults.VisualizationLatencyCorrection;
      VisualizationType = Defaults.VisualizationType;
      
      WMPEffectClsId = Defaults.WMPEffectClsId;
      WMPEffectPreset = Defaults.WMPEffectPreset;
      WMPEffectFps = Defaults.WMPEffectFps;
    }

    public void LoadSettings()
    {
      using (MediaPortal.Profile.Settings xmlreader = new MediaPortal.Profile.Settings(Config.GetFile(Config.Dir.Config, ConfigFile)))
      {
        string section = ConfigSection;

        OutputMode = (OutputMode)xmlreader.GetValueAsInt(section, PropNames.OutputMode, (int)Defaults.OutputMode);
        ChannelAssignmentDef = (ChannelAssignmentDef)xmlreader.GetValueAsInt(section, PropNames.ChannelAssignmentDef, (int)Defaults.ChannelAssignmentDef);
        EnableOversampling = xmlreader.GetValueAsBool(section, PropNames.EnableOversampling, Defaults.EnableOversampling);
        EnableUpmixing = xmlreader.GetValueAsBool(section, PropNames.EnableUpmixing, Defaults.EnableUpmixing);

        DirectSoundDevice = xmlreader.GetValueAsString(section, PropNames.DirectSoundDevice, Defaults.DirectSoundDevice);
        DirectSoundBufferSize = TimeSpan.FromMilliseconds(xmlreader.GetValueAsInt(section, PropNames.DirectSoundBufferSize, (int)Defaults.DirectSoundBufferSize.TotalMilliseconds));

        ASIODevice = xmlreader.GetValueAsString(section, PropNames.ASIODevice, Defaults.ASIODevice);
        ASIOFirstChan = xmlreader.GetValueAsInt(section, PropNames.ASIOFirstChan, Defaults.ASIOFirstChan);
        ASIOLastChan = xmlreader.GetValueAsInt(section, PropNames.ASIOLastChan, Defaults.ASIOLastChan);
        ASIOMaxRate = xmlreader.GetValueAsInt(section, PropNames.ASIOMaxRate, Defaults.ASIOMaxRate);
        ASIOMinRate = xmlreader.GetValueAsInt(section, PropNames.ASIOMinRate, Defaults.ASIOMinRate);
        ASIOUseMaxBufferSize = xmlreader.GetValueAsBool(section, PropNames.ASIOUseMaxBufferSize, Defaults.ASIOUseMaxBufferSize);

        PlaybackMode = (PlaybackMode)xmlreader.GetValueAsInt(section, PropNames.PlaybackMode, (int)Defaults.PlaybackMode);
        FadeDuration = TimeSpan.FromMilliseconds(xmlreader.GetValueAsInt(section, PropNames.FadeDuration, (int)Defaults.FadeDuration.TotalMilliseconds));
        CrossFadeDuration = TimeSpan.FromMilliseconds(xmlreader.GetValueAsInt(section, PropNames.CrossFadeDuration, (int)Defaults.CrossFadeDuration.TotalMilliseconds));

        PlaybackBufferSize = TimeSpan.FromMilliseconds(xmlreader.GetValueAsInt(section, PropNames.PlaybackBufferSize, (int)Defaults.PlaybackBufferSize.TotalMilliseconds));

        SupportedExtensions = xmlreader.GetValueAsString(section, PropNames.SupportedExtensions, Defaults.SupportedExtensions);

        UseForCDDA = xmlreader.GetValueAsBool(section, PropNames.UseForCDDA, Defaults.UseForCDDA);
        UseForWebStream = xmlreader.GetValueAsBool(section, PropNames.UseForWebStream, Defaults.UseForWebStream);
        UseForLastFMWebStream = xmlreader.GetValueAsBool(section, PropNames.UseForLastFMWebStream, Defaults.UseForLastFMWebStream);

        VisualizationUseAGC = xmlreader.GetValueAsBool(section, PropNames.VisualizationUseAGC, Defaults.VisualizationUseAGC);
        VisualizationLatencyCorrection = TimeSpan.FromMilliseconds(xmlreader.GetValueAsInt(section, PropNames.VisualizationLatencyCorrection, (int)Defaults.VisualizationLatencyCorrection.TotalMilliseconds));
        VisualizationType = (VisualizationType)xmlreader.GetValueAsInt(section, PropNames.VisualizationType, (int)Defaults.VisualizationType);

        WMPEffectClsId = xmlreader.GetValueAsString(section, PropNames.WMPEffectClsId, Defaults.WMPEffectClsId);
        WMPEffectPreset = xmlreader.GetValueAsInt(section, PropNames.WMPEffectPreset, Defaults.WMPEffectPreset);
        WMPEffectFps = xmlreader.GetValueAsInt(section, PropNames.WMPEffectFps, Defaults.WMPEffectFps);
      }
    }

    public void SaveSettings()
    {
      using (MediaPortal.Profile.Settings xmlWriter = new MediaPortal.Profile.Settings(Config.GetFile(Config.Dir.Config, ConfigFile)))
      {
        string section = ConfigSection;

        xmlWriter.SetValue(section, PropNames.OutputMode, (int)OutputMode);
        xmlWriter.SetValue(section, PropNames.ChannelAssignmentDef, (int)ChannelAssignmentDef);
        xmlWriter.SetValueAsBool(section, PropNames.EnableOversampling, EnableOversampling);
        xmlWriter.SetValueAsBool(section, PropNames.EnableUpmixing, EnableUpmixing);

        xmlWriter.SetValue(section, PropNames.DirectSoundDevice, DirectSoundDevice);
        xmlWriter.SetValue(section, PropNames.DirectSoundBufferSize, DirectSoundBufferSize.TotalMilliseconds);

        xmlWriter.SetValue(section, PropNames.ASIODevice, ASIODevice);
        xmlWriter.SetValue(section, PropNames.ASIOFirstChan, ASIOFirstChan);
        xmlWriter.SetValue(section, PropNames.ASIOLastChan, ASIOLastChan);
        xmlWriter.SetValue(section, PropNames.ASIOMaxRate, ASIOMaxRate);
        xmlWriter.SetValue(section, PropNames.ASIOMinRate, ASIOMinRate);
        xmlWriter.SetValueAsBool(section, PropNames.ASIOUseMaxBufferSize, ASIOUseMaxBufferSize);

        xmlWriter.SetValue(section, PropNames.PlaybackMode, (int)PlaybackMode);
        xmlWriter.SetValue(section, PropNames.FadeDuration, FadeDuration.TotalMilliseconds);
        xmlWriter.SetValue(section, PropNames.CrossFadeDuration, CrossFadeDuration.TotalMilliseconds);

        xmlWriter.SetValue(section, PropNames.PlaybackBufferSize, PlaybackBufferSize.TotalMilliseconds);

        xmlWriter.SetValue(section, PropNames.SupportedExtensions, SupportedExtensions);
        xmlWriter.SetValueAsBool(section, PropNames.UseForCDDA, UseForCDDA);
        xmlWriter.SetValueAsBool(section, PropNames.UseForWebStream, UseForWebStream);
        xmlWriter.SetValueAsBool(section, PropNames.UseForLastFMWebStream, UseForLastFMWebStream);

        xmlWriter.SetValueAsBool(section, PropNames.VisualizationUseAGC, VisualizationUseAGC);
        xmlWriter.SetValue(section, PropNames.VisualizationLatencyCorrection, VisualizationLatencyCorrection.TotalMilliseconds);
        xmlWriter.SetValue(section, PropNames.VisualizationType ,(int)VisualizationType );

        xmlWriter.SetValue(section, PropNames.WMPEffectClsId , WMPEffectClsId);
        xmlWriter.SetValue(section, PropNames.WMPEffectPreset ,WMPEffectPreset );
        xmlWriter.SetValue(section, PropNames.WMPEffectFps , WMPEffectFps);

        MediaPortal.Profile.Settings.SaveCache();
      }
    }

    #endregion
  }
}
