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
using Un4seen.Bass;

namespace MediaPortal.Plugins.PureAudio
{
  public partial class BassPlayer
  {
    private partial class InputSourceFactory
    {
      /// <summary>
      /// Represents a MOD file inputsource implemented by the Bass library.
      /// </summary>
      private class BassMODFileInputSource : IInputSource
      {
        #region Static members

        /// <summary>
        /// Creates and initializes an new instance.
        /// </summary>
        /// <param name="mediaItem">The mediaItem to be handled by the instance.</param>
        /// <returns>The new instance.</returns>
        public static BassMODFileInputSource Create(IMediaItem mediaItem)
        {
          BassMODFileInputSource inputSource = new BassMODFileInputSource(mediaItem);
          inputSource.Initialize();
          return inputSource;
        }

        #endregion

        #region Fields

        private IMediaItem _MediaItem;
        private IMetaData _MetaData;
        private BassStream _BassStream;
        private IChannelAssignmentInfo _ChannelAssignmentInfo;

        #endregion

        #region IInputSource Members

        public IMediaItem MediaItem
        {
          get { return _MediaItem; }
        }

        public MediaItemMainType MediaItemType
        {
          get { return MediaItemMainType.MODFile; }
        }

        public IMetaData MetaData
        {
          get { return _MetaData; }
        }

        public BassStream OutputStream
        {
          get { return _BassStream; }
        }

        public IChannelAssignmentInfo ChannelAssignmentInfo
        {
          get { return _ChannelAssignmentInfo; }
        }

        public bool RequiresFadeIn
        {
          get { return false; }
        }

        public int PlaybackSpeed
        {
          get { return 1; }
          set { }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
          if (OutputStream != null)
            OutputStream.Dispose();
        }

        #endregion

        #region Public members

        #endregion

        #region Private members

        private BassMODFileInputSource(IMediaItem mediaItem)
        {
          _MediaItem = mediaItem;
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        private void Initialize()
        {
          Log.Debug("BassMODFileInputSource.Initialize()");

          BASSFlag flags =
              BASSFlag.BASS_SAMPLE_SOFTWARE |
              BASSFlag.BASS_SAMPLE_FLOAT |
              BASSFlag.BASS_MUSIC_AUTOFREE |
              BASSFlag.BASS_MUSIC_PRESCAN;

          int handle = Bass.BASS_MusicLoad(_MediaItem.ContentUri.LocalPath, 0, 0, flags, 0);

          if (handle == BassConstants.BassInvalidHandle)
            throw new BassLibraryException("BASS_MusicLoad");

          _BassStream = BassStream.Create(handle);

          // Todo: determine real channelassignment
          _ChannelAssignmentInfo = ChannelAssignmentHelper.GuessChannelAssignmentInfo(_BassStream.Channels);
        }

        #endregion
      }
    }
  }
}