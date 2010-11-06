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

#include "InstalledDriver.h"

// we need this for registry access
using namespace Microsoft::Win32;

// and we need this for typed lists
using namespace System::Collections::Generic;

namespace BlueWave
{
	namespace Interop
	{
		namespace Asio
		{
			InstalledDriver::InstalledDriver(String^ name, String^ clsId)
			{
				// remember the name and CLSID
				_name = name;
				_clsId = clsId;
			}

			array<InstalledDriver^>^ InstalledDriver::GetInstalledDriversFromRegistry()
			{
				// create a generic list of installed drivers
				List<InstalledDriver^> list = gcnew List<InstalledDriver^>();

				// our settings are in the local machine
				RegistryKey^ localMachine = Registry::LocalMachine;

				// in the software/asio folder
				RegistryKey^ asioRoot = localMachine->OpenSubKey("SOFTWARE\\ASIO");

				if (asioRoot != nullptr && asioRoot->SubKeyCount > 0)
				{
					// now read all the names of subkeys below that
					array<String^>^ subkeyNames = asioRoot->GetSubKeyNames();

					// iterate through and get the stuff we need
					for (int index = 0; index < subkeyNames->Length; index++)
					{
						// get the registry key detailing the driver
						RegistryKey^ driverKey = asioRoot->OpenSubKey(subkeyNames[index]);

						// and extract what we need
						String^ name = static_cast<String^>(driverKey->GetValue("Description"));
						String^ clsid = static_cast<String^>(driverKey->GetValue("CLSID"));

						// ST: If the description value is not present, use the subkeyname
						if (!name)
							name = subkeyNames[index];

						// and close again
						driverKey->Close();

						// add to our list
						list.Add(gcnew InstalledDriver(name, clsid));
					}
				}
				// and return as an array
				return list.ToArray();
			}

			String^ InstalledDriver::ClsId::get()
			{
				return _clsId;
			}

			String^ InstalledDriver::Name::get()
			{
				return _name;
			}

			String^ InstalledDriver::ToString()
			{
				return _name;
			}
		}
	}
}