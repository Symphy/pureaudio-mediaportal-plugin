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
using System.ComponentModel;
using System.Windows.Forms;
using MediaPortal.GUI.Library;
using Un4seen.Bass;
using Un4seen.BassWasapi;
using MediaPortal.Player.PureAudio.Asio;
using BlueWave.Interop.Asio;
using System.Diagnostics;
using WMPEffects.Interop;

namespace MediaPortal.Player.PureAudio
{
  /// <summary>
  /// Summary description for Configuration.
  /// </summary>
  public class ConfigurationForm : System.Windows.Forms.Form
  {
    private PureAudioPlugin _pureAudioPlugin;
    private ConfigProfile _Profile = new ConfigProfile();
    private AsioEngine _asioEngine = new AsioEngine();

    private MediaPortal.UserInterface.Controls.MPButton btnOk;
    private MediaPortal.UserInterface.Controls.MPButton btnCancel;
    private MediaPortal.UserInterface.Controls.MPLabel mpLabel4;
    private MediaPortal.UserInterface.Controls.MPLabel mpLabel5;
    private TrackBar trackBarPlayBackBufferSize;
    private ComboBox cboStereoUpMix;
    private CheckBox chkDoSoftStop;
    private MediaPortal.UserInterface.Controls.MPLabel mpLabel2;
    private TrackBar trackBarSeekIncrement;
    private ComboBox cboASIODevice;
    private MediaPortal.UserInterface.Controls.MPLabel mpLabel3;
    private MediaPortal.UserInterface.Controls.MPLabel lblSoftStopDurationLabel;
    private TrackBar trackBarSoftStopDuration;
    private MediaPortal.UserInterface.Controls.MPLabel lblPlayBackBufferSize;
    private MediaPortal.UserInterface.Controls.MPLabel lblSoftStopDuration;
    private MediaPortal.UserInterface.Controls.MPLabel lblSeekIncrement;
    private MediaPortal.UserInterface.Controls.MPLabel mpLabel12;
    private MediaPortal.UserInterface.Controls.MPLabel mpLabel10;
    private MediaPortal.UserInterface.Controls.MPLabel lblSoftStopDurationUnits;
    private TrackBar trackBarFileBufferSize;
    private MediaPortal.UserInterface.Controls.MPLabel mpLabel14;
    private MediaPortal.UserInterface.Controls.MPLabel lblBASSPlayBackBufferSize;
    private TrackBar trackBarGapLength;
    private MediaPortal.UserInterface.Controls.MPLabel mpLabel6;
    private MediaPortal.UserInterface.Controls.MPLabel lblGapLength;
    private MediaPortal.UserInterface.Controls.MPLabel mpLabel8;
    private TabControl tabControl;
    private TabPage tabPageGeneral;
    private TabPage tabPageAdvanced;
    private TabPage tabPageExtensions;
    private TextBox tbExtension;
    private MediaPortal.UserInterface.Controls.MPButton btnAddExt;
    private MediaPortal.UserInterface.Controls.MPButton btnDeleteExt;
    private MediaPortal.UserInterface.Controls.MPButton btnASIOControlPanel;
    private TabPage tabPageUpmixing;
    private MediaPortal.UserInterface.Controls.MPLabel mpLabel7;
    private ComboBox cboDefaultPlayBackMode;
    private TabPage tabPageAbout;
    private MediaPortal.UserInterface.Controls.MPLabel mpLabel11;
    private MediaPortal.UserInterface.Controls.MPLabel mpLabel15;
    private MediaPortal.UserInterface.Controls.MPLabel mpLabel13;
    private MediaPortal.UserInterface.Controls.MPLabel lblPlayerName;
    private MediaPortal.UserInterface.Controls.MPLabel lblAuthorName;
    private MediaPortal.UserInterface.Controls.MPLabel lblVersion;
    private MediaPortal.UserInterface.Controls.MPLabel lblDescription;
    private MediaPortal.UserInterface.Controls.MPLabel mpLabel16;
    private CheckBox chkUseMaxASIOBufferSize;
    private TabPage tabPageDevice;
    private RadioButton rbUseASIO;
    private Panel pnlDevice;
    private RadioButton rbUseSound;
    private ComboBox cboSoundDevice;
    private MediaPortal.UserInterface.Controls.MPButton btnDefault;
    private GroupBox groupBox1;
    private GroupBox groupBox2;
    private TreeView tvwMenu;
    private GroupBox groupBox3;
    private GroupBox groupBox4;
    private GroupBox groupBox5;
    private GroupBox groupBox6;
    private GroupBox groupBox7;
    private GroupBox groupBox8;
    private MediaPortal.UserInterface.Controls.MPButton btnMMEControlPanel;
    private LinkLabel lnkDownload;
    private LinkLabel lnkForum;
    private TabPage tabPageASIO;
    private TabPage tabPageWaveOut;
    private ListView lvExtensions;
    private MediaPortal.UserInterface.Controls.MPLabel mpLabel23;
    private MediaPortal.UserInterface.Controls.MPGradientLabel ctlHeader;
    private MediaPortal.UserInterface.Controls.MPBeveledLine beveledLine1;
    private MediaPortal.UserInterface.Controls.MPLabel lblASIOFirstChan;
    private ComboBox cboASIOFirstChan;
    private MediaPortal.UserInterface.Controls.MPLabel lblASIOLastChan;
    private ComboBox cboASIOLastChan;
    private Panel pnlSoftStopDuration;
    private GroupBox groupBox10;
    private ComboBox cboFiveDotOneUpMix;
    private MediaPortal.UserInterface.Controls.MPLabel mpLabel25;
    private MediaPortal.UserInterface.Controls.MPLabel mpLabel9;
    private MediaPortal.UserInterface.Controls.MPLabel lblVizLatency;
    private MediaPortal.UserInterface.Controls.MPLabel mpLabel27;
    private TrackBar trackBarVizLatency;
    private TabPage tabPageVisualization;
    private GroupBox groupBox11;
    private GroupBox groupBox12;
    private MediaPortal.UserInterface.Controls.MPLabel mpLabel29;
    private MediaPortal.UserInterface.Controls.MPLabel lblWMPVizFFTFallBack;
    private TrackBar trackBarWMPVizFFTFallBack;
    private MediaPortal.UserInterface.Controls.MPLabel mpLabel30;
    private MediaPortal.UserInterface.Controls.MPLabel mpLabel31;
    private MediaPortal.UserInterface.Controls.MPLabel lblWMPVizFFTMinimum;
    private TrackBar trackBarWMPVizFFTMinimum;
    private MediaPortal.UserInterface.Controls.MPLabel mpLabel33;
    private TabPage tabPageWMPViz;
    private CheckBox chkWMPVizFFTHalf;
    private GroupBox groupBox14;
    private CheckBox chkUseOverSampling;
    private GroupBox groupBox15;
    private MediaPortal.UserInterface.Controls.MPLabel mpLabel36;
    private ComboBox cboMonoUpMix;
    private GroupBox groupBox9;
    private MediaPortal.UserInterface.Controls.MPLabel mpLabel20;
    private MediaPortal.UserInterface.Controls.MPLabel mpLabel19;
    private MediaPortal.UserInterface.Controls.MPLabel mpLabel17;
    private MediaPortal.UserInterface.Controls.MPLabel mpLabel1;
    private ComboBox cboMaxASIORate;
    private ComboBox cboMinASIORate;
    private TabPage tabPageDSP;
    private GroupBox groupBox16;
    private TextBox textBox2;
    private TextBox textBox3;
    private TextBox textBox4;
    private TextBox textBox7;
    private TextBox textBox6;
    private TextBox textBox9;
    private TextBox textBox8;
    private GroupBox groupBox17;
    private CheckBox chkUseForWebStream;
    private CheckBox chkUseForCDDA;
    private TextBox textBox1;
    private CheckBox chkUseVizAGC;
    private GroupBox groupBox18;
    private CheckBox chkUseReplayGain;
    private CheckBox chkUseRGAlbumGain;
    private GroupBox groupBox19;
    private Label label1;
    private GroupBox groupBox20;
    private ComboBox cboQuadraphonicUpMix;
    private MediaPortal.UserInterface.Controls.MPLabel mpLabel18;
    private CheckBox chkUseForLastFMWebStream;
    private Label label11;
    private Label label10;
    private ComboBox cboWMPVisualizationPresets;
    private ComboBox cboWMPVisualizations;
    private NumericUpDown nudWMPVizFps;
    private Label label2;
    private RadioButton rbUseWASAPI;
    private UserInterface.Controls.MPButton btnWASAPIControlPanel;
    private ComboBox cboWASAPIDevice;
    private CheckBox chkWASAPIExclusive;
    private TabPage tabPageWASAPI;
    private GroupBox groupBox13;
    private TextBox textBox5;
    private UserInterface.Controls.MPLabel mpLabel21;
    private UserInterface.Controls.MPLabel mpLabel22;
    private UserInterface.Controls.MPLabel mpLabel24;
    private UserInterface.Controls.MPLabel mpLabel26;
    private ComboBox cboMaxWASAPIRate;
    private ComboBox cboMinWASAPIRate;
    private GroupBox groupBox21;
    private CheckBox chkWASAPIEvent;
    private UserInterface.Controls.MPLabel lblWASAPISpeakerLayout;
    private ComboBox cboWASAPISpeakerLayout;
    private GroupBox groupBox22;
    private ComboBox cboFiveDotZeroUpMix;
    private UserInterface.Controls.MPLabel mpLabel28;
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.Container components = null;

    public ConfigurationForm()
    {
      // Required for Windows Form Designer support
      InitializeComponent();
    }

    public ConfigurationForm(PureAudioPlugin plugin)
    {
      _pureAudioPlugin = plugin;

      // Required for Windows Form Designer support
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
      System.Windows.Forms.TreeNode treeNode23 = new System.Windows.Forms.TreeNode("Output Device");
      System.Windows.Forms.TreeNode treeNode24 = new System.Windows.Forms.TreeNode("Upmixing");
      System.Windows.Forms.TreeNode treeNode25 = new System.Windows.Forms.TreeNode("Extensions");
      System.Windows.Forms.TreeNode treeNode26 = new System.Windows.Forms.TreeNode("General Settings", new System.Windows.Forms.TreeNode[] {
            treeNode24,
            treeNode25});
      System.Windows.Forms.TreeNode treeNode27 = new System.Windows.Forms.TreeNode("Visualizations");
      System.Windows.Forms.TreeNode treeNode28 = new System.Windows.Forms.TreeNode("DSP\'s");
      System.Windows.Forms.TreeNode treeNode29 = new System.Windows.Forms.TreeNode("ASIO");
      System.Windows.Forms.TreeNode treeNode30 = new System.Windows.Forms.TreeNode("WaveOut");
      System.Windows.Forms.TreeNode treeNode31 = new System.Windows.Forms.TreeNode("WASAPI");
      System.Windows.Forms.TreeNode treeNode32 = new System.Windows.Forms.TreeNode("Advanced Settings", new System.Windows.Forms.TreeNode[] {
            treeNode29,
            treeNode30,
            treeNode31});
      System.Windows.Forms.TreeNode treeNode33 = new System.Windows.Forms.TreeNode("PureAudio Plugin", new System.Windows.Forms.TreeNode[] {
            treeNode23,
            treeNode26,
            treeNode27,
            treeNode28,
            treeNode32});
      this.btnOk = new MediaPortal.UserInterface.Controls.MPButton();
      this.btnCancel = new MediaPortal.UserInterface.Controls.MPButton();
      this.mpLabel4 = new MediaPortal.UserInterface.Controls.MPLabel();
      this.mpLabel5 = new MediaPortal.UserInterface.Controls.MPLabel();
      this.mpLabel14 = new MediaPortal.UserInterface.Controls.MPLabel();
      this.lblBASSPlayBackBufferSize = new MediaPortal.UserInterface.Controls.MPLabel();
      this.trackBarFileBufferSize = new System.Windows.Forms.TrackBar();
      this.mpLabel12 = new MediaPortal.UserInterface.Controls.MPLabel();
      this.lblPlayBackBufferSize = new MediaPortal.UserInterface.Controls.MPLabel();
      this.trackBarPlayBackBufferSize = new System.Windows.Forms.TrackBar();
      this.cboStereoUpMix = new System.Windows.Forms.ComboBox();
      this.chkDoSoftStop = new System.Windows.Forms.CheckBox();
      this.mpLabel2 = new MediaPortal.UserInterface.Controls.MPLabel();
      this.trackBarSeekIncrement = new System.Windows.Forms.TrackBar();
      this.cboASIODevice = new System.Windows.Forms.ComboBox();
      this.mpLabel3 = new MediaPortal.UserInterface.Controls.MPLabel();
      this.lblSoftStopDurationLabel = new MediaPortal.UserInterface.Controls.MPLabel();
      this.trackBarSoftStopDuration = new System.Windows.Forms.TrackBar();
      this.lblSoftStopDuration = new MediaPortal.UserInterface.Controls.MPLabel();
      this.lblSeekIncrement = new MediaPortal.UserInterface.Controls.MPLabel();
      this.mpLabel10 = new MediaPortal.UserInterface.Controls.MPLabel();
      this.lblSoftStopDurationUnits = new MediaPortal.UserInterface.Controls.MPLabel();
      this.trackBarGapLength = new System.Windows.Forms.TrackBar();
      this.mpLabel6 = new MediaPortal.UserInterface.Controls.MPLabel();
      this.lblGapLength = new MediaPortal.UserInterface.Controls.MPLabel();
      this.mpLabel8 = new MediaPortal.UserInterface.Controls.MPLabel();
      this.tabControl = new System.Windows.Forms.TabControl();
      this.tabPageAbout = new System.Windows.Forms.TabPage();
      this.groupBox8 = new System.Windows.Forms.GroupBox();
      this.label1 = new System.Windows.Forms.Label();
      this.lnkForum = new System.Windows.Forms.LinkLabel();
      this.lnkDownload = new System.Windows.Forms.LinkLabel();
      this.mpLabel16 = new MediaPortal.UserInterface.Controls.MPLabel();
      this.lblPlayerName = new MediaPortal.UserInterface.Controls.MPLabel();
      this.mpLabel11 = new MediaPortal.UserInterface.Controls.MPLabel();
      this.lblAuthorName = new MediaPortal.UserInterface.Controls.MPLabel();
      this.mpLabel13 = new MediaPortal.UserInterface.Controls.MPLabel();
      this.lblVersion = new MediaPortal.UserInterface.Controls.MPLabel();
      this.mpLabel15 = new MediaPortal.UserInterface.Controls.MPLabel();
      this.lblDescription = new MediaPortal.UserInterface.Controls.MPLabel();
      this.tabPageDevice = new System.Windows.Forms.TabPage();
      this.groupBox3 = new System.Windows.Forms.GroupBox();
      this.pnlDevice = new System.Windows.Forms.Panel();
      this.lblWASAPISpeakerLayout = new MediaPortal.UserInterface.Controls.MPLabel();
      this.cboWASAPISpeakerLayout = new System.Windows.Forms.ComboBox();
      this.chkWASAPIExclusive = new System.Windows.Forms.CheckBox();
      this.btnWASAPIControlPanel = new MediaPortal.UserInterface.Controls.MPButton();
      this.cboWASAPIDevice = new System.Windows.Forms.ComboBox();
      this.rbUseWASAPI = new System.Windows.Forms.RadioButton();
      this.lblASIOLastChan = new MediaPortal.UserInterface.Controls.MPLabel();
      this.cboASIOLastChan = new System.Windows.Forms.ComboBox();
      this.lblASIOFirstChan = new MediaPortal.UserInterface.Controls.MPLabel();
      this.cboASIOFirstChan = new System.Windows.Forms.ComboBox();
      this.btnMMEControlPanel = new MediaPortal.UserInterface.Controls.MPButton();
      this.cboSoundDevice = new System.Windows.Forms.ComboBox();
      this.rbUseSound = new System.Windows.Forms.RadioButton();
      this.btnASIOControlPanel = new MediaPortal.UserInterface.Controls.MPButton();
      this.rbUseASIO = new System.Windows.Forms.RadioButton();
      this.tabPageGeneral = new System.Windows.Forms.TabPage();
      this.groupBox4 = new System.Windows.Forms.GroupBox();
      this.pnlSoftStopDuration = new System.Windows.Forms.Panel();
      this.cboDefaultPlayBackMode = new System.Windows.Forms.ComboBox();
      this.mpLabel7 = new MediaPortal.UserInterface.Controls.MPLabel();
      this.tabPageUpmixing = new System.Windows.Forms.TabPage();
      this.groupBox20 = new System.Windows.Forms.GroupBox();
      this.cboQuadraphonicUpMix = new System.Windows.Forms.ComboBox();
      this.mpLabel18 = new MediaPortal.UserInterface.Controls.MPLabel();
      this.groupBox15 = new System.Windows.Forms.GroupBox();
      this.mpLabel36 = new MediaPortal.UserInterface.Controls.MPLabel();
      this.cboMonoUpMix = new System.Windows.Forms.ComboBox();
      this.groupBox10 = new System.Windows.Forms.GroupBox();
      this.cboFiveDotOneUpMix = new System.Windows.Forms.ComboBox();
      this.mpLabel25 = new MediaPortal.UserInterface.Controls.MPLabel();
      this.groupBox5 = new System.Windows.Forms.GroupBox();
      this.tabPageExtensions = new System.Windows.Forms.TabPage();
      this.groupBox17 = new System.Windows.Forms.GroupBox();
      this.chkUseForLastFMWebStream = new System.Windows.Forms.CheckBox();
      this.chkUseForWebStream = new System.Windows.Forms.CheckBox();
      this.chkUseForCDDA = new System.Windows.Forms.CheckBox();
      this.groupBox7 = new System.Windows.Forms.GroupBox();
      this.mpLabel23 = new MediaPortal.UserInterface.Controls.MPLabel();
      this.lvExtensions = new System.Windows.Forms.ListView();
      this.tbExtension = new System.Windows.Forms.TextBox();
      this.btnDefault = new MediaPortal.UserInterface.Controls.MPButton();
      this.btnDeleteExt = new MediaPortal.UserInterface.Controls.MPButton();
      this.btnAddExt = new MediaPortal.UserInterface.Controls.MPButton();
      this.tabPageAdvanced = new System.Windows.Forms.TabPage();
      this.groupBox19 = new System.Windows.Forms.GroupBox();
      this.chkUseOverSampling = new System.Windows.Forms.CheckBox();
      this.groupBox18 = new System.Windows.Forms.GroupBox();
      this.chkUseRGAlbumGain = new System.Windows.Forms.CheckBox();
      this.chkUseReplayGain = new System.Windows.Forms.CheckBox();
      this.groupBox6 = new System.Windows.Forms.GroupBox();
      this.textBox3 = new System.Windows.Forms.TextBox();
      this.tabPageASIO = new System.Windows.Forms.TabPage();
      this.groupBox9 = new System.Windows.Forms.GroupBox();
      this.textBox2 = new System.Windows.Forms.TextBox();
      this.mpLabel20 = new MediaPortal.UserInterface.Controls.MPLabel();
      this.mpLabel19 = new MediaPortal.UserInterface.Controls.MPLabel();
      this.mpLabel17 = new MediaPortal.UserInterface.Controls.MPLabel();
      this.mpLabel1 = new MediaPortal.UserInterface.Controls.MPLabel();
      this.cboMaxASIORate = new System.Windows.Forms.ComboBox();
      this.cboMinASIORate = new System.Windows.Forms.ComboBox();
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.chkUseMaxASIOBufferSize = new System.Windows.Forms.CheckBox();
      this.tabPageWaveOut = new System.Windows.Forms.TabPage();
      this.groupBox2 = new System.Windows.Forms.GroupBox();
      this.textBox7 = new System.Windows.Forms.TextBox();
      this.tabPageVisualization = new System.Windows.Forms.TabPage();
      this.groupBox14 = new System.Windows.Forms.GroupBox();
      this.nudWMPVizFps = new System.Windows.Forms.NumericUpDown();
      this.label2 = new System.Windows.Forms.Label();
      this.label11 = new System.Windows.Forms.Label();
      this.label10 = new System.Windows.Forms.Label();
      this.cboWMPVisualizationPresets = new System.Windows.Forms.ComboBox();
      this.cboWMPVisualizations = new System.Windows.Forms.ComboBox();
      this.groupBox11 = new System.Windows.Forms.GroupBox();
      this.textBox1 = new System.Windows.Forms.TextBox();
      this.chkUseVizAGC = new System.Windows.Forms.CheckBox();
      this.textBox6 = new System.Windows.Forms.TextBox();
      this.mpLabel9 = new MediaPortal.UserInterface.Controls.MPLabel();
      this.trackBarVizLatency = new System.Windows.Forms.TrackBar();
      this.mpLabel27 = new MediaPortal.UserInterface.Controls.MPLabel();
      this.lblVizLatency = new MediaPortal.UserInterface.Controls.MPLabel();
      this.tabPageWMPViz = new System.Windows.Forms.TabPage();
      this.groupBox12 = new System.Windows.Forms.GroupBox();
      this.textBox9 = new System.Windows.Forms.TextBox();
      this.textBox8 = new System.Windows.Forms.TextBox();
      this.chkWMPVizFFTHalf = new System.Windows.Forms.CheckBox();
      this.mpLabel31 = new MediaPortal.UserInterface.Controls.MPLabel();
      this.lblWMPVizFFTMinimum = new MediaPortal.UserInterface.Controls.MPLabel();
      this.trackBarWMPVizFFTMinimum = new System.Windows.Forms.TrackBar();
      this.mpLabel33 = new MediaPortal.UserInterface.Controls.MPLabel();
      this.mpLabel29 = new MediaPortal.UserInterface.Controls.MPLabel();
      this.lblWMPVizFFTFallBack = new MediaPortal.UserInterface.Controls.MPLabel();
      this.trackBarWMPVizFFTFallBack = new System.Windows.Forms.TrackBar();
      this.mpLabel30 = new MediaPortal.UserInterface.Controls.MPLabel();
      this.tabPageDSP = new System.Windows.Forms.TabPage();
      this.groupBox16 = new System.Windows.Forms.GroupBox();
      this.textBox4 = new System.Windows.Forms.TextBox();
      this.tabPageWASAPI = new System.Windows.Forms.TabPage();
      this.groupBox21 = new System.Windows.Forms.GroupBox();
      this.chkWASAPIEvent = new System.Windows.Forms.CheckBox();
      this.groupBox13 = new System.Windows.Forms.GroupBox();
      this.textBox5 = new System.Windows.Forms.TextBox();
      this.mpLabel21 = new MediaPortal.UserInterface.Controls.MPLabel();
      this.mpLabel22 = new MediaPortal.UserInterface.Controls.MPLabel();
      this.mpLabel24 = new MediaPortal.UserInterface.Controls.MPLabel();
      this.mpLabel26 = new MediaPortal.UserInterface.Controls.MPLabel();
      this.cboMaxWASAPIRate = new System.Windows.Forms.ComboBox();
      this.cboMinWASAPIRate = new System.Windows.Forms.ComboBox();
      this.tvwMenu = new System.Windows.Forms.TreeView();
      this.ctlHeader = new MediaPortal.UserInterface.Controls.MPGradientLabel();
      this.beveledLine1 = new MediaPortal.UserInterface.Controls.MPBeveledLine();
      this.groupBox22 = new System.Windows.Forms.GroupBox();
      this.cboFiveDotZeroUpMix = new System.Windows.Forms.ComboBox();
      this.mpLabel28 = new MediaPortal.UserInterface.Controls.MPLabel();
      ((System.ComponentModel.ISupportInitialize)(this.trackBarFileBufferSize)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.trackBarPlayBackBufferSize)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.trackBarSeekIncrement)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.trackBarSoftStopDuration)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.trackBarGapLength)).BeginInit();
      this.tabControl.SuspendLayout();
      this.tabPageAbout.SuspendLayout();
      this.groupBox8.SuspendLayout();
      this.tabPageDevice.SuspendLayout();
      this.groupBox3.SuspendLayout();
      this.pnlDevice.SuspendLayout();
      this.tabPageGeneral.SuspendLayout();
      this.groupBox4.SuspendLayout();
      this.pnlSoftStopDuration.SuspendLayout();
      this.tabPageUpmixing.SuspendLayout();
      this.groupBox20.SuspendLayout();
      this.groupBox15.SuspendLayout();
      this.groupBox10.SuspendLayout();
      this.groupBox5.SuspendLayout();
      this.tabPageExtensions.SuspendLayout();
      this.groupBox17.SuspendLayout();
      this.groupBox7.SuspendLayout();
      this.tabPageAdvanced.SuspendLayout();
      this.groupBox19.SuspendLayout();
      this.groupBox18.SuspendLayout();
      this.groupBox6.SuspendLayout();
      this.tabPageASIO.SuspendLayout();
      this.groupBox9.SuspendLayout();
      this.groupBox1.SuspendLayout();
      this.tabPageWaveOut.SuspendLayout();
      this.groupBox2.SuspendLayout();
      this.tabPageVisualization.SuspendLayout();
      this.groupBox14.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.nudWMPVizFps)).BeginInit();
      this.groupBox11.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.trackBarVizLatency)).BeginInit();
      this.tabPageWMPViz.SuspendLayout();
      this.groupBox12.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.trackBarWMPVizFFTMinimum)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.trackBarWMPVizFFTFallBack)).BeginInit();
      this.tabPageDSP.SuspendLayout();
      this.groupBox16.SuspendLayout();
      this.tabPageWASAPI.SuspendLayout();
      this.groupBox21.SuspendLayout();
      this.groupBox13.SuspendLayout();
      this.groupBox22.SuspendLayout();
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
      this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
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
      // mpLabel4
      // 
      this.mpLabel4.Location = new System.Drawing.Point(21, 44);
      this.mpLabel4.Name = "mpLabel4";
      this.mpLabel4.Size = new System.Drawing.Size(100, 23);
      this.mpLabel4.TabIndex = 16;
      this.mpLabel4.Text = "Buffer size:";
      // 
      // mpLabel5
      // 
      this.mpLabel5.Location = new System.Drawing.Point(21, 44);
      this.mpLabel5.Name = "mpLabel5";
      this.mpLabel5.Size = new System.Drawing.Size(112, 24);
      this.mpLabel5.TabIndex = 17;
      this.mpLabel5.Text = "Playback buffer size:";
      // 
      // mpLabel14
      // 
      this.mpLabel14.Location = new System.Drawing.Point(420, 44);
      this.mpLabel14.Name = "mpLabel14";
      this.mpLabel14.Size = new System.Drawing.Size(71, 18);
      this.mpLabel14.TabIndex = 26;
      this.mpLabel14.Text = "milliseconds";
      // 
      // lblBASSPlayBackBufferSize
      // 
      this.lblBASSPlayBackBufferSize.Location = new System.Drawing.Point(373, 44);
      this.lblBASSPlayBackBufferSize.Name = "lblBASSPlayBackBufferSize";
      this.lblBASSPlayBackBufferSize.Size = new System.Drawing.Size(37, 21);
      this.lblBASSPlayBackBufferSize.TabIndex = 27;
      this.lblBASSPlayBackBufferSize.Text = "0";
      this.lblBASSPlayBackBufferSize.TextAlign = System.Drawing.ContentAlignment.TopRight;
      // 
      // trackBarFileBufferSize
      // 
      this.trackBarFileBufferSize.LargeChange = 1;
      this.trackBarFileBufferSize.Location = new System.Drawing.Point(148, 38);
      this.trackBarFileBufferSize.Minimum = 1;
      this.trackBarFileBufferSize.Name = "trackBarFileBufferSize";
      this.trackBarFileBufferSize.Size = new System.Drawing.Size(219, 45);
      this.trackBarFileBufferSize.TabIndex = 3;
      this.trackBarFileBufferSize.Value = 1;
      this.trackBarFileBufferSize.ValueChanged += new System.EventHandler(this.trackBarFileBufferSize_ValueChanged);
      // 
      // mpLabel12
      // 
      this.mpLabel12.Location = new System.Drawing.Point(416, 44);
      this.mpLabel12.Name = "mpLabel12";
      this.mpLabel12.Size = new System.Drawing.Size(68, 18);
      this.mpLabel12.TabIndex = 26;
      this.mpLabel12.Text = "milliseconds";
      // 
      // lblPlayBackBufferSize
      // 
      this.lblPlayBackBufferSize.Location = new System.Drawing.Point(373, 44);
      this.lblPlayBackBufferSize.Name = "lblPlayBackBufferSize";
      this.lblPlayBackBufferSize.Size = new System.Drawing.Size(37, 21);
      this.lblPlayBackBufferSize.TabIndex = 24;
      this.lblPlayBackBufferSize.Text = "0";
      this.lblPlayBackBufferSize.TextAlign = System.Drawing.ContentAlignment.TopRight;
      // 
      // trackBarPlayBackBufferSize
      // 
      this.trackBarPlayBackBufferSize.LargeChange = 1;
      this.trackBarPlayBackBufferSize.Location = new System.Drawing.Point(148, 38);
      this.trackBarPlayBackBufferSize.Maximum = 16;
      this.trackBarPlayBackBufferSize.Minimum = 4;
      this.trackBarPlayBackBufferSize.Name = "trackBarPlayBackBufferSize";
      this.trackBarPlayBackBufferSize.Size = new System.Drawing.Size(219, 45);
      this.trackBarPlayBackBufferSize.TabIndex = 1;
      this.trackBarPlayBackBufferSize.Value = 4;
      this.trackBarPlayBackBufferSize.ValueChanged += new System.EventHandler(this.trackBarPlayBackBufferSize_ValueChanged);
      // 
      // cboStereoUpMix
      // 
      this.cboStereoUpMix.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cboStereoUpMix.FormattingEnabled = true;
      this.cboStereoUpMix.Location = new System.Drawing.Point(92, 19);
      this.cboStereoUpMix.Name = "cboStereoUpMix";
      this.cboStereoUpMix.Size = new System.Drawing.Size(215, 21);
      this.cboStereoUpMix.TabIndex = 2;
      this.cboStereoUpMix.SelectedIndexChanged += new System.EventHandler(this.cboStereoUpMix_SelectedIndexChanged);
      // 
      // chkDoSoftStop
      // 
      this.chkDoSoftStop.AutoSize = true;
      this.chkDoSoftStop.Location = new System.Drawing.Point(12, 148);
      this.chkDoSoftStop.Name = "chkDoSoftStop";
      this.chkDoSoftStop.Size = new System.Drawing.Size(182, 17);
      this.chkDoSoftStop.TabIndex = 4;
      this.chkDoSoftStop.Text = "Fade-in on start, fade-out on stop";
      this.chkDoSoftStop.UseVisualStyleBackColor = true;
      this.chkDoSoftStop.CheckedChanged += new System.EventHandler(this.chkDoSoftStop_CheckedChanged);
      // 
      // mpLabel2
      // 
      this.mpLabel2.Location = new System.Drawing.Point(6, 22);
      this.mpLabel2.Name = "mpLabel2";
      this.mpLabel2.Size = new System.Drawing.Size(80, 18);
      this.mpLabel2.TabIndex = 12;
      this.mpLabel2.Text = "Upmix to:";
      // 
      // trackBarSeekIncrement
      // 
      this.trackBarSeekIncrement.LargeChange = 1;
      this.trackBarSeekIncrement.Location = new System.Drawing.Point(151, 97);
      this.trackBarSeekIncrement.Maximum = 6;
      this.trackBarSeekIncrement.Minimum = 1;
      this.trackBarSeekIncrement.Name = "trackBarSeekIncrement";
      this.trackBarSeekIncrement.Size = new System.Drawing.Size(219, 45);
      this.trackBarSeekIncrement.TabIndex = 3;
      this.trackBarSeekIncrement.Value = 1;
      this.trackBarSeekIncrement.ValueChanged += new System.EventHandler(this.trackBarSeekIncrement_ValueChanged);
      // 
      // cboASIODevice
      // 
      this.cboASIODevice.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cboASIODevice.FormattingEnabled = true;
      this.cboASIODevice.Location = new System.Drawing.Point(33, 36);
      this.cboASIODevice.Name = "cboASIODevice";
      this.cboASIODevice.Size = new System.Drawing.Size(267, 21);
      this.cboASIODevice.TabIndex = 3;
      this.cboASIODevice.SelectedIndexChanged += new System.EventHandler(this.cboASIODevice_SelectedIndexChanged);
      // 
      // mpLabel3
      // 
      this.mpLabel3.Location = new System.Drawing.Point(9, 102);
      this.mpLabel3.Name = "mpLabel3";
      this.mpLabel3.Size = new System.Drawing.Size(136, 21);
      this.mpLabel3.TabIndex = 15;
      this.mpLabel3.Text = "Forward/Back skip:";
      // 
      // lblSoftStopDurationLabel
      // 
      this.lblSoftStopDurationLabel.Location = new System.Drawing.Point(16, 6);
      this.lblSoftStopDurationLabel.Name = "lblSoftStopDurationLabel";
      this.lblSoftStopDurationLabel.Size = new System.Drawing.Size(108, 28);
      this.lblSoftStopDurationLabel.TabIndex = 2;
      this.lblSoftStopDurationLabel.Text = "Fading duration:";
      // 
      // trackBarSoftStopDuration
      // 
      this.trackBarSoftStopDuration.LargeChange = 1;
      this.trackBarSoftStopDuration.Location = new System.Drawing.Point(139, 3);
      this.trackBarSoftStopDuration.Minimum = 1;
      this.trackBarSoftStopDuration.Name = "trackBarSoftStopDuration";
      this.trackBarSoftStopDuration.Size = new System.Drawing.Size(219, 45);
      this.trackBarSoftStopDuration.TabIndex = 1;
      this.trackBarSoftStopDuration.Value = 1;
      this.trackBarSoftStopDuration.ValueChanged += new System.EventHandler(this.trackBarSoftStopDuration_ValueChanged);
      // 
      // lblSoftStopDuration
      // 
      this.lblSoftStopDuration.Location = new System.Drawing.Point(364, 6);
      this.lblSoftStopDuration.Name = "lblSoftStopDuration";
      this.lblSoftStopDuration.Size = new System.Drawing.Size(37, 21);
      this.lblSoftStopDuration.TabIndex = 22;
      this.lblSoftStopDuration.Text = "0";
      this.lblSoftStopDuration.TextAlign = System.Drawing.ContentAlignment.TopRight;
      // 
      // lblSeekIncrement
      // 
      this.lblSeekIncrement.Location = new System.Drawing.Point(376, 102);
      this.lblSeekIncrement.Name = "lblSeekIncrement";
      this.lblSeekIncrement.Size = new System.Drawing.Size(37, 21);
      this.lblSeekIncrement.TabIndex = 23;
      this.lblSeekIncrement.Text = "0";
      this.lblSeekIncrement.TextAlign = System.Drawing.ContentAlignment.TopRight;
      // 
      // mpLabel10
      // 
      this.mpLabel10.Location = new System.Drawing.Point(419, 102);
      this.mpLabel10.Name = "mpLabel10";
      this.mpLabel10.Size = new System.Drawing.Size(61, 18);
      this.mpLabel10.TabIndex = 24;
      this.mpLabel10.Text = "seconds";
      // 
      // lblSoftStopDurationUnits
      // 
      this.lblSoftStopDurationUnits.Location = new System.Drawing.Point(406, 6);
      this.lblSoftStopDurationUnits.Name = "lblSoftStopDurationUnits";
      this.lblSoftStopDurationUnits.Size = new System.Drawing.Size(73, 18);
      this.lblSoftStopDurationUnits.TabIndex = 25;
      this.lblSoftStopDurationUnits.Text = "milliseconds";
      // 
      // trackBarGapLength
      // 
      this.trackBarGapLength.LargeChange = 1;
      this.trackBarGapLength.Location = new System.Drawing.Point(151, 46);
      this.trackBarGapLength.Maximum = 8;
      this.trackBarGapLength.Name = "trackBarGapLength";
      this.trackBarGapLength.Size = new System.Drawing.Size(219, 45);
      this.trackBarGapLength.TabIndex = 2;
      this.trackBarGapLength.ValueChanged += new System.EventHandler(this.trackBarGapLength_ValueChanged);
      // 
      // mpLabel6
      // 
      this.mpLabel6.Location = new System.Drawing.Point(419, 50);
      this.mpLabel6.Name = "mpLabel6";
      this.mpLabel6.Size = new System.Drawing.Size(73, 18);
      this.mpLabel6.TabIndex = 28;
      this.mpLabel6.Text = "milliseconds";
      // 
      // lblGapLength
      // 
      this.lblGapLength.Location = new System.Drawing.Point(376, 50);
      this.lblGapLength.Name = "lblGapLength";
      this.lblGapLength.Size = new System.Drawing.Size(37, 21);
      this.lblGapLength.TabIndex = 27;
      this.lblGapLength.Text = "0";
      this.lblGapLength.TextAlign = System.Drawing.ContentAlignment.TopRight;
      // 
      // mpLabel8
      // 
      this.mpLabel8.Location = new System.Drawing.Point(9, 50);
      this.mpLabel8.Name = "mpLabel8";
      this.mpLabel8.Size = new System.Drawing.Size(136, 40);
      this.mpLabel8.TabIndex = 29;
      this.mpLabel8.Text = "Gap length in \'Normal\' playback mode:";
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
      this.tabControl.Controls.Add(this.tabPageWASAPI);
      this.tabControl.Location = new System.Drawing.Point(204, 13);
      this.tabControl.Name = "tabControl";
      this.tabControl.SelectedIndex = 0;
      this.tabControl.Size = new System.Drawing.Size(511, 348);
      this.tabControl.TabIndex = 2;
      // 
      // tabPageAbout
      // 
      this.tabPageAbout.Controls.Add(this.groupBox8);
      this.tabPageAbout.Location = new System.Drawing.Point(4, 25);
      this.tabPageAbout.Name = "tabPageAbout";
      this.tabPageAbout.Size = new System.Drawing.Size(503, 319);
      this.tabPageAbout.TabIndex = 4;
      this.tabPageAbout.Text = "About";
      this.tabPageAbout.UseVisualStyleBackColor = true;
      // 
      // groupBox8
      // 
      this.groupBox8.Controls.Add(this.label1);
      this.groupBox8.Controls.Add(this.lnkForum);
      this.groupBox8.Controls.Add(this.lnkDownload);
      this.groupBox8.Controls.Add(this.mpLabel16);
      this.groupBox8.Controls.Add(this.lblPlayerName);
      this.groupBox8.Controls.Add(this.mpLabel11);
      this.groupBox8.Controls.Add(this.lblAuthorName);
      this.groupBox8.Controls.Add(this.mpLabel13);
      this.groupBox8.Controls.Add(this.lblVersion);
      this.groupBox8.Controls.Add(this.mpLabel15);
      this.groupBox8.Controls.Add(this.lblDescription);
      this.groupBox8.Location = new System.Drawing.Point(3, 3);
      this.groupBox8.Name = "groupBox8";
      this.groupBox8.Size = new System.Drawing.Size(497, 308);
      this.groupBox8.TabIndex = 19;
      this.groupBox8.TabStop = false;
      this.groupBox8.Text = "About this plugin";
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(22, 191);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(285, 13);
      this.label1.TabIndex = 19;
      this.label1.Text = "ASIO Technology by Steinberg Media Technologies GmbH";
      // 
      // lnkForum
      // 
      this.lnkForum.AutoSize = true;
      this.lnkForum.Location = new System.Drawing.Point(22, 149);
      this.lnkForum.Name = "lnkForum";
      this.lnkForum.Size = new System.Drawing.Size(230, 13);
      this.lnkForum.TabIndex = 2;
      this.lnkForum.TabStop = true;
      this.lnkForum.Text = "Visit forumthread at www.team-mediaportal.com";
      this.lnkForum.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkForum_LinkClicked);
      // 
      // lnkDownload
      // 
      this.lnkDownload.AutoSize = true;
      this.lnkDownload.Location = new System.Drawing.Point(22, 122);
      this.lnkDownload.Name = "lnkDownload";
      this.lnkDownload.Size = new System.Drawing.Size(176, 13);
      this.lnkDownload.TabIndex = 1;
      this.lnkDownload.TabStop = true;
      this.lnkDownload.Text = "Visit homepage at code.google.com";
      this.lnkDownload.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkDownLoad_LinkClicked);
      // 
      // mpLabel16
      // 
      this.mpLabel16.Location = new System.Drawing.Point(22, 30);
      this.mpLabel16.Name = "mpLabel16";
      this.mpLabel16.Size = new System.Drawing.Size(100, 23);
      this.mpLabel16.TabIndex = 13;
      this.mpLabel16.Text = "Name:";
      // 
      // lblPlayerName
      // 
      this.lblPlayerName.Location = new System.Drawing.Point(128, 30);
      this.lblPlayerName.Name = "lblPlayerName";
      this.lblPlayerName.Size = new System.Drawing.Size(347, 23);
      this.lblPlayerName.TabIndex = 18;
      this.lblPlayerName.Text = "Name:";
      // 
      // mpLabel11
      // 
      this.mpLabel11.Location = new System.Drawing.Point(22, 53);
      this.mpLabel11.Name = "mpLabel11";
      this.mpLabel11.Size = new System.Drawing.Size(100, 23);
      this.mpLabel11.TabIndex = 10;
      this.mpLabel11.Text = "Description:";
      // 
      // lblAuthorName
      // 
      this.lblAuthorName.Location = new System.Drawing.Point(128, 99);
      this.lblAuthorName.Name = "lblAuthorName";
      this.lblAuthorName.Size = new System.Drawing.Size(347, 23);
      this.lblAuthorName.TabIndex = 17;
      this.lblAuthorName.Text = "Author:";
      // 
      // mpLabel13
      // 
      this.mpLabel13.Location = new System.Drawing.Point(22, 76);
      this.mpLabel13.Name = "mpLabel13";
      this.mpLabel13.Size = new System.Drawing.Size(100, 23);
      this.mpLabel13.TabIndex = 11;
      this.mpLabel13.Text = "Version:";
      // 
      // lblVersion
      // 
      this.lblVersion.Location = new System.Drawing.Point(128, 76);
      this.lblVersion.Name = "lblVersion";
      this.lblVersion.Size = new System.Drawing.Size(347, 23);
      this.lblVersion.TabIndex = 16;
      this.lblVersion.Text = "Version:";
      // 
      // mpLabel15
      // 
      this.mpLabel15.Location = new System.Drawing.Point(22, 99);
      this.mpLabel15.Name = "mpLabel15";
      this.mpLabel15.Size = new System.Drawing.Size(100, 23);
      this.mpLabel15.TabIndex = 12;
      this.mpLabel15.Text = "Author:";
      // 
      // lblDescription
      // 
      this.lblDescription.Location = new System.Drawing.Point(128, 53);
      this.lblDescription.Name = "lblDescription";
      this.lblDescription.Size = new System.Drawing.Size(347, 23);
      this.lblDescription.TabIndex = 15;
      this.lblDescription.Text = "Description:";
      // 
      // tabPageDevice
      // 
      this.tabPageDevice.Controls.Add(this.groupBox3);
      this.tabPageDevice.Location = new System.Drawing.Point(4, 25);
      this.tabPageDevice.Name = "tabPageDevice";
      this.tabPageDevice.Size = new System.Drawing.Size(503, 319);
      this.tabPageDevice.TabIndex = 5;
      this.tabPageDevice.Text = "Device";
      this.tabPageDevice.UseVisualStyleBackColor = true;
      // 
      // groupBox3
      // 
      this.groupBox3.Controls.Add(this.pnlDevice);
      this.groupBox3.Location = new System.Drawing.Point(3, 3);
      this.groupBox3.Name = "groupBox3";
      this.groupBox3.Size = new System.Drawing.Size(497, 308);
      this.groupBox3.TabIndex = 12;
      this.groupBox3.TabStop = false;
      this.groupBox3.Text = "Select output device";
      // 
      // pnlDevice
      // 
      this.pnlDevice.Controls.Add(this.lblWASAPISpeakerLayout);
      this.pnlDevice.Controls.Add(this.cboWASAPISpeakerLayout);
      this.pnlDevice.Controls.Add(this.chkWASAPIExclusive);
      this.pnlDevice.Controls.Add(this.btnWASAPIControlPanel);
      this.pnlDevice.Controls.Add(this.cboWASAPIDevice);
      this.pnlDevice.Controls.Add(this.rbUseWASAPI);
      this.pnlDevice.Controls.Add(this.lblASIOLastChan);
      this.pnlDevice.Controls.Add(this.cboASIOLastChan);
      this.pnlDevice.Controls.Add(this.lblASIOFirstChan);
      this.pnlDevice.Controls.Add(this.cboASIOFirstChan);
      this.pnlDevice.Controls.Add(this.btnMMEControlPanel);
      this.pnlDevice.Controls.Add(this.cboSoundDevice);
      this.pnlDevice.Controls.Add(this.rbUseSound);
      this.pnlDevice.Controls.Add(this.btnASIOControlPanel);
      this.pnlDevice.Controls.Add(this.cboASIODevice);
      this.pnlDevice.Controls.Add(this.rbUseASIO);
      this.pnlDevice.Location = new System.Drawing.Point(6, 19);
      this.pnlDevice.Name = "pnlDevice";
      this.pnlDevice.Size = new System.Drawing.Size(397, 283);
      this.pnlDevice.TabIndex = 11;
      // 
      // lblWASAPISpeakerLayout
      // 
      this.lblWASAPISpeakerLayout.Location = new System.Drawing.Point(52, 242);
      this.lblWASAPISpeakerLayout.Name = "lblWASAPISpeakerLayout";
      this.lblWASAPISpeakerLayout.Size = new System.Drawing.Size(121, 18);
      this.lblWASAPISpeakerLayout.TabIndex = 43;
      this.lblWASAPISpeakerLayout.Text = "Speaker configuration:";
      // 
      // cboWASAPISpeakerLayout
      // 
      this.cboWASAPISpeakerLayout.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cboWASAPISpeakerLayout.FormattingEnabled = true;
      this.cboWASAPISpeakerLayout.Location = new System.Drawing.Point(202, 239);
      this.cboWASAPISpeakerLayout.Name = "cboWASAPISpeakerLayout";
      this.cboWASAPISpeakerLayout.Size = new System.Drawing.Size(98, 21);
      this.cboWASAPISpeakerLayout.TabIndex = 41;
      this.cboWASAPISpeakerLayout.SelectedIndexChanged += new System.EventHandler(this.cboWASAPISpeakerLayout_SelectedIndexChanged);
      // 
      // chkWASAPIExclusive
      // 
      this.chkWASAPIExclusive.AutoSize = true;
      this.chkWASAPIExclusive.Location = new System.Drawing.Point(33, 222);
      this.chkWASAPIExclusive.Name = "chkWASAPIExclusive";
      this.chkWASAPIExclusive.Size = new System.Drawing.Size(71, 17);
      this.chkWASAPIExclusive.TabIndex = 42;
      this.chkWASAPIExclusive.Text = "Exclusive";
      this.chkWASAPIExclusive.UseVisualStyleBackColor = true;
      this.chkWASAPIExclusive.CheckedChanged += new System.EventHandler(this.chkWASAPIExclusive_CheckedChanged);
      // 
      // btnWASAPIControlPanel
      // 
      this.btnWASAPIControlPanel.Location = new System.Drawing.Point(309, 189);
      this.btnWASAPIControlPanel.Name = "btnWASAPIControlPanel";
      this.btnWASAPIControlPanel.Size = new System.Drawing.Size(75, 23);
      this.btnWASAPIControlPanel.TabIndex = 40;
      this.btnWASAPIControlPanel.Text = "Settings...";
      this.btnWASAPIControlPanel.UseVisualStyleBackColor = true;
      this.btnWASAPIControlPanel.Click += new System.EventHandler(this.btnWASAPIControlPanel_Click);
      // 
      // cboWASAPIDevice
      // 
      this.cboWASAPIDevice.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cboWASAPIDevice.FormattingEnabled = true;
      this.cboWASAPIDevice.Location = new System.Drawing.Point(33, 191);
      this.cboWASAPIDevice.Name = "cboWASAPIDevice";
      this.cboWASAPIDevice.Size = new System.Drawing.Size(267, 21);
      this.cboWASAPIDevice.TabIndex = 39;
      this.cboWASAPIDevice.SelectedIndexChanged += new System.EventHandler(this.cboWASAPIDevice_SelectedIndexChanged);
      // 
      // rbUseWASAPI
      // 
      this.rbUseWASAPI.AutoSize = true;
      this.rbUseWASAPI.Location = new System.Drawing.Point(14, 168);
      this.rbUseWASAPI.Name = "rbUseWASAPI";
      this.rbUseWASAPI.Size = new System.Drawing.Size(127, 17);
      this.rbUseWASAPI.TabIndex = 38;
      this.rbUseWASAPI.TabStop = true;
      this.rbUseWASAPI.Text = "Use WASAPI device:";
      this.rbUseWASAPI.UseVisualStyleBackColor = true;
      this.rbUseWASAPI.CheckedChanged += new System.EventHandler(this.rbUseWASAPI_CheckedChanged);
      // 
      // lblASIOLastChan
      // 
      this.lblASIOLastChan.Location = new System.Drawing.Point(30, 93);
      this.lblASIOLastChan.Name = "lblASIOLastChan";
      this.lblASIOLastChan.Size = new System.Drawing.Size(82, 21);
      this.lblASIOLastChan.TabIndex = 37;
      this.lblASIOLastChan.Text = "Last channel:";
      // 
      // cboASIOLastChan
      // 
      this.cboASIOLastChan.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cboASIOLastChan.FormattingEnabled = true;
      this.cboASIOLastChan.Location = new System.Drawing.Point(118, 90);
      this.cboASIOLastChan.Name = "cboASIOLastChan";
      this.cboASIOLastChan.Size = new System.Drawing.Size(182, 21);
      this.cboASIOLastChan.TabIndex = 6;
      this.cboASIOLastChan.SelectedIndexChanged += new System.EventHandler(this.cboASIOLastChan_SelectedIndexChanged);
      // 
      // lblASIOFirstChan
      // 
      this.lblASIOFirstChan.Location = new System.Drawing.Point(30, 66);
      this.lblASIOFirstChan.Name = "lblASIOFirstChan";
      this.lblASIOFirstChan.Size = new System.Drawing.Size(82, 21);
      this.lblASIOFirstChan.TabIndex = 35;
      this.lblASIOFirstChan.Text = "First channel:";
      // 
      // cboASIOFirstChan
      // 
      this.cboASIOFirstChan.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cboASIOFirstChan.FormattingEnabled = true;
      this.cboASIOFirstChan.Location = new System.Drawing.Point(118, 63);
      this.cboASIOFirstChan.Name = "cboASIOFirstChan";
      this.cboASIOFirstChan.Size = new System.Drawing.Size(182, 21);
      this.cboASIOFirstChan.TabIndex = 5;
      this.cboASIOFirstChan.SelectedIndexChanged += new System.EventHandler(this.cboASIOFirstChan_SelectedIndexChanged);
      // 
      // btnMMEControlPanel
      // 
      this.btnMMEControlPanel.Location = new System.Drawing.Point(309, 139);
      this.btnMMEControlPanel.Name = "btnMMEControlPanel";
      this.btnMMEControlPanel.Size = new System.Drawing.Size(75, 23);
      this.btnMMEControlPanel.TabIndex = 8;
      this.btnMMEControlPanel.Text = "Settings...";
      this.btnMMEControlPanel.UseVisualStyleBackColor = true;
      this.btnMMEControlPanel.Click += new System.EventHandler(this.btnMMEControlPanel_Click);
      // 
      // cboSoundDevice
      // 
      this.cboSoundDevice.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cboSoundDevice.FormattingEnabled = true;
      this.cboSoundDevice.Location = new System.Drawing.Point(33, 141);
      this.cboSoundDevice.Name = "cboSoundDevice";
      this.cboSoundDevice.Size = new System.Drawing.Size(267, 21);
      this.cboSoundDevice.TabIndex = 7;
      this.cboSoundDevice.SelectedIndexChanged += new System.EventHandler(this.cboSoundDevice_SelectedIndexChanged);
      // 
      // rbUseSound
      // 
      this.rbUseSound.AutoSize = true;
      this.rbUseSound.Location = new System.Drawing.Point(14, 117);
      this.rbUseSound.Name = "rbUseSound";
      this.rbUseSound.Size = new System.Drawing.Size(129, 17);
      this.rbUseSound.TabIndex = 2;
      this.rbUseSound.TabStop = true;
      this.rbUseSound.Text = "Use Windows device:";
      this.rbUseSound.UseVisualStyleBackColor = true;
      // 
      // btnASIOControlPanel
      // 
      this.btnASIOControlPanel.Location = new System.Drawing.Point(309, 36);
      this.btnASIOControlPanel.Name = "btnASIOControlPanel";
      this.btnASIOControlPanel.Size = new System.Drawing.Size(75, 23);
      this.btnASIOControlPanel.TabIndex = 4;
      this.btnASIOControlPanel.Text = "Settings...";
      this.btnASIOControlPanel.UseVisualStyleBackColor = true;
      this.btnASIOControlPanel.Click += new System.EventHandler(this.btnASIOControlPanel_Click);
      // 
      // rbUseASIO
      // 
      this.rbUseASIO.AutoSize = true;
      this.rbUseASIO.Location = new System.Drawing.Point(14, 13);
      this.rbUseASIO.Name = "rbUseASIO";
      this.rbUseASIO.Size = new System.Drawing.Size(110, 17);
      this.rbUseASIO.TabIndex = 1;
      this.rbUseASIO.TabStop = true;
      this.rbUseASIO.Text = "Use ASIO device:";
      this.rbUseASIO.UseVisualStyleBackColor = true;
      this.rbUseASIO.CheckedChanged += new System.EventHandler(this.rbUseASIO_CheckedChanged);
      // 
      // tabPageGeneral
      // 
      this.tabPageGeneral.Controls.Add(this.groupBox4);
      this.tabPageGeneral.Location = new System.Drawing.Point(4, 25);
      this.tabPageGeneral.Name = "tabPageGeneral";
      this.tabPageGeneral.Padding = new System.Windows.Forms.Padding(3);
      this.tabPageGeneral.Size = new System.Drawing.Size(503, 319);
      this.tabPageGeneral.TabIndex = 0;
      this.tabPageGeneral.Text = "General";
      this.tabPageGeneral.UseVisualStyleBackColor = true;
      // 
      // groupBox4
      // 
      this.groupBox4.Controls.Add(this.pnlSoftStopDuration);
      this.groupBox4.Controls.Add(this.cboDefaultPlayBackMode);
      this.groupBox4.Controls.Add(this.mpLabel7);
      this.groupBox4.Controls.Add(this.lblSeekIncrement);
      this.groupBox4.Controls.Add(this.mpLabel8);
      this.groupBox4.Controls.Add(this.chkDoSoftStop);
      this.groupBox4.Controls.Add(this.mpLabel10);
      this.groupBox4.Controls.Add(this.mpLabel6);
      this.groupBox4.Controls.Add(this.lblGapLength);
      this.groupBox4.Controls.Add(this.trackBarSeekIncrement);
      this.groupBox4.Controls.Add(this.mpLabel3);
      this.groupBox4.Controls.Add(this.trackBarGapLength);
      this.groupBox4.Location = new System.Drawing.Point(3, 3);
      this.groupBox4.Name = "groupBox4";
      this.groupBox4.Size = new System.Drawing.Size(497, 308);
      this.groupBox4.TabIndex = 34;
      this.groupBox4.TabStop = false;
      this.groupBox4.Text = "General settings";
      // 
      // pnlSoftStopDuration
      // 
      this.pnlSoftStopDuration.Controls.Add(this.trackBarSoftStopDuration);
      this.pnlSoftStopDuration.Controls.Add(this.lblSoftStopDurationLabel);
      this.pnlSoftStopDuration.Controls.Add(this.lblSoftStopDuration);
      this.pnlSoftStopDuration.Controls.Add(this.lblSoftStopDurationUnits);
      this.pnlSoftStopDuration.Location = new System.Drawing.Point(12, 171);
      this.pnlSoftStopDuration.Name = "pnlSoftStopDuration";
      this.pnlSoftStopDuration.Size = new System.Drawing.Size(483, 51);
      this.pnlSoftStopDuration.TabIndex = 5;
      // 
      // cboDefaultPlayBackMode
      // 
      this.cboDefaultPlayBackMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cboDefaultPlayBackMode.FormattingEnabled = true;
      this.cboDefaultPlayBackMode.Location = new System.Drawing.Point(151, 19);
      this.cboDefaultPlayBackMode.Name = "cboDefaultPlayBackMode";
      this.cboDefaultPlayBackMode.Size = new System.Drawing.Size(219, 21);
      this.cboDefaultPlayBackMode.TabIndex = 1;
      this.cboDefaultPlayBackMode.SelectedIndexChanged += new System.EventHandler(this.cboDefaultPlayBackMode_SelectedIndexChanged);
      // 
      // mpLabel7
      // 
      this.mpLabel7.Location = new System.Drawing.Point(9, 22);
      this.mpLabel7.Name = "mpLabel7";
      this.mpLabel7.Size = new System.Drawing.Size(136, 21);
      this.mpLabel7.TabIndex = 33;
      this.mpLabel7.Text = "Default playback mode:";
      // 
      // tabPageUpmixing
      // 
      this.tabPageUpmixing.Controls.Add(this.groupBox22);
      this.tabPageUpmixing.Controls.Add(this.groupBox20);
      this.tabPageUpmixing.Controls.Add(this.groupBox15);
      this.tabPageUpmixing.Controls.Add(this.groupBox10);
      this.tabPageUpmixing.Controls.Add(this.groupBox5);
      this.tabPageUpmixing.Location = new System.Drawing.Point(4, 25);
      this.tabPageUpmixing.Name = "tabPageUpmixing";
      this.tabPageUpmixing.Padding = new System.Windows.Forms.Padding(3);
      this.tabPageUpmixing.Size = new System.Drawing.Size(503, 319);
      this.tabPageUpmixing.TabIndex = 3;
      this.tabPageUpmixing.Text = "Upmixing";
      this.tabPageUpmixing.UseVisualStyleBackColor = true;
      // 
      // groupBox20
      // 
      this.groupBox20.Controls.Add(this.cboQuadraphonicUpMix);
      this.groupBox20.Controls.Add(this.mpLabel18);
      this.groupBox20.Location = new System.Drawing.Point(3, 126);
      this.groupBox20.Name = "groupBox20";
      this.groupBox20.Size = new System.Drawing.Size(497, 54);
      this.groupBox20.TabIndex = 15;
      this.groupBox20.TabStop = false;
      this.groupBox20.Text = "Upmixing for 4.0 sources";
      // 
      // cboQuadraphonicUpMix
      // 
      this.cboQuadraphonicUpMix.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cboQuadraphonicUpMix.FormattingEnabled = true;
      this.cboQuadraphonicUpMix.Location = new System.Drawing.Point(92, 19);
      this.cboQuadraphonicUpMix.Name = "cboQuadraphonicUpMix";
      this.cboQuadraphonicUpMix.Size = new System.Drawing.Size(215, 21);
      this.cboQuadraphonicUpMix.TabIndex = 3;
      this.cboQuadraphonicUpMix.SelectedIndexChanged += new System.EventHandler(this.cboQuadraphonicUpMix_SelectedIndexChanged);
      // 
      // mpLabel18
      // 
      this.mpLabel18.Location = new System.Drawing.Point(6, 23);
      this.mpLabel18.Name = "mpLabel18";
      this.mpLabel18.Size = new System.Drawing.Size(80, 18);
      this.mpLabel18.TabIndex = 14;
      this.mpLabel18.Text = "Upmix to:";
      // 
      // groupBox15
      // 
      this.groupBox15.Controls.Add(this.mpLabel36);
      this.groupBox15.Controls.Add(this.cboMonoUpMix);
      this.groupBox15.Location = new System.Drawing.Point(3, 3);
      this.groupBox15.Name = "groupBox15";
      this.groupBox15.Size = new System.Drawing.Size(497, 56);
      this.groupBox15.TabIndex = 1;
      this.groupBox15.TabStop = false;
      this.groupBox15.Text = "Upmixing for mono sources";
      // 
      // mpLabel36
      // 
      this.mpLabel36.Location = new System.Drawing.Point(6, 22);
      this.mpLabel36.Name = "mpLabel36";
      this.mpLabel36.Size = new System.Drawing.Size(80, 18);
      this.mpLabel36.TabIndex = 12;
      this.mpLabel36.Text = "Upmix to:";
      // 
      // cboMonoUpMix
      // 
      this.cboMonoUpMix.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cboMonoUpMix.FormattingEnabled = true;
      this.cboMonoUpMix.Location = new System.Drawing.Point(92, 19);
      this.cboMonoUpMix.Name = "cboMonoUpMix";
      this.cboMonoUpMix.Size = new System.Drawing.Size(215, 21);
      this.cboMonoUpMix.TabIndex = 1;
      this.cboMonoUpMix.SelectedIndexChanged += new System.EventHandler(this.cboMonoUpMix_SelectedIndexChanged);
      // 
      // groupBox10
      // 
      this.groupBox10.Controls.Add(this.cboFiveDotOneUpMix);
      this.groupBox10.Controls.Add(this.mpLabel25);
      this.groupBox10.Location = new System.Drawing.Point(3, 246);
      this.groupBox10.Name = "groupBox10";
      this.groupBox10.Size = new System.Drawing.Size(497, 54);
      this.groupBox10.TabIndex = 3;
      this.groupBox10.TabStop = false;
      this.groupBox10.Text = "Upmixing for 5.1 sources";
      // 
      // cboFiveDotOneUpMix
      // 
      this.cboFiveDotOneUpMix.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cboFiveDotOneUpMix.FormattingEnabled = true;
      this.cboFiveDotOneUpMix.Location = new System.Drawing.Point(92, 19);
      this.cboFiveDotOneUpMix.Name = "cboFiveDotOneUpMix";
      this.cboFiveDotOneUpMix.Size = new System.Drawing.Size(215, 21);
      this.cboFiveDotOneUpMix.TabIndex = 5;
      this.cboFiveDotOneUpMix.SelectedIndexChanged += new System.EventHandler(this.cboFiveDotOneUpMix_SelectedIndexChanged);
      // 
      // mpLabel25
      // 
      this.mpLabel25.Location = new System.Drawing.Point(6, 23);
      this.mpLabel25.Name = "mpLabel25";
      this.mpLabel25.Size = new System.Drawing.Size(80, 18);
      this.mpLabel25.TabIndex = 14;
      this.mpLabel25.Text = "Upmix to:";
      // 
      // groupBox5
      // 
      this.groupBox5.Controls.Add(this.mpLabel2);
      this.groupBox5.Controls.Add(this.cboStereoUpMix);
      this.groupBox5.Location = new System.Drawing.Point(3, 65);
      this.groupBox5.Name = "groupBox5";
      this.groupBox5.Size = new System.Drawing.Size(497, 55);
      this.groupBox5.TabIndex = 2;
      this.groupBox5.TabStop = false;
      this.groupBox5.Text = "Upmixing for stereo sources";
      // 
      // tabPageExtensions
      // 
      this.tabPageExtensions.Controls.Add(this.groupBox17);
      this.tabPageExtensions.Controls.Add(this.groupBox7);
      this.tabPageExtensions.Location = new System.Drawing.Point(4, 25);
      this.tabPageExtensions.Name = "tabPageExtensions";
      this.tabPageExtensions.Padding = new System.Windows.Forms.Padding(3);
      this.tabPageExtensions.Size = new System.Drawing.Size(503, 319);
      this.tabPageExtensions.TabIndex = 2;
      this.tabPageExtensions.Text = "Extensions";
      this.tabPageExtensions.UseVisualStyleBackColor = true;
      this.tabPageExtensions.Validating += new System.ComponentModel.CancelEventHandler(this.tabPageExtensions_Validating);
      // 
      // groupBox17
      // 
      this.groupBox17.Controls.Add(this.chkUseForLastFMWebStream);
      this.groupBox17.Controls.Add(this.chkUseForWebStream);
      this.groupBox17.Controls.Add(this.chkUseForCDDA);
      this.groupBox17.Location = new System.Drawing.Point(3, 225);
      this.groupBox17.Name = "groupBox17";
      this.groupBox17.Size = new System.Drawing.Size(497, 86);
      this.groupBox17.TabIndex = 3;
      this.groupBox17.TabStop = false;
      this.groupBox17.Text = "Options";
      // 
      // chkUseForLastFMWebStream
      // 
      this.chkUseForLastFMWebStream.AutoSize = true;
      this.chkUseForLastFMWebStream.Location = new System.Drawing.Point(15, 63);
      this.chkUseForLastFMWebStream.Name = "chkUseForLastFMWebStream";
      this.chkUseForLastFMWebStream.Size = new System.Drawing.Size(154, 17);
      this.chkUseForLastFMWebStream.TabIndex = 2;
      this.chkUseForLastFMWebStream.Text = "Use player for Last.fm radio";
      this.chkUseForLastFMWebStream.UseVisualStyleBackColor = true;
      this.chkUseForLastFMWebStream.CheckedChanged += new System.EventHandler(this.chkUseForLastFMWebStream_CheckedChanged);
      // 
      // chkUseForWebStream
      // 
      this.chkUseForWebStream.AutoSize = true;
      this.chkUseForWebStream.Location = new System.Drawing.Point(15, 42);
      this.chkUseForWebStream.Name = "chkUseForWebStream";
      this.chkUseForWebStream.Size = new System.Drawing.Size(150, 17);
      this.chkUseForWebStream.TabIndex = 1;
      this.chkUseForWebStream.Text = "Use player for webstreams";
      this.chkUseForWebStream.UseVisualStyleBackColor = true;
      this.chkUseForWebStream.CheckedChanged += new System.EventHandler(this.chkUseForWebStream_CheckedChanged);
      // 
      // chkUseForCDDA
      // 
      this.chkUseForCDDA.AutoSize = true;
      this.chkUseForCDDA.Location = new System.Drawing.Point(15, 19);
      this.chkUseForCDDA.Name = "chkUseForCDDA";
      this.chkUseForCDDA.Size = new System.Drawing.Size(155, 17);
      this.chkUseForCDDA.TabIndex = 0;
      this.chkUseForCDDA.Text = "Use player for CD playback";
      this.chkUseForCDDA.UseVisualStyleBackColor = true;
      this.chkUseForCDDA.CheckedChanged += new System.EventHandler(this.chkUseForCDDA_CheckedChanged);
      // 
      // groupBox7
      // 
      this.groupBox7.Controls.Add(this.mpLabel23);
      this.groupBox7.Controls.Add(this.lvExtensions);
      this.groupBox7.Controls.Add(this.tbExtension);
      this.groupBox7.Controls.Add(this.btnDefault);
      this.groupBox7.Controls.Add(this.btnDeleteExt);
      this.groupBox7.Controls.Add(this.btnAddExt);
      this.groupBox7.Location = new System.Drawing.Point(3, 3);
      this.groupBox7.Name = "groupBox7";
      this.groupBox7.Size = new System.Drawing.Size(497, 216);
      this.groupBox7.TabIndex = 1;
      this.groupBox7.TabStop = false;
      this.groupBox7.Text = "Extensions this player will be used for";
      // 
      // mpLabel23
      // 
      this.mpLabel23.Location = new System.Drawing.Point(110, 21);
      this.mpLabel23.Name = "mpLabel23";
      this.mpLabel23.Size = new System.Drawing.Size(123, 18);
      this.mpLabel23.TabIndex = 35;
      this.mpLabel23.Text = "New extension to add:";
      // 
      // lvExtensions
      // 
      this.lvExtensions.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
      this.lvExtensions.HideSelection = false;
      this.lvExtensions.Location = new System.Drawing.Point(6, 45);
      this.lvExtensions.Name = "lvExtensions";
      this.lvExtensions.Size = new System.Drawing.Size(308, 165);
      this.lvExtensions.Sorting = System.Windows.Forms.SortOrder.Ascending;
      this.lvExtensions.TabIndex = 3;
      this.lvExtensions.UseCompatibleStateImageBehavior = false;
      this.lvExtensions.View = System.Windows.Forms.View.List;
      // 
      // tbExtension
      // 
      this.tbExtension.AcceptsReturn = true;
      this.tbExtension.Location = new System.Drawing.Point(239, 19);
      this.tbExtension.Name = "tbExtension";
      this.tbExtension.Size = new System.Drawing.Size(75, 20);
      this.tbExtension.TabIndex = 1;
      // 
      // btnDefault
      // 
      this.btnDefault.Location = new System.Drawing.Point(320, 74);
      this.btnDefault.Name = "btnDefault";
      this.btnDefault.Size = new System.Drawing.Size(75, 23);
      this.btnDefault.TabIndex = 5;
      this.btnDefault.Text = "Default";
      this.btnDefault.UseVisualStyleBackColor = true;
      this.btnDefault.Click += new System.EventHandler(this.btnDefault_Click);
      // 
      // btnDeleteExt
      // 
      this.btnDeleteExt.Location = new System.Drawing.Point(320, 45);
      this.btnDeleteExt.Name = "btnDeleteExt";
      this.btnDeleteExt.Size = new System.Drawing.Size(75, 23);
      this.btnDeleteExt.TabIndex = 4;
      this.btnDeleteExt.Text = "Delete";
      this.btnDeleteExt.UseVisualStyleBackColor = true;
      this.btnDeleteExt.Click += new System.EventHandler(this.btnDeleteExt_Click);
      // 
      // btnAddExt
      // 
      this.btnAddExt.Location = new System.Drawing.Point(320, 16);
      this.btnAddExt.Name = "btnAddExt";
      this.btnAddExt.Size = new System.Drawing.Size(75, 23);
      this.btnAddExt.TabIndex = 2;
      this.btnAddExt.Text = "Add";
      this.btnAddExt.UseVisualStyleBackColor = true;
      this.btnAddExt.Click += new System.EventHandler(this.btnAddExt_Click);
      // 
      // tabPageAdvanced
      // 
      this.tabPageAdvanced.Controls.Add(this.groupBox19);
      this.tabPageAdvanced.Controls.Add(this.groupBox18);
      this.tabPageAdvanced.Controls.Add(this.groupBox6);
      this.tabPageAdvanced.Location = new System.Drawing.Point(4, 25);
      this.tabPageAdvanced.Name = "tabPageAdvanced";
      this.tabPageAdvanced.Padding = new System.Windows.Forms.Padding(3);
      this.tabPageAdvanced.Size = new System.Drawing.Size(503, 319);
      this.tabPageAdvanced.TabIndex = 1;
      this.tabPageAdvanced.Text = "Advanced";
      this.tabPageAdvanced.UseVisualStyleBackColor = true;
      // 
      // groupBox19
      // 
      this.groupBox19.Controls.Add(this.chkUseOverSampling);
      this.groupBox19.Location = new System.Drawing.Point(3, 183);
      this.groupBox19.Name = "groupBox19";
      this.groupBox19.Size = new System.Drawing.Size(497, 128);
      this.groupBox19.TabIndex = 37;
      this.groupBox19.TabStop = false;
      this.groupBox19.Text = "Miscellaneous";
      // 
      // chkUseOverSampling
      // 
      this.chkUseOverSampling.AutoSize = true;
      this.chkUseOverSampling.Location = new System.Drawing.Point(12, 19);
      this.chkUseOverSampling.Name = "chkUseOverSampling";
      this.chkUseOverSampling.Size = new System.Drawing.Size(163, 17);
      this.chkUseOverSampling.TabIndex = 5;
      this.chkUseOverSampling.Text = "Perform 2 times oversampling";
      this.chkUseOverSampling.CheckedChanged += new System.EventHandler(this.chkUseOverSampling_CheckedChanged);
      // 
      // groupBox18
      // 
      this.groupBox18.Controls.Add(this.chkUseRGAlbumGain);
      this.groupBox18.Controls.Add(this.chkUseReplayGain);
      this.groupBox18.Location = new System.Drawing.Point(3, 99);
      this.groupBox18.Name = "groupBox18";
      this.groupBox18.Size = new System.Drawing.Size(497, 78);
      this.groupBox18.TabIndex = 36;
      this.groupBox18.TabStop = false;
      this.groupBox18.Text = "Replay Gain";
      // 
      // chkUseRGAlbumGain
      // 
      this.chkUseRGAlbumGain.AutoSize = true;
      this.chkUseRGAlbumGain.Location = new System.Drawing.Point(24, 42);
      this.chkUseRGAlbumGain.Name = "chkUseRGAlbumGain";
      this.chkUseRGAlbumGain.Size = new System.Drawing.Size(152, 17);
      this.chkUseRGAlbumGain.TabIndex = 3;
      this.chkUseRGAlbumGain.Text = "Use album gain if available";
      this.chkUseRGAlbumGain.UseVisualStyleBackColor = true;
      this.chkUseRGAlbumGain.CheckedChanged += new System.EventHandler(this.chkUseRGAlbumGain_CheckedChanged);
      // 
      // chkUseReplayGain
      // 
      this.chkUseReplayGain.AutoSize = true;
      this.chkUseReplayGain.Location = new System.Drawing.Point(12, 19);
      this.chkUseReplayGain.Name = "chkUseReplayGain";
      this.chkUseReplayGain.Size = new System.Drawing.Size(120, 17);
      this.chkUseReplayGain.TabIndex = 2;
      this.chkUseReplayGain.Text = "Enable Replay Gain";
      this.chkUseReplayGain.UseVisualStyleBackColor = true;
      this.chkUseReplayGain.CheckedChanged += new System.EventHandler(this.chkUseReplayGain_CheckedChanged);
      // 
      // groupBox6
      // 
      this.groupBox6.Controls.Add(this.textBox3);
      this.groupBox6.Controls.Add(this.mpLabel5);
      this.groupBox6.Controls.Add(this.lblPlayBackBufferSize);
      this.groupBox6.Controls.Add(this.mpLabel12);
      this.groupBox6.Controls.Add(this.trackBarPlayBackBufferSize);
      this.groupBox6.Location = new System.Drawing.Point(3, 3);
      this.groupBox6.Name = "groupBox6";
      this.groupBox6.Size = new System.Drawing.Size(497, 90);
      this.groupBox6.TabIndex = 27;
      this.groupBox6.TabStop = false;
      this.groupBox6.Text = "Playback buffer";
      // 
      // textBox3
      // 
      this.textBox3.BorderStyle = System.Windows.Forms.BorderStyle.None;
      this.textBox3.Location = new System.Drawing.Point(12, 19);
      this.textBox3.Name = "textBox3";
      this.textBox3.ReadOnly = true;
      this.textBox3.Size = new System.Drawing.Size(482, 13);
      this.textBox3.TabIndex = 28;
      this.textBox3.TabStop = false;
      this.textBox3.Text = "Increase this value if gapless playback is not working.";
      // 
      // tabPageASIO
      // 
      this.tabPageASIO.Controls.Add(this.groupBox9);
      this.tabPageASIO.Controls.Add(this.groupBox1);
      this.tabPageASIO.Location = new System.Drawing.Point(4, 25);
      this.tabPageASIO.Name = "tabPageASIO";
      this.tabPageASIO.Size = new System.Drawing.Size(503, 319);
      this.tabPageASIO.TabIndex = 6;
      this.tabPageASIO.Text = "ASIO";
      this.tabPageASIO.UseVisualStyleBackColor = true;
      // 
      // groupBox9
      // 
      this.groupBox9.Controls.Add(this.textBox2);
      this.groupBox9.Controls.Add(this.mpLabel20);
      this.groupBox9.Controls.Add(this.mpLabel19);
      this.groupBox9.Controls.Add(this.mpLabel17);
      this.groupBox9.Controls.Add(this.mpLabel1);
      this.groupBox9.Controls.Add(this.cboMaxASIORate);
      this.groupBox9.Controls.Add(this.cboMinASIORate);
      this.groupBox9.Location = new System.Drawing.Point(3, 76);
      this.groupBox9.Name = "groupBox9";
      this.groupBox9.Size = new System.Drawing.Size(497, 235);
      this.groupBox9.TabIndex = 2;
      this.groupBox9.TabStop = false;
      this.groupBox9.Text = "ASIO device supported samplingrates";
      // 
      // textBox2
      // 
      this.textBox2.BorderStyle = System.Windows.Forms.BorderStyle.None;
      this.textBox2.Location = new System.Drawing.Point(12, 19);
      this.textBox2.Multiline = true;
      this.textBox2.Name = "textBox2";
      this.textBox2.ReadOnly = true;
      this.textBox2.Size = new System.Drawing.Size(479, 32);
      this.textBox2.TabIndex = 101;
      this.textBox2.TabStop = false;
      this.textBox2.Text = "Here you can define the minimum and maximum samplingrate supported by your ASIO d" +
    "evice. Any sources beond this range are automatically resampled to the nearest s" +
    "upported rate.";
      // 
      // mpLabel20
      // 
      this.mpLabel20.Location = new System.Drawing.Point(204, 60);
      this.mpLabel20.Name = "mpLabel20";
      this.mpLabel20.Size = new System.Drawing.Size(50, 24);
      this.mpLabel20.TabIndex = 35;
      this.mpLabel20.Text = "kHz";
      // 
      // mpLabel19
      // 
      this.mpLabel19.Location = new System.Drawing.Point(204, 87);
      this.mpLabel19.Name = "mpLabel19";
      this.mpLabel19.Size = new System.Drawing.Size(50, 24);
      this.mpLabel19.TabIndex = 34;
      this.mpLabel19.Text = "kHz";
      // 
      // mpLabel17
      // 
      this.mpLabel17.Location = new System.Drawing.Point(28, 87);
      this.mpLabel17.Name = "mpLabel17";
      this.mpLabel17.Size = new System.Drawing.Size(66, 24);
      this.mpLabel17.TabIndex = 31;
      this.mpLabel17.Text = "Maximum:";
      // 
      // mpLabel1
      // 
      this.mpLabel1.Location = new System.Drawing.Point(28, 60);
      this.mpLabel1.Name = "mpLabel1";
      this.mpLabel1.Size = new System.Drawing.Size(66, 24);
      this.mpLabel1.TabIndex = 30;
      this.mpLabel1.Text = "Minimum:";
      // 
      // cboMaxASIORate
      // 
      this.cboMaxASIORate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cboMaxASIORate.FormattingEnabled = true;
      this.cboMaxASIORate.Location = new System.Drawing.Point(100, 84);
      this.cboMaxASIORate.Name = "cboMaxASIORate";
      this.cboMaxASIORate.Size = new System.Drawing.Size(98, 21);
      this.cboMaxASIORate.TabIndex = 3;
      this.cboMaxASIORate.SelectedIndexChanged += new System.EventHandler(this.cboMaxASIORate_SelectedIndexChanged);
      // 
      // cboMinASIORate
      // 
      this.cboMinASIORate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cboMinASIORate.FormattingEnabled = true;
      this.cboMinASIORate.Location = new System.Drawing.Point(100, 57);
      this.cboMinASIORate.Name = "cboMinASIORate";
      this.cboMinASIORate.Size = new System.Drawing.Size(98, 21);
      this.cboMinASIORate.TabIndex = 2;
      this.cboMinASIORate.SelectedIndexChanged += new System.EventHandler(this.cboMinASIORate_SelectedIndexChanged);
      // 
      // groupBox1
      // 
      this.groupBox1.Controls.Add(this.chkUseMaxASIOBufferSize);
      this.groupBox1.Location = new System.Drawing.Point(3, 3);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(497, 67);
      this.groupBox1.TabIndex = 1;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Advanced ASIO settings";
      // 
      // chkUseMaxASIOBufferSize
      // 
      this.chkUseMaxASIOBufferSize.AutoSize = true;
      this.chkUseMaxASIOBufferSize.Location = new System.Drawing.Point(12, 19);
      this.chkUseMaxASIOBufferSize.Name = "chkUseMaxASIOBufferSize";
      this.chkUseMaxASIOBufferSize.Size = new System.Drawing.Size(278, 17);
      this.chkUseMaxASIOBufferSize.TabIndex = 1;
      this.chkUseMaxASIOBufferSize.Text = "Always use maximum available buffer on ASIO device";
      this.chkUseMaxASIOBufferSize.UseVisualStyleBackColor = true;
      this.chkUseMaxASIOBufferSize.CheckedChanged += new System.EventHandler(this.chkUseMaxASIOBufferSize_CheckedChanged);
      // 
      // tabPageWaveOut
      // 
      this.tabPageWaveOut.Controls.Add(this.groupBox2);
      this.tabPageWaveOut.Location = new System.Drawing.Point(4, 25);
      this.tabPageWaveOut.Name = "tabPageWaveOut";
      this.tabPageWaveOut.Size = new System.Drawing.Size(503, 319);
      this.tabPageWaveOut.TabIndex = 7;
      this.tabPageWaveOut.Text = "WaveOut";
      this.tabPageWaveOut.UseVisualStyleBackColor = true;
      // 
      // groupBox2
      // 
      this.groupBox2.Controls.Add(this.textBox7);
      this.groupBox2.Controls.Add(this.mpLabel4);
      this.groupBox2.Controls.Add(this.trackBarFileBufferSize);
      this.groupBox2.Controls.Add(this.mpLabel14);
      this.groupBox2.Controls.Add(this.lblBASSPlayBackBufferSize);
      this.groupBox2.Location = new System.Drawing.Point(3, 3);
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.Size = new System.Drawing.Size(497, 308);
      this.groupBox2.TabIndex = 3;
      this.groupBox2.TabStop = false;
      this.groupBox2.Text = "Advanced WaveOut settings";
      // 
      // textBox7
      // 
      this.textBox7.BorderStyle = System.Windows.Forms.BorderStyle.None;
      this.textBox7.Location = new System.Drawing.Point(12, 19);
      this.textBox7.Multiline = true;
      this.textBox7.Name = "textBox7";
      this.textBox7.ReadOnly = true;
      this.textBox7.Size = new System.Drawing.Size(471, 20);
      this.textBox7.TabIndex = 29;
      this.textBox7.TabStop = false;
      this.textBox7.Text = "Increase this value if you experience hickups, pops or clicks in playback.";
      // 
      // tabPageVisualization
      // 
      this.tabPageVisualization.Controls.Add(this.groupBox14);
      this.tabPageVisualization.Controls.Add(this.groupBox11);
      this.tabPageVisualization.Location = new System.Drawing.Point(4, 25);
      this.tabPageVisualization.Name = "tabPageVisualization";
      this.tabPageVisualization.Size = new System.Drawing.Size(503, 319);
      this.tabPageVisualization.TabIndex = 8;
      this.tabPageVisualization.Text = "Visualizations";
      this.tabPageVisualization.UseVisualStyleBackColor = true;
      // 
      // groupBox14
      // 
      this.groupBox14.Controls.Add(this.nudWMPVizFps);
      this.groupBox14.Controls.Add(this.label2);
      this.groupBox14.Controls.Add(this.label11);
      this.groupBox14.Controls.Add(this.label10);
      this.groupBox14.Controls.Add(this.cboWMPVisualizationPresets);
      this.groupBox14.Controls.Add(this.cboWMPVisualizations);
      this.groupBox14.Location = new System.Drawing.Point(3, 3);
      this.groupBox14.Name = "groupBox14";
      this.groupBox14.Size = new System.Drawing.Size(497, 134);
      this.groupBox14.TabIndex = 1;
      this.groupBox14.TabStop = false;
      this.groupBox14.Text = "Visualization selection";
      // 
      // nudWMPVizFps
      // 
      this.nudWMPVizFps.Location = new System.Drawing.Point(83, 77);
      this.nudWMPVizFps.Maximum = new decimal(new int[] {
            75,
            0,
            0,
            0});
      this.nudWMPVizFps.Minimum = new decimal(new int[] {
            20,
            0,
            0,
            0});
      this.nudWMPVizFps.Name = "nudWMPVizFps";
      this.nudWMPVizFps.Size = new System.Drawing.Size(62, 20);
      this.nudWMPVizFps.TabIndex = 3;
      this.nudWMPVizFps.Value = new decimal(new int[] {
            25,
            0,
            0,
            0});
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(9, 79);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(64, 13);
      this.label2.TabIndex = 38;
      this.label2.Text = "Target FPS:";
      // 
      // label11
      // 
      this.label11.AutoSize = true;
      this.label11.Location = new System.Drawing.Point(9, 52);
      this.label11.Name = "label11";
      this.label11.Size = new System.Drawing.Size(40, 13);
      this.label11.TabIndex = 37;
      this.label11.Text = "Preset:";
      // 
      // label10
      // 
      this.label10.AutoSize = true;
      this.label10.Location = new System.Drawing.Point(9, 25);
      this.label10.Name = "label10";
      this.label10.Size = new System.Drawing.Size(68, 13);
      this.label10.TabIndex = 35;
      this.label10.Text = "Visualization:";
      // 
      // cboWMPVisualizationPresets
      // 
      this.cboWMPVisualizationPresets.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cboWMPVisualizationPresets.FormattingEnabled = true;
      this.cboWMPVisualizationPresets.Location = new System.Drawing.Point(83, 49);
      this.cboWMPVisualizationPresets.Name = "cboWMPVisualizationPresets";
      this.cboWMPVisualizationPresets.Size = new System.Drawing.Size(289, 21);
      this.cboWMPVisualizationPresets.TabIndex = 2;
      this.cboWMPVisualizationPresets.SelectedIndexChanged += new System.EventHandler(this.cboWMPVisualizationPresets_SelectedIndexChanged);
      // 
      // cboWMPVisualizations
      // 
      this.cboWMPVisualizations.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cboWMPVisualizations.FormattingEnabled = true;
      this.cboWMPVisualizations.Location = new System.Drawing.Point(83, 22);
      this.cboWMPVisualizations.Name = "cboWMPVisualizations";
      this.cboWMPVisualizations.Size = new System.Drawing.Size(289, 21);
      this.cboWMPVisualizations.TabIndex = 1;
      this.cboWMPVisualizations.SelectedIndexChanged += new System.EventHandler(this.cboWMPVisualizations_SelectedIndexChanged);
      // 
      // groupBox11
      // 
      this.groupBox11.Controls.Add(this.textBox1);
      this.groupBox11.Controls.Add(this.chkUseVizAGC);
      this.groupBox11.Controls.Add(this.textBox6);
      this.groupBox11.Controls.Add(this.mpLabel9);
      this.groupBox11.Controls.Add(this.trackBarVizLatency);
      this.groupBox11.Controls.Add(this.mpLabel27);
      this.groupBox11.Controls.Add(this.lblVizLatency);
      this.groupBox11.Location = new System.Drawing.Point(3, 143);
      this.groupBox11.Name = "groupBox11";
      this.groupBox11.Size = new System.Drawing.Size(497, 168);
      this.groupBox11.TabIndex = 2;
      this.groupBox11.TabStop = false;
      this.groupBox11.Text = "Settings";
      // 
      // textBox1
      // 
      this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
      this.textBox1.Location = new System.Drawing.Point(12, 106);
      this.textBox1.Multiline = true;
      this.textBox1.Name = "textBox1";
      this.textBox1.ReadOnly = true;
      this.textBox1.Size = new System.Drawing.Size(478, 20);
      this.textBox1.TabIndex = 37;
      this.textBox1.TabStop = false;
      this.textBox1.Text = "Automatic Gain Control amplifies lower volumes to always get maximum visualizatio" +
    "n effects.\r\n";
      // 
      // chkUseVizAGC
      // 
      this.chkUseVizAGC.AutoSize = true;
      this.chkUseVizAGC.Location = new System.Drawing.Point(24, 132);
      this.chkUseVizAGC.Name = "chkUseVizAGC";
      this.chkUseVizAGC.Size = new System.Drawing.Size(170, 17);
      this.chkUseVizAGC.TabIndex = 5;
      this.chkUseVizAGC.Text = "Enable Automatic Gain Control";
      this.chkUseVizAGC.UseVisualStyleBackColor = true;
      this.chkUseVizAGC.CheckedChanged += new System.EventHandler(this.chkUseVizAGC_CheckedChanged);
      // 
      // textBox6
      // 
      this.textBox6.BorderStyle = System.Windows.Forms.BorderStyle.None;
      this.textBox6.Location = new System.Drawing.Point(12, 19);
      this.textBox6.Multiline = true;
      this.textBox6.Name = "textBox6";
      this.textBox6.ReadOnly = true;
      this.textBox6.Size = new System.Drawing.Size(478, 33);
      this.textBox6.TabIndex = 35;
      this.textBox6.TabStop = false;
      this.textBox6.Text = "If visualization is lagging behind sound, decrease this value. If sound is laggin" +
    "g behind visualization, increase this value.";
      // 
      // mpLabel9
      // 
      this.mpLabel9.Location = new System.Drawing.Point(21, 60);
      this.mpLabel9.Name = "mpLabel9";
      this.mpLabel9.Size = new System.Drawing.Size(112, 24);
      this.mpLabel9.TabIndex = 29;
      this.mpLabel9.Text = "Latency correction:";
      // 
      // trackBarVizLatency
      // 
      this.trackBarVizLatency.LargeChange = 1;
      this.trackBarVizLatency.Location = new System.Drawing.Point(148, 55);
      this.trackBarVizLatency.Maximum = 40;
      this.trackBarVizLatency.Name = "trackBarVizLatency";
      this.trackBarVizLatency.Size = new System.Drawing.Size(219, 45);
      this.trackBarVizLatency.TabIndex = 4;
      this.trackBarVizLatency.Value = 20;
      this.trackBarVizLatency.ValueChanged += new System.EventHandler(this.trackBarVizLatency_ValueChanged);
      // 
      // mpLabel27
      // 
      this.mpLabel27.Location = new System.Drawing.Point(416, 60);
      this.mpLabel27.Name = "mpLabel27";
      this.mpLabel27.Size = new System.Drawing.Size(68, 18);
      this.mpLabel27.TabIndex = 31;
      this.mpLabel27.Text = "milliseconds";
      // 
      // lblVizLatency
      // 
      this.lblVizLatency.Location = new System.Drawing.Point(373, 60);
      this.lblVizLatency.Name = "lblVizLatency";
      this.lblVizLatency.Size = new System.Drawing.Size(37, 21);
      this.lblVizLatency.TabIndex = 30;
      this.lblVizLatency.Text = "0";
      this.lblVizLatency.TextAlign = System.Drawing.ContentAlignment.TopRight;
      // 
      // tabPageWMPViz
      // 
      this.tabPageWMPViz.Controls.Add(this.groupBox12);
      this.tabPageWMPViz.Location = new System.Drawing.Point(4, 25);
      this.tabPageWMPViz.Name = "tabPageWMPViz";
      this.tabPageWMPViz.Size = new System.Drawing.Size(503, 319);
      this.tabPageWMPViz.TabIndex = 9;
      this.tabPageWMPViz.Text = "Windows Media Player";
      this.tabPageWMPViz.UseVisualStyleBackColor = true;
      // 
      // groupBox12
      // 
      this.groupBox12.Controls.Add(this.textBox9);
      this.groupBox12.Controls.Add(this.textBox8);
      this.groupBox12.Controls.Add(this.chkWMPVizFFTHalf);
      this.groupBox12.Controls.Add(this.mpLabel31);
      this.groupBox12.Controls.Add(this.lblWMPVizFFTMinimum);
      this.groupBox12.Controls.Add(this.trackBarWMPVizFFTMinimum);
      this.groupBox12.Controls.Add(this.mpLabel33);
      this.groupBox12.Controls.Add(this.mpLabel29);
      this.groupBox12.Controls.Add(this.lblWMPVizFFTFallBack);
      this.groupBox12.Controls.Add(this.trackBarWMPVizFFTFallBack);
      this.groupBox12.Controls.Add(this.mpLabel30);
      this.groupBox12.Location = new System.Drawing.Point(3, 3);
      this.groupBox12.Name = "groupBox12";
      this.groupBox12.Size = new System.Drawing.Size(497, 308);
      this.groupBox12.TabIndex = 1;
      this.groupBox12.TabStop = false;
      this.groupBox12.Text = "Settings for Windows Media Player visualizations";
      // 
      // textBox9
      // 
      this.textBox9.BorderStyle = System.Windows.Forms.BorderStyle.None;
      this.textBox9.Location = new System.Drawing.Point(12, 91);
      this.textBox9.Multiline = true;
      this.textBox9.Name = "textBox9";
      this.textBox9.ReadOnly = true;
      this.textBox9.Size = new System.Drawing.Size(479, 30);
      this.textBox9.TabIndex = 45;
      this.textBox9.TabStop = false;
      this.textBox9.Text = "The spectrum fallback is the time it takes for a value in the frequence spectrum " +
    "to fall back from 0 dB to whatever is set as minimum.";
      // 
      // textBox8
      // 
      this.textBox8.BorderStyle = System.Windows.Forms.BorderStyle.None;
      this.textBox8.Location = new System.Drawing.Point(12, 19);
      this.textBox8.Multiline = true;
      this.textBox8.Name = "textBox8";
      this.textBox8.ReadOnly = true;
      this.textBox8.Size = new System.Drawing.Size(479, 20);
      this.textBox8.TabIndex = 44;
      this.textBox8.TabStop = false;
      this.textBox8.Text = "The spectrum minimum is the minimum for frequency spectrum values to become visib" +
    "le.";
      // 
      // chkWMPVizFFTHalf
      // 
      this.chkWMPVizFFTHalf.AutoSize = true;
      this.chkWMPVizFFTHalf.Location = new System.Drawing.Point(12, 178);
      this.chkWMPVizFFTHalf.Name = "chkWMPVizFFTHalf";
      this.chkWMPVizFFTHalf.Size = new System.Drawing.Size(292, 17);
      this.chkWMPVizFFTHalf.TabIndex = 43;
      this.chkWMPVizFFTHalf.Text = "Use only first half of frequency spectrum (till 11 / 12 kHz)";
      this.chkWMPVizFFTHalf.UseVisualStyleBackColor = true;
      this.chkWMPVizFFTHalf.CheckedChanged += new System.EventHandler(this.chkWMPVizFFTHalf_CheckedChanged);
      // 
      // mpLabel31
      // 
      this.mpLabel31.Location = new System.Drawing.Point(20, 45);
      this.mpLabel31.Name = "mpLabel31";
      this.mpLabel31.Size = new System.Drawing.Size(112, 24);
      this.mpLabel31.TabIndex = 39;
      this.mpLabel31.Text = "Spectrum minimum:";
      // 
      // lblWMPVizFFTMinimum
      // 
      this.lblWMPVizFFTMinimum.Location = new System.Drawing.Point(372, 45);
      this.lblWMPVizFFTMinimum.Name = "lblWMPVizFFTMinimum";
      this.lblWMPVizFFTMinimum.Size = new System.Drawing.Size(37, 21);
      this.lblWMPVizFFTMinimum.TabIndex = 40;
      this.lblWMPVizFFTMinimum.Text = "0";
      this.lblWMPVizFFTMinimum.TextAlign = System.Drawing.ContentAlignment.TopRight;
      // 
      // trackBarWMPVizFFTMinimum
      // 
      this.trackBarWMPVizFFTMinimum.LargeChange = 1;
      this.trackBarWMPVizFFTMinimum.Location = new System.Drawing.Point(147, 40);
      this.trackBarWMPVizFFTMinimum.Maximum = 20;
      this.trackBarWMPVizFFTMinimum.Name = "trackBarWMPVizFFTMinimum";
      this.trackBarWMPVizFFTMinimum.Size = new System.Drawing.Size(219, 45);
      this.trackBarWMPVizFFTMinimum.TabIndex = 1;
      this.trackBarWMPVizFFTMinimum.Value = 12;
      this.trackBarWMPVizFFTMinimum.ValueChanged += new System.EventHandler(this.trackBarWMPVizFFTMinimum_ValueChanged);
      // 
      // mpLabel33
      // 
      this.mpLabel33.Location = new System.Drawing.Point(415, 45);
      this.mpLabel33.Name = "mpLabel33";
      this.mpLabel33.Size = new System.Drawing.Size(68, 18);
      this.mpLabel33.TabIndex = 41;
      this.mpLabel33.Text = "dB";
      // 
      // mpLabel29
      // 
      this.mpLabel29.Location = new System.Drawing.Point(20, 132);
      this.mpLabel29.Name = "mpLabel29";
      this.mpLabel29.Size = new System.Drawing.Size(112, 24);
      this.mpLabel29.TabIndex = 34;
      this.mpLabel29.Text = "Spectrum fallback:";
      // 
      // lblWMPVizFFTFallBack
      // 
      this.lblWMPVizFFTFallBack.Location = new System.Drawing.Point(372, 132);
      this.lblWMPVizFFTFallBack.Name = "lblWMPVizFFTFallBack";
      this.lblWMPVizFFTFallBack.Size = new System.Drawing.Size(37, 21);
      this.lblWMPVizFFTFallBack.TabIndex = 35;
      this.lblWMPVizFFTFallBack.Text = "0";
      this.lblWMPVizFFTFallBack.TextAlign = System.Drawing.ContentAlignment.TopRight;
      // 
      // trackBarWMPVizFFTFallBack
      // 
      this.trackBarWMPVizFFTFallBack.LargeChange = 1;
      this.trackBarWMPVizFFTFallBack.Location = new System.Drawing.Point(147, 127);
      this.trackBarWMPVizFFTFallBack.Maximum = 20;
      this.trackBarWMPVizFFTFallBack.Name = "trackBarWMPVizFFTFallBack";
      this.trackBarWMPVizFFTFallBack.Size = new System.Drawing.Size(219, 45);
      this.trackBarWMPVizFFTFallBack.TabIndex = 2;
      this.trackBarWMPVizFFTFallBack.Value = 12;
      this.trackBarWMPVizFFTFallBack.ValueChanged += new System.EventHandler(this.trackBarWMPVizFFTFallBack_ValueChanged);
      // 
      // mpLabel30
      // 
      this.mpLabel30.Location = new System.Drawing.Point(415, 132);
      this.mpLabel30.Name = "mpLabel30";
      this.mpLabel30.Size = new System.Drawing.Size(68, 18);
      this.mpLabel30.TabIndex = 36;
      this.mpLabel30.Text = "milliseconds";
      // 
      // tabPageDSP
      // 
      this.tabPageDSP.Controls.Add(this.groupBox16);
      this.tabPageDSP.Location = new System.Drawing.Point(4, 25);
      this.tabPageDSP.Name = "tabPageDSP";
      this.tabPageDSP.Size = new System.Drawing.Size(503, 319);
      this.tabPageDSP.TabIndex = 11;
      this.tabPageDSP.Text = "DSP";
      this.tabPageDSP.UseVisualStyleBackColor = true;
      // 
      // groupBox16
      // 
      this.groupBox16.Controls.Add(this.textBox4);
      this.groupBox16.Location = new System.Drawing.Point(3, 3);
      this.groupBox16.Name = "groupBox16";
      this.groupBox16.Size = new System.Drawing.Size(497, 308);
      this.groupBox16.TabIndex = 0;
      this.groupBox16.TabStop = false;
      this.groupBox16.Text = "DSP selection";
      // 
      // textBox4
      // 
      this.textBox4.BorderStyle = System.Windows.Forms.BorderStyle.None;
      this.textBox4.Location = new System.Drawing.Point(12, 19);
      this.textBox4.Multiline = true;
      this.textBox4.Name = "textBox4";
      this.textBox4.ReadOnly = true;
      this.textBox4.Size = new System.Drawing.Size(470, 40);
      this.textBox4.TabIndex = 35;
      this.textBox4.TabStop = false;
      this.textBox4.Text = "The DSP\'s to use can be selected and configured in the \"Music/Music DSP\" section " +
    "of MediaPortal setup. Supported are VST plugins and WinAmp DSP plugins.";
      // 
      // tabPageWASAPI
      // 
      this.tabPageWASAPI.Controls.Add(this.groupBox21);
      this.tabPageWASAPI.Controls.Add(this.groupBox13);
      this.tabPageWASAPI.Location = new System.Drawing.Point(4, 25);
      this.tabPageWASAPI.Name = "tabPageWASAPI";
      this.tabPageWASAPI.Size = new System.Drawing.Size(503, 319);
      this.tabPageWASAPI.TabIndex = 12;
      this.tabPageWASAPI.Text = "WASAPI";
      this.tabPageWASAPI.UseVisualStyleBackColor = true;
      // 
      // groupBox21
      // 
      this.groupBox21.Controls.Add(this.chkWASAPIEvent);
      this.groupBox21.Location = new System.Drawing.Point(3, 5);
      this.groupBox21.Name = "groupBox21";
      this.groupBox21.Size = new System.Drawing.Size(497, 160);
      this.groupBox21.TabIndex = 4;
      this.groupBox21.TabStop = false;
      this.groupBox21.Text = "Advanced WASAPI settings";
      // 
      // chkWASAPIEvent
      // 
      this.chkWASAPIEvent.AutoSize = true;
      this.chkWASAPIEvent.Location = new System.Drawing.Point(12, 19);
      this.chkWASAPIEvent.Name = "chkWASAPIEvent";
      this.chkWASAPIEvent.Size = new System.Drawing.Size(216, 17);
      this.chkWASAPIEvent.TabIndex = 1;
      this.chkWASAPIEvent.Text = "Enable event-driven buffering (preferred)";
      this.chkWASAPIEvent.UseVisualStyleBackColor = true;
      this.chkWASAPIEvent.CheckedChanged += new System.EventHandler(this.chkWASAPIEvent_CheckedChanged);
      // 
      // groupBox13
      // 
      this.groupBox13.Controls.Add(this.textBox5);
      this.groupBox13.Controls.Add(this.mpLabel21);
      this.groupBox13.Controls.Add(this.mpLabel22);
      this.groupBox13.Controls.Add(this.mpLabel24);
      this.groupBox13.Controls.Add(this.mpLabel26);
      this.groupBox13.Controls.Add(this.cboMaxWASAPIRate);
      this.groupBox13.Controls.Add(this.cboMinWASAPIRate);
      this.groupBox13.Location = new System.Drawing.Point(3, 171);
      this.groupBox13.Name = "groupBox13";
      this.groupBox13.Size = new System.Drawing.Size(497, 140);
      this.groupBox13.TabIndex = 3;
      this.groupBox13.TabStop = false;
      this.groupBox13.Text = "WASAPI exclusive mode supported samplingrates";
      // 
      // textBox5
      // 
      this.textBox5.BorderStyle = System.Windows.Forms.BorderStyle.None;
      this.textBox5.Location = new System.Drawing.Point(12, 19);
      this.textBox5.Multiline = true;
      this.textBox5.Name = "textBox5";
      this.textBox5.ReadOnly = true;
      this.textBox5.Size = new System.Drawing.Size(479, 43);
      this.textBox5.TabIndex = 101;
      this.textBox5.TabStop = false;
      this.textBox5.Text = "Here you can define the minimum and maximum samplingrate supported by your WASAPI" +
    " device in exclusive mode. Any sources beond this range are automatically resamp" +
    "led to the nearest supported rate.";
      // 
      // mpLabel21
      // 
      this.mpLabel21.Location = new System.Drawing.Point(201, 71);
      this.mpLabel21.Name = "mpLabel21";
      this.mpLabel21.Size = new System.Drawing.Size(50, 24);
      this.mpLabel21.TabIndex = 35;
      this.mpLabel21.Text = "kHz";
      // 
      // mpLabel22
      // 
      this.mpLabel22.Location = new System.Drawing.Point(201, 98);
      this.mpLabel22.Name = "mpLabel22";
      this.mpLabel22.Size = new System.Drawing.Size(50, 24);
      this.mpLabel22.TabIndex = 34;
      this.mpLabel22.Text = "kHz";
      // 
      // mpLabel24
      // 
      this.mpLabel24.Location = new System.Drawing.Point(25, 98);
      this.mpLabel24.Name = "mpLabel24";
      this.mpLabel24.Size = new System.Drawing.Size(66, 24);
      this.mpLabel24.TabIndex = 31;
      this.mpLabel24.Text = "Maximum:";
      // 
      // mpLabel26
      // 
      this.mpLabel26.Location = new System.Drawing.Point(25, 71);
      this.mpLabel26.Name = "mpLabel26";
      this.mpLabel26.Size = new System.Drawing.Size(66, 24);
      this.mpLabel26.TabIndex = 30;
      this.mpLabel26.Text = "Minimum:";
      // 
      // cboMaxWASAPIRate
      // 
      this.cboMaxWASAPIRate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cboMaxWASAPIRate.FormattingEnabled = true;
      this.cboMaxWASAPIRate.Location = new System.Drawing.Point(97, 95);
      this.cboMaxWASAPIRate.Name = "cboMaxWASAPIRate";
      this.cboMaxWASAPIRate.Size = new System.Drawing.Size(98, 21);
      this.cboMaxWASAPIRate.TabIndex = 3;
      this.cboMaxWASAPIRate.SelectedIndexChanged += new System.EventHandler(this.cboMaxWASAPIRate_SelectedIndexChanged);
      // 
      // cboMinWASAPIRate
      // 
      this.cboMinWASAPIRate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cboMinWASAPIRate.FormattingEnabled = true;
      this.cboMinWASAPIRate.Location = new System.Drawing.Point(97, 68);
      this.cboMinWASAPIRate.Name = "cboMinWASAPIRate";
      this.cboMinWASAPIRate.Size = new System.Drawing.Size(98, 21);
      this.cboMinWASAPIRate.TabIndex = 2;
      this.cboMinWASAPIRate.SelectedIndexChanged += new System.EventHandler(this.cboMinWASAPIRate_SelectedIndexChanged);
      // 
      // tvwMenu
      // 
      this.tvwMenu.HideSelection = false;
      this.tvwMenu.Location = new System.Drawing.Point(13, 13);
      this.tvwMenu.Name = "tvwMenu";
      treeNode23.Name = "NodeDevice";
      treeNode23.Text = "Output Device";
      treeNode24.Name = "NodeUpmixing";
      treeNode24.Text = "Upmixing";
      treeNode25.Name = "NodeExtensions";
      treeNode25.Text = "Extensions";
      treeNode26.Name = "NodeGeneral";
      treeNode26.Text = "General Settings";
      treeNode27.Name = "NodeVisualizations";
      treeNode27.Text = "Visualizations";
      treeNode28.Name = "NodeDSP";
      treeNode28.Text = "DSP\'s";
      treeNode29.Name = "NodeASIO";
      treeNode29.Text = "ASIO";
      treeNode30.Name = "NodeWaveOut";
      treeNode30.Text = "WaveOut";
      treeNode31.Name = "NodeWASAPI";
      treeNode31.Text = "WASAPI";
      treeNode32.Name = "NodeAdvanced";
      treeNode32.Text = "Advanced Settings";
      treeNode33.Name = "NodeRoot";
      treeNode33.Text = "PureAudio Plugin";
      this.tvwMenu.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode33});
      this.tvwMenu.Size = new System.Drawing.Size(185, 336);
      this.tvwMenu.TabIndex = 1;
      this.tvwMenu.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvwMenu_AfterSelect);
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
      // groupBox22
      // 
      this.groupBox22.Controls.Add(this.cboFiveDotZeroUpMix);
      this.groupBox22.Controls.Add(this.mpLabel28);
      this.groupBox22.Location = new System.Drawing.Point(3, 186);
      this.groupBox22.Name = "groupBox22";
      this.groupBox22.Size = new System.Drawing.Size(497, 54);
      this.groupBox22.TabIndex = 15;
      this.groupBox22.TabStop = false;
      this.groupBox22.Text = "Upmixing for 5.0 sources";
      // 
      // cboFiveDotZeroUpMix
      // 
      this.cboFiveDotZeroUpMix.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cboFiveDotZeroUpMix.FormattingEnabled = true;
      this.cboFiveDotZeroUpMix.Location = new System.Drawing.Point(92, 19);
      this.cboFiveDotZeroUpMix.Name = "cboFiveDotZeroUpMix";
      this.cboFiveDotZeroUpMix.Size = new System.Drawing.Size(215, 21);
      this.cboFiveDotZeroUpMix.TabIndex = 4;
      this.cboFiveDotZeroUpMix.SelectedIndexChanged += new System.EventHandler(this.cboFiveDotZeroUpMix_SelectedIndexChanged);
      // 
      // mpLabel28
      // 
      this.mpLabel28.Location = new System.Drawing.Point(6, 23);
      this.mpLabel28.Name = "mpLabel28";
      this.mpLabel28.Size = new System.Drawing.Size(80, 18);
      this.mpLabel28.TabIndex = 14;
      this.mpLabel28.Text = "Upmix to:";
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
      this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ConfigurationForm_FormClosed);
      this.Load += new System.EventHandler(this.ConfigurationForm_Load);
      ((System.ComponentModel.ISupportInitialize)(this.trackBarFileBufferSize)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.trackBarPlayBackBufferSize)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.trackBarSeekIncrement)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.trackBarSoftStopDuration)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.trackBarGapLength)).EndInit();
      this.tabControl.ResumeLayout(false);
      this.tabPageAbout.ResumeLayout(false);
      this.groupBox8.ResumeLayout(false);
      this.groupBox8.PerformLayout();
      this.tabPageDevice.ResumeLayout(false);
      this.groupBox3.ResumeLayout(false);
      this.pnlDevice.ResumeLayout(false);
      this.pnlDevice.PerformLayout();
      this.tabPageGeneral.ResumeLayout(false);
      this.groupBox4.ResumeLayout(false);
      this.groupBox4.PerformLayout();
      this.pnlSoftStopDuration.ResumeLayout(false);
      this.pnlSoftStopDuration.PerformLayout();
      this.tabPageUpmixing.ResumeLayout(false);
      this.groupBox20.ResumeLayout(false);
      this.groupBox15.ResumeLayout(false);
      this.groupBox10.ResumeLayout(false);
      this.groupBox5.ResumeLayout(false);
      this.tabPageExtensions.ResumeLayout(false);
      this.groupBox17.ResumeLayout(false);
      this.groupBox17.PerformLayout();
      this.groupBox7.ResumeLayout(false);
      this.groupBox7.PerformLayout();
      this.tabPageAdvanced.ResumeLayout(false);
      this.groupBox19.ResumeLayout(false);
      this.groupBox19.PerformLayout();
      this.groupBox18.ResumeLayout(false);
      this.groupBox18.PerformLayout();
      this.groupBox6.ResumeLayout(false);
      this.groupBox6.PerformLayout();
      this.tabPageASIO.ResumeLayout(false);
      this.groupBox9.ResumeLayout(false);
      this.groupBox9.PerformLayout();
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.tabPageWaveOut.ResumeLayout(false);
      this.groupBox2.ResumeLayout(false);
      this.groupBox2.PerformLayout();
      this.tabPageVisualization.ResumeLayout(false);
      this.groupBox14.ResumeLayout(false);
      this.groupBox14.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.nudWMPVizFps)).EndInit();
      this.groupBox11.ResumeLayout(false);
      this.groupBox11.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.trackBarVizLatency)).EndInit();
      this.tabPageWMPViz.ResumeLayout(false);
      this.groupBox12.ResumeLayout(false);
      this.groupBox12.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.trackBarWMPVizFFTMinimum)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.trackBarWMPVizFFTFallBack)).EndInit();
      this.tabPageDSP.ResumeLayout(false);
      this.groupBox16.ResumeLayout(false);
      this.groupBox16.PerformLayout();
      this.tabPageWASAPI.ResumeLayout(false);
      this.groupBox21.ResumeLayout(false);
      this.groupBox21.PerformLayout();
      this.groupBox13.ResumeLayout(false);
      this.groupBox13.PerformLayout();
      this.groupBox22.ResumeLayout(false);
      this.ResumeLayout(false);

    }
    #endregion

    private void ConfigurationForm_Load(object sender, System.EventArgs e)
    {
      _Profile.LoadSettings();

      tabControl.Region =
        new Region(
        new RectangleF(
        tabPageAbout.Left,
        tabPageAbout.Top,
        tabPageAbout.Width,
        tabPageAbout.Height));

      tvwMenu.ExpandAll();

      foreach (InstalledDriver driver in AsioDriver.InstalledDrivers)
      {
        cboASIODevice.Items.Add(driver.Name);
      }

      BASS_WASAPI_DEVICEINFO[] wasapiDevices = BassWasapi.BASS_WASAPI_GetDeviceInfos();
      for (int i = 0; i < wasapiDevices.Length; i++)
      {
        if (wasapiDevices[i].IsEnabled && !wasapiDevices[i].IsInput)
          cboWASAPIDevice.Items.Add(wasapiDevices[i].name);
      }

      cboSoundDevice.Items.Add(ConfigProfile.Defaults.DirectSoundDevice);
      BASS_DEVICEINFO[] soundDevices = Bass.BASS_GetDeviceInfos();
      for (int i = 0; i < soundDevices.Length; i++)
      {
        // Ignore the 'no sound' device
        if (i != 0)
          cboSoundDevice.Items.Add(soundDevices[i].name);
      }

      // Should match MediaPortal.Player.PureAudio.SpeakerLayout enum.
      cboWASAPISpeakerLayout.Items.Add("Mono");
      cboWASAPISpeakerLayout.Items.Add("Stereo");
      cboWASAPISpeakerLayout.Items.Add("Quadraphonic");
      cboWASAPISpeakerLayout.Items.Add("5.1 Surround");
      cboWASAPISpeakerLayout.Items.Add("7.1 Surround");

      // Should match MediaPortal.Player.PureAudio.PlayBackMode enum
      cboDefaultPlayBackMode.Items.Add("Normal");
      cboDefaultPlayBackMode.Items.Add("Gapless");

      // Should match MediaPortal.Player.PureAudio.MonoUpMix enum.
      cboMonoUpMix.Items.Add("No upmixing");
      cboMonoUpMix.Items.Add("Stereo");
      cboMonoUpMix.Items.Add("QuadraphonicPhonic");
      cboMonoUpMix.Items.Add("5.1 Surround");
      cboMonoUpMix.Items.Add("7.1 Surround");

      // Should match MediaPortal.Player.PureAudio.StereoUpMix enum.
      cboStereoUpMix.Items.Add("No upmixing");
      cboStereoUpMix.Items.Add("QuadraphonicPhonic");
      cboStereoUpMix.Items.Add("5.1 Surround");
      cboStereoUpMix.Items.Add("7.1 Surround");

      // Should match MediaPortal.Player.PureAudio.FiveDotOneUpMix enum.
      cboFiveDotZeroUpMix.Items.Add("No upmixing");
      cboFiveDotZeroUpMix.Items.Add("5.1 Surround");
      cboFiveDotZeroUpMix.Items.Add("7.1 Surround");

      // Should match MediaPortal.Player.PureAudio.FiveDotOneUpMix enum.
      cboFiveDotOneUpMix.Items.Add("No upmixing");
      cboFiveDotOneUpMix.Items.Add("7.1 Surround");

      // Should match MediaPortal.Player.PureAudio.QuadraphonicUpMixMode enum.
      cboQuadraphonicUpMix.Items.Add("No upmixing");
      cboQuadraphonicUpMix.Items.Add("5.1 Surround");
      cboQuadraphonicUpMix.Items.Add("7.1 Surround");

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
      cboMinASIORate.Items.AddRange(rates);
      cboMinWASAPIRate.Items.AddRange(rates);

      rates = new object[]{
					"Auto",
					"44100",
					"48000",
					"88200",
					"96000",
					"176400",
					"192000"
				};
      cboMaxASIORate.Items.AddRange(rates);
      cboMaxWASAPIRate.Items.AddRange(rates);

      rbUseASIO.Checked = (_Profile.OutputMode == OutputMode.ASIO);
      rbUseSound.Checked = (_Profile.OutputMode == OutputMode.DirectSound);
      rbUseWASAPI.Checked = (_Profile.OutputMode == OutputMode.WASAPI);

      cboWASAPISpeakerLayout.SelectedIndex = (int)_Profile.WASAPISpeakerLayout;
      cboDefaultPlayBackMode.SelectedIndex = (int)_Profile.DefaultPlayBackMode;
      cboMonoUpMix.SelectedIndex = (int)_Profile.MonoUpMix;
      cboStereoUpMix.SelectedIndex = (int)_Profile.StereoUpMix;
      cboQuadraphonicUpMix.SelectedIndex = (int)_Profile.QuadraphonicUpMix;
      cboFiveDotZeroUpMix.SelectedIndex = (int)_Profile.FiveDotZeroUpMix;
      cboFiveDotOneUpMix.SelectedIndex = (int)_Profile.FiveDotOneUpMix;

      if (_Profile.ForceMaxASIORate == 0)
        cboMaxASIORate.SelectedItem = "Auto";
      else
        cboMaxASIORate.SelectedItem = _Profile.ForceMaxASIORate.ToString();

      if (_Profile.ForceMinASIORate == 0)
        cboMinASIORate.SelectedItem = "Auto";
      else
        cboMinASIORate.SelectedItem = _Profile.ForceMinASIORate.ToString();


      if (_Profile.ForceMaxWASAPIRate == 0)
        cboMaxWASAPIRate.SelectedItem = "Auto";
      else
        cboMaxWASAPIRate.SelectedItem = _Profile.ForceMaxWASAPIRate.ToString();

      if (_Profile.ForceMinWASAPIRate == 0)
        cboMinWASAPIRate.SelectedItem = "Auto";
      else
        cboMinWASAPIRate.SelectedItem = _Profile.ForceMinWASAPIRate.ToString();

      cboASIODevice.SelectedItem = _Profile.ASIODevice;
      cboSoundDevice.SelectedItem = _Profile.DirectSoundDevice;
      cboWASAPIDevice.SelectedItem = _Profile.WASAPIDevice;


      nudWMPVizFps.DataBindings.Add("Value", _Profile, "WMPVizFps");

      chkWASAPIEvent.Checked = _Profile.WASAPIEvent;
      chkWASAPIExclusive.Checked = _Profile.WASAPIExclusive;
      chkDoSoftStop.Checked = _Profile.DoSoftStop;
      chkUseMaxASIOBufferSize.Checked = _Profile.UseMaxASIOBufferSize;
      chkUseOverSampling.Checked = _Profile.UseOverSampling;
      chkWMPVizFFTHalf.Checked = _Profile.WMPVizFFTHalf;
      chkUseVizAGC.Checked = _Profile.UseVizAGC;
      chkUseForCDDA.Checked = _Profile.UseForCDDA;
      chkUseForWebStream.Checked = _Profile.UseForWebStream;
      chkUseForLastFMWebStream.Checked = _Profile.UseForLastFMWebStream;
      chkUseReplayGain.Checked = _Profile.UseReplayGain;
      chkUseRGAlbumGain.Checked = _Profile.UseRGAlbumGain;

      chkUseRGAlbumGain.Enabled = _Profile.UseReplayGain;

      trackBarSeekIncrement.Value = _Profile.SeekIncrement / 5;
      trackBarSoftStopDuration.Value = _Profile.SoftStopDuration / 100;
      trackBarGapLength.Value = _Profile.GapLength / 250;
      trackBarPlayBackBufferSize.Value = _Profile.PlayBackBufferSize / 250;
      trackBarFileBufferSize.Value = _Profile.BASSPlayBackBufferSize / 100;
      trackBarVizLatency.Value = _Profile.VizLatencyCorrection / 25 + 20;
      trackBarWMPVizFFTFallBack.Value = _Profile.WMPVizFFTFallBack / 50;
      trackBarWMPVizFFTMinimum.Value = _Profile.WMPVizFFTMinimum / -5;

      lblSeekIncrement.Text = _Profile.SeekIncrement.ToString();
      lblSoftStopDuration.Text = _Profile.SoftStopDuration.ToString();
      lblGapLength.Text = _Profile.GapLength.ToString();
      lblPlayBackBufferSize.Text = _Profile.PlayBackBufferSize.ToString();
      lblBASSPlayBackBufferSize.Text = _Profile.BASSPlayBackBufferSize.ToString();
      lblVizLatency.Text = _Profile.VizLatencyCorrection.ToString();
      lblWMPVizFFTFallBack.Text = _Profile.WMPVizFFTFallBack.ToString();
      lblWMPVizFFTMinimum.Text = _Profile.WMPVizFFTMinimum.ToString();

      cboASIODevice.DataBindings.Add("Enabled", rbUseASIO, "Checked");
      btnASIOControlPanel.DataBindings.Add("Enabled", rbUseASIO, "Checked");
      lblASIOFirstChan.DataBindings.Add("Enabled", rbUseASIO, "Checked");
      lblASIOLastChan.DataBindings.Add("Enabled", rbUseASIO, "Checked");
      cboASIOFirstChan.DataBindings.Add("Enabled", rbUseASIO, "Checked");
      cboASIOLastChan.DataBindings.Add("Enabled", rbUseASIO, "Checked");

      cboWASAPIDevice.DataBindings.Add("Enabled", rbUseWASAPI, "Checked");
      btnWASAPIControlPanel.DataBindings.Add("Enabled", rbUseWASAPI, "Checked");
      chkWASAPIExclusive.DataBindings.Add("Enabled", rbUseWASAPI, "Checked");
      
      cboWASAPISpeakerLayout.DataBindings.Add("Enabled", rbUseWASAPI, "Checked");
      lblWASAPISpeakerLayout.DataBindings.Add("Enabled", rbUseWASAPI, "Checked");

      cboSoundDevice.DataBindings.Add("Enabled", rbUseSound, "Checked");
      btnMMEControlPanel.DataBindings.Add("Enabled", rbUseSound, "Checked");

      pnlSoftStopDuration.DataBindings.Add("Enabled", chkDoSoftStop, "Checked");

      DisplayExtList();

      if (_pureAudioPlugin != null)
      {
        lblPlayerName.Text = _pureAudioPlugin.PlayerName;
        lblDescription.Text = _pureAudioPlugin.Description();
        lblVersion.Text = _pureAudioPlugin.VersionNumber;
        lblAuthorName.Text = _pureAudioPlugin.AuthorName;
      }

      cboWMPVisualizations.Items.Add(new InstalledEffectInfo("None"));
      InstalledEffect[] installedEffects = WMPEffect.InstalledEffects;
      foreach (InstalledEffect effect in installedEffects)
      {
        object item = new InstalledEffectInfo(effect);
        cboWMPVisualizations.Items.Add(item);

        if (effect.ClsId == _Profile.WMPVizClsId)
          cboWMPVisualizations.SelectedItem = item;
      }

      // There is always 1 item  
      if (cboWMPVisualizations.SelectedIndex == -1)
        cboWMPVisualizations.SelectedIndex = 0;
    }

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

    private void tvwMenu_AfterSelect(object sender, TreeViewEventArgs e)
    {
      ctlHeader.Caption = tvwMenu.SelectedNode.Text;
      string nodeName = tvwMenu.SelectedNode.Name;
      switch (nodeName)
      {
        case "NodeRoot":
          tabControl.SelectedTab = tabPageAbout;
          break;
        case "NodeDevice":
          tabControl.SelectedTab = tabPageDevice;
          break;
        case "NodeGeneral":
          tabControl.SelectedTab = tabPageGeneral;
          break;
        case "NodeUpmixing":
          tabControl.SelectedTab = tabPageUpmixing;
          break;
        case "NodeDSP":
          tabControl.SelectedTab = tabPageDSP;
          break;
        case "NodeVisualizations":
          tabControl.SelectedTab = tabPageVisualization;
          break;
        case "NodeWMPViz":
          tabControl.SelectedTab = tabPageWMPViz;
          break;
        case "NodeExtensions":
          tabControl.SelectedTab = tabPageExtensions;
          break;
        case "NodeAdvanced":
          tabControl.SelectedTab = tabPageAdvanced;
          break;
        case "NodeWaveOut":
          tabControl.SelectedTab = tabPageWaveOut;
          break;
        case "NodeASIO":
          tabControl.SelectedTab = tabPageASIO;
          break;
        case "NodeWASAPI":
          tabControl.SelectedTab = tabPageWASAPI;
          break;
      }
    }

    private void lnkDownLoad_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
      Process.Start(
        "http://code.google.com/p/pureaudio-mediaportal-plugin");
    }

    private void lnkForum_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
      Process.Start(
        "http://forum.team-mediaportal.com/asio-music-player-245/");
    }

    private void rbUseASIO_CheckedChanged(object sender, EventArgs e)
    {
      _Profile.OutputMode = OutputMode.ASIO;
    }

    private void rbUseWASAPI_CheckedChanged(object sender, EventArgs e)
    {
      _Profile.OutputMode = OutputMode.WASAPI;
    }

    private void btnASIOControlPanel_Click(object sender, EventArgs e)
    {
      bool result = InitASIODevice();
      if (result)
      {
        result = _asioEngine.Driver.ShowControlPanel();
        if (!result)
          Log.Error(String.Format("PureAudio: Configuration: _asioEngine.Driver.ShowControlPanel() failed. Errorcode: {0}", _asioEngine.Driver.LastASIOError));
      }
    }

    private void btnMMEControlPanel_Click(object sender, EventArgs e)
    {
      Process.Start("mmsys.cpl");
    }

    private void btnWASAPIControlPanel_Click(object sender, EventArgs e)
    {
      Process.Start("mmsys.cpl");
    }

    private void cboASIODevice_SelectedIndexChanged(object sender, EventArgs e)
    {
      _Profile.ASIODevice = cboASIODevice.SelectedItem.ToString();

      int device = cboASIODevice.SelectedIndex;
      if (device > -1)
      {
        cboASIOFirstChan.Items.Clear();
        cboASIOLastChan.Items.Clear();

        if (InitASIODevice())
        {
          for (int i = 0; i < _asioEngine.Driver.OutputChannels.Length; i++)
          {
            Channel channel = _asioEngine.Driver.OutputChannels[i];
            string descr = String.Format("{0}: {1}", i + 1, channel.Name);

            cboASIOFirstChan.Items.Add(descr);
            cboASIOLastChan.Items.Add(descr);
          }

          int first = _Profile.ASIOFirstChan;
          int last = _Profile.ASIOLastChan;

          if (first == -1)
            first = 0;

          if (last == -1)
            last = cboASIOLastChan.Items.Count - 1;

          first = Math.Min(first, cboASIOFirstChan.Items.Count - 1);
          last = Math.Min(last, cboASIOLastChan.Items.Count - 1);

          cboASIOFirstChan.SelectedIndex = first;
          cboASIOLastChan.SelectedIndex = last;
        }
      }
    }

    private void cboASIOFirstChan_SelectedIndexChanged(object sender, EventArgs e)
    {
      _Profile.ASIOFirstChan = cboASIOFirstChan.SelectedIndex;
    }

    private void cboASIOLastChan_SelectedIndexChanged(object sender, EventArgs e)
    {
      _Profile.ASIOLastChan = cboASIOLastChan.SelectedIndex;
    }

    private void cboSoundDevice_SelectedIndexChanged(object sender, EventArgs e)
    {
      _Profile.DirectSoundDevice = cboSoundDevice.SelectedItem.ToString();
    }

    private void cboWASAPIDevice_SelectedIndexChanged(object sender, EventArgs e)
    {
      _Profile.WASAPIDevice = cboWASAPIDevice.SelectedItem.ToString();
    }

    private void cboWASAPISpeakerLayout_SelectedIndexChanged(object sender, EventArgs e)
    {
      _Profile.WASAPISpeakerLayout = (SpeakerLayout)cboWASAPISpeakerLayout.SelectedIndex;
    }

    private void cboDefaultPlayBackMode_SelectedIndexChanged(object sender, EventArgs e)
    {
      _Profile.DefaultPlayBackMode = (PlayBackMode)cboDefaultPlayBackMode.SelectedIndex;
    }

    private void cboMonoUpMix_SelectedIndexChanged(object sender, EventArgs e)
    {
      _Profile.MonoUpMix = (MonoUpMix)cboMonoUpMix.SelectedIndex;
    }

    private void cboQuadraphonicUpMix_SelectedIndexChanged(object sender, EventArgs e)
    {
      _Profile.QuadraphonicUpMix = (QuadraphonicUpMix)cboQuadraphonicUpMix.SelectedIndex;
    }

    private void cboStereoUpMix_SelectedIndexChanged(object sender, EventArgs e)
    {
      _Profile.StereoUpMix = (StereoUpMix)cboStereoUpMix.SelectedIndex;
    }

    private void cboFiveDotZeroUpMix_SelectedIndexChanged(object sender, EventArgs e)
    {
      _Profile.FiveDotZeroUpMix = (FiveDotZeroUpMix)cboFiveDotZeroUpMix.SelectedIndex;
    }

    private void cboFiveDotOneUpMix_SelectedIndexChanged(object sender, EventArgs e)
    {
      _Profile.FiveDotOneUpMix = (FiveDotOneUpMix)cboFiveDotOneUpMix.SelectedIndex;
    }

    private void trackBarSeekIncrement_ValueChanged(object sender, EventArgs e)
    {
      int value = trackBarSeekIncrement.Value * 5;
      _Profile.SeekIncrement = value;
      lblSeekIncrement.Text = value.ToString();
    }

    private void trackBarSoftStopDuration_ValueChanged(object sender, EventArgs e)
    {
      int value = trackBarSoftStopDuration.Value * 100;
      _Profile.SoftStopDuration = value;
      lblSoftStopDuration.Text = value.ToString();
    }

    private void trackBarGapLength_ValueChanged(object sender, EventArgs e)
    {
      int value = trackBarGapLength.Value * 250;
      _Profile.GapLength = value;
      lblGapLength.Text = value.ToString();
    }

    private void trackBarPlayBackBufferSize_ValueChanged(object sender, EventArgs e)
    {
      int value = trackBarPlayBackBufferSize.Value * 250;
      _Profile.PlayBackBufferSize = value;
      lblPlayBackBufferSize.Text = value.ToString();
    }

    private void trackBarFileBufferSize_ValueChanged(object sender, EventArgs e)
    {
      int value = trackBarFileBufferSize.Value * 100;
      _Profile.BASSPlayBackBufferSize = value;
      lblBASSPlayBackBufferSize.Text = value.ToString();
    }

    private void trackBarVizLatency_ValueChanged(object sender, EventArgs e)
    {
      int value = (trackBarVizLatency.Value - 20) * 25;
      _Profile.VizLatencyCorrection = value;
      lblVizLatency.Text = value.ToString();
    }

    private void trackBarWMPVizFFTFallBack_ValueChanged(object sender, EventArgs e)
    {
      int value = (trackBarWMPVizFFTFallBack.Value) * 50;
      _Profile.WMPVizFFTFallBack = value;
      lblWMPVizFFTFallBack.Text = value.ToString();
    }

    private void trackBarWMPVizFFTMinimum_ValueChanged(object sender, EventArgs e)
    {
      int value = (trackBarWMPVizFFTMinimum.Value) * -5;
      _Profile.WMPVizFFTMinimum = value;
      lblWMPVizFFTMinimum.Text = value.ToString();
    }

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
        _Profile.Extensions = ConfigProfile.Defaults.Extensions;
        DisplayExtList();
      }
    }

    private void tabPageExtensions_Validating(object sender, CancelEventArgs e)
    {
      string ext = String.Empty;
      foreach (ListViewItem item in lvExtensions.Items)
      {
        if (ext != String.Empty)
          ext += ",";
        ext += item.Text;
      }
      _Profile.Extensions = ext;
    }

    private void chkUseReplayGain_CheckedChanged(object sender, EventArgs e)
    {
      _Profile.UseReplayGain = chkUseReplayGain.Checked;
      chkUseRGAlbumGain.Enabled = chkUseReplayGain.Checked;
    }

    private void cboMinASIORate_SelectedIndexChanged(object sender, EventArgs e)
    {
      string value = cboMinASIORate.SelectedItem.ToString();
      if (value == "Auto")
        _Profile.ForceMinASIORate = 0;
      else
        _Profile.ForceMinASIORate = Convert.ToInt32(value);
    }

    private void cboMaxASIORate_SelectedIndexChanged(object sender, EventArgs e)
    {
      string value = cboMaxASIORate.SelectedItem.ToString();
      if (value == "Auto")
        _Profile.ForceMaxASIORate = 0;
      else
        _Profile.ForceMaxASIORate = Convert.ToInt32(value);
    }

    private void btnOk_Click(object sender, System.EventArgs e)
    {
      _Profile.SaveSettings();
      Close();
    }

    private void ConfigurationForm_FormClosed(object sender, FormClosedEventArgs e)
    {
      _asioEngine.Dispose();
    }

    private bool InitASIODevice()
    {
      int device = cboASIODevice.SelectedIndex;

      // use first available
      if (device == -1)
        device = 0;

      bool result = (device > -1);
      if (!result)
        MessageBox.Show("No device is currently selected", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

      if (result)
      {
        _asioEngine.ReleaseDriver();

        result = _asioEngine.InitDriver(device, this.Handle, false);
        if (!result)
          Log.Error("PureAudio: Configuration: Initializing ASIO device {0} failed.", device);
      }

      return result;
    }

    private bool InitSoundDevice()
    {
      int device;
      if (cboSoundDevice.Text == ConfigProfile.Defaults.DirectSoundDevice)
        device = -1;
      else
        device = cboSoundDevice.SelectedIndex;

      bool result = Bass.BASS_Init(device, 44100, BASSInit.BASS_DEVICE_DEFAULT, IntPtr.Zero, Guid.Empty);
      if (!result)
      {
        BASSError error = Bass.BASS_ErrorGetCode();
        result = (error == BASSError.BASS_ERROR_ALREADY);
      }
      if (!result)
        Log.Error(
          String.Format("PureAudio: Configuration: BASS_Init() failed: {0}",
          Bass.BASS_ErrorGetCode()));

      return result;
    }

    private void DisplayExtList()
    {
      string[] ext = _Profile.Extensions.Split(new string[] { "," }, StringSplitOptions.None);

      lvExtensions.Items.Clear();
      for (int i = 0; i < ext.Length; i++)
      {
        lvExtensions.Items.Add(ext[i]);
      }
    }

    private void cboWMPVisualizations_SelectedIndexChanged(object sender, EventArgs e)
    {
      cboWMPVisualizationPresets.Items.Clear();

      _Profile.VizType = (int)VisualizationType.None;
      _Profile.WMPVizClsId = "";

      InstalledEffectInfo effectInfo = (InstalledEffectInfo)cboWMPVisualizations.SelectedItem;
      if (effectInfo.InstalledEffect != null)
      {
        _Profile.VizType = (int)VisualizationType.WMP;
        _Profile.WMPVizClsId = effectInfo.InstalledEffect.ClsId;

        WMPEffect wmpEffect = WMPEffect.SelectEffect(effectInfo.InstalledEffect, Handle);
        if (wmpEffect != null)
        {
          for (int index = 0; index < wmpEffect.Presets.Length; index++)
          {
            Preset preset = wmpEffect.Presets[index];
            cboWMPVisualizationPresets.Items.Add(preset.Name);

            if (index == _Profile.WMPVizPreset)
              cboWMPVisualizationPresets.SelectedIndex = index;
          }

          if (cboWMPVisualizationPresets.Items.Count > _Profile.WMPVizPreset)
            cboWMPVisualizationPresets.SelectedIndex = _Profile.WMPVizPreset;

          wmpEffect.Release();
        }
      }

      cboWMPVisualizationPresets.Enabled = cboWMPVisualizationPresets.Items.Count > 1;
      if (cboWMPVisualizationPresets.Enabled && cboWMPVisualizations.SelectedIndex == -1)
        cboWMPVisualizationPresets.SelectedIndex = 0;
    }

    private void cboWMPVisualizationPresets_SelectedIndexChanged(object sender, EventArgs e)
    {
      _Profile.WMPVizPreset = cboWMPVisualizationPresets.SelectedIndex;
    }

    private void chkWASAPIExclusive_CheckedChanged(object sender, EventArgs e)
    {
      _Profile.WASAPIExclusive = chkWASAPIExclusive.Checked;
    }

    private void chkDoSoftStop_CheckedChanged(object sender, EventArgs e)
    {
      _Profile.DoSoftStop = chkDoSoftStop.Checked;
    }

    private void chkUseForCDDA_CheckedChanged(object sender, EventArgs e)
    {
      _Profile.UseForCDDA = chkUseForCDDA.Checked;
    }

    private void chkUseForWebStream_CheckedChanged(object sender, EventArgs e)
    {
      _Profile.UseForWebStream = chkUseForWebStream.Checked;
    }

    private void chkUseForLastFMWebStream_CheckedChanged(object sender, EventArgs e)
    {
      _Profile.UseForLastFMWebStream = chkUseForLastFMWebStream.Checked;
    }

    private void chkUseMaxASIOBufferSize_CheckedChanged(object sender, EventArgs e)
    {
      _Profile.UseMaxASIOBufferSize = chkUseMaxASIOBufferSize.Checked;
    }

    private void chkWASAPIEvent_CheckedChanged(object sender, EventArgs e)
    {
      _Profile.WASAPIEvent = chkWASAPIEvent.Checked;
    }

    private void chkUseOverSampling_CheckedChanged(object sender, EventArgs e)
    {
      _Profile.UseOverSampling = chkUseOverSampling.Checked;
    }

    private void chkUseRGAlbumGain_CheckedChanged(object sender, EventArgs e)
    {
      _Profile.UseRGAlbumGain = chkUseRGAlbumGain.Checked;
    }

    private void chkUseVizAGC_CheckedChanged(object sender, EventArgs e)
    {
      _Profile.UseVizAGC = chkUseVizAGC.Checked;
    }

    private void chkWMPVizFFTHalf_CheckedChanged(object sender, EventArgs e)
    {
      _Profile.WMPVizFFTHalf = chkWMPVizFFTHalf.Checked;
    }

    private void cboMaxWASAPIRate_SelectedIndexChanged(object sender, EventArgs e)
    {
      string value = cboMaxWASAPIRate.SelectedItem.ToString();
      if (value == "Auto")
        _Profile.ForceMaxWASAPIRate = 0;
      else
        _Profile.ForceMaxWASAPIRate = Convert.ToInt32(value);
    }

    private void cboMinWASAPIRate_SelectedIndexChanged(object sender, EventArgs e)
    {
      string value = cboMinWASAPIRate.SelectedItem.ToString();
      if (value == "Auto")
        _Profile.ForceMinWASAPIRate = 0;
      else
        _Profile.ForceMinWASAPIRate = Convert.ToInt32(value);
    }
  }
}
