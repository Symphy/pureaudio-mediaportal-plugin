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
using Un4seen.Bass.AddOn.Tags;

namespace MediaPortal.Plugins.PureAudio
{
  public partial class Player
  {
    private partial class InputSourceFactory
    {
      /// <summary>
      /// Represents a file inputsource implemented by the Bass library.
      /// </summary>
      private class BassAudioFileInputSource : IInputSource
      {
        #region Static members

        /// <summary>
        /// Creates and initializes an new instance.
        /// </summary>
        /// <param name="mediaItem">The mediaItem to be handled by the instance.</param>
        /// <returns>The new instance.</returns>
        public static BassAudioFileInputSource Create(IMediaItem mediaItem)
        {
          BassAudioFileInputSource inputSource = new BassAudioFileInputSource(mediaItem);
          inputSource.Initialize();
          return inputSource;
        }

        #endregion

        #region Fields

        private IMediaItem _MediaItem;
        private IMetaData _MetaData;
        private BassStream _BassStream;
        private BassStreamWinder _BassStreamWinder;
        private IChannelAssignmentInfo _ChannelAssignmentInfo;

        #endregion

        #region IInputSource Members

        public IMediaItem MediaItem
        {
          get { return _MediaItem; }
        }

        public MediaItemMainType MediaItemType
        {
          get { return MediaItemMainType.AudioFile; }
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
          get { return _BassStreamWinder.PlaybackSpeed; }
          set { _BassStreamWinder.PlaybackSpeed = value; }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
          if (OutputStream != null)
          {
            _BassStreamWinder.Dispose();
            OutputStream.Dispose();
          }
        }

        #endregion

        #region Public members

        #endregion

        #region Private Members

        private BassAudioFileInputSource(IMediaItem mediaItem)
        {
          _MediaItem = mediaItem;
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        private void Initialize()
        {
          Log.Debug("BassAudioFileInputSource.Initialize()");

          string file = _MediaItem.ContentUri.LocalPath;

          BASSFlag flags =
              BASSFlag.BASS_STREAM_DECODE |
              BASSFlag.BASS_SAMPLE_FLOAT;

          int handle = Bass.BASS_StreamCreateFile(file, 0, 0, flags);

          if (handle == BassConstants.BassInvalidHandle)
            throw new BassLibraryException("BASS_StreamCreateFile");

          _BassStream = BassStream.Create(handle);
          _MetaData = BassMetaData.Create(_BassStream);
          _BassStreamWinder = BassStreamWinder.Create(_BassStream);

          if (_MetaData.WaveFormatExtensibleChannelMask.HasValue)
            _ChannelAssignmentInfo = ChannelAssignmentHelper.GetChannelAssignmentInfo(_MetaData.WaveFormatExtensibleChannelMask.Value);
          else
            _ChannelAssignmentInfo = ChannelAssignmentHelper.GuessChannelAssignmentInfo(_BassStream.Channels);
        }

        #endregion
      }
    }
  }
}