namespace MediaPortal.Plugins.PureAudio.Configuration.Sections
{
  partial class Advanced
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
      this.groupBox3 = new System.Windows.Forms.GroupBox();
      this.chkEnableOverSampling = new System.Windows.Forms.CheckBox();
      this.groupBox2 = new System.Windows.Forms.GroupBox();
      this.chkEnableRGAlbumGain = new System.Windows.Forms.CheckBox();
      this.chkEnableReplayGain = new System.Windows.Forms.CheckBox();
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.textBox3 = new System.Windows.Forms.TextBox();
      this.mpLabel5 = new MediaPortal.UserInterface.Controls.MPLabel();
      this.lblPlaybackBufferSize = new MediaPortal.UserInterface.Controls.MPLabel();
      this.mpLabel12 = new MediaPortal.UserInterface.Controls.MPLabel();
      this.trackBarPlaybackBufferSize = new System.Windows.Forms.TrackBar();
      this.groupBox3.SuspendLayout();
      this.groupBox2.SuspendLayout();
      this.groupBox1.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.trackBarPlaybackBufferSize)).BeginInit();
      this.SuspendLayout();
      // 
      // groupBox3
      // 
      this.groupBox3.Controls.Add(this.chkEnableOverSampling);
      this.groupBox3.Location = new System.Drawing.Point(0, 180);
      this.groupBox3.Name = "groupBox3";
      this.groupBox3.Size = new System.Drawing.Size(497, 128);
      this.groupBox3.TabIndex = 40;
      this.groupBox3.TabStop = false;
      this.groupBox3.Text = "Miscellaneous";
      // 
      // chkEnableOverSampling
      // 
      this.chkEnableOverSampling.AutoSize = true;
      this.chkEnableOverSampling.Location = new System.Drawing.Point(12, 19);
      this.chkEnableOverSampling.Name = "chkEnableOverSampling";
      this.chkEnableOverSampling.Size = new System.Drawing.Size(163, 17);
      this.chkEnableOverSampling.TabIndex = 4;
      this.chkEnableOverSampling.Text = "Perform 2 times oversampling";
      this.chkEnableOverSampling.CheckedChanged += new System.EventHandler(this.chkEnableOverSampling_CheckedChanged);
      // 
      // groupBox2
      // 
      this.groupBox2.Controls.Add(this.chkEnableRGAlbumGain);
      this.groupBox2.Controls.Add(this.chkEnableReplayGain);
      this.groupBox2.Location = new System.Drawing.Point(0, 96);
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.Size = new System.Drawing.Size(497, 78);
      this.groupBox2.TabIndex = 39;
      this.groupBox2.TabStop = false;
      this.groupBox2.Text = "Replay Gain";
      // 
      // chkEnableRGAlbumGain
      // 
      this.chkEnableRGAlbumGain.AutoSize = true;
      this.chkEnableRGAlbumGain.Enabled = false;
      this.chkEnableRGAlbumGain.Location = new System.Drawing.Point(24, 42);
      this.chkEnableRGAlbumGain.Name = "chkEnableRGAlbumGain";
      this.chkEnableRGAlbumGain.Size = new System.Drawing.Size(152, 17);
      this.chkEnableRGAlbumGain.TabIndex = 3;
      this.chkEnableRGAlbumGain.Text = "Use album gain if available";
      this.chkEnableRGAlbumGain.UseVisualStyleBackColor = true;
      // 
      // chkEnableReplayGain
      // 
      this.chkEnableReplayGain.AutoSize = true;
      this.chkEnableReplayGain.Enabled = false;
      this.chkEnableReplayGain.Location = new System.Drawing.Point(12, 19);
      this.chkEnableReplayGain.Name = "chkEnableReplayGain";
      this.chkEnableReplayGain.Size = new System.Drawing.Size(120, 17);
      this.chkEnableReplayGain.TabIndex = 2;
      this.chkEnableReplayGain.Text = "Enable Replay Gain";
      this.chkEnableReplayGain.UseVisualStyleBackColor = true;
      // 
      // groupBox1
      // 
      this.groupBox1.Controls.Add(this.textBox3);
      this.groupBox1.Controls.Add(this.mpLabel5);
      this.groupBox1.Controls.Add(this.lblPlaybackBufferSize);
      this.groupBox1.Controls.Add(this.mpLabel12);
      this.groupBox1.Controls.Add(this.trackBarPlaybackBufferSize);
      this.groupBox1.Location = new System.Drawing.Point(0, 0);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(497, 90);
      this.groupBox1.TabIndex = 38;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Playback buffer";
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
      this.textBox3.Text = "Increase this value if you experience hickups, pops or clicks in playback.";
      // 
      // mpLabel5
      // 
      this.mpLabel5.Location = new System.Drawing.Point(21, 44);
      this.mpLabel5.Name = "mpLabel5";
      this.mpLabel5.Size = new System.Drawing.Size(112, 24);
      this.mpLabel5.TabIndex = 17;
      this.mpLabel5.Text = "Playback buffer size:";
      // 
      // lblPlaybackBufferSize
      // 
      this.lblPlaybackBufferSize.Location = new System.Drawing.Point(373, 44);
      this.lblPlaybackBufferSize.Name = "lblPlaybackBufferSize";
      this.lblPlaybackBufferSize.Size = new System.Drawing.Size(37, 21);
      this.lblPlaybackBufferSize.TabIndex = 24;
      this.lblPlaybackBufferSize.Text = "0";
      this.lblPlaybackBufferSize.TextAlign = System.Drawing.ContentAlignment.TopRight;
      // 
      // mpLabel12
      // 
      this.mpLabel12.Location = new System.Drawing.Point(416, 44);
      this.mpLabel12.Name = "mpLabel12";
      this.mpLabel12.Size = new System.Drawing.Size(68, 18);
      this.mpLabel12.TabIndex = 26;
      this.mpLabel12.Text = "milliseconds";
      // 
      // trackBarPlaybackBufferSize
      // 
      this.trackBarPlaybackBufferSize.LargeChange = 1;
      this.trackBarPlaybackBufferSize.Location = new System.Drawing.Point(148, 38);
      this.trackBarPlaybackBufferSize.Minimum = 1;
      this.trackBarPlaybackBufferSize.Name = "trackBarPlaybackBufferSize";
      this.trackBarPlaybackBufferSize.Size = new System.Drawing.Size(219, 45);
      this.trackBarPlaybackBufferSize.TabIndex = 1;
      this.trackBarPlaybackBufferSize.Value = 1;
      this.trackBarPlaybackBufferSize.ValueChanged += new System.EventHandler(this.trackBarPlaybackBufferSize_ValueChanged);
      // 
      // Advanced
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.groupBox3);
      this.Controls.Add(this.groupBox2);
      this.Controls.Add(this.groupBox1);
      this.Name = "Advanced";
      this.groupBox3.ResumeLayout(false);
      this.groupBox3.PerformLayout();
      this.groupBox2.ResumeLayout(false);
      this.groupBox2.PerformLayout();
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.trackBarPlaybackBufferSize)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.GroupBox groupBox3;
    private System.Windows.Forms.CheckBox chkEnableOverSampling;
    private System.Windows.Forms.GroupBox groupBox2;
    private System.Windows.Forms.CheckBox chkEnableRGAlbumGain;
    private System.Windows.Forms.CheckBox chkEnableReplayGain;
    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.TextBox textBox3;
    private MediaPortal.UserInterface.Controls.MPLabel mpLabel5;
    private MediaPortal.UserInterface.Controls.MPLabel lblPlaybackBufferSize;
    private MediaPortal.UserInterface.Controls.MPLabel mpLabel12;
    private System.Windows.Forms.TrackBar trackBarPlaybackBufferSize;

  }
}
