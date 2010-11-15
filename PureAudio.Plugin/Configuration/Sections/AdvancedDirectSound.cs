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
  public partial class AdvancedDirectSound : SectionBase
  {

    #region constructors

    public AdvancedDirectSound()
    {
      InitializeComponent();
    }

    #endregion

    #region public members

    public override void ReadSettings(ConfigurationForm form)
    {
      base.ReadSettings(form);

      trackBarDirectSoundBufferSize.Value = (int)BassPlayerSettings.DirectSoundBufferSize.TotalMilliseconds / 100;
      lblDirectSoundBufferSize.Text = BassPlayerSettings.DirectSoundBufferSize.TotalMilliseconds.ToString();
    }

    #endregion

    #region event handlers

    private void trackBarDirectSoundBufferSize_ValueChanged(object sender, EventArgs e)
    {
      int value = trackBarDirectSoundBufferSize.Value * 100;
      BassPlayerSettings.DirectSoundBufferSize = TimeSpan.FromMilliseconds(value);
      lblDirectSoundBufferSize.Text = value.ToString();
    }

    #endregion

    #region private members

    #endregion
  }
}
