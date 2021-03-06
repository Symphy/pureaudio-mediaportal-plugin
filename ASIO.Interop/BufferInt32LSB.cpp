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

#pragma endregion

#include "BufferInt32LSB.h"

namespace PureAudio
{
	namespace Asio
	{
		namespace Interop
		{
			BufferInt32LSB::BufferInt32LSB(void* pTheirBuffer0, void* pTheirBuffer1) : BufferInt32LSB::ChannelBuffer()
			{
				// remember the two buffers (one plays the other updates)
				_pTheirBuffer0 = (DWORD*)pTheirBuffer0;
				_pTheirBuffer1 = (DWORD*)pTheirBuffer1;
			}

			void BufferInt32LSB::SetDoubleBufferIndex(long doubleBufferIndex)
			{
				// set what buffer should be affected by our indexer

				if (doubleBufferIndex == 0)
					_pTheirCurrentBuffer = _pTheirBuffer0;
				else
					_pTheirCurrentBuffer = _pTheirBuffer1;
			}

			void BufferInt32LSB::default::set(int sample, float value)
			{
				// set the value of a sample

				// convert float between -1.0 and 1.0 to signed integer
				_pTheirCurrentBuffer[sample] = (DWORD)(value * 2147483648.0f);
			}

			float BufferInt32LSB::default::get(int sample)
			{
				// get the value of a sample

				// convert signed integer to float between -1.0 and 1.0
				return (float)(_pTheirCurrentBuffer[sample] / 2147483648.0f);
			}
		}
	}
}