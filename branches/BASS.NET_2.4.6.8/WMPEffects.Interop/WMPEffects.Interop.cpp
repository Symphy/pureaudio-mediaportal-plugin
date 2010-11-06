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
#include "WMPEffects.Interop.h"
#include <vcclr.h>

#define ENABLETRACE 0

using namespace System::Diagnostics;
using namespace System::Collections::Generic;

namespace WMPEffects
{
	namespace Interop
	{
		array<InstalledEffect^>^ WMPEffect::InstalledEffects::get()
		{
			// if we don't know what effects are installed, ask the InstalledEffect class
			if (!_installedEffects) _installedEffects = InstalledEffect::GetInstalledEffectsFromRegistry();

			// and return
			return _installedEffects;
		}

		// return the singleton instance
		WMPEffect^ WMPEffect::Instance::get()
		{
			return _instance;
		}

		WMPEffect^ WMPEffect::SelectEffect(InstalledEffect^ installedEffect, IntPtr winHandle)
		{
			return WMPEffect::SelectEffect(installedEffect->ClsId, winHandle);
		}

		WMPEffect^ WMPEffect::SelectEffect(String^ effectClsId, IntPtr winHandle)
		{
			// create a new instance of the driver (this will become the singleton)
			_instance = gcnew WMPEffect();

			if (_instance->InternalSelectEffect(effectClsId, winHandle))
			{
				// and return the instance
				return _instance;
			}
			else
			{
				return nullptr;
			}
		}

		array<Preset^>^ WMPEffect::Presets::get()
		{
			return _presets;
		}

		Preset^ WMPEffect::CurrentPreset::get()
		{
			long preset = 0;
			_pWMPEffects->GetCurrentPreset(&preset);
			return _presets[(int)preset];
		}

		bool WMPEffect::IsIWMPEffect2::get()
		{
			return _isIWMPEffect2;
		}

		bool WMPEffect::IsWindowLess::get()
		{
			return _isWindowLess;
		}

		String^ WMPEffect::Title::get()
		{
			BSTR bstr = SysAllocString ( L"" );
			_pWMPEffects->GetTitle(&bstr);

			const wchar_t* wchar = (const wchar_t*)bstr;
			String^ title = gcnew String(wchar);
			
			SysFreeString ( bstr );
			
			return title;
		}

		WMPEffect::WMPEffect()
		{
			_pTimedLevel = new TimedLevel();
			_pRectangle = new RECT();
		}

		bool WMPEffect::InternalSelectEffect(String^ effectClsId, IntPtr winHandle)
		{

#if ENABLETRACE

			TextWriterTraceListener^ myWriter = gcnew TextWriterTraceListener( "Trace.log" );
			Trace::Listeners->Add( myWriter );

#endif
			// initialize COM lib
			CoInitialize(0);

			// convert clsid from managed string to unmanaged chaos string
			pin_ptr<const wchar_t> pClsId = PtrToStringChars(effectClsId);

			// convert string to CLSID
			CLSID clsId;
			CLSIDFromString((LPOLESTR)pClsId, &clsId);

			// convert iid from managed string to unmanaged chaos string
			pin_ptr<const wchar_t> pIid = PtrToStringChars("{695386EC-AA3C-4618-A5E1-DD9A8B987632}");
			
			// convert string to IID
			IID iid;
			CLSIDFromString((LPOLESTR)pIid, &iid);

			// attempt to create an IWMPEffect2 object
			LPVOID pWMPEffect = NULL;
			HRESULT rc = CoCreateInstance(clsId, NULL, CLSCTX_INPROC_SERVER, iid, &pWMPEffect);
			
			// later we need to know if we have an IWMPEffect2 or not
			_isIWMPEffect2 = (rc == S_OK);
			
			// if this fails, maybe it's an old IWMPEffect object
			if (rc == E_NOINTERFACE)
			{
				// convert iid from managed string to unmanaged chaos string
				pIid = PtrToStringChars("{D3984C13-C3CB-48e2-8BE5-5168340B4F35}");

				// convert string to IID
				CLSIDFromString((LPOLESTR)pIid, &iid);
			
				// attempt to create the old IWMPEffect object
				rc = CoCreateInstance(clsId, NULL, CLSCTX_INPROC_SERVER, iid, &pWMPEffect);
			}

			if (rc != S_OK)
			{
				// release COM lib
				CoUninitialize();
			}

#if ENABLETRACE

			if (rc != S_OK)
			{
				if (rc == REGDB_E_CLASSNOTREG)
					Trace::WriteLine("REGDB_E_CLASSNOTREG");
				else if (rc == CLASS_E_NOAGGREGATION)
					Trace::WriteLine("CLASS_E_NOAGGREGATION");
				else if (rc == E_NOINTERFACE)
					Trace::WriteLine("E_NOINTERFACE");
			}

#endif
				
			if (rc == S_OK)
			{
				// cast the result back to our IWMPEffect2 interface
				_pWMPEffects = (IWMPEffects2*) pWMPEffect;
				
				if (_isIWMPEffect2)
				{
					//// if this succeeds; the viz is supposed to be windowless
					//rc = _pWMPEffects->Create(NULL);
					//_isWindowLess = (rc == S_OK);
					
					//if (!_isWindowLess)
					//	rc = _pWMPEffects->Create((HWND)(winHandle.ToPointer()));
					
					_isWindowLess = false;
					rc = _pWMPEffects->Create((HWND)(winHandle.ToPointer()));

				}
			}

			if (rc == S_OK && _isIWMPEffect2)
			{
				// Alchemy fails here: however it can be instantiated as IWMPEffect2
				//rc = _pWMPEffects->SetCore(NULL);
			}

			long presetCount = 0;
			if (rc == S_OK)
			{
				rc = _pWMPEffects->GetPresetCount(&presetCount);
			}

			if (rc == S_OK)
			{
				// create a generic list of presets
				List<Preset^>^ presetList = gcnew List<Preset^>();

				BSTR bstrTitle = SysAllocString(L"");
				
				for (long i = 0; i < presetCount; i++)
				{
					rc = _pWMPEffects->GetPresetTitle(i, &bstrTitle);

					const wchar_t* wcharTitle = (const wchar_t*)bstrTitle;
					String^ title = gcnew String(wcharTitle);
					presetList->Add(gcnew Preset(title, i));
				}
				
				SysFreeString(bstrTitle);
				_presets = presetList->ToArray();
			}

			if (rc == S_OK)
			{
				rc = _pWMPEffects->SetCurrentPreset(0);
			}

#if ENABLETRACE

			Trace::Flush();

#endif

			return (rc == S_OK);
		}

		void WMPEffect::SetCurrentPreset(Preset^ preset)
		{
			_pWMPEffects->SetCurrentPreset(preset->_number);
		}

		void WMPEffect::SetCurrentPreset(int number)
		{
			_pWMPEffects->SetCurrentPreset(number);
		}

		void WMPEffect::MediaInfo(int channels, int samplerate, String^ title)
		{
			pin_ptr<const wchar_t> t = PtrToStringChars(title);
			_pWMPEffects->MediaInfo((long)channels, (long)samplerate, (BSTR)t);
		}

		void WMPEffect::Render(array<float,2>^ waveData, array<float,2>^ fftData, IntPtr hdc, System::Drawing::Rectangle rectangle)
		{
			_pTimedLevel->timeStamp = DateTime::Now.Ticks;
			_pTimedLevel->state = play_state;
			
			for (int channel = 0; channel < SA_CHANNEL_COUNT; channel++)
			{
				for (int sample = 0; sample < SA_BUFFER_SIZE; sample++)
				{
					// Convert float value between -1 and 1 to 0 and 255
					float floatWaveValue = (waveData[channel, sample] + 1.0f) * 127.5f;

					if (floatWaveValue < 0.0f)
						floatWaveValue = 0.0f;
					else if (floatWaveValue > 255.0f)
						floatWaveValue = 255.0f;

					_pTimedLevel->waveform[channel][sample] = (unsigned char) floatWaveValue;


					// Project the float values between 1 en 0 on logarithmic scale between 
					// 90 and 0 (representing a scale from 0 to -90 dB)
					// then convert to values between 255 and 0
					// Anything below zero will be cut off later.
					float floatFftValue = (90.0f + ((float)Math::Log10(fftData[channel, sample]) * 20.0f)) * (255.0f / 90.0f);

					if (floatFftValue < 0.0f)
						floatFftValue = 0.0f;
					else if (floatFftValue > 255.0f)
						floatFftValue = 255.0f;

					_pTimedLevel->frequency[channel][sample] = (unsigned char) floatFftValue;
				}
			}

			if (!_isIWMPEffect2 || _isWindowLess)
			{
				_pRectangle->bottom = rectangle.Bottom;
				_pRectangle->left= rectangle.Left;
				_pRectangle->right = rectangle.Right;
				_pRectangle->top = rectangle.Top;

				HDC hDC = ::GetDC((HWND)(hdc.ToPointer())); 

				_pWMPEffects->Render(_pTimedLevel, hDC, _pRectangle);
			}
			else
			{
				_pWMPEffects->RenderWindowed(_pTimedLevel, false);
			}
		}

		void WMPEffect::Release()
		{
			// only if an effect has been engaged
			if (_pWMPEffects != NULL)
			{
				if (_isIWMPEffect2)
					_pWMPEffects->Destroy();

				// release COM object
				_pWMPEffects->Release();
				_pWMPEffects = NULL;
			}

			// release COM lib
			CoUninitialize();

#if ENABLETRACE

			// flush all trace info to disk
			Trace::Flush();

#endif

		}
	}
}