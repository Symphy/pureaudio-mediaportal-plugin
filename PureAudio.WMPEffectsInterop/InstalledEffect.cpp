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

#include "stdafx.h"
#include "InstalledEffect.h"

// we need this for registry access
using namespace Microsoft::Win32;

// and we need this for typed lists
using namespace System::Collections::Generic;

namespace MediaPortal
{
  namespace Plugins
  {
    namespace PureAudio
    {
      namespace WMPEffectsInterop
      {
        InstalledEffect::InstalledEffect(String^ name, String^ description, String^ clsId)
        {
          // remember the name, description and CLSID
          _name = name;
          _description = description;
          _clsId = clsId;
        }

        array<InstalledEffect^>^ InstalledEffect::GetInstalledEffectsFromRegistry()
        {
          // our settings are in the local machine
          RegistryKey^ localMachine = Registry::LocalMachine;

          // in the software/asio folder
          RegistryKey^ effectRoot = localMachine->OpenSubKey("SOFTWARE\\Microsoft\\MediaPlayer\\Objects\\Effects");

          // now read all the names of subkeys below that
          array<String^>^ subkeyNames = effectRoot->GetSubKeyNames();

          // create a generic list of installed drivers
          List<InstalledEffect^> list = gcnew List<InstalledEffect^>();

          // iterate through and get the stuff we need
          for (int index = 0; index < subkeyNames->Length; index++)
          {
            // get the registry key detailing the effect
            RegistryKey^ effectKey = effectRoot->OpenSubKey(subkeyNames[index]);

            // get the properties subkey
            RegistryKey^ propertiesKey = effectKey->OpenSubKey("Properties");

            // and extract what we need
            String^ clsid = static_cast<String^>(propertiesKey->GetValue("classid"));
            String^ name = static_cast<String^>(propertiesKey->GetValue("name"));
            String^ description = static_cast<String^>(propertiesKey->GetValue("description"));

            // for now this is how we deal with the res urls
            if (name == nullptr || name->StartsWith("res://"))
            {
              name = subkeyNames[index];
            }
            if (description == nullptr || description->StartsWith("res://"))
            {
              description = subkeyNames[index];
            }

            // and close again
            propertiesKey->Close();
            effectKey->Close();

            // add to our list
            if (
              clsid != "{48501FF0-F6A9-11D2-9435-00A0C92A2F2D}" && // "Bars" visualization
              clsid != "{8D2A317B-D98B-4efa-89D6-E49F0ACF98A1}") // "Battery" visualization

              list.Add(gcnew InstalledEffect(name, description, clsid));
          }

          // and return as an array
          return list.ToArray();
        }

        String^ InstalledEffect::ClsId::get()
        {
          return _clsId;
        }

        String^ InstalledEffect::Name::get()
        {
          return _name;
        }

        String^ InstalledEffect::Description::get()
        {
          return _description;
        }

        String^ InstalledEffect::ToString()
        {
          return _name;
        }
      }
    }
  }
}