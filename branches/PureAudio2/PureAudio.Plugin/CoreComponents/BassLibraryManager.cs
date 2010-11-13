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
using System.IO;
using System.Collections.Generic;
using Un4seen.Bass;

namespace MediaPortal.Player.PureAudio
{
  public partial class BassPlayer
  {
    /// <summary>
    /// Manages the Bass audio library and its plugins.
    /// </summary>
    private class BassLibraryManager : IDisposable
    {
      #region Static members

      private static bool _BassInitialized;
      private static List<int> _DecoderPluginHandles = new List<int>();

      /// <summary>
      /// Loads and initializes the Bass library.
      /// </summary>
      /// <returns>The new instance.</returns>
      public static BassLibraryManager Create()
      {
        BassLibraryManager bassLibrary = new BassLibraryManager();
        bassLibrary.Initialize();
        return bassLibrary;
      }

      #endregion

      #region Fields

      #endregion

      #region Public members

      #endregion

      #region Private members

      private BassLibraryManager()
      {
      }

      private void Initialize()
      {
        if (!BassLibraryManager._BassInitialized)
        {
          Log.Debug("Initializing BASS library");

          // Register BASS.Net
          BassRegistration.BassRegistration.Register();

          //bool value = Bass.BASS_GetConfigBool(BASSConfig.BASS_CONFIG_CURVE_VOL);
          //Log.Debug("BASS_CONFIG_CURVE_VOL = {0}", value);
          
          if (!Bass.BASS_Init(0, 44100, 0, IntPtr.Zero, Guid.Empty))
            throw new BassLibraryException("BASS_Init");

          if (!Bass.BASS_SetConfig(BASSConfig.BASS_CONFIG_GVOL_STREAM, 10000))
            throw new BassLibraryException("BASS_SetConfig");

          if (!Bass.BASS_SetConfig(BASSConfig.BASS_CONFIG_UPDATEPERIOD, 0))
            throw new BassLibraryException("BASS_SetConfig");

          if (!Bass.BASS_SetConfig(BASSConfig.BASS_CONFIG_CURVE_VOL, true))
            throw new BassLibraryException("BASS_SetConfig");

          LoadAudioDecoderPlugins();

          // For Bass Wma: must be set after LoadAudioDecoderPlugins()
          if (!Bass.BASS_SetConfig(BASSConfig.BASS_CONFIG_NET_PLAYLIST, 2))
            throw new BassLibraryException("BASS_SetConfig");

          BassLibraryManager._BassInitialized = true;
        }
      }

      private void LoadAudioDecoderPlugins()
      {
        // Bass_ofr 2.4.0.0 gives BASS_ERROR_FILEOPEN error.

        string appPath = System.Windows.Forms.Application.StartupPath;
        string decoderFolderPath = Path.Combine(appPath, InternalSettings.AudioDecoderPath);

        Log.Info("Loading audio decoder add-ins from {0}", decoderFolderPath);

        if (!Directory.Exists(decoderFolderPath))
        {
          Log.Error("Unable to find \"{0}\" folder in MediaPortal.exe path.", InternalSettings.AudioDecoderPath);
          return;
        }

        DirectoryInfo dirInfo = new DirectoryInfo(decoderFolderPath);
        FileInfo[] decoders = dirInfo.GetFiles();

        int pluginHandle = 0;
        int decoderCount = 0;
        int errorCount = 0;

        foreach (FileInfo file in decoders)
        {
          if (Path.GetExtension(file.FullName).ToLower() != ".dll")
            continue;

          Log.Debug("  Loading: {0}", file.Name);
          pluginHandle = Bass.BASS_PluginLoad(file.FullName);

          if (pluginHandle != 0)
          {
            BassLibraryManager._DecoderPluginHandles.Add(pluginHandle);
            decoderCount++;
            Log.Debug("  Added: {0}", file.Name);
          }

          else
          {
            BASSError error = (BASSError)Bass.BASS_ErrorGetCode();
            if (error == BASSError.BASS_ERROR_ALREADY)
              Log.Debug("  Already loaded: {0}", file.Name);
            else
            {
              errorCount++;
              Log.Error("  Unable to load: {0}: {1}", file.Name, error.ToString());
            }
          }
        }

        if (errorCount == 0)
        {
          if (decoderCount == 0)
            Log.Info("No Audio Decoders loaded; probably already loaded.");
          else
            Log.Info("Loaded {0} Audio Decoders.", decoderCount);
        }
      }

      #endregion

      #region IDisposable Members

      public void Dispose()
      {
        Log.Debug("BassLibraryManager.Dispose()");

        foreach (int pluginHandle in BassLibraryManager._DecoderPluginHandles)
        {
          Bass.BASS_PluginFree(pluginHandle);
        }

        if (!Bass.BASS_Free())
        {
          throw new BassLibraryException("BASS_Free");
        }
      }

      #endregion
    }
  }
}
