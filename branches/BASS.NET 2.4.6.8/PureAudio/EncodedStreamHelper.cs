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
using System.Collections.Generic;
using System.Text;
using Un4seen.Bass;
using MediaPortal.GUI.Library;

namespace MediaPortal.Player.PureAudio
{
  public static class EncodedStreamHelper
  {
    public static StreamContentType GetStreamContentType(int stream)
    {
      StreamContentType result;
      if (Is14bitDTS(stream))
        result = StreamContentType.DTS14Bit;
      else if (IsIEC(stream))
        result = StreamContentType.IEC61937;
      else if (IsDD(stream))
        result = StreamContentType.DD;
      else if (IsDTS(stream))
        result = StreamContentType.DTS;
      else
        result = StreamContentType.PCM;

      return result;
    }

    public static bool Sync(int stream, StreamContentType streamContentType)
    {
      SyncWord syncWord;
      switch (streamContentType)
      {
        case StreamContentType.DD:
          syncWord = new DDSyncWord();
          break;

        case StreamContentType.DTS14Bit:
          syncWord = new DTS14bitSyncWord();
          break;

        case StreamContentType.DTS:
          syncWord = new DTSSyncWord();
          break;

        case StreamContentType.IEC61937:
          syncWord = new IECSyncWord();
          break;

        default:
          syncWord = null;
          break;
      }

      if (syncWord != null)
        return SyncToWord(stream, syncWord);
      else
        return false;
    }

    private static bool IsIEC(int stream)
    {
      SyncWord syncWord = new IECSyncWord();
      return IsEncoded(stream, syncWord);
    }

    private static bool IsDD(int stream)
    {
      SyncWord syncWord = new DDSyncWord();
      return IsEncoded(stream, syncWord);
    }

    private static bool Is14bitDTS(int stream)
    {
      SyncWord syncWord = new DTS14bitSyncWord();
      return IsEncoded(stream, syncWord);
    }

    private static bool IsDTS(int stream)
    {
      SyncWord syncWord = new DTSSyncWord();
      return IsEncoded(stream, syncWord);
    }

    private static bool IsEncoded(int stream, SyncWord syncWord)
    {
      const int framesToCheck = 5;
      const int bytesPerSample = 4;
      const int bytesPerWord = 2;
      const int channelCount = 2;

      long streamLength = Bass.BASS_ChannelGetLength(stream);
      long currentPosition = 0;
      if (streamLength > 0)
      {
        currentPosition = Bass.BASS_ChannelGetPosition(stream);
        if (currentPosition != 0)
          Bass.BASS_ChannelSetPosition(stream, 0);
      }

      SyncFifoBuffer syncFifoBuffer = new SyncFifoBuffer(syncWord);
      float[] readBuffer = new float[channelCount];

      int lastSyncWordPosition = -1;
      int lastFrameSize = -1;
      int frameCount = 0;

      bool result = false;
      bool endOfStream = false;
      int sampleIndex = 0;
      int maxSampleIndex = (syncWord.MaxFrameSize / bytesPerWord) * framesToCheck + syncWord.WordLength;

      while (!result && !endOfStream && sampleIndex < maxSampleIndex)
      {
        int bytesRead = Bass.BASS_ChannelGetData(stream, readBuffer, readBuffer.Length * bytesPerSample);
        endOfStream = bytesRead <= 0;
        if (!endOfStream)
        {
          int samplesRead = bytesRead / bytesPerSample;
          int readSample = 0;
          while (!result && readSample < samplesRead)
          {
            // Convert float value to word
            UInt16 word = (UInt16)(readBuffer[readSample] * 32768);

            // Add word to fifo buffer
            syncFifoBuffer.Write(word);

            // Check Sync word
            if (syncFifoBuffer.IsMatch())
            {
              int newSyncWordPosition = (sampleIndex - syncWord.WordLength + 1) * bytesPerWord;
              if (lastSyncWordPosition != -1)
              {
                int thisFrameSize = newSyncWordPosition - lastSyncWordPosition;
                if (lastFrameSize != -1)
                {
                  if (thisFrameSize != lastFrameSize)
                    break;
                }
                lastFrameSize = thisFrameSize;
                frameCount++;
              }
              lastSyncWordPosition = newSyncWordPosition;
              result = (frameCount == framesToCheck);
            }
            sampleIndex++;
            readSample++;
          }
        }
        else
          endOfStream = true;
      }

      if (streamLength > 0)
        Bass.BASS_ChannelSetPosition(stream, currentPosition);

      return result;
    }

    private static bool SyncToWord(int stream, SyncWord syncWord)
    {
      const int bytesPerSample = 4;
      const int bytesPerWord = 2;
      const int channelCount = 2;

      long streamLength = Bass.BASS_ChannelGetLength(stream);
      long currentPosition = 0;
      if (streamLength > 0)
        currentPosition = Bass.BASS_ChannelGetPosition(stream);

      SyncFifoBuffer syncFifoBuffer = new SyncFifoBuffer(syncWord);
      float[] readBuffer = new float[channelCount];

      bool result = false;
      bool endOfStream = false;
      int sampleIndex = 0;
      int maxSampleIndex = (syncWord.MaxFrameSize / bytesPerWord) + syncWord.WordLength;

      while (!result && !endOfStream && sampleIndex < maxSampleIndex)
      {
        // For float streams we get one float value for each 16bit word
        int bytesRead = Bass.BASS_ChannelGetData(stream, readBuffer, readBuffer.Length * bytesPerSample);
        endOfStream = bytesRead <= 0;
        if (!endOfStream)
        {
          int samplesRead = bytesRead / bytesPerSample;
          int readSample = 0;
          while (!result && readSample < samplesRead)
          {
            // Convert float value to word
            UInt16 word = (UInt16)(readBuffer[readSample] * 32768);

            // Add word to fifo buffer
            syncFifoBuffer.Write(word);

            // Check Sync word
            if (syncFifoBuffer.IsMatch())
            {
              long pos = currentPosition + (sampleIndex - syncWord.WordLength + 1) * bytesPerWord;
              //Log.Debug("PureAudio: Sync to next frame: changing position from {0} to {1}", currentPosition, pos); 
              Bass.BASS_ChannelSetPosition(stream, pos);
              result = true;
            }
            sampleIndex++;
            readSample++;
          }
        }
      }

      if (!result && streamLength > 0)
          Bass.BASS_ChannelSetPosition(stream, currentPosition);

      return result;
    }

    class SyncFifoBuffer
    {
      SyncWord _SyncWord;
      UInt16[] _Buffer;

      public SyncFifoBuffer(SyncWord syncWord)
      {
        _SyncWord = syncWord;
        _Buffer = new UInt16[syncWord.WordLength];
      }

      public void Write(UInt16 value)
      {
        for (int i = 1; i < _Buffer.Length; i++)
        {
          _Buffer[i - 1] = _Buffer[i];
        }
        _Buffer[_Buffer.Length - 1] = value;
      }

      public bool IsMatch()
      {
        bool isSyncWord = true;
        int index = 0;
        while (isSyncWord && index < _SyncWord.WordLength)
        {
          if (_Buffer[index] < _SyncWord.Word[index, 0] || _Buffer[index] > _SyncWord.Word[index, 1])
            isSyncWord = false;
          index++;
        }
        return isSyncWord;
      }
    }

    abstract class SyncWord
    {
      protected UInt16[,] _Word;
      protected int _WordLength;

      protected int _MaxFrameSize;

      public UInt16[,] Word
      {
        get { return _Word; }
      }

      public int WordLength
      {
        get
        {
          if (_WordLength == 0)
            _WordLength = _Word.GetLength(0);
          return _WordLength;
        }
      }

      public int MaxFrameSize
      {
        get { return _MaxFrameSize; }
      }

      public SyncWord()
      {
      }
    }

    class IECSyncWord : SyncWord
    {
      public IECSyncWord()
      {
        // IEC 61937 (S/PDIF compressed audio) sync word:
        // 0xF8724E1F

        _MaxFrameSize = 8192;
        _Word = new UInt16[2, 2];
        _Word[0, 0] = 0xF872;
        _Word[0, 1] = 0xF872;
        _Word[1, 0] = 0x4E1F;
        _Word[1, 1] = 0x4E1F;
      }
    }

    class DDSyncWord : SyncWord
    {
      public DDSyncWord()
      {
        // DD sync word:
        // 0x0B77

        _MaxFrameSize = 8192;
        _Word = new UInt16[1, 2];
        _Word[0, 0] = 0x0B77;
        _Word[0, 1] = 0x0B77;
      }
    }

    class DTS14bitSyncWord : SyncWord
    {
      public DTS14bitSyncWord()
      {
        // DTS Sync word plus extension in 14 bit format:
        // 0x1FFF              0xE800              0x07F
        // 0001 1111 1111 1111 1110 1000 0000 0000 0000 0111 1111

        _MaxFrameSize = 8192;
        _Word = new UInt16[3, 2];
        _Word[0, 0] = 0x1FFF;
        _Word[0, 1] = 0x1FFF;
        _Word[1, 0] = 0xE800;
        _Word[1, 1] = 0xE800;
        _Word[2, 0] = 0x07F0;
        _Word[2, 1] = 0x07FF;
      }
    }

    class DTSSyncWord : SyncWord
    {
      public DTSSyncWord()
      {
        // DTS Sync word:
        // 0x7FFE8001
        // 0111 1111 1111 1110 1000 0000 0000 0001

        _MaxFrameSize = 8192;
        _Word = new UInt16[2, 2];
        _Word[0, 0] = 0x7FFE;
        _Word[0, 1] = 0x7FFE;
        _Word[1, 0] = 0x8001;
        _Word[1, 1] = 0x8001;
      }
    }
  }
}
