namespace MediaPortal.Plugins.PureAudio.Configuration.Sections
{
  partial class Visualizations
  {
    /// <summary> 
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary> 
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Component Designer generated code

    /// <summary> 
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.groupBox14 = new System.Windows.Forms.GroupBox();
      this.nudWMPEffectFps = new System.Windows.Forms.NumericUpDown();
      this.label2 = new System.Windows.Forms.Label();
      this.label11 = new System.Windows.Forms.Label();
      this.label10 = new System.Windows.Forms.Label();
      this.cboWMPEffectPresets = new System.Windows.Forms.ComboBox();
      this.cboWMPEffects = new System.Windows.Forms.ComboBox();
      this.groupBox11 = new System.Windows.Forms.GroupBox();
      this.textBox1 = new System.Windows.Forms.TextBox();
      this.chkVisualizationUseAGC = new System.Windows.Forms.CheckBox();
      this.textBox6 = new System.Windows.Forms.TextBox();
      this.mpLabel9 = new MediaPortal.UserInterface.Controls.MPLabel();
      this.trackBarVisualizationLatencyCorrection = new System.Windows.Forms.TrackBar();
      this.mpLabel27 = new MediaPortal.UserInterface.Controls.MPLabel();
      this.lblVisualizationLatencyCorrection = new MediaPortal.UserInterface.Controls.MPLabel();
      this.groupBox14.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.nudWMPEffectFps)).BeginInit();
      this.groupBox11.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.trackBarVisualizationLatencyCorrection)).BeginInit();
      this.SuspendLayout();
      // 
      // groupBox14
      // 
      this.groupBox14.Controls.Add(this.nudWMPEffectFps);
      this.groupBox14.Controls.Add(this.label2);
      this.groupBox14.Controls.Add(this.label11);
      this.groupBox14.Controls.Add(this.label10);
      this.groupBox14.Controls.Add(this.cboWMPEffectPresets);
      this.groupBox14.Controls.Add(this.cboWMPEffects);
      this.groupBox14.Location = new System.Drawing.Point(0, 0);
      this.groupBox14.Name = "groupBox14";
      this.groupBox14.Size = new System.Drawing.Size(497, 134);
      this.groupBox14.TabIndex = 3;
      this.groupBox14.TabStop = false;
      this.groupBox14.Text = "Visualization selection";
      // 
      // nudWMPEffectFps
      // 
      this.nudWMPEffectFps.Location = new System.Drawing.Point(83, 77);
      this.nudWMPEffectFps.Maximum = new decimal(new int[] {
            75,
            0,
            0,
            0});
      this.nudWMPEffectFps.Minimum = new decimal(new int[] {
            20,
            0,
            0,
            0});
      this.nudWMPEffectFps.Name = "nudWMPEffectFps";
      this.nudWMPEffectFps.Size = new System.Drawing.Size(62, 20);
      this.nudWMPEffectFps.TabIndex = 3;
      this.nudWMPEffectFps.Value = new decimal(new int[] {
            25,
            0,
            0,
            0});
      this.nudWMPEffectFps.ValueChanged += new System.EventHandler(this.nudWMPEffectFps_ValueChanged);
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
      // cboWMPEffectPresets
      // 
      this.cboWMPEffectPresets.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cboWMPEffectPresets.FormattingEnabled = true;
      this.cboWMPEffectPresets.Location = new System.Drawing.Point(83, 49);
      this.cboWMPEffectPresets.Name = "cboWMPEffectPresets";
      this.cboWMPEffectPresets.Size = new System.Drawing.Size(289, 21);
      this.cboWMPEffectPresets.TabIndex = 2;
      this.cboWMPEffectPresets.SelectedIndexChanged += new System.EventHandler(this.cboWMPEffectPresets_SelectedIndexChanged);
      // 
      // cboWMPEffects
      // 
      this.cboWMPEffects.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cboWMPEffects.FormattingEnabled = true;
      this.cboWMPEffects.Location = new System.Drawing.Point(83, 22);
      this.cboWMPEffects.Name = "cboWMPEffects";
      this.cboWMPEffects.Size = new System.Drawing.Size(289, 21);
      this.cboWMPEffects.TabIndex = 1;
      this.cboWMPEffects.SelectedIndexChanged += new System.EventHandler(this.cboWMPEffects_SelectedIndexChanged);
      // 
      // groupBox11
      // 
      this.groupBox11.Controls.Add(this.textBox1);
      this.groupBox11.Controls.Add(this.chkVisualizationUseAGC);
      this.groupBox11.Controls.Add(this.textBox6);
      this.groupBox11.Controls.Add(this.mpLabel9);
      this.groupBox11.Controls.Add(this.trackBarVisualizationLatencyCorrection);
      this.groupBox11.Controls.Add(this.mpLabel27);
      this.groupBox11.Controls.Add(this.lblVisualizationLatencyCorrection);
      this.groupBox11.Location = new System.Drawing.Point(0, 140);
      this.groupBox11.Name = "groupBox11";
      this.groupBox11.Size = new System.Drawing.Size(497, 168);
      this.groupBox11.TabIndex = 4;
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
      // chkVisualizationUseAGC
      // 
      this.chkVisualizationUseAGC.AutoSize = true;
      this.chkVisualizationUseAGC.Location = new System.Drawing.Point(24, 132);
      this.chkVisualizationUseAGC.Name = "chkVisualizationUseAGC";
      this.chkVisualizationUseAGC.Size = new System.Drawing.Size(170, 17);
      this.chkVisualizationUseAGC.TabIndex = 5;
      this.chkVisualizationUseAGC.Text = "Enable Automatic Gain Control";
      this.chkVisualizationUseAGC.UseVisualStyleBackColor = true;
      this.chkVisualizationUseAGC.CheckedChanged += new System.EventHandler(this.chkVisualizationUseAGC_CheckedChanged);
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
      // trackBarVisualizationLatencyCorrection
      // 
      this.trackBarVisualizationLatencyCorrection.LargeChange = 1;
      this.trackBarVisualizationLatencyCorrection.Location = new System.Drawing.Point(148, 55);
      this.trackBarVisualizationLatencyCorrection.Maximum = 40;
      this.trackBarVisualizationLatencyCorrection.Name = "trackBarVisualizationLatencyCorrection";
      this.trackBarVisualizationLatencyCorrection.Size = new System.Drawing.Size(219, 45);
      this.trackBarVisualizationLatencyCorrection.TabIndex = 4;
      this.trackBarVisualizationLatencyCorrection.Value = 20;
      this.trackBarVisualizationLatencyCorrection.ValueChanged += new System.EventHandler(this.trackBarVisualizationLatency_ValueChanged);
      // 
      // mpLabel27
      // 
      this.mpLabel27.Location = new System.Drawing.Point(416, 60);
      this.mpLabel27.Name = "mpLabel27";
      this.mpLabel27.Size = new System.Drawing.Size(68, 18);
      this.mpLabel27.TabIndex = 31;
      this.mpLabel27.Text = "milliseconds";
      // 
      // lblVisualizationLatencyCorrection
      // 
      this.lblVisualizationLatencyCorrection.Location = new System.Drawing.Point(373, 60);
      this.lblVisualizationLatencyCorrection.Name = "lblVisualizationLatencyCorrection";
      this.lblVisualizationLatencyCorrection.Size = new System.Drawing.Size(37, 21);
      this.lblVisualizationLatencyCorrection.TabIndex = 30;
      this.lblVisualizationLatencyCorrection.Text = "0";
      this.lblVisualizationLatencyCorrection.TextAlign = System.Drawing.ContentAlignment.TopRight;
      // 
      // Visualizations
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.groupBox14);
      this.Controls.Add(this.groupBox11);
      this.Name = "Visualizations";
      this.groupBox14.ResumeLayout(false);
      this.groupBox14.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.nudWMPEffectFps)).EndInit();
      this.groupBox11.ResumeLayout(false);
      this.groupBox11.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.trackBarVisualizationLatencyCorrection)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.GroupBox groupBox14;
    private System.Windows.Forms.NumericUpDown nudWMPEffectFps;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label label11;
    private System.Windows.Forms.Label label10;
    private System.Windows.Forms.ComboBox cboWMPEffectPresets;
    private System.Windows.Forms.ComboBox cboWMPEffects;
    private System.Windows.Forms.GroupBox groupBox11;
    private System.Windows.Forms.TextBox textBox1;
    private System.Windows.Forms.CheckBox chkVisualizationUseAGC;
    private System.Windows.Forms.TextBox textBox6;
    private MediaPortal.UserInterface.Controls.MPLabel mpLabel9;
    private System.Windows.Forms.TrackBar trackBarVisualizationLatencyCorrection;
    private MediaPortal.UserInterface.Controls.MPLabel mpLabel27;
    private MediaPortal.UserInterface.Controls.MPLabel lblVisualizationLatencyCorrection;

  }
}
