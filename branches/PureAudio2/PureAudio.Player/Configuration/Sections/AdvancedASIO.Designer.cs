namespace MediaPortal.Plugins.PureAudio.Configuration.Sections
{
  partial class AdvancedASIO
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
      this.groupBox9 = new System.Windows.Forms.GroupBox();
      this.textBox2 = new System.Windows.Forms.TextBox();
      this.mpLabel20 = new MediaPortal.UserInterface.Controls.MPLabel();
      this.mpLabel19 = new MediaPortal.UserInterface.Controls.MPLabel();
      this.mpLabel17 = new MediaPortal.UserInterface.Controls.MPLabel();
      this.mpLabel1 = new MediaPortal.UserInterface.Controls.MPLabel();
      this.cboASIOMaxRate = new System.Windows.Forms.ComboBox();
      this.cboASIOMinRate = new System.Windows.Forms.ComboBox();
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.chkASIOUseMaxBufferSize = new System.Windows.Forms.CheckBox();
      this.groupBox9.SuspendLayout();
      this.groupBox1.SuspendLayout();
      this.SuspendLayout();
      // 
      // groupBox9
      // 
      this.groupBox9.Controls.Add(this.textBox2);
      this.groupBox9.Controls.Add(this.mpLabel20);
      this.groupBox9.Controls.Add(this.mpLabel19);
      this.groupBox9.Controls.Add(this.mpLabel17);
      this.groupBox9.Controls.Add(this.mpLabel1);
      this.groupBox9.Controls.Add(this.cboASIOMaxRate);
      this.groupBox9.Controls.Add(this.cboASIOMinRate);
      this.groupBox9.Location = new System.Drawing.Point(0, 73);
      this.groupBox9.Name = "groupBox9";
      this.groupBox9.Size = new System.Drawing.Size(497, 235);
      this.groupBox9.TabIndex = 4;
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
      // cboASIOMaxRate
      // 
      this.cboASIOMaxRate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cboASIOMaxRate.FormattingEnabled = true;
      this.cboASIOMaxRate.Location = new System.Drawing.Point(100, 84);
      this.cboASIOMaxRate.Name = "cboASIOMaxRate";
      this.cboASIOMaxRate.Size = new System.Drawing.Size(98, 21);
      this.cboASIOMaxRate.TabIndex = 3;
      this.cboASIOMaxRate.SelectedIndexChanged += new System.EventHandler(this.cboASIOMaxRate_SelectedIndexChanged);
      // 
      // cboASIOMinRate
      // 
      this.cboASIOMinRate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cboASIOMinRate.FormattingEnabled = true;
      this.cboASIOMinRate.Location = new System.Drawing.Point(100, 57);
      this.cboASIOMinRate.Name = "cboASIOMinRate";
      this.cboASIOMinRate.Size = new System.Drawing.Size(98, 21);
      this.cboASIOMinRate.TabIndex = 2;
      this.cboASIOMinRate.SelectedIndexChanged += new System.EventHandler(this.cboASIOMinRate_SelectedIndexChanged);
      // 
      // groupBox1
      // 
      this.groupBox1.Controls.Add(this.chkASIOUseMaxBufferSize);
      this.groupBox1.Location = new System.Drawing.Point(0, 0);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(497, 67);
      this.groupBox1.TabIndex = 3;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Advanced ASIO settings";
      // 
      // chkASIOUseMaxBufferSize
      // 
      this.chkASIOUseMaxBufferSize.AutoSize = true;
      this.chkASIOUseMaxBufferSize.Location = new System.Drawing.Point(12, 19);
      this.chkASIOUseMaxBufferSize.Name = "chkASIOUseMaxBufferSize";
      this.chkASIOUseMaxBufferSize.Size = new System.Drawing.Size(278, 17);
      this.chkASIOUseMaxBufferSize.TabIndex = 1;
      this.chkASIOUseMaxBufferSize.Text = "Always use maximum available buffer on ASIO device";
      this.chkASIOUseMaxBufferSize.UseVisualStyleBackColor = true;
      this.chkASIOUseMaxBufferSize.CheckedChanged += new System.EventHandler(this.chkASIOUseMaxBufferSize_CheckedChanged);
      // 
      // AdvancedASIO
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.groupBox9);
      this.Controls.Add(this.groupBox1);
      this.Name = "AdvancedASIO";
      this.groupBox9.ResumeLayout(false);
      this.groupBox9.PerformLayout();
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.GroupBox groupBox9;
    private System.Windows.Forms.TextBox textBox2;
    private MediaPortal.UserInterface.Controls.MPLabel mpLabel20;
    private MediaPortal.UserInterface.Controls.MPLabel mpLabel19;
    private MediaPortal.UserInterface.Controls.MPLabel mpLabel17;
    private MediaPortal.UserInterface.Controls.MPLabel mpLabel1;
    private System.Windows.Forms.ComboBox cboASIOMaxRate;
    private System.Windows.Forms.ComboBox cboASIOMinRate;
    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.CheckBox chkASIOUseMaxBufferSize;
  }
}
