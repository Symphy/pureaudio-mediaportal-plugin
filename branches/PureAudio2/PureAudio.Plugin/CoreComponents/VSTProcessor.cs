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

// Todo: Implement. This is just a passthrough stub.

using System;

namespace MediaPortal.Plugins.PureAudio
{
  public partial class Player
  {
    /// <summary>
    /// Performs signal processing using VST plugins.
    /// </summary>
    private class VSTProcessor : IDisposable
    {
      #region Static members

      /// <summary>
      /// Creates and initializes an new instance.
      /// </summary>
      /// <param name="player">Reference to containing IPlayer object.</param>
      /// <returns>The new instance.</returns>
      public static VSTProcessor Create(Player player)
      {
        VSTProcessor vstProcessor = new VSTProcessor(player);
        vstProcessor.Initialize();
        return vstProcessor;
      }

      #endregion

      #region Fields

      Player _Player;
      private BassStream _InputStream;
      private BassStream _OutputStream;
      private bool _Initialized;

      #endregion

      #region IDisposable Members

      public void Dispose()
      {
      }

      #endregion

      #region Public members

      /// <summary>
      /// Gets the current inputstream as set with SetInputStream.
      /// </summary>
      public BassStream InputStream
      {
        get { return _InputStream; }
      }

      /// <summary>
      /// Gets the output Bass stream.
      /// </summary>
      public BassStream OutputStream
      {
        get { return _OutputStream; }
      }

      /// <summary>
      /// Sets the Bass inputstream.
      /// </summary>
      /// <param name="stream"></param>
      public void SetInputStream(BassStream stream)
      {
        ResetInputStream();
        _InputStream = stream;
        _OutputStream = stream;
        _Initialized = true;
      }

      /// <summary>
      /// Resets the instance to its uninitialized state.
      /// </summary>
      public void ResetInputStream()
      {
        if (_Initialized)
        {
          _Initialized = false;

          if (!_OutputStream.Equals(_InputStream))
            _OutputStream.Dispose();
          
          _OutputStream = null;
          _InputStream = null;
        }
      }

      #endregion

      #region Private members

      private VSTProcessor(Player player)
      {
        _Player = player;
      }

      /// <summary>
      /// Initializes a new instance.
      /// </summary>
      private void Initialize()
      {
      }

      #endregion
    }
  }
}