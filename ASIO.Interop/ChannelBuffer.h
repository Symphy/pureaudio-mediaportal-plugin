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

#pragma once
#pragma managed

using namespace System;

namespace PureAudio
{
	namespace Asio
	{
		namespace Interop
		{
			// represents the buffer for an ASIO channel
			public ref class ChannelBuffer
			{
			internal:

				// internal construction only
				ChannelBuffer::ChannelBuffer();

				// what buffer should be affected by our indexer
				virtual void SetDoubleBufferIndex(long doubleBufferIndex);

				// indexer for setting the value of sample in the buffer
				property float default[int] { virtual void set(int sample, float value); virtual float get(int sample); }

			public:

			};
		};
	}
}
