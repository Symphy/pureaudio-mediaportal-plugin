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
  public partial class AdvancedASIO : SectionBase
  {

    #region constructors

    public AdvancedASIO()
    {
      InitializeComponent();

      object[] rates = new object[]{
					"Auto",
					"8000",
					"9600",
					"11025",
					"12000",
					"16000",
					"22050",
					"24000",
					"32000",
					"44100",
					"48000",
				};
      cboASIOMinRate.Items.AddRange(rates);

      rates = new object[]{
					"Auto",
					"44100",
					"48000",
					"88200",
					"96000",
					"176400",
					"192000"
				};
      cboASIOMaxRate.Items.AddRange(rates);
    }

    #endregion

    #region public members

    public override void ReadSettings(ConfigurationForm form)
    {
      base.ReadSettings(form);

      chkASIOUseMaxBufferSize.Checked = BassPlayerSettings.ASIOUseMaxBufferSize;

      cboASIOMinRate.SelectedItem =
        BassPlayerSettings.ASIOMinRate == BassPlayerSettings.Constants.Auto
        ? "Auto"
        : BassPlayerSettings.ASIOMinRate.ToString();

      cboASIOMaxRate.SelectedItem =
        BassPlayerSettings.ASIOMaxRate == BassPlayerSettings.Constants.Auto
        ? "Auto"
        : BassPlayerSettings.ASIOMaxRate.ToString();
    }

    #endregion

    #region event handlers

    private void chkASIOUseMaxBufferSize_CheckedChanged(object sender, EventArgs e)
    {
      BassPlayerSettings.ASIOUseMaxBufferSize = chkASIOUseMaxBufferSize.Checked;
    }

    private void cboASIOMinRate_SelectedIndexChanged(object sender, EventArgs e)
    {
      string value = cboASIOMinRate.SelectedItem.ToString();
      if (value == "Auto")
        BassPlayerSettings.ASIOMinRate = BassPlayerSettings.Constants.Auto;
      else
        BassPlayerSettings.ASIOMinRate = Convert.ToInt32(value);
    }

    private void cboASIOMaxRate_SelectedIndexChanged(object sender, EventArgs e)
    {
      string value = cboASIOMaxRate.SelectedItem.ToString();
      if (value == "Auto")
        BassPlayerSettings.ASIOMaxRate = BassPlayerSettings.Constants.Auto;
      else
        BassPlayerSettings.ASIOMaxRate = Convert.ToInt32(value);
    }
    #endregion

    #region private members

    #endregion
  }
}
