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

namespace MediaPortal.Plugins.PureAudio
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

        if (itemType.MainType == MediaItemMainType.CDTrack)
          return _Player._Settings.UseForCDDA;

        else if (itemType.MainType == MediaItemMainType.WebStream)
          if (itemType.SubType == MediaItemSubType.LastFmWebStream)
            return _Player._Settings.UseForLastFMWebStream;
          else
            return _Player._Settings.UseForWebStream;

        else
          return itemType.MainType != MediaItemMainType.Unknown;
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

        switch (itemType.MainType)
        {
          case MediaItemMainType.AudioFile:
            inputSource = BassAudioFileInputSource.Create(mediaItem);
            break;

          case MediaItemMainType.CDTrack:
            inputSource = BassCDTrackInputSource.Create(mediaItem);
            break;

          case MediaItemMainType.MODFile:
            inputSource = BassMODFileInputSource.Create(mediaItem);
            break;

          case MediaItemMainType.WebStream:
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
        mediaItemType.MainType = MediaItemMainType.Unknown;
        mediaItemType.SubType = MediaItemSubType.None;

        if (uri.IsFile)
        {
          string filePath = uri.LocalPath;

          if (String.IsNullOrEmpty(filePath))
          {
            mediaItemType.MainType = MediaItemMainType.Unknown;
            mediaItemType.SubType = MediaItemSubType.None;
          }

          else if (IsCDDA(filePath))
          {
            mediaItemType.MainType = MediaItemMainType.CDTrack;
            mediaItemType.SubType = MediaItemSubType.None;
          }

          else if (IsASXFile(filePath))
          {
            mediaItemType.MainType = MediaItemMainType.WebStream;
            mediaItemType.SubType = MediaItemSubType.ASXWebStream;
          }

          else if (IsMODFile(filePath))
          {
            mediaItemType.MainType = MediaItemMainType.MODFile;
            mediaItemType.SubType = MediaItemSubType.None;
          }

          else
          {
            mediaItemType.MainType = MediaItemMainType.AudioFile;
            mediaItemType.SubType = MediaItemSubType.None;
          }
        }
        else
        {
          string ext = Path.GetExtension(uri.LocalPath).ToLower();
          bool supported = (ext == "" || IsExtensionSupported(ext));

          if (supported)
          {
            mediaItemType.MainType = MediaItemMainType.WebStream;
            if (IsLastFmWebStream(uri.PathAndQuery))
              mediaItemType.SubType = MediaItemSubType.LastFmWebStream;

            else
              mediaItemType.SubType = MediaItemSubType.None;
          }
          else
          {
            mediaItemType.MainType = MediaItemMainType.Unknown;
            mediaItemType.SubType = MediaItemSubType.None;
          }
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

      /// <summary>
      /// Determines if a given url represents a last.fm webstream.
      /// </summary>
      /// <param name="path"></param>
      /// <returns></returns>
      private static bool IsLastFmWebStream(string url)
      {
        return
          (url.Contains(@".last.fm/") ||
          url.Contains(@"/last.mp3?session="));
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
