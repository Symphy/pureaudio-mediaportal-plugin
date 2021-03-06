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

#include "ChannelBuffer.h"
#include "BufferInt32LSB.h"
#include "BufferInt24LSB.h"
#include "BufferInt16LSB.h"

#pragma once
#pragma managed

using namespace System;

namespace PureAudio
{
	namespace Asio
	{
		namespace Interop
		{
			// represents a single audio channel (input or output) on the soundcard
			public ref class Channel
			{
			internal:

				// true is this is an input channel
				bool _isInput;

				// the channel name
				String^ _name;

				// sample format
				ASIOSampleType _sampleType;

				// the actual buffer
				ChannelBuffer^ _buffer;
				
				// clip value for our float sample data
				float maxSampleValue;

				// internal construction only
				Channel(IAsio* pAsio, bool IsInput, int channelNumber, void* pTheirBuffer0, void* pTheirBuffer1);

			public:

				// the channel name
				property String^ Name { String^ get(); }

				// the sample type
				property ASIOSampleType SampleType { ASIOSampleType get(); }

				// indexer for setting the value of sample in the buffer
				property float default[int] { void set(int sample, float value); float get(int sample); }

			};
		};
	}
}
