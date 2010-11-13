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
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Text;
using System.IO;
using MediaPortal.GUI.Library;
using MediaPortal.Player.PureAudio;
using Un4seen.Bass;
using System.Diagnostics;
using Microsoft.Win32;

namespace MediaPortal.Player.PureAudio
{
  /// <summary>
  /// Summary description for Configuration.
  /// </summary>
  public class ConfigurationForm : System.Windows.Forms.Form
  {
    private MediaPortal.UserInterface.Controls.MPButton btnOk;
    private MediaPortal.UserInterface.Controls.MPButton btnCancel;
    private TabControl tabControl;
    private TabPage tabPageGeneral;
    private TabPage tabPageAdvanced;
    private TabPage tabPageExtensions;
    private TabPage tabPageUpmixing;
    private TabPage tabPageAbout;
    private TabPage tabPageDevice;
    private TreeView tvwMenu;
    private TabPage tabPageASIO;
    private TabPage tabPageWaveOut;
    private MediaPortal.UserInterface.Controls.MPGradientLabel ctlHeader;
    private MediaPortal.UserInterface.Controls.MPBeveledLine beveledLine1;
    private TabPage tabPageVisualization;
    private TabPage tabPageWMPViz;
    private TabPage tabPageDSP;
    private global::PureAudio.Plugin.Configuration.Sections.About about1;
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.Container components = null;

    public ConfigurationForm()
    {
      //
      // Required for Windows Form Designer support
      //
      InitializeComponent();
    }

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    protected override void Dispose(bool disposing)
    {
      if (disposing)
      {
        if (components != null)
        {
          components.Dispose();
        }
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code
    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      System.Windows.Forms.TreeNode treeNode31 = new System.Windows.Forms.TreeNode("Output Device");
      System.Windows.Forms.TreeNode treeNode32 = new System.Windows.Forms.TreeNode("Upmixing");
      System.Windows.Forms.TreeNode treeNode33 = new System.Windows.Forms.TreeNode("Extensions");
      System.Windows.Forms.TreeNode treeNode34 = new System.Windows.Forms.TreeNode("General Settings", new System.Windows.Forms.TreeNode[] {
            treeNode32,
            treeNode33});
      System.Windows.Forms.TreeNode treeNode35 = new System.Windows.Forms.TreeNode("Visualizations");
      System.Windows.Forms.TreeNode treeNode36 = new System.Windows.Forms.TreeNode("DSP\'s");
      System.Windows.Forms.TreeNode treeNode37 = new System.Windows.Forms.TreeNode("ASIO");
      System.Windows.Forms.TreeNode treeNode38 = new System.Windows.Forms.TreeNode("WaveOut");
      System.Windows.Forms.TreeNode treeNode39 = new System.Windows.Forms.TreeNode("Advanced Settings", new System.Windows.Forms.TreeNode[] {
            treeNode37,
            treeNode38});
      System.Windows.Forms.TreeNode treeNode40 = new System.Windows.Forms.TreeNode("PureAudio Plugin", new System.Windows.Forms.TreeNode[] {
            treeNode31,
            treeNode34,
            treeNode35,
            treeNode36,
            treeNode39});
      this.btnOk = new MediaPortal.UserInterface.Controls.MPButton();
      this.btnCancel = new MediaPortal.UserInterface.Controls.MPButton();
      this.tabControl = new System.Windows.Forms.TabControl();
      this.tabPageAbout = new System.Windows.Forms.TabPage();
      this.tabPageDevice = new System.Windows.Forms.TabPage();
      this.tabPageGeneral = new System.Windows.Forms.TabPage();
      this.tabPageUpmixing = new System.Windows.Forms.TabPage();
      this.tabPageExtensions = new System.Windows.Forms.TabPage();
      this.tabPageAdvanced = new System.Windows.Forms.TabPage();
      this.tabPageASIO = new System.Windows.Forms.TabPage();
      this.tabPageWaveOut = new System.Windows.Forms.TabPage();
      this.tabPageVisualization = new System.Windows.Forms.TabPage();
      this.tabPageWMPViz = new System.Windows.Forms.TabPage();
      this.tabPageDSP = new System.Windows.Forms.TabPage();
      this.tvwMenu = new System.Windows.Forms.TreeView();
      this.ctlHeader = new MediaPortal.UserInterface.Controls.MPGradientLabel();
      this.beveledLine1 = new MediaPortal.UserInterface.Controls.MPBeveledLine();
      this.about1 = new PureAudio.Plugin.Configuration.Sections.About();
      this.tabControl.SuspendLayout();
      this.tabPageAbout.SuspendLayout();
      this.SuspendLayout();
      // 
      // btnOk
      // 
      this.btnOk.Location = new System.Drawing.Point(559, 371);
      this.btnOk.Name = "btnOk";
      this.btnOk.Size = new System.Drawing.Size(75, 23);
      this.btnOk.TabIndex = 3;
      this.btnOk.Text = "Ok";
      this.btnOk.UseVisualStyleBackColor = true;
      // 
      // btnCancel
      // 
      this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.btnCancel.Location = new System.Drawing.Point(640, 371);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new System.Drawing.Size(75, 23);
      this.btnCancel.TabIndex = 4;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      // 
      // tabControl
      // 
      this.tabControl.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
      this.tabControl.Controls.Add(this.tabPageAbout);
      this.tabControl.Controls.Add(this.tabPageDevice);
      this.tabControl.Controls.Add(this.tabPageGeneral);
      this.tabControl.Controls.Add(this.tabPageUpmixing);
      this.tabControl.Controls.Add(this.tabPageExtensions);
      this.tabControl.Controls.Add(this.tabPageAdvanced);
      this.tabControl.Controls.Add(this.tabPageASIO);
      this.tabControl.Controls.Add(this.tabPageWaveOut);
      this.tabControl.Controls.Add(this.tabPageVisualization);
      this.tabControl.Controls.Add(this.tabPageWMPViz);
      this.tabControl.Controls.Add(this.tabPageDSP);
      this.tabControl.Location = new System.Drawing.Point(204, 13);
      this.tabControl.Name = "tabControl";
      this.tabControl.SelectedIndex = 0;
      this.tabControl.Size = new System.Drawing.Size(511, 348);
      this.tabControl.TabIndex = 2;
      // 
      // tabPageAbout
      // 
      this.tabPageAbout.Controls.Add(this.about1);
      this.tabPageAbout.Location = new System.Drawing.Point(4, 25);
      this.tabPageAbout.Name = "tabPageAbout";
      this.tabPageAbout.Size = new System.Drawing.Size(503, 319);
      this.tabPageAbout.TabIndex = 4;
      this.tabPageAbout.Text = "About";
      this.tabPageAbout.UseVisualStyleBackColor = true;
      // 
      // tabPageDevice
      // 
      this.tabPageDevice.Location = new System.Drawing.Point(4, 25);
      this.tabPageDevice.Name = "tabPageDevice";
      this.tabPageDevice.Size = new System.Drawing.Size(503, 319);
      this.tabPageDevice.TabIndex = 5;
      this.tabPageDevice.Text = "Device";
      this.tabPageDevice.UseVisualStyleBackColor = true;
      // 
      // tabPageGeneral
      // 
      this.tabPageGeneral.Location = new System.Drawing.Point(4, 25);
      this.tabPageGeneral.Name = "tabPageGeneral";
      this.tabPageGeneral.Padding = new System.Windows.Forms.Padding(3);
      this.tabPageGeneral.Size = new System.Drawing.Size(503, 319);
      this.tabPageGeneral.TabIndex = 0;
      this.tabPageGeneral.Text = "General";
      this.tabPageGeneral.UseVisualStyleBackColor = true;
      // 
      // tabPageUpmixing
      // 
      this.tabPageUpmixing.Location = new System.Drawing.Point(4, 25);
      this.tabPageUpmixing.Name = "tabPageUpmixing";
      this.tabPageUpmixing.Padding = new System.Windows.Forms.Padding(3);
      this.tabPageUpmixing.Size = new System.Drawing.Size(503, 319);
      this.tabPageUpmixing.TabIndex = 3;
      this.tabPageUpmixing.Text = "Upmixing";
      this.tabPageUpmixing.UseVisualStyleBackColor = true;
      // 
      // tabPageExtensions
      // 
      this.tabPageExtensions.Location = new System.Drawing.Point(4, 25);
      this.tabPageExtensions.Name = "tabPageExtensions";
      this.tabPageExtensions.Padding = new System.Windows.Forms.Padding(3);
      this.tabPageExtensions.Size = new System.Drawing.Size(503, 319);
      this.tabPageExtensions.TabIndex = 2;
      this.tabPageExtensions.Text = "Extensions";
      this.tabPageExtensions.UseVisualStyleBackColor = true;
      // 
      // tabPageAdvanced
      // 
      this.tabPageAdvanced.Location = new System.Drawing.Point(4, 25);
      this.tabPageAdvanced.Name = "tabPageAdvanced";
      this.tabPageAdvanced.Padding = new System.Windows.Forms.Padding(3);
      this.tabPageAdvanced.Size = new System.Drawing.Size(503, 319);
      this.tabPageAdvanced.TabIndex = 1;
      this.tabPageAdvanced.Text = "Advanced";
      this.tabPageAdvanced.UseVisualStyleBackColor = true;
      // 
      // tabPageASIO
      // 
      this.tabPageASIO.Location = new System.Drawing.Point(4, 25);
      this.tabPageASIO.Name = "tabPageASIO";
      this.tabPageASIO.Size = new System.Drawing.Size(503, 319);
      this.tabPageASIO.TabIndex = 6;
      this.tabPageASIO.Text = "ASIO";
      this.tabPageASIO.UseVisualStyleBackColor = true;
      // 
      // tabPageWaveOut
      // 
      this.tabPageWaveOut.Location = new System.Drawing.Point(4, 25);
      this.tabPageWaveOut.Name = "tabPageWaveOut";
      this.tabPageWaveOut.Size = new System.Drawing.Size(503, 319);
      this.tabPageWaveOut.TabIndex = 7;
      this.tabPageWaveOut.Text = "WaveOut";
      this.tabPageWaveOut.UseVisualStyleBackColor = true;
      // 
      // tabPageVisualization
      // 
      this.tabPageVisualization.Location = new System.Drawing.Point(4, 25);
      this.tabPageVisualization.Name = "tabPageVisualization";
      this.tabPageVisualization.Size = new System.Drawing.Size(503, 319);
      this.tabPageVisualization.TabIndex = 8;
      this.tabPageVisualization.Text = "Visualizations";
      this.tabPageVisualization.UseVisualStyleBackColor = true;
      // 
      // tabPageWMPViz
      // 
      this.tabPageWMPViz.Location = new System.Drawing.Point(4, 25);
      this.tabPageWMPViz.Name = "tabPageWMPViz";
      this.tabPageWMPViz.Size = new System.Drawing.Size(503, 319);
      this.tabPageWMPViz.TabIndex = 9;
      this.tabPageWMPViz.Text = "Windows Media Player";
      this.tabPageWMPViz.UseVisualStyleBackColor = true;
      // 
      // tabPageDSP
      // 
      this.tabPageDSP.Location = new System.Drawing.Point(4, 25);
      this.tabPageDSP.Name = "tabPageDSP";
      this.tabPageDSP.Size = new System.Drawing.Size(503, 319);
      this.tabPageDSP.TabIndex = 11;
      this.tabPageDSP.Text = "DSP";
      this.tabPageDSP.UseVisualStyleBackColor = true;
      // 
      // tvwMenu
      // 
      this.tvwMenu.HideSelection = false;
      this.tvwMenu.Location = new System.Drawing.Point(13, 13);
      this.tvwMenu.Name = "tvwMenu";
      treeNode31.Name = "NodeDevice";
      treeNode31.Text = "Output Device";
      treeNode32.Name = "NodeUpmixing";
      treeNode32.Text = "Upmixing";
      treeNode33.Name = "NodeExtensions";
      treeNode33.Text = "Extensions";
      treeNode34.Name = "NodeGeneral";
      treeNode34.Text = "General Settings";
      treeNode35.Name = "NodeVisualizations";
      treeNode35.Text = "Visualizations";
      treeNode36.Name = "NodeDSP";
      treeNode36.Text = "DSP\'s";
      treeNode37.Name = "NodeASIO";
      treeNode37.Text = "ASIO";
      treeNode38.Name = "NodeWaveOut";
      treeNode38.Text = "WaveOut";
      treeNode39.Name = "NodeAdvanced";
      treeNode39.Text = "Advanced Settings";
      treeNode40.Name = "NodeRoot";
      treeNode40.Text = "PureAudio Plugin";
      this.tvwMenu.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode40});
      this.tvwMenu.Size = new System.Drawing.Size(185, 336);
      this.tvwMenu.TabIndex = 1;
      // 
      // ctlHeader
      // 
      this.ctlHeader.Caption = "";
      this.ctlHeader.FirstColor = System.Drawing.SystemColors.InactiveCaption;
      this.ctlHeader.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.ctlHeader.LastColor = System.Drawing.Color.WhiteSmoke;
      this.ctlHeader.Location = new System.Drawing.Point(204, 13);
      this.ctlHeader.Name = "ctlHeader";
      this.ctlHeader.PaddingLeft = 2;
      this.ctlHeader.Size = new System.Drawing.Size(511, 24);
      this.ctlHeader.TabIndex = 5;
      this.ctlHeader.TabStop = false;
      this.ctlHeader.TextColor = System.Drawing.Color.WhiteSmoke;
      this.ctlHeader.TextFont = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      // 
      // beveledLine1
      // 
      this.beveledLine1.Location = new System.Drawing.Point(13, 363);
      this.beveledLine1.Name = "beveledLine1";
      this.beveledLine1.Size = new System.Drawing.Size(701, 2);
      this.beveledLine1.TabIndex = 6;
      this.beveledLine1.TabStop = false;
      // 
      // about1
      // 
      this.about1.Location = new System.Drawing.Point(0, 0);
      this.about1.Name = "about1";
      this.about1.Size = new System.Drawing.Size(497, 308);
      this.about1.TabIndex = 0;
      // 
      // ConfigurationForm
      // 
      this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
      this.ClientSize = new System.Drawing.Size(727, 404);
      this.Controls.Add(this.beveledLine1);
      this.Controls.Add(this.ctlHeader);
      this.Controls.Add(this.tvwMenu);
      this.Controls.Add(this.btnCancel);
      this.Controls.Add(this.btnOk);
      this.Controls.Add(this.tabControl);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "ConfigurationForm";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "PureAudio Player Configuration";
      this.Load += new System.EventHandler(this.ConfigurationForm_Load);
      this.tabControl.ResumeLayout(false);
      this.tabPageAbout.ResumeLayout(false);
      this.ResumeLayout(false);

    }
    #endregion

    private void ConfigurationForm_Load(object sender, EventArgs e)
    {
    }

  }
}
