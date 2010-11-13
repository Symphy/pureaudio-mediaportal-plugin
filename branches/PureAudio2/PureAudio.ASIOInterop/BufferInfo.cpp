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

#include "AsioRedirect.h"
#include "BufferInfo.h"

namespace Media
{
  namespace Players
  {
    namespace BassPlayer
    {
      namespace ASIOInterop
      {
        BufferInfo::BufferInfo(IAsio* pAsio)
        {
          long p1, p2, p3, p4;

          // ask the driver for the four bits of info
          pAsio->getBufferSize(&p1, &p2, &p3, &p4);

          // and set them
          m_nMinSize = p1;
          m_nMaxSize = p2;
          m_nPreferredSize = p3;
          m_nGranularity = p4;
        }

        int BufferInfo::MinSize::get()
        {
          return m_nMinSize;
        }

        int BufferInfo::MaxSize::get()
        {
          return m_nMaxSize;
        }

        int BufferInfo::PreferredSize::get()
        {
          return m_nPreferredSize;
        }

        int BufferInfo::Granularity::get()
        {
          return m_nGranularity;
        }
      }
    }
  }
}