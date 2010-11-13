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
using Un4seen.Bass;

namespace MediaPortal.Player.PureAudio
{
  public partial class BassPlayer
  {
    /// <summary>
    /// Creates inputsource objects.
    /// </summary>
    private partial class InputSourceFactory : IDisposable
    {
      #region Fields

      BassPlayer _Player;

      #endregion

      #region IDisposable Members

      public void Dispose()
      {
      }

      #endregion

      #region Public members

      public InputSourceFactory(BassPlayer player)
      {
        _Player = player;
      }
      /// <summary>
      /// Determines if a given mediaitem is supported.
      /// </summary>
      /// <param name="mediaItem"></param>
      /// <returns></returns>
      public bool IsSupported(IMediaItem mediaItem)
      {
        MediaItemType itemType = GetMediaItemType(mediaItem);
        return itemType != MediaItemType.Unknown;
      }

      /// <summary>
      /// Creates an IInputSource object for a given mediaitem.
      /// </summary>
      /// <param name="mediaItem"></param>
      /// <returns></returns>
      public InputSourceHolder CreateInputSource(IMediaItem mediaItem)
      {
        MediaItemType itemType = GetMediaItemType(mediaItem);
        Log.Info("Media item type: {0}", itemType);

        IInputSource inputSource;

        switch (itemType)
        {
          case MediaItemType.AudioFile:
            inputSource = BassAudioFileInputSource.Create(mediaItem);
            break;

          case MediaItemType.CDTrack:
            inputSource = BassCDTrackInputSource.Create(mediaItem);
            break;

          case MediaItemType.MODFile:
            inputSource = BassMODFileInputSource.Create(mediaItem);
            break;

          case MediaItemType.WebStream:
            _Player._Controller.OnWaitStarted();
            inputSource = BassWebStreamInputSource.Create(mediaItem);
            _Player._Controller.OnWaitEnded();

            break;

          default:
            throw new BassPlayerException(String.Format("Unknown constant MediaItemType.{0}", itemType));
        }
        return InputSourceHolder.Create(_Player._Settings, inputSource);
      }

      /// <summary>
      /// Determines the mediaitem type for a given mediaitem.
      /// </summary>
      /// <param name="mediaItem">Mediaitem to analize.</param>
      /// <returns>One of the MediaItemType enumeration values.</returns>
      public MediaItemType GetMediaItemType(IMediaItem mediaItem)
      {
        Uri uri = mediaItem.ContentUri;
        MediaItemType mediaItemType;

        if (uri.IsFile)
        {
          string filePath = uri.LocalPath;

          if (String.IsNullOrEmpty(filePath))
            mediaItemType = MediaItemType.Unknown;

          else if (IsCDDA(filePath))
            mediaItemType = MediaItemType.CDTrack;

          else if (IsASXFile(filePath))
            mediaItemType = MediaItemType.WebStream;

          else if (IsMODFile(filePath))
            mediaItemType = MediaItemType.MODFile;

          else
            mediaItemType = MediaItemType.AudioFile;
        }
        else
        {
          string ext = Path.GetExtension(uri.LocalPath).ToLower();
          bool supported = (ext == "" || IsExtensionSupported(ext));

          if (supported)
            mediaItemType = MediaItemType.WebStream;

          else
            mediaItemType = MediaItemType.Unknown;
        }

        return mediaItemType;
      }

      #endregion

      #region Private members

      /// <summary>
      /// Determines if a given path represents a MOD music file.
      /// </summary>
      /// <param name="filePath"></param>
      /// <returns></returns>
      private bool IsMODFile(string path)
      {
        string ext = Path.GetExtension(path).ToLower();

        switch (ext)
        {
          case ".mod":
          case ".mo3":
          case ".it":
          case ".xm":
          case ".s3m":
          case ".mtm":
          case ".umx":
            return true;

          default:
            return false;
        }
      }

      /// <summary>
      /// Determines if a given path represents a audio CD track.
      /// </summary>
      /// <param name="filePath"></param>
      /// <returns></returns>
      private bool IsCDDA(string path)
      {
        path = path.ToLower();
        return
            (path.IndexOf("cdda:") >= 0 ||
            path.IndexOf(".cda") >= 0);
      }

      /// <summary>
      /// Determines if a given path represents a ASX file.
      /// </summary>
      /// <param name="filePath"></param>
      /// <returns></returns>
      private bool IsASXFile(string path)
      {
        return (Path.GetExtension(path).ToLower() == ".asx");
      }

      private bool IsSupportedAudioFile(string path)
      {
        string ext = Path.GetExtension(path).ToLower();
        return IsExtensionSupported(ext);
      }

      private bool IsExtensionSupported(string ext)
      {
        return
          _Player._Settings.SupportedExtensions.Contains(String.Format("{0},", ext)) ||
          _Player._Settings.SupportedExtensions.EndsWith(ext);
      }

      #endregion

    }
  }
}
