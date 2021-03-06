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

namespace PureAudio
{
	namespace Asio
	{
		namespace Interop
		{
			// represents buffer size info specified by the driver
			public ref class BufferInfo
			{
			internal:

				// internal construction only
				BufferInfo(IAsio* pAsio);

				// these four things constitute a buffer size
				long m_nMinSize;
				long m_nMaxSize;
				long m_nPreferredSize;
				long m_nGranularity;

			public:

				// and this is where you can retrieve them
				property int MinSize { int get(); }
				property int MaxSize { int get(); }
				property int PreferredSize { int get(); }
				property int Granularity { int get(); }
			};
		}
	}
}
