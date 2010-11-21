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

#pragma once

#include "Preset.h"
#include "InstalledEffect.h"

#define SA_BUFFER_SIZE 1024
#define SA_CHANNEL_COUNT 2

using namespace System;
using namespace System::Drawing;

namespace MediaPortal
{
  namespace Plugins
  {
    namespace PureAudio
    {
      namespace WMPEffectsInterop
      {
        public ref class WMPEffect
        {
        internal:

          // we'll maintain a list of effects
          static array<InstalledEffect^>^ _installedEffects;

          // but you can only have one active at once
          static WMPEffect^ _instance;

          // return the instance of currently selected driver
          static property WMPEffect^ Instance { WMPEffect^ get(); }

          IWMPEffects2* _pWMPEffects;
          TimedLevel* _pTimedLevel;
          RECT* _pRectangle;
          array<Preset^>^ _presets;
          bool _isIWMPEffect2;
          bool _isWindowLess;

          // internal construction only
          WMPEffect();

          // select an effect once an instance of this class has been created
          bool InternalSelectEffect(String^ effectClsId, IntPtr winHandle);

        public:

          // returns the installed effect
          static property array<InstalledEffect^>^ InstalledEffects	{ array<InstalledEffect^>^ get(); }

          // select and initialise effect
          static WMPEffect^ SelectEffect(String^ effectClsId, IntPtr sysHandle);
          static WMPEffect^ SelectEffect(InstalledEffect^ installedEffect, IntPtr sysHandle);

          property array<Preset^>^ Presets { array<Preset^>^ get(); }
          property Preset^ CurrentPreset { Preset^ get(); }
          property bool IsIWMPEffect2 { bool get(); }
          property bool IsWindowLess { bool get(); }
          property String^ Title { String^ get(); }

          void MediaInfo(int channels, int samplerate, String^ title);
          void SetCurrentPreset(Preset^ preset);
          void WMPEffect::SetCurrentPreset(int number);
          void Render(array<float,2>^ waveData, array<float,2>^ fftData, IntPtr hdc, System::Drawing::Rectangle rectangle);
          void Release();
        };
      }
    }
  }
}