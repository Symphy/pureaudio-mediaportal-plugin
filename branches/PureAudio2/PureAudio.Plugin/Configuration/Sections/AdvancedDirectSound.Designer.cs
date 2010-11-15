namespace MediaPortal.Plugins.PureAudio.Configuration.Sections
{
  partial class AdvancedDirectSound
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
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.textBox7 = new System.Windows.Forms.TextBox();
      this.mpLabel4 = new MediaPortal.UserInterface.Controls.MPLabel();
      this.trackBarDirectSoundBufferSize = new System.Windows.Forms.TrackBar();
      this.mpLabel14 = new MediaPortal.UserInterface.Controls.MPLabel();
      this.lblDirectSoundBufferSize = new MediaPortal.UserInterface.Controls.MPLabel();
      this.groupBox1.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.trackBarDirectSoundBufferSize)).BeginInit();
      this.SuspendLayout();
      // 
      // groupBox1
      // 
      this.groupBox1.Controls.Add(this.textBox7);
      this.groupBox1.Controls.Add(this.mpLabel4);
      this.groupBox1.Controls.Add(this.trackBarDirectSoundBufferSize);
      this.groupBox1.Controls.Add(this.mpLabel14);
      this.groupBox1.Controls.Add(this.lblDirectSoundBufferSize);
      this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.groupBox1.Location = new System.Drawing.Point(0, 0);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(497, 308);
      this.groupBox1.TabIndex = 0;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Advanced DirectSound settings";
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
      // mpLabel4
      // 
      this.mpLabel4.Location = new System.Drawing.Point(21, 44);
      this.mpLabel4.Name = "mpLabel4";
      this.mpLabel4.Size = new System.Drawing.Size(100, 23);
      this.mpLabel4.TabIndex = 16;
      this.mpLabel4.Text = "Buffer size:";
      // 
      // trackBarDirectSoundBufferSize
      // 
      this.trackBarDirectSoundBufferSize.LargeChange = 1;
      this.trackBarDirectSoundBufferSize.Location = new System.Drawing.Point(148, 38);
      this.trackBarDirectSoundBufferSize.Minimum = 1;
      this.trackBarDirectSoundBufferSize.Name = "trackBarDirectSoundBufferSize";
      this.trackBarDirectSoundBufferSize.Size = new System.Drawing.Size(219, 45);
      this.trackBarDirectSoundBufferSize.TabIndex = 1;
      this.trackBarDirectSoundBufferSize.Value = 1;
      this.trackBarDirectSoundBufferSize.ValueChanged += new System.EventHandler(this.trackBarDirectSoundBufferSize_ValueChanged);
      // 
      // mpLabel14
      // 
      this.mpLabel14.Location = new System.Drawing.Point(420, 44);
      this.mpLabel14.Name = "mpLabel14";
      this.mpLabel14.Size = new System.Drawing.Size(71, 18);
      this.mpLabel14.TabIndex = 26;
      this.mpLabel14.Text = "milliseconds";
      // 
      // lblDirectSoundBufferSize
      // 
      this.lblDirectSoundBufferSize.Location = new System.Drawing.Point(373, 44);
      this.lblDirectSoundBufferSize.Name = "lblDirectSoundBufferSize";
      this.lblDirectSoundBufferSize.Size = new System.Drawing.Size(37, 21);
      this.lblDirectSoundBufferSize.TabIndex = 27;
      this.lblDirectSoundBufferSize.Text = "0";
      this.lblDirectSoundBufferSize.TextAlign = System.Drawing.ContentAlignment.TopRight;
      // 
      // AdvancedDirectSound
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.groupBox1);
      this.Name = "AdvancedDirectSound";
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.trackBarDirectSoundBufferSize)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.TextBox textBox7;
    private MediaPortal.UserInterface.Controls.MPLabel mpLabel4;
    private System.Windows.Forms.TrackBar trackBarDirectSoundBufferSize;
    private MediaPortal.UserInterface.Controls.MPLabel mpLabel14;
    private MediaPortal.UserInterface.Controls.MPLabel lblDirectSoundBufferSize;

  }
}
