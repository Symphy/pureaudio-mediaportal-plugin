#pragma region Copyright (C) 2005-2010 Team MediaPortal

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

#pragma endregion

#pragma region Credits

// Based on the CodeProject article http://www.codeproject.com/KB/audio-video/Asio_Net.aspx
// ASIO is a trademark and software of Steinberg Media Technologies GmbH.

#pragma endregion

#include "ASIOSampleType.h"
#include "ChannelBuffer.h"

namespace MediaPortal
{
  namespace Plugins
  {
    namespace PureAudio
    {
      namespace ASIOInterop
      {
        ChannelBuffer::ChannelBuffer()
        {
        }

        void ChannelBuffer::SetDoubleBufferIndex(long doubleBufferIndex)
        {
          // set what buffer should be affected by our indexer

        }

        void ChannelBuffer::default::set(int sample, float value)
        {
          // set the value of a sample

        }

        float ChannelBuffer::default::get(int sample)
        {
          // get the value of a sample
          return 0.0f;
        }
      }
    }
  }
}