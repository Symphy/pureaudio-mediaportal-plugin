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

namespace MediaPortal.Player.PureAudio
{
  /// <summary>
  /// Contains the bass player user configuration.
  /// </summary>
  public class BassPlayerSettings
  {
    public static class Constants
    {
      public const int Auto = -1;
    }

    public static class Defaults
    {
      public const OutputMode OutputMode = MediaPortal.Player.PureAudio.OutputMode.ASIO;
      public const ChannelAssignmentDef ChannelAssignmentDef = MediaPortal.Player.PureAudio.ChannelAssignmentDef._4_0;
      public const bool EnableOversampling = false;
      public const bool EnableUpmixing = false;
      
      public const string DirectSoundDevice = "";
      public const int DirectSoundBufferSizeMilliSecs = 200;

      public const string ASIODevice = "";
      public const int ASIOFirstChan = Constants.Auto;
      public const int ASIOLastChan = Constants.Auto;
      public const bool ASIOUseMaxBufferSize = true;
      public const int ASIOMaxRate = Constants.Auto;
      public const int ASIOMinRate = Constants.Auto;
      
      public const PlaybackMode PlaybackMode = MediaPortal.Player.PureAudio.PlaybackMode.Normal;
      public const int FadeDurationMilliSecs = 1000;
      public const int CrossFadeDurationMilliSecs = 5000;
      
      public const int PlaybackBufferSizeMilliSecs = 500;
      public const int VizStreamLatencyCorrectionMilliSecs = 0;
      
      public const string SupportedExtensions =
        ".mp3,.ogg,.wav,.flac,.cda,.asx,.dts,.mod,.mo3,.s3m,.xm,.it,.mtm,.umx,.mdz,.s3z,.itz,.xmz,.mp2,.mp1,.aiff,.m2a,.mpa,.m1a,.swa,.aif,.mp3pro,.aac,.mp4,.m4a,.m4b,.ac3,.aac,.mov,.ape,.apl,.midi,.mid,.rmi,.kar,.mpc,.mpp,.mp+,.ofr,.ofs,.spx,.tta,.wma,.wv";
    }

    #region Fields

    private OutputMode _OutputMode = Defaults.OutputMode;
    private ChannelAssignmentDef _ChannelAssignmentDef = Defaults.ChannelAssignmentDef;
    private bool _EnableOversampling = Defaults.EnableOversampling;
    private bool _EnableUpmixing = Defaults.EnableUpmixing;

    private string _DirectSoundDevice = Defaults.DirectSoundDevice;
    private TimeSpan _DirectSoundBufferSize = TimeSpan.FromMilliseconds(Defaults.DirectSoundBufferSizeMilliSecs);

    private string _ASIODevice = Defaults.ASIODevice;
    private int _ASIOFirstChan = Defaults.ASIOFirstChan;
    private int _ASIOLastChan = Defaults.ASIOLastChan;
    private int _ASIOMaxRate = Defaults.ASIOMaxRate;
    private int _ASIOMinRate = Defaults.ASIOMinRate;
    private bool _ASIOUseMaxBufferSize = Defaults.ASIOUseMaxBufferSize;
    
    private PlaybackMode _PlaybackMode = Defaults.PlaybackMode;
    private TimeSpan _FadeDuration = TimeSpan.FromMilliseconds(Defaults.FadeDurationMilliSecs);
    private TimeSpan _CrossFadeDuration = TimeSpan.FromMilliseconds(Defaults.CrossFadeDurationMilliSecs);

    private TimeSpan _PlaybackBufferSize = TimeSpan.FromMilliseconds(Defaults.PlaybackBufferSizeMilliSecs);
    private TimeSpan _VizStreamLatencyCorrection = TimeSpan.FromMilliseconds(Defaults.VizStreamLatencyCorrectionMilliSecs);
    
    private string _SupportedExtensions = Defaults.SupportedExtensions;

    #endregion

    #region Public members

    public OutputMode OutputMode
    {
      get { return _OutputMode; }
      set { _OutputMode = value; }
    }

    public ChannelAssignmentDef ChannelAssignmentDef
    {
      get { return _ChannelAssignmentDef; }
      set { _ChannelAssignmentDef = value; }
    }

    public bool EnableOversampling
    {
      get { return _EnableOversampling; }
      set { _EnableOversampling = value; }
    }

    public bool EnableUpmixing
    {
      get { return _EnableUpmixing; }
      set { _EnableUpmixing = value; }
    }

    public string DirectSoundDevice
    {
      get { return _DirectSoundDevice; }
      set { _DirectSoundDevice = value; }
    }

    public int DirectSoundBufferSizeMilliSecs
    {
      get { return (int)_DirectSoundBufferSize.TotalMilliseconds; }
      set { _DirectSoundBufferSize = TimeSpan.FromMilliseconds(value); }
    }

    public TimeSpan DirectSoundBufferSize
    {
      get { return _DirectSoundBufferSize; }
      set { _DirectSoundBufferSize = value; }
    }

    public string ASIODevice
    {
      get { return _ASIODevice; }
      set { _ASIODevice = value; }
    }

    public int ASIOFirstChan
    {
      get { return _ASIOFirstChan; }
      set { _ASIOFirstChan = value; }
    }

    public int ASIOLastChan
    {
      get { return _ASIOLastChan; }
      set { _ASIOLastChan = value; }
    }

    public int ASIOMaxRate
    {
      get { return _ASIOMaxRate; }
      set { _ASIOMaxRate = value; }
    }

    public int ASIOMinRate
    {
      get { return _ASIOMinRate; }
      set { _ASIOMinRate = value; }
    }

    public bool ASIOUseMaxBufferSize
    {
      get { return _ASIOUseMaxBufferSize; }
      set { _ASIOUseMaxBufferSize = value; }
    }

    public int PlaybackBufferSizeMilliSecs
    {
      get { return (int)_PlaybackBufferSize.TotalMilliseconds; }
      set { _PlaybackBufferSize = TimeSpan.FromMilliseconds(value); }
    }

    public TimeSpan PlaybackBufferSize
    {
      get { return _PlaybackBufferSize; }
      set { _PlaybackBufferSize = value; }
    }

    public PlaybackMode PlaybackMode
    {
      get { return _PlaybackMode; }
      set { _PlaybackMode = value; }
    }

    public int FadeDurationMilliSecs
    {
      get { return (int)_FadeDuration.TotalMilliseconds; }
      set { _FadeDuration = TimeSpan.FromMilliseconds(value); }
    }

    public TimeSpan FadeDuration
    {
      get { return _FadeDuration; }
      set { _FadeDuration = value; }
    }

    public int CrossFadeDurationMilliSecs
    {
      get { return (int)_CrossFadeDuration.TotalMilliseconds; }
      set { _CrossFadeDuration = TimeSpan.FromMilliseconds(value); }
    }

    public TimeSpan CrossFadeDuration
    {
      get { return _CrossFadeDuration; }
      set { _CrossFadeDuration = value; }
    }

    public string SupportedExtensions
    {
      get { return _SupportedExtensions; }
      set { _SupportedExtensions = value; }
    }

    public int VizStreamLatencyCorrectionMilliSecs
    {
      get { return (int)_VizStreamLatencyCorrection.TotalMilliseconds; }
      set { _VizStreamLatencyCorrection = TimeSpan.FromMilliseconds(value); }
    }

    public TimeSpan VizStreamLatencyCorrection
    {
      get { return _VizStreamLatencyCorrection; }
      set { _VizStreamLatencyCorrection = value; }
    }

    public BassPlayerSettings()
    {
    }

    public void LoadSettings()
    {
      // Todo: implement
    }

    public void SaveSettings()
    {
      // Todo: implement
    }

    #endregion
  }
}
