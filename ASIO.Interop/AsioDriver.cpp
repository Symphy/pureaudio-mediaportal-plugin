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

#include "stdio.h"
#include "BufferInfo.h"
#include "LatencyInfo.h"
#include "Channel.h"
#include "InstalledDriver.h"
#include "AsioDriver.h"
#include "Channel.h"
#include <vcclr.h>

#define ENABLETRACE 0

using namespace System::Diagnostics;
//using namespace MediaPortal::GUI::Library;

namespace BlueWave
{
	namespace Interop
	{
		namespace Asio
		{
			array<InstalledDriver^>^ AsioDriver::InstalledDrivers::get()
			{
				// if we don't know what drivers are installed, ask the InstalledDriver class
				if (!_installedDrivers) _installedDrivers = InstalledDriver::GetInstalledDriversFromRegistry();

				// and return
				return _installedDrivers;
			}

			// static forward to instance method
			void BufferSwitch(long bufferIndex, ASIOBool directProcess)
			{
				AsioDriver::Instance->OnBufferSwitch(bufferIndex, directProcess);
			}

			// static forward to instance method
			ASIOTime* BufferSwitchTimeInfo(ASIOTime* params, long doubleBufferIndex, ASIOBool directProcess)
			{
				return AsioDriver::Instance->OnBufferSwitchTimeInfo(params, doubleBufferIndex, directProcess);
			}

			// static forward to instance method
			void SampleRateDidChange(ASIOSampleRate rate)
			{
				AsioDriver::Instance->OnSampleRateDidChange(rate);
			}

			// static forward to instance method
			long AsioMessage(long a, long b , void* c , double* d)
			{
				return AsioDriver::Instance->OnAsioMessage(a, b, c, d);
			}

			// return the singleton instance
			AsioDriver^ AsioDriver::Instance::get()
			{
				return _instance;
			}

			int AsioDriver::Version::get()
			{
				// make sure a driver has been engaged
				CheckInitialised();

				// refer to driver
				return _pDriver->getDriverVersion();
			}

			String^ AsioDriver::DriverName::get()
			{
				// make sure a driver has been engaged
				CheckInitialised();

				// refer to driver
				char driverName[1000];
				_pDriver->getDriverName(driverName);
				return gcnew String(driverName);
			}

			int AsioDriver::BufferSize::get()
			{
				// make sure a driver has been engaged
				CheckInitialised();

				return _bufferSize;
			}

			BufferInfo^ AsioDriver::Buffer::get()
			{
				// make sure a driver has been engaged
				CheckInitialised();

				return gcnew BufferInfo(_pDriver);
			}

			LatencyInfo^ AsioDriver::Latency::get()
			{
				// make sure a driver has been engaged
				CheckInitialised();

				return gcnew LatencyInfo(_pDriver);
			}

			array<Channel^>^ AsioDriver::InputChannels::get()
			{
				// make sure a driver has been engaged
				CheckInitialised();

				return _inputChannels;
			}

			array<Channel^>^ AsioDriver::OutputChannels::get()
			{
				// make sure a driver has been engaged
				CheckInitialised();

				return _outputChannels;
			}

			ASIOError AsioDriver::LastASIOError::get()
			{
				return _lastASIOError;
			}
      
      // Contructor
      AsioDriver::AsioDriver()
      {
        _bufferUpdateEventArgs = gcnew EventArgs();
      }

			// selects a driver into our singleton instance
			AsioDriver^ AsioDriver::SelectDriver(InstalledDriver^ installedDriver, IntPtr sysHandle)
			{
				// create a new instance of the driver (this will become the singleton)
				_instance = gcnew AsioDriver();

				if (_instance->InternalSelectDriver(installedDriver, sysHandle))
				{
					// and return the instance
					return _instance;
				}
				else
				{
					return nullptr;
				}
			}

			bool AsioDriver::InternalSelectDriver(InstalledDriver^ installedDriver, IntPtr sysHandle)
			{
#if ENABLETRACE
				TextWriterTraceListener^ myWriter = gcnew TextWriterTraceListener( "Trace.log" );
				Trace::Listeners->Add( myWriter );
#endif

				// initialize COM lib
				CoInitialize(0);
				
				long inputs, outputs;

				// class and interface id for Asio driver
				CLSID m_clsid;

				// convert from managed string to unmanaged chaos string
				pin_ptr<const wchar_t> clsid = PtrToStringChars(installedDriver->ClsId);

				// convert string from registry to CLSID
				CLSIDFromString((LPOLESTR)clsid, &m_clsid);

				// and actually create the object and return its interface (clsid used twice)
				LPVOID pAsio = NULL;
				HRESULT rc = CoCreateInstance(m_clsid, NULL, CLSCTX_INPROC_SERVER, m_clsid, &pAsio);

				// cast the result back to our ASIO interface
				_pDriver = (IAsio*) pAsio;

				// and we're ready to use it
				
				bool result = (_pDriver->init(sysHandle.ToPointer()) == ASIOTrue);
				if (result)
				{
					// get the number of inputs and outputs supported by the driver
					_lastASIOError = _pDriver->getChannels(&inputs, &outputs);
					result = (_lastASIOError == ASE_OK);
				}
				
				if (result)
				{
					// and remember these (with a host specified ceiling)
					_nInputs = inputs;
					_nOutputs = outputs;

					// create the ASIO callback struct
					_pCallbacks = new ASIOCallbacks();

					// and convert our delegates to unmanaged typedefs
					_pCallbacks->bufferSwitch = BufferSwitch; 
					_pCallbacks->asioMessage = AsioMessage;
					_pCallbacks->bufferSwitchTimeInfo = BufferSwitchTimeInfo;
					_pCallbacks->sampleRateDidChange = SampleRateDidChange;
				}
				return result;
			}

			bool AsioDriver::CreateBuffers(bool useMaxBufferSize)
			{
				// we need the total number of channels here
				int totalChannels = _nInputs + _nOutputs;

				// create our input and output channel arrays
				_inputChannels = gcnew array<Channel^>(_nInputs);
				_outputChannels = gcnew array<Channel^>(_nOutputs);

				// each channel needs a buffer info
				ASIOBufferInfo* pBufferInfos = new ASIOBufferInfo[totalChannels];

				// now create each input channel and set up its buffer
				for (int index = 0; index < _nInputs; index++)
				{
					pBufferInfos[index].isInput = 1;
					pBufferInfos[index].channelNum = index;
					pBufferInfos[index].buffers[0] = 0;
					pBufferInfos[index].buffers[1] = 0;
				}

				// and do the same for output channels
				for (int index = 0; index < _nOutputs; index++)
				{
					pBufferInfos[index + _nInputs].isInput = 0;
					pBufferInfos[index + _nInputs].channelNum = index;
					pBufferInfos[index + _nInputs].buffers[0] = 0;
					pBufferInfos[index + _nInputs].buffers[1] = 0;
				}

				if (useMaxBufferSize)
				{
					// use the drivers maximum buffer size
					_bufferSize = Buffer->m_nMaxSize;
				}
				else
				{
					// use the drivers preferred buffer size
					_bufferSize = Buffer->m_nPreferredSize;
				}

				// get the driver to create its buffers
				_lastASIOError = _pDriver->createBuffers(pBufferInfos, totalChannels, _bufferSize, _pCallbacks);
				
				bool result = (_lastASIOError == ASE_OK);
				if (result)
				{
					// now go and create the managed channel objects to manipulate these buffers
					for (int index = 0; index < _nInputs; index++)
					{
						_inputChannels[index] = gcnew Channel(_pDriver, true, index,
							pBufferInfos[index].buffers[0],
							pBufferInfos[index].buffers[1]);
					}

					for (int index = 0; index < _nOutputs; index++)
					{
						_outputChannels[index] = gcnew Channel(_pDriver, false, index,
							pBufferInfos[index + _nInputs].buffers[0],
							pBufferInfos[index + _nInputs].buffers[1]);
					}
					_outputReadySupport = _pDriver->outputReady();
#if ENABLETRACE
					Trace::WriteLine(String::Format("outputReady(): {0}", _outputReadySupport));
#endif
				}
				return result;	
			}

			bool AsioDriver::ShowControlPanel()
			{
				// make sure a driver has been engaged
				CheckInitialised();

				_lastASIOError = _pDriver->controlPanel();
				return (_lastASIOError == ASE_OK);
			}

			int AsioDriver::GetSampleRate()
			{
				// make sure a driver has been engaged
				CheckInitialised();
				
				// refer to driver
				double rate;
				_lastASIOError = _pDriver->getSampleRate(&rate);
				
				if (_lastASIOError == ASE_OK)
					return Convert::ToInt32(rate);
				else
					return 0;
			}

			bool AsioDriver::SetSampleRate(double rate)
			{
				// make sure a driver has been engaged
				CheckInitialised();

				_lastASIOError = _pDriver->setSampleRate(rate);
				return (_lastASIOError == ASE_OK);
			}

			bool AsioDriver::CanSampleRate(double rate)
			{
				// make sure a driver has been engaged
				CheckInitialised();

				return (_pDriver->canSampleRate(rate) == ASE_OK);
			}

			bool AsioDriver::Start()
			{
				// make sure a driver has been engaged
				CheckInitialised();

				_lastASIOError = _pDriver->start();
				return (_lastASIOError == ASE_OK);
			}

			bool AsioDriver::Stop()
			{
				// make sure a driver has been engaged
				CheckInitialised();

				_lastASIOError = _pDriver->stop();
				return (_lastASIOError == ASE_OK);
			}

			bool AsioDriver::DisposeBuffers()
			{
				// make sure a driver has been engaged
				CheckInitialised();

				_lastASIOError = _pDriver->disposeBuffers();
				return (_lastASIOError == ASE_OK);
			}

			String^ AsioDriver::GetErrorMessage()
			{
				// make sure a driver has been engaged
				CheckInitialised();

				// refer to driver
				char errorMessage[1000];
				_pDriver->getErrorMessage(errorMessage);
				return gcnew String(errorMessage);
			}

			void AsioDriver::Release()
			{
				// only if a driver has been engaged
				if (_pDriver != NULL)
				{
					// release COM object
					_pDriver->Release();
					_pDriver = NULL;
				}
				
				// release COM lib
				CoUninitialize();

#if ENABLETRACE
				// flush all trace info to disk
				Trace::Flush();
#endif
			}

			void AsioDriver::OnBufferSwitch(long doubleBufferIndex, ASIOBool directProcess)
			{
#if ENABLETRACE
				Trace::WriteLine(String::Format("OnBufferSwitch({0}, {1})", doubleBufferIndex, directProcess));
#endif

        // a buffer switch is occuring, first off, 
				// tell all channels what buffer needs to be read/written
				for (int index = 0; index < _nInputs; index++)
					_inputChannels[index]->_buffer->SetDoubleBufferIndex(doubleBufferIndex);
				
        for (int index = 0; index < _nOutputs; index++)
					_outputChannels[index]->_buffer->SetDoubleBufferIndex(doubleBufferIndex);

				// next we raise an event so that the caller can do their buffer manipulation
        // Re-use the available eventargs instance to minimize the load on gc
				if (_bufferUpdateEvent != nullptr)
					_bufferUpdateEvent(this, _bufferUpdateEventArgs);

				// notify driver we're done, see ASIO SDK for further explanation
				if (_outputReadySupport == ASE_OK)
					_pDriver->outputReady();
			}

			ASIOTime* AsioDriver::OnBufferSwitchTimeInfo(ASIOTime* params, long doubleBufferIndex, ASIOBool directProcess)
			{
				// no implementation
				return nullptr;
			}

			void AsioDriver::OnSampleRateDidChange(ASIOSampleRate rate)
			{
				// no implementation
			}

			long AsioDriver::OnAsioMessage(long selector, long value, void* message, double* opt)
			{
#if ENABLETRACE
				Trace::WriteLine(String::Format("OnAsioMessage({0})", selector));
#endif

				switch (selector)
				{
				case kAsioSelectorSupported:
					switch (value)
					{
					case kAsioEngineVersion:
						return 1;
					case kAsioResetRequest:
						return 0;
					case kAsioBufferSizeChange:
						return 0;
					case kAsioResyncRequest:
						return 0;
					case kAsioLatenciesChanged:
						return 0;
					case kAsioSupportsTimeInfo:
						return 1;
					case kAsioSupportsTimeCode:
						return 1;
					}
				case kAsioEngineVersion:
					return 2;
				case kAsioResetRequest:
					return 1;
				case kAsioBufferSizeChange:
					return 0;
				case kAsioResyncRequest:
					return 0;
				case kAsioLatenciesChanged:
					return 0;
				case kAsioSupportsTimeInfo:
					return 0;
				case kAsioSupportsTimeCode:
					return 0;
				}
				return 0;
			}

			void AsioDriver::CheckInitialised()
			{
				if (_pDriver == NULL)
				{
					throw gcnew ApplicationException("Select driver first");
				}
			}
		}
	}
}