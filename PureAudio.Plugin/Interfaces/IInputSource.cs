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
  public partial class BassPlayer
  {
    /// <summary>
    /// Provides members to control and read an inputsource.
    /// </summary>
    private interface IInputSource : IDisposable
    {
      /// <summary>
      /// Gets the mediaitem object that is processed by the inputsource.
      /// </summary>
      IMediaItem MediaItem { get; }

      /// <summary>
      /// Gets the mediaitem type for the inputsource.
      /// </summary>
      MediaItemMainType MediaItemType { get; }

      /// <summary>
      /// Gets the output Bass stream.
      /// </summary>
      BassStream OutputStream { get; }

      /// <summary>
      /// Gets info about the channel assignments.
      /// </summary>
      IChannelAssignmentInfo ChannelAssignmentInfo { get; }

      /// <summary>
      /// Gets embedded metadata (tags).
      /// </summary>
      IMetaData MetaData { get; }

      /// <summary>
      /// Gets whether a fadein is required on playback start.
      /// </summary>
      bool RequiresFadeIn { get; }

      /// <summary>
      /// Gets or set the playbackspeed
      /// </summary>
      int PlaybackSpeed { get; set; }
    }
  }
}