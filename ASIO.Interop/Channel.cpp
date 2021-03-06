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
#include "Channel.h"

namespace PureAudio
{
	namespace Asio
	{
		namespace Interop
		{
			Channel::Channel(IAsio* pAsio, bool IsInput, int channelNumber, void* pTheirBuffer0, void* pTheirBuffer1)
			{
				// we need one of these to query the driver
				ASIOChannelInfo* pChannelInfo = new ASIOChannelInfo();

				// populated with this
				pChannelInfo->channel = channelNumber;
				pChannelInfo->isInput = IsInput;

				// now we can get the data
				pAsio->getChannelInfo(pChannelInfo);

				// get channelinfo
				_isInput = pChannelInfo->isInput != 0;
				_name = gcnew String(pChannelInfo->name);
				_sampleType = (ASIOSampleType)pChannelInfo->type;
				
				// create an object representing the buffer
				switch (_sampleType)
				{
				case ASIOSTInt32LSB:
					_buffer = gcnew BufferInt32LSB(pTheirBuffer0, pTheirBuffer1);
					break;
				
				case ASIOSTInt24LSB:
					_buffer = gcnew BufferInt24LSB(pTheirBuffer0, pTheirBuffer1);
					break;

				case ASIOSTInt16LSB:
					_buffer = gcnew BufferInt16LSB(pTheirBuffer0, pTheirBuffer1);
					break;
				
				default:
					throw gcnew Exception(System::String::Format("Sampletype '{0}' is not supported.", _sampleType));
				}
				
				// clip value for our float sample data
				maxSampleValue = 8388607.0f / 8388608.0f;
			}

			String^ Channel::Name::get()
			{
				return _name;
			}

			ASIOSampleType Channel::SampleType::get()
			{
				return _sampleType;
			}

			void Channel::default::set(int sample, float value)
			{
				// set the value of a sample

				// clip sample value to avoid problems with conversion.
				if (value > maxSampleValue)
					value = maxSampleValue;
				else if (value < -1.0f)
					value = -1.0f;

				_buffer[sample] = value;
			}

			float Channel::default::get(int sample)
			{
				// get the value of a sample
				return _buffer[sample];
			}
		}
	}
}