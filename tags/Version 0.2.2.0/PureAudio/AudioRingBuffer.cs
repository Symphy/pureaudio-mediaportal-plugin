#region Copyright (C) 2005-2010 Team MediaPortal

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

#endregion

using System;
using System.Runtime.InteropServices;

namespace MediaPortal.Player.PureAudio
{
	public class AudioRingBuffer
	{
		private Object _locker = new Object();

		private int _delay;

		private byte[] _buffer;
		private int _writePointer;
		private int _readPointer;
		private int _space;
		private int _bufferLength;
		private int _bytesPerMilliSec;
		private bool _Is32bit = false;

		/// <summary>
		/// Returns the current delay in milliseconds (delay for the actual filled part of the buffer).
		/// </summary>
		public int Delay
		{
			get
			{
				return _delay - ((_space / _bufferLength) * _delay);
			}
		}

		public int Length
		{
			get
			{
				return _bufferLength;
			}
		}
		public int BytesPerMilliSec
		{
			get
			{
				return _bytesPerMilliSec;
			}
		}

		/// <summary>
		/// Returns the amount of bytes available for writing
		/// </summary>
		public int Space
		{
			get
			{
				return _space;
			}
		}

		/// <summary>
		/// Returns the amount of bytes available for reading
		/// </summary>
		public int Count
		{
			get
			{
				return _bufferLength - _space;
			}
		}

		public AudioRingBuffer(int samplingRate, int channels, int delay)
		{
			_Is32bit = (IntPtr.Size == 4);
			_delay = delay;
			_bufferLength = AudioRingBuffer.CalculateLength(samplingRate, channels, delay);
			_bytesPerMilliSec = AudioRingBuffer.CalculateLength(samplingRate, channels, 1);
			_buffer = new byte[_bufferLength];

			Clear();
		}

		public static int CalculateLength(int samplingRate, int channels, int delay)
		{
			int sampleSize = channels * 4;

			// Use the (long) cast to avoid arithmetic overflow 
			int length = (int)((long)samplingRate * sampleSize * delay / 1000);

			// round to whole samples
			length = length / sampleSize;
			length = length * sampleSize;

			return length;
		}

		// Write:
		// - Buffer         0123456789  0123456789  0123456789
		// - Write pointer  >>>   |>>>     |>>>     >>>>>|>>>>
		// - Read pointer      |               |         |

		// Read:
		// - Buffer         0123456789  0123456789  0123456789
		// - Write pointer        |         |          |
		// - Read pointer      |>>      >>>>|>>>>>  >>>   |>>>

		public int Write(byte[] data, int count)
		{
			// Validate parameters, may be left out for performance
			//if (count > data.Length)
			//   count = data.Length;

			int readPointer;
			int writePointer;
			int space;

			// To minimize locking contention, reserve the space we are going to fill 
			// before we actually fill it.
			lock (_locker)
			{
				readPointer = _readPointer;
				writePointer = _writePointer;
				space = _space;

				if (count > space)
					count = space;

				_space = _space - count;
				_writePointer = writePointer + count;

				if (_writePointer > _bufferLength)
					_writePointer = _writePointer - _bufferLength;
			}

			if (writePointer >= readPointer)
			{
				int count1 = Math.Min(count, _bufferLength - writePointer);
				Array.Copy(data, 0, _buffer, writePointer, count1);

				int count2 = Math.Min(count - count1, readPointer);
				if (count2 > 0)
					Array.Copy(data, count1, _buffer, 0, count2);

				return count1 + count2;
			}
			else
			{
				int count1 = Math.Min(count, readPointer - writePointer);
				Array.Copy(data, 0, _buffer, writePointer, count1);

				return count1;
			}
		}

		public int ShadowRead(IntPtr buffer, int requested, int offset)
		{
			int read;
			int readPointer;
			int writePointer;
			int space;

			lock (_locker)
			{
				readPointer = _readPointer;
				writePointer = _writePointer;
				space = _space;
			}

			offset = Math.Min(offset, _bufferLength - space);

			readPointer += offset;
			if (readPointer > _bufferLength)
				readPointer -= _bufferLength;

			requested = Math.Min(requested, _bufferLength - space - offset);

			if (writePointer > readPointer)
			{
				int count1 = Math.Min(requested, writePointer - readPointer);

				Marshal.Copy(_buffer, readPointer, buffer, count1);
				readPointer += count1;

				read = count1;
			}
			else
			{
				int count1 = Math.Min(requested, _bufferLength - readPointer);

				Marshal.Copy(_buffer, readPointer, buffer, count1);
				readPointer += count1;

				if (readPointer == _buffer.Length)
					readPointer = 0;

				int count2 = Math.Min(requested - count1, writePointer);
				if (count2 > 0)
				{
					IntPtr ptr;
					if (_Is32bit)
						ptr = new IntPtr(buffer.ToInt32() + count1);
					else
						ptr = new IntPtr(buffer.ToInt64() + count1);

					Marshal.Copy(_buffer, 0, ptr, count2);
					readPointer = count2;
				}
				read = count1 + count2;
			}
			return read;
		}

		public int Read(IntPtr buffer, int requested, int offset)
		{
			int read;
			int readPointer;
			int writePointer;
			int space;

			lock (_locker)
			{
				readPointer = _readPointer;
				writePointer = _writePointer;
				space = _space;
			}

			offset = Math.Min(offset, _bufferLength - space);

			readPointer += offset;
			if (readPointer > _bufferLength)
				readPointer -= _bufferLength;

			requested = Math.Min(requested, _bufferLength - space - offset);

			if (writePointer > readPointer)
			{
				int count1 = Math.Min(requested, writePointer - readPointer);

				if (buffer != IntPtr.Zero)
					Marshal.Copy(_buffer, readPointer, buffer, count1);
				readPointer += count1;

				read = count1;
			}
			else
			{
				int count1 = Math.Min(requested, _bufferLength - readPointer);

				if (buffer != IntPtr.Zero)
					Marshal.Copy(_buffer, readPointer, buffer, count1);
				readPointer += count1;

				if (readPointer == _buffer.Length)
					readPointer = 0;

				int count2 = Math.Min(requested - count1, writePointer);
				if (count2 > 0)
				{
					if (buffer != IntPtr.Zero)
					{
						IntPtr ptr;
						if (_Is32bit)
							ptr = new IntPtr(buffer.ToInt32() + count1);
						else
							ptr = new IntPtr(buffer.ToInt64() + count1);

						Marshal.Copy(_buffer, 0, ptr, count2);
					}
					readPointer = count2;
				}
				read = count1 + count2;
			}

			readPointer = readPointer - offset;
			if (readPointer < 0)
				readPointer += _bufferLength;

			lock (_locker)
			{
				_readPointer = readPointer;
				_space += read;
			}
			return read;
		}

		public void Clear()
		{
			_writePointer = 0;
			_readPointer = 0;
			_space = _bufferLength;
		}
	}
}
