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

namespace BlueWave
{
	namespace Interop
	{
		namespace Asio
		{
			// represents an installed ASIO driver
			public ref class InstalledDriver
			{
			private:

				// the name of the driver
				String^ _name;

				// its COM CLSID
				String^ _clsId;

			internal:

				// internal construction only
				InstalledDriver(String^ name, String^ clsId);

				// this will read all drivers and the CLSIDs from the registry
				static array<InstalledDriver^>^ GetInstalledDriversFromRegistry();

				// this returns a string representation of the CLSID
				property String^ ClsId { String^ get(); };

			public:

				// both these just return name
				virtual String^ ToString() override;
				property String^ Name { String^ get(); };
			};
		}
	}
}
