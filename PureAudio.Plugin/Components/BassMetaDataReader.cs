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
using System.Collections.Generic;
using System.Globalization;
using Un4seen.Bass;

namespace MediaPortal.Plugins.PureAudio
{
  public partial class BassPlayer
  {
    private partial class BassMetaData
    {
      private partial class BassMetaDataReader
      {
        #region Fields

        BassMetaData _MetaData;

        #endregion

        #region Public members

        public BassMetaDataReader(BassMetaData metaData)
        {
          _MetaData = metaData;
        }

        public void UpdateMetaData()
        {
          bool isWebStream = (_MetaData._Stream.Length == TimeSpan.Zero);
          if (isWebStream)
          {
          }
          else
          {
            bool tagsFound;

            BASSChannelType cType = _MetaData._Stream.BassInfo.ctype;
            switch (cType)
            {
              case BASSChannelType.BASS_CTYPE_STREAM_AAC:
                tagsFound = ProcessMP4Tags();

                if (!tagsFound)
                  tagsFound = ProcessID3V2Tags();

                if (!tagsFound)
                  tagsFound = ProcessAPETags();

                if (!tagsFound)
                  tagsFound = ProcessOGGTags();

                break;

              case BASSChannelType.BASS_CTYPE_STREAM_APE:
                ProcessAPETags();
                break;

              case BASSChannelType.BASS_CTYPE_STREAM_MP1:
                ProcessStreamTypeMPx();
                break;

              case BASSChannelType.BASS_CTYPE_STREAM_MP2:
                ProcessStreamTypeMPx();
                break;

              case BASSChannelType.BASS_CTYPE_STREAM_MP3:
                ProcessStreamTypeMPx();
                break;

              case BASSChannelType.BASS_CTYPE_STREAM_MP4:
                tagsFound = ProcessMP4Tags();

                if (!tagsFound)
                  tagsFound = ProcessID3V2Tags();

                if (!tagsFound)
                  tagsFound = ProcessAPETags();

                if (!tagsFound)
                  tagsFound = ProcessOGGTags();

                break;

              case BASSChannelType.BASS_CTYPE_STREAM_FLAC:
                ProcessStreamTypeMisc();
                break;

              case BASSChannelType.BASS_CTYPE_STREAM_OGG:
                tagsFound = ProcessOGGTags();

                if (!tagsFound)
                  tagsFound = ProcessAPETags();

                break;

              case BASSChannelType.BASS_CTYPE_STREAM_WAV:
                ProcessRIFFTags();
                break;

              case BASSChannelType.BASS_CTYPE_STREAM_WAV_FLOAT:
                ProcessStreamTypeWAV();
                break;

              case BASSChannelType.BASS_CTYPE_STREAM_WAV_PCM:
                ProcessStreamTypeWAV();
                break;

              case BASSChannelType.BASS_CTYPE_STREAM_WMA:
                ProcessWMATags();
                break;

              case BASSChannelType.BASS_CTYPE_STREAM_WMA_MP3:
                ProcessWMATags();
                break;
            }
          }
        }

        #endregion

        #region Private members

        private bool ProcessStreamTypeMisc()
        {
          bool tagsFound = ProcessAPETags();

          if (!tagsFound)
            tagsFound = ProcessOGGTags();

          if (!tagsFound)
            tagsFound = ProcessID3V2Tags();

          if (!tagsFound)
            tagsFound = ProcessID3V1Tags();

          return tagsFound;
        }

        private bool ProcessStreamTypeWAV()
        {
          bool tagsFound = ProcessRIFFTags();

          if (!tagsFound)
            tagsFound = ProcessBWFTags();

          return tagsFound;
        }

        private bool ProcessStreamTypeMPx()
        {
          bool tagsFound = ProcessID3V2Tags();

          if (!tagsFound)
            tagsFound = ProcessID3V1Tags();

          if (!tagsFound)
            tagsFound = ProcessAPETags();

          if (!tagsFound)
            tagsFound = ProcessBWFTags();

          return tagsFound;
        }

        private bool ProcessAPETags()
        {
          string[] tags = Bass.BASS_ChannelGetTagsAPE(_MetaData._Stream.Handle);
          ParseKeyValueTags(tags);
          return (tags != null);
        }

        private bool ProcessBWFTags()
        {
          string[] tags = Bass.BASS_ChannelGetTagsBWF(_MetaData._Stream.Handle);
          ParseBWFTags(tags);
          return (tags != null);
        }

        private bool ProcessID3V1Tags()
        {
          string[] tags = Bass.BASS_ChannelGetTagsID3V1(_MetaData._Stream.Handle);
          ParseID3V1Tags(tags);
          return (tags != null);
        }

        private bool ProcessID3V2Tags()
        {
          string[] tags = Bass.BASS_ChannelGetTagsID3V2(_MetaData._Stream.Handle);
          ParseKeyValueTags(tags);
          return (tags != null);
        }

        private bool ProcessMP4Tags()
        {
          string[] tags = Bass.BASS_ChannelGetTagsMP4(_MetaData._Stream.Handle);
          ParseKeyValueTags(tags);
          return (tags != null);
        }

        private bool ProcessOGGTags()
        {
          string[] tags = Bass.BASS_ChannelGetTagsOGG(_MetaData._Stream.Handle);
          ParseKeyValueTags(tags);
          return (tags != null);
        }

        private bool ProcessRIFFTags()
        {
          string[] tags = Bass.BASS_ChannelGetTagsRIFF(_MetaData._Stream.Handle);
          ParseKeyValueTags(tags);
          return (tags != null);
        }

        private bool ProcessWMATags()
        {
          string[] tags = Bass.BASS_ChannelGetTagsWMA(_MetaData._Stream.Handle);
          ParseKeyValueTags(tags);
          return (tags != null);
        }

        private void ParseKeyValueTags(string[] tags)
        {
          ParseKeyValueTags(tags, "=");
        }

        private void ParseKeyValueTags(string[] tags, string seperator)
        {
          if (tags != null)
          {
            List<string> txxxTags = new List<string>();
            foreach (string tag in tags)
            {
              string key = "";
              string value = "";

              int seperatorPos = tag.IndexOf(seperator);
              if (seperatorPos > -1 && seperatorPos < tag.Length - 1)
              {
                key = tag.Substring(0, seperatorPos).Trim().ToLower();
                value = tag.Substring(seperatorPos + 1).Trim();
              }

              if (key != "")
              {
                Log.Debug("Parsed tag: {0} = {1}", key, value);
                switch (key)
                {
                  case "txxx":
                    txxxTags.Add(value);
                    break;

                  case "replaygain_track_gain":
                    _MetaData._ReplayGainTrackGain = ParseNumericTagValue(value);
                    break;

                  case "replaygain_track_peak":
                    _MetaData._ReplayGainTrackPeak = ParseNumericTagValue(value);
                    break;

                  case "replaygain_album_gain":
                    _MetaData._ReplayGainAlbumGain = ParseNumericTagValue(value);
                    break;

                  case "replaygain_album_peak":
                    _MetaData._ReplayGainAlbumPeak = ParseNumericTagValue(value);
                    break;

                  case "waveformatextensible_channel_mask":
                    _MetaData._WaveFormatExtensibleChannelMask = ParseHexTagValue(value);
                    break;
                }
              }
            }
            if (txxxTags.Count > 0)
            {
              ParseKeyValueTags(txxxTags.ToArray(), ":");
            }
          }
        }

        private int? ParseHexTagValue(string value)
        {
          if (value.TrimStart().StartsWith("0x",StringComparison.OrdinalIgnoreCase) && value.Length > 2)
            value = value.Substring(2);

          NumberFormatInfo formatInfo = new NumberFormatInfo();

          int intValue = 0;
          if (Int32.TryParse(value, NumberStyles.HexNumber, formatInfo, out intValue))
            return new int?(intValue);
          else
            return new int?();
        }

        private float? ParseNumericTagValue(string value)
        {
          // Remove "dB"
          int pos = value.IndexOf(" ");
          if (pos > -1)
            value = value.Substring(0, pos);

          NumberFormatInfo formatInfo = new NumberFormatInfo();
          formatInfo.NumberDecimalSeparator = ".";
          formatInfo.PercentGroupSeparator = ",";
          formatInfo.NegativeSign = "-";

          float f;
          if (float.TryParse(value, NumberStyles.Number, formatInfo, out f))
            return new float?(f);
          else
            return new float?();
        }

        private void ParseBWFTags(string[] tags)
        {
        }

        private void ParseID3V1Tags(string[] tags)
        {
        }

        #endregion

      }
    }
  }
}