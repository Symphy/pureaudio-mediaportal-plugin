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

namespace MediaPortal.Plugins.PureAudio
{
  public partial class PureAudioPlayer
  {
    enum PlaybackState
    {
      Init,
      Playing,
      Paused,
      Ended,
      Stopped
    }

    enum MediaItemMainType
    {
      Unknown = 0,
      WebStream = 1,
      MODFile = 2,
      AudioFile = 3,
      CDTrack = 4
    }

    enum MediaItemSubType
    {
      None = 0,
      ASXWebStream = 1,
      LastFmWebStream = 3
    }

    enum InternalPlayBackState
    {
      Initializing,
      Playing,
      Pausing,
      Paused,
      Resuming,
      Stopping,
      Stopped
    }

    enum DeviceState
    {
      Started,
      Stopped
    }
  }

  public enum StreamContentType
  {
    Unknown,
    PCM,
    IEC61937,
    DD,
    DTS,
    DTS14Bit
  }

  public enum OutputMode
  {
    DirectSound,
    ASIO
  }

  public enum PlaybackMode
  {
    // Must match int values as used by MP for IPlayer.PlaybackType
    Normal = 0,
    Gapless = 1,
    CrossFading = 2
  }

  /// <summary>
  /// All supported channel assignments definitions
  /// </summary>
  public enum ChannelAssignmentDef
  {
    /// <summary>
    /// 1.0 Mono 
    /// </summary>
    _1_0,
    
    /// <summary>
    /// 2.0 Stereo
    /// </summary>
    _2_0,
    
    /// <summary>
    /// 4.0 Quadrophonic 
    /// </summary>
    _4_0,
    
    /// <summary>
    /// 4.1 
    /// </summary>
    _4_1,

    /// <summary>
    /// 5.0
    /// </summary>
    _5_0,

    /// <summary>
    /// 5.1 Side speaker
    /// </summary>
    _5_1,
    
    /// <summary>
    /// 7.1 Home theater 
    /// </summary>
    _7_1,

    Custom
  }
  
  public enum ChannelName
  {
    /// <summary>
    /// Left front
    /// </summary>
    Lf,
    
    /// <summary>
    /// Right front
    /// </summary>
    Rf,
    
    /// <summary>
    /// Center (front) 
    /// </summary>
    C,
    
    /// <summary>
    /// Low frequencey effects
    /// </summary>
    LFE,
    
    /// <summary>
    /// Left surround
    /// </summary>
    Ls,
    
    /// <summary>
    /// Right surround
    /// </summary>
    Rs,
    
    /// <summary>
    /// Left rear surround
    /// </summary>
    Lrs,
    
    /// <summary>
    /// Right rear surround
    /// </summary>
    Rrs
  }

  public enum VisualizationType
  {
    None = 0,
    WMP = 1
  }

}