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
using MediaPortal.Plugins.PureAudio.WMPEffectsInterop;

namespace MediaPortal.Plugins.PureAudio.Configuration.Sections
{
  public partial class Visualizations : SectionBase
  {

    #region constructors

    public Visualizations()
    {
      InitializeComponent();
    }
    
    #endregion

    #region public members

    public override void ReadSettings(ConfigurationForm form)
    {
      base.ReadSettings(form);

      cboWMPEffects.Items.Add(new InstalledEffectInfo("None"));
      InstalledEffect[] installedEffects = WMPEffect.InstalledEffects;
      foreach (InstalledEffect effect in installedEffects)
      {
        object item = new InstalledEffectInfo(effect);
        cboWMPEffects.Items.Add(item);

        if (effect.ClsId == BassPlayerSettings.WMPEffectClsId)
          cboWMPEffects.SelectedItem = item;
      }

      // There is always 1 item  
      if (cboWMPEffects.SelectedIndex == -1)
        cboWMPEffects.SelectedIndex = 0;

      nudWMPEffectFps.Value = BassPlayerSettings.WMPEffectFps;
      
      trackBarVisualizationLatencyCorrection.Value = (int)BassPlayerSettings.VisualizationLatencyCorrection.TotalMilliseconds / 25 + 20;
      lblVisualizationLatencyCorrection.Text = BassPlayerSettings.VisualizationLatencyCorrection.TotalMilliseconds.ToString();

      chkVisualizationUseAGC.Checked = BassPlayerSettings.VisualizationUseAGC;
    }

    #endregion

    #region event handlers

    private void cboWMPEffects_SelectedIndexChanged(object sender, EventArgs e)
    {
      cboWMPEffectPresets.Items.Clear();

      BassPlayerSettings.VisualizationType = VisualizationType.None;
      BassPlayerSettings.WMPEffectClsId = "";

      InstalledEffectInfo effectInfo = (InstalledEffectInfo)cboWMPEffects.SelectedItem;
      if (effectInfo.InstalledEffect != null)
      {
        BassPlayerSettings.VisualizationType = VisualizationType.WMP;
        BassPlayerSettings.WMPEffectClsId = effectInfo.InstalledEffect.ClsId;

        WMPEffect wmpEffect = WMPEffect.SelectEffect(effectInfo.InstalledEffect, Handle);
        if (wmpEffect != null)
        {
          for (int index = 0; index < wmpEffect.Presets.Length; index++)
          {
            Preset preset = wmpEffect.Presets[index];
            cboWMPEffectPresets.Items.Add(preset.Name);

            if (index == BassPlayerSettings.WMPEffectPreset)
              cboWMPEffectPresets.SelectedIndex = index;
          }

          if (cboWMPEffectPresets.Items.Count > BassPlayerSettings.WMPEffectPreset)
            cboWMPEffectPresets.SelectedIndex = BassPlayerSettings.WMPEffectPreset;

          wmpEffect.Release();
        }
      }

      cboWMPEffectPresets.Enabled = cboWMPEffectPresets.Items.Count > 1;
      if (cboWMPEffectPresets.Enabled && cboWMPEffects.SelectedIndex == -1)
        cboWMPEffectPresets.SelectedIndex = 0;
    }

    private void cboWMPEffectPresets_SelectedIndexChanged(object sender, EventArgs e)
    {
      BassPlayerSettings.WMPEffectPreset = cboWMPEffectPresets.SelectedIndex;
    }

    private void nudWMPEffectFps_ValueChanged(object sender, EventArgs e)
    {
      BassPlayerSettings.WMPEffectFps = Convert.ToInt32(nudWMPEffectFps.Value);
    }

    private void trackBarVisualizationLatency_ValueChanged(object sender, EventArgs e)
    {
      int value = (trackBarVisualizationLatencyCorrection.Value - 20) * 25;
      BassPlayerSettings.VisualizationLatencyCorrection = TimeSpan.FromMilliseconds(value);
      lblVisualizationLatencyCorrection.Text = value.ToString();
    }

    private void chkVisualizationUseAGC_CheckedChanged(object sender, EventArgs e)
    {
      BassPlayerSettings.VisualizationUseAGC = chkVisualizationUseAGC.Checked;
    }

    #endregion
    
    #region nested types

    public class InstalledEffectInfo
    {
      public InstalledEffect InstalledEffect { get; set; }
      private string name { get; set; }

      public InstalledEffectInfo(string name)
      {
        this.name = name;
      }

      public InstalledEffectInfo(InstalledEffect installedEffect)
      {
        this.name = installedEffect.Name;
        InstalledEffect = installedEffect;
      }

      public override string ToString()
      {
        return name;
      }
    }

    #endregion

  }
}
