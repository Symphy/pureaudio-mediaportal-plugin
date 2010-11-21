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
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace MediaPortal.Plugins.PureAudio.Configuration.Sections
{
  public partial class About : SectionBase
  {

    #region constructors

    public About()
    {
      InitializeComponent();
    }
    
    #endregion

    #region public members

    public override void ReadSettings(ConfigurationForm form)
    {
      base.ReadSettings(form);
      lblPlayerName.Text = form.Player.PlayerName;
      lblDescription.Text = form.Player.Description();
      lblVersion.Text = form.Player.VersionNumber;
      lblAuthorName.Text = form.Player.AuthorName;
    }

    #endregion

    #region event handlers

    private void lnkDownload_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
        Process.Start("http://code.google.com/p/pureaudio-mediaportal-plugin");
    }

    private void lnkForum_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
        Process.Start("http://forum.team-mediaportal.com/asio-music-player-245/");
    }
    
    #endregion

  }
}
