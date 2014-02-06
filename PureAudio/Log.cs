using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MediaPortal.Player.PureAudio
{
  public static class Log
  {
    public delegate void LogDelegate(LogType logType, string format, params object[] arg);
    public static event LogDelegate Write;
    
    public static void Debug(string format, params object[] arg)
    {
      OnWrite(LogType.Debug, format, arg);
    }
    
    public static void Info(string format, params object[] arg)
    {
      OnWrite(LogType.Info, format, arg);
    }
    
    public static void Error(string format, params object[] arg)
    {
      OnWrite(LogType.Error, format, arg);
    }

    public static void Warn(string format, params object[] arg)
    {
      OnWrite(LogType.Warn, format, arg);
    }

    private static void OnWrite(LogType logType, string format, params object[] arg)
    {
      if (Write != null)
        Write(logType, format, arg);
    }
    
    public enum LogType
    {
      Error,
      Info,
      Debug,
      Warn
    }
  }
}
