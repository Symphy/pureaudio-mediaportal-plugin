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
using Un4seen.Bass;
using Un4seen.Bass.AddOn.Mix;

namespace MediaPortal.Player.PureAudio
{
  public partial class BassPlayer
  {
    private partial class InputSourceHolder
    {
      private partial class MixingMatrix
      {

        #region Static members

        /// <summary>
        /// Creates and initializes an new instance.
        /// </summary>
        /// <param name="mediaItem">The mediaItem to be handled by the instance.</param>
        /// <returns>The new instance.</returns>
        public static MixingMatrix Create(IChannelAssignmentInfo inputAssignmentInfo, IChannelAssignmentInfo outputAssignmentInfo, bool upmix)
        {
          MixingMatrix instance = new MixingMatrix(inputAssignmentInfo, outputAssignmentInfo, upmix);
          instance.Initialize();
          return instance;
        }

        #endregion

        #region Fields

        private IChannelAssignmentInfo _InputAssignmentInfo;
        private IChannelAssignmentInfo _OutputAssignmentInfo;
        private bool _Upmix;
        
        private float[,] _BassMixMatrix;
        private float _Scaling;

        #endregion

        #region Public members

        public float Scaling
        {
          get { return _Scaling; }
        }

        public bool IsPassThrough
        {
          get { return _BassMixMatrix == null; }
        }

        public float[,] BassMixMatrix
        {
          get { return _BassMixMatrix; }
        }

        #endregion

        #region Private members

        private MixingMatrix(IChannelAssignmentInfo inputAssignmentInfo, IChannelAssignmentInfo outputAssignmentInfo, bool upmix)
        {
          _InputAssignmentInfo = inputAssignmentInfo;
          _OutputAssignmentInfo = outputAssignmentInfo;
          _Upmix = upmix;
        }

        private void Initialize()
        {
          MixingMatrixBuilder matrixBuilder = new MixingMatrixBuilder(this);
          matrixBuilder.BuildMixingMatrix();
        }

        #endregion

      }
    }
  }
}
