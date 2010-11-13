using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace PureAudio.Plugin.Configuration.Sections
{
  public partial class About : SectionBase
  {
    public About()
    {
      InitializeComponent();
    }

    private void lnkDownload_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
        Process.Start(
          "http://code.google.com/p/pureaudio-mediaportal-plugin");
    }

    private void lnkForum_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
        Process.Start(
          "http://forum.team-mediaportal.com/asio-music-player-245/");
    }
  }
}
