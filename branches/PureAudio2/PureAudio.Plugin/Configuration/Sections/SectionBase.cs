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
  public partial class SectionBase : UserControl
  {

    #region constructors

    public SectionBase()
    {
      InitializeComponent();
    }

    #endregion

    #region protected members

    protected ConfigurationForm GetForm()
    {
      return (ConfigurationForm)FindForm();
    }

    protected PureAudioSettings Settings
    {
      get
      {
        ConfigurationForm form = GetForm();
        return form != null ? form.Settings : null;
      }
    }

    #endregion

    #region public members

    public virtual void ReadSettings(ConfigurationForm form)
    {
    }

    #endregion

  }
}
