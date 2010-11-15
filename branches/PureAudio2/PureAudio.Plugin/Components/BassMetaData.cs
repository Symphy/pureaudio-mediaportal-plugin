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
    /// <summary>
    /// Provides meta data retrieved from a BASS stream.
    /// </summary>
    private partial class BassMetaData : IMetaData
    {
      #region Static members

      /// <summary>
      /// Creates a new instance.
      /// </summary>
      /// <param name="stream"></param>
      /// <returns></returns>
      public static BassMetaData Create(BassStream stream)
      {
        BassMetaData instance = new BassMetaData(stream);
        instance.Initialize();
        return instance;
      }

      #endregion

      #region Fields

      private BassStream _Stream;
      private int? _WaveFormatExtensibleChannelMask;
      private float? _ReplayGainTrackGain;
      private float? _ReplayGainTrackPeak;
      private float? _ReplayGainAlbumGain;
      private float? _ReplayGainAlbumPeak;

      #endregion

      #region IMetaData Members

      public int? WaveFormatExtensibleChannelMask
      {
        get { return _WaveFormatExtensibleChannelMask; }
      }

      public float? ReplayGainAlbumGain
      {
        get { return _ReplayGainAlbumGain; }
      }

      public float? ReplayGainAlbumPeak
      {
        get { return _ReplayGainAlbumPeak; }
      }

      public float? ReplayGainTrackGain
      {
        get { return _ReplayGainTrackGain; }
      }

      public float? ReplayGainTrackPeak
      {
        get { return _ReplayGainTrackPeak; }
      }

      #endregion

      #region Public members

      #endregion

      #region Private members

      private BassMetaData(BassStream stream)
      {
        _Stream = stream;
      }

      private void Initialize()
      {
        BassMetaDataReader reader = new BassMetaDataReader(this);
        reader.UpdateMetaData();
      }

      #endregion

    }
  }
}
