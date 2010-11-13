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
using System.Threading;
using MediaPortal.Core;
using MediaPortal.Configuration;
using MediaPortal.Services;
using MediaPortal.Profile;

namespace MediaPortal.Player.PureAudio
{
  /// <summary>
  /// Logger service wrapper.
  /// </summary>
  internal static class Log
  {
    #region Fields

    private const string _Prefix = "PureAudio: ";
    private static bool? _IsDebugLevel;

    #endregion

    #region Public Members

    /// <summary>
    /// Returns if logging level is set to debug.
    /// </summary>
    public static bool IsDebugLevel
    {
      get
      {
        if (!_IsDebugLevel.HasValue)
        {
          using (Settings xmlreader = new Settings(Config.GetFile(Config.Dir.Config, "MediaPortal.xml")))
          {
            _IsDebugLevel = ((Level)Enum.Parse(typeof(Level), xmlreader.GetValueAsString("general", "loglevel", "3")) == Level.Debug);
          }
        }
        return _IsDebugLevel.Value;
      }
    }

    /// <summary>
    /// Writes an informational message to the log.
    /// </summary>
    /// <param name="format">A composite format string.</param>
    /// <param name="args">An array of objects to write using format.</param>
    public static void Info(string format, params object[] args)
    {
      MediaPortal.GUI.Library.Log.Info(GetPrefix() + format, args);
    }

    /// <summary>
    /// Writes a debug message to the log.
    /// </summary>
    /// <param name="format">A composite format string.</param>
    /// <param name="args">An array of objects to write using format.</param>
    public static void Debug(string format, params object[] args)
    {
      MediaPortal.GUI.Library.Log.Debug(GetPrefix() + format, args);
    }

    /// <summary>
    /// Writes an error message to the log.
    /// </summary>
    /// <param name="format">A composite format string.</param>
    /// <param name="args">An array of objects to write using format.</param>
    public static void Error(string format, params object[] args)
    {
      MediaPortal.GUI.Library.Log.Error(GetPrefix() + format, args);
    }

    /// <summary>
    /// Writes a warning message to the log.
    /// </summary>
    /// <param name="format">A composite format string.</param>
    /// <param name="args">An array of objects to write using format.</param>
    public static void Warn(string format, params object[] args)
    {
      MediaPortal.GUI.Library.Log.Warn(GetPrefix() + format, args);
    }

    /// <summary>
    /// Writes a critical error to the log.
    /// </summary>
    /// <param name="format">A composite format string.</param>
    /// <param name="args">An array of objects to write using format.</param>
    public static void Critical(string format, params object[] args)
    {
      MediaPortal.GUI.Library.Log.Error(GetPrefix() + format, args);
    }

    private static string GetPrefix()
    {
      if (Thread.CurrentThread.Name.StartsWith("PureAudio"))
        return "";
      else
        return _Prefix;
    }

    #endregion
  }
}
