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
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace MediaPortal.Plugins.PureAudio.Configuration.Sections
{
  public partial class Extensions : SectionBase
  {
    #region constructors

    public Extensions()
    {
      InitializeComponent();
    }
    
    #endregion

    #region public members

    public override void ReadSettings(ConfigurationForm form)
    {
      base.ReadSettings(form);
      DisplayExtList();

      chkUseForCDDA.Checked = BassPlayerSettings.UseForCDDA;
      chkUseForWebStream.Checked = BassPlayerSettings.UseForWebStream;
      chkUseForLastFMWebStream.Checked = BassPlayerSettings.UseForLastFMWebStream;
    }

    #endregion

    #region event handlers
    
    private void btnAddExt_Click(object sender, EventArgs e)
    {
      string ext = tbExtension.Text.Trim();
      if (ext != String.Empty)
      {
        if (!ext.StartsWith("."))
          ext = "." + ext;

        ListViewItem item = lvExtensions.Items.Add(ext);
        lvExtensions.SelectedItems.Clear();
        item.Selected = true;
        tbExtension.Text = String.Empty;
      }
    }

    private void btnDeleteExt_Click(object sender, EventArgs e)
    {
      ListView.SelectedIndexCollection selected = lvExtensions.SelectedIndices;
      int index = 0;
      while (index < lvExtensions.Items.Count)
      {
        if (selected.Contains(index))
          lvExtensions.Items.RemoveAt(index);
        else
          index++;
      }
    }

    private void btnDefault_Click(object sender, EventArgs e)
    {
      DialogResult result = MessageBox.Show(this, "Do you want to restore the default extensionlist?", this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
      if (result == DialogResult.Yes)
      {
        BassPlayerSettings.SupportedExtensions = BassPlayerSettings.Defaults.SupportedExtensions;
        DisplayExtList();
      }
    }

    private void chkUseForCDDA_CheckedChanged(object sender, EventArgs e)
    {
      BassPlayerSettings.UseForCDDA = chkUseForCDDA.Checked;
    }

    private void chkUseForWebStream_CheckedChanged(object sender, EventArgs e)
    {
      BassPlayerSettings.UseForWebStream = chkUseForWebStream.Checked;
    }

    private void chkUseForLastFMWebStream_CheckedChanged(object sender, EventArgs e)
    {
      BassPlayerSettings.UseForLastFMWebStream = chkUseForLastFMWebStream.Checked;
    }

    private void Extensions_Validating(object sender, CancelEventArgs e)
    {
      string ext = String.Empty;
      foreach (ListViewItem item in lvExtensions.Items)
      {
        if (ext != String.Empty)
          ext += ",";
        ext += item.Text;
      }
      BassPlayerSettings.SupportedExtensions = ext;
    }

    #endregion

    #region private members

    private void DisplayExtList()
    {
      string[] ext = BassPlayerSettings.SupportedExtensions.Split(new string[] { "," }, StringSplitOptions.None);

      lvExtensions.Items.Clear();
      for (int i = 0; i < ext.Length; i++)
      {
        lvExtensions.Items.Add(ext[i]);
      }
    }
 
    #endregion

  }
}
