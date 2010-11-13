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

namespace MediaPortal.Player.PureAudio
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
      public const string VizStreamLatencyCorrection = "VizStreamLatencyCorrection";

      public const string SupportedExtensions = "SupportedExtensions";
    }

    public static class Defaults
    {
      public const OutputMode OutputMode = MediaPortal.Player.PureAudio.OutputMode.ASIO;
      public const ChannelAssignmentDef ChannelAssignmentDef = MediaPortal.Player.PureAudio.ChannelAssignmentDef._4_0;
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

      public const PlaybackMode PlaybackMode = MediaPortal.Player.PureAudio.PlaybackMode.Normal;
      public static TimeSpan FadeDuration = TimeSpan.FromMilliseconds(1000);
      public static TimeSpan CrossFadeDuration = TimeSpan.FromMilliseconds(5000);

      public static TimeSpan PlaybackBufferSize = TimeSpan.FromMilliseconds(500);
      public static TimeSpan VizStreamLatencyCorrection = TimeSpan.FromMilliseconds(0);

      public const string SupportedExtensions =
        ".mp3,.ogg,.wav,.flac,.cda,.asx,.dts,.mod,.mo3,.s3m,.xm,.it,.mtm,.umx,.mdz,.s3z,.itz,.xmz,.mp2,.mp1,.aiff,.m2a,.mpa,.m1a,.swa,.aif,.mp3pro,.aac,.mp4,.m4a,.m4b,.ac3,.aac,.mov,.ape,.apl,.midi,.mid,.rmi,.kar,.mpc,.mpp,.mp+,.ofr,.ofs,.spx,.tta,.wma,.wv";
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
    public TimeSpan VizStreamLatencyCorrection { get; set; }

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
      VizStreamLatencyCorrection = Defaults.VizStreamLatencyCorrection;

      SupportedExtensions = Defaults.SupportedExtensions;
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
        VizStreamLatencyCorrection = TimeSpan.FromMilliseconds(xmlreader.GetValueAsInt(section, PropNames.VizStreamLatencyCorrection, (int)Defaults.VizStreamLatencyCorrection.TotalMilliseconds));

        SupportedExtensions = xmlreader.GetValueAsString(section, PropNames.SupportedExtensions, Defaults.SupportedExtensions);
      }
    }

    public void SaveSettings()
    {
      using (MediaPortal.Profile.Settings xmlWriter = new MediaPortal.Profile.Settings(Config.GetFile(Config.Dir.Config, ConfigFile)))
      {
        string section = ConfigSection;

        xmlWriter.SetValue(section, PropNames.OutputMode, Defaults.OutputMode);
        xmlWriter.SetValue(section, PropNames.ChannelAssignmentDef, Defaults.ChannelAssignmentDef);
        xmlWriter.SetValue(section, PropNames.EnableOversampling, Defaults.EnableOversampling);
        xmlWriter.SetValue(section, PropNames.EnableUpmixing, Defaults.EnableUpmixing);

        xmlWriter.SetValue(section, PropNames.DirectSoundDevice, Defaults.DirectSoundDevice);
        xmlWriter.SetValue(section, PropNames.DirectSoundBufferSize, Defaults.DirectSoundBufferSize);

        xmlWriter.SetValue(section, PropNames.ASIODevice, Defaults.ASIODevice);
        xmlWriter.SetValue(section, PropNames.ASIOFirstChan, Defaults.ASIOFirstChan);
        xmlWriter.SetValue(section, PropNames.ASIOLastChan, Defaults.ASIOLastChan);
        xmlWriter.SetValue(section, PropNames.ASIOMaxRate, Defaults.ASIOMaxRate);
        xmlWriter.SetValue(section, PropNames.ASIOMinRate, Defaults.ASIOMinRate);
        xmlWriter.SetValue(section, PropNames.ASIOUseMaxBufferSize, Defaults.ASIOUseMaxBufferSize);

        xmlWriter.SetValue(section, PropNames.PlaybackMode, Defaults.PlaybackMode);
        xmlWriter.SetValue(section, PropNames.FadeDuration, Defaults.FadeDuration);
        xmlWriter.SetValue(section, PropNames.CrossFadeDuration, Defaults.CrossFadeDuration);

        xmlWriter.SetValue(section, PropNames.PlaybackBufferSize, Defaults.PlaybackBufferSize);
        xmlWriter.SetValue(section, PropNames.VizStreamLatencyCorrection, Defaults.VizStreamLatencyCorrection);

        xmlWriter.SetValue(section, PropNames.SupportedExtensions, Defaults.SupportedExtensions);

        MediaPortal.Profile.Settings.SaveCache();

      }
    }

    #endregion
  }
}
