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

#include "AsioRedirect.h"

#pragma once
#pragma managed

using namespace System;

namespace BlueWave
{
	namespace Interop
	{
		namespace Asio
		{
			public enum struct AsioSampleType : long
			{
				ASIOSTInt16MSB = 0,
				ASIOSTInt24MSB = 1,			// used for 20 bits as well
				ASIOSTInt32MSB = 2,

				ASIOSTFloat32MSB = 3,		// IEEE 754 32 bit float
				ASIOSTFloat64MSB = 4,		// IEEE 754 64 bit double float

				// these are used for 32 bit data buffer, with different alignment of the data inside
				// 32 bit PCI bus systems can be more easily used with these

				ASIOSTInt32MSB16 = 8,		// 32 bit data with 16 bit alignment
				ASIOSTInt32MSB18 = 9,		// 32 bit data with 18 bit alignment
				ASIOSTInt32MSB20 = 10,		// 32 bit data with 20 bit alignment
				ASIOSTInt32MSB24 = 11,		// 32 bit data with 24 bit alignment

				ASIOSTInt16LSB = 16,
				ASIOSTInt24LSB = 17,			// used for 20 bits as well
				ASIOSTInt32LSB = 18,

				ASIOSTFloat32LSB = 19,		// IEEE 754 32 bit float, as found on Intel x86 architecture
				ASIOSTFloat64LSB = 20, 		// IEEE 754 64 bit double float, as found on Intel x86 architecture

				// these are used for 32 bit data buffer, with different alignment of the data inside
				// 32 bit PCI bus systems can more easily used with these

				ASIOSTInt32LSB16 = 24,		// 32 bit data with 18 bit alignment
				ASIOSTInt32LSB18 = 25,		// 32 bit data with 18 bit alignment
				ASIOSTInt32LSB20 = 26,		// 32 bit data with 20 bit alignment
				ASIOSTInt32LSB24 = 27,		// 32 bit data with 24 bit alignment

				//	ASIO DSD format.

				ASIOSTDSDInt8LSB1 = 32,		// DSD 1 bit data, 8 samples per byte. First sample in Least significant bit.
				ASIOSTDSDInt8MSB1 = 33,		// DSD 1 bit data, 8 samples per byte. First sample in Most significant bit.
				ASIOSTDSDInt8NER8 = 40,		// DSD 8 bit data, 1 sample per byte. No Endianness required.
			};
		};
	}
}
