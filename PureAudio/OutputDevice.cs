using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MediaPortal.Player.PureAudio
{
  public abstract class OutputDevice
  {
    private BASSPlayer _player;

    public OutputDevice(BASSPlayer player)
    {
      _player = player;
    }

    public abstract bool Prepare();
    public abstract bool Init();
    public abstract bool Start();
    public abstract bool Stop();
    public abstract bool Release();
    public abstract void ClearPlayBackBuffer();
    public abstract bool InitOutputStream();
    public abstract int GetFadeStream();
  }
}
