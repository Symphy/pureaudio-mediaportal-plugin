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
  public partial class Player
  {
    /// <summary>
    /// Represents a media item.
    /// </summary>
    private class MediaItem : IMediaItem
    {
      private string _Title;
      private Uri _ContentUri;

      /// <summary>
      /// Returns the title of the media item.
      /// </summary>
      public string Title
      {
        get { return _Title; }
        set { _Title = value; }
      }

      /// <summary>
      /// Gets the content URI for this item
      /// </summary>
      /// <value>The content URI.</value>
      public Uri ContentUri 
      {
        get { return _ContentUri; }
      }

      public MediaItem(Uri contentUri)
      {
        _ContentUri = contentUri;
      }
    }
  }
}