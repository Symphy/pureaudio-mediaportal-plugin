namespace PureAudioTester
{
	partial class PureAudioTester
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

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
      this.components = new System.ComponentModel.Container();
      this.btnStart = new System.Windows.Forms.Button();
      this.btnStop = new System.Windows.Forms.Button();
      this.btnPause = new System.Windows.Forms.Button();
      this.tmrStatus = new System.Windows.Forms.Timer(this.components);
      this.btnBack = new System.Windows.Forms.Button();
      this.btnForward = new System.Windows.Forms.Button();
      this.btnToggleMode = new System.Windows.Forms.Button();
      this.btnConfig = new System.Windows.Forms.Button();
      this.tbEvents = new System.Windows.Forms.TextBox();
      this.label3 = new System.Windows.Forms.Label();
      this.tbLog = new System.Windows.Forms.TextBox();
      this.btnGetFile = new System.Windows.Forms.Button();
      this.cboFileName = new System.Windows.Forms.ComboBox();
      this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
      this.label4 = new System.Windows.Forms.Label();
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.label20 = new System.Windows.Forms.Label();
      this.lblCurrentFileType = new System.Windows.Forms.Label();
      this.label6 = new System.Windows.Forms.Label();
      this.lblCurrentFilePath = new System.Windows.Forms.Label();
      this.label5 = new System.Windows.Forms.Label();
      this.lblLastError = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.label1 = new System.Windows.Forms.Label();
      this.lblPlaybackMode = new System.Windows.Forms.Label();
      this.lblCurrentPosition = new System.Windows.Forms.Label();
      this.label7 = new System.Windows.Forms.Label();
      this.groupBox1.SuspendLayout();
      this.SuspendLayout();
      // 
      // btnStart
      // 
      this.btnStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.btnStart.Location = new System.Drawing.Point(635, 9);
      this.btnStart.Name = "btnStart";
      this.btnStart.Size = new System.Drawing.Size(103, 23);
      this.btnStart.TabIndex = 0;
      this.btnStart.Text = "Start";
      this.btnStart.UseVisualStyleBackColor = true;
      this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
      // 
      // btnStop
      // 
      this.btnStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.btnStop.Location = new System.Drawing.Point(635, 67);
      this.btnStop.Name = "btnStop";
      this.btnStop.Size = new System.Drawing.Size(103, 23);
      this.btnStop.TabIndex = 1;
      this.btnStop.Text = "Stop";
      this.btnStop.UseVisualStyleBackColor = true;
      this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
      // 
      // btnPause
      // 
      this.btnPause.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.btnPause.Location = new System.Drawing.Point(635, 38);
      this.btnPause.Name = "btnPause";
      this.btnPause.Size = new System.Drawing.Size(103, 23);
      this.btnPause.TabIndex = 2;
      this.btnPause.Text = "Pause";
      this.btnPause.UseVisualStyleBackColor = true;
      this.btnPause.Click += new System.EventHandler(this.btnPause_Click);
      // 
      // tmrStatus
      // 
      this.tmrStatus.Interval = 500;
      this.tmrStatus.Tick += new System.EventHandler(this.tmrStatus_Tick);
      // 
      // btnBack
      // 
      this.btnBack.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.btnBack.Location = new System.Drawing.Point(635, 96);
      this.btnBack.Name = "btnBack";
      this.btnBack.Size = new System.Drawing.Size(103, 23);
      this.btnBack.TabIndex = 4;
      this.btnBack.Text = "Back";
      this.btnBack.UseVisualStyleBackColor = true;
      this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
      // 
      // btnForward
      // 
      this.btnForward.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.btnForward.Location = new System.Drawing.Point(635, 125);
      this.btnForward.Name = "btnForward";
      this.btnForward.Size = new System.Drawing.Size(103, 23);
      this.btnForward.TabIndex = 5;
      this.btnForward.Text = "Forward";
      this.btnForward.UseVisualStyleBackColor = true;
      this.btnForward.Click += new System.EventHandler(this.btnForward_Click);
      // 
      // btnToggleMode
      // 
      this.btnToggleMode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.btnToggleMode.Location = new System.Drawing.Point(635, 154);
      this.btnToggleMode.Name = "btnToggleMode";
      this.btnToggleMode.Size = new System.Drawing.Size(103, 23);
      this.btnToggleMode.TabIndex = 6;
      this.btnToggleMode.Text = "Toggle Mode";
      this.btnToggleMode.UseVisualStyleBackColor = true;
      this.btnToggleMode.Click += new System.EventHandler(this.btnToggleMode_Click);
      // 
      // btnConfig
      // 
      this.btnConfig.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.btnConfig.Location = new System.Drawing.Point(635, 183);
      this.btnConfig.Name = "btnConfig";
      this.btnConfig.Size = new System.Drawing.Size(103, 23);
      this.btnConfig.TabIndex = 8;
      this.btnConfig.Text = "Configuration";
      this.btnConfig.UseVisualStyleBackColor = true;
      this.btnConfig.Click += new System.EventHandler(this.btnConfig_Click);
      // 
      // tbEvents
      // 
      this.tbEvents.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.tbEvents.BackColor = System.Drawing.SystemColors.Window;
      this.tbEvents.Location = new System.Drawing.Point(635, 241);
      this.tbEvents.Multiline = true;
      this.tbEvents.Name = "tbEvents";
      this.tbEvents.ReadOnly = true;
      this.tbEvents.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
      this.tbEvents.Size = new System.Drawing.Size(103, 275);
      this.tbEvents.TabIndex = 11;
      // 
      // label3
      // 
      this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(632, 225);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(43, 13);
      this.label3.TabIndex = 12;
      this.label3.Text = "Events:";
      // 
      // tbLog
      // 
      this.tbLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.tbLog.BackColor = System.Drawing.SystemColors.Window;
      this.tbLog.Location = new System.Drawing.Point(15, 151);
      this.tbLog.Multiline = true;
      this.tbLog.Name = "tbLog";
      this.tbLog.ReadOnly = true;
      this.tbLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
      this.tbLog.Size = new System.Drawing.Size(611, 365);
      this.tbLog.TabIndex = 13;
      // 
      // btnGetFile
      // 
      this.btnGetFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.btnGetFile.Location = new System.Drawing.Point(604, 7);
      this.btnGetFile.Name = "btnGetFile";
      this.btnGetFile.Size = new System.Drawing.Size(25, 23);
      this.btnGetFile.TabIndex = 14;
      this.btnGetFile.Text = "...";
      this.btnGetFile.UseVisualStyleBackColor = true;
      this.btnGetFile.Click += new System.EventHandler(this.btnGetFile_Click);
      // 
      // cboFileName
      // 
      this.cboFileName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.cboFileName.FormattingEnabled = true;
      this.cboFileName.Location = new System.Drawing.Point(78, 9);
      this.cboFileName.Name = "cboFileName";
      this.cboFileName.Size = new System.Drawing.Size(520, 21);
      this.cboFileName.TabIndex = 15;
      this.cboFileName.Validated += new System.EventHandler(this.cboFileName_Validated);
      // 
      // openFileDialog
      // 
      this.openFileDialog.FileName = "File";
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(12, 14);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(60, 13);
      this.label4.TabIndex = 16;
      this.label4.Text = "File to play:";
      // 
      // groupBox1
      // 
      this.groupBox1.Controls.Add(this.label20);
      this.groupBox1.Controls.Add(this.lblCurrentFileType);
      this.groupBox1.Controls.Add(this.label6);
      this.groupBox1.Controls.Add(this.lblCurrentFilePath);
      this.groupBox1.Controls.Add(this.label5);
      this.groupBox1.Controls.Add(this.lblLastError);
      this.groupBox1.Controls.Add(this.label2);
      this.groupBox1.Controls.Add(this.label1);
      this.groupBox1.Controls.Add(this.lblPlaybackMode);
      this.groupBox1.Controls.Add(this.lblCurrentPosition);
      this.groupBox1.Location = new System.Drawing.Point(15, 38);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(611, 92);
      this.groupBox1.TabIndex = 23;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Properties";
      // 
      // label20
      // 
      this.label20.AutoSize = true;
      this.label20.Location = new System.Drawing.Point(19, 29);
      this.label20.Name = "label20";
      this.label20.Size = new System.Drawing.Size(84, 13);
      this.label20.TabIndex = 32;
      this.label20.Text = "CurrentFileType:";
      // 
      // lblCurrentFileType
      // 
      this.lblCurrentFileType.AutoSize = true;
      this.lblCurrentFileType.Location = new System.Drawing.Point(120, 29);
      this.lblCurrentFileType.Name = "lblCurrentFileType";
      this.lblCurrentFileType.Size = new System.Drawing.Size(91, 13);
      this.lblCurrentFileType.TabIndex = 31;
      this.lblCurrentFileType.Text = "lblCurrentFileType";
      // 
      // label6
      // 
      this.label6.AutoSize = true;
      this.label6.Location = new System.Drawing.Point(19, 16);
      this.label6.Name = "label6";
      this.label6.Size = new System.Drawing.Size(82, 13);
      this.label6.TabIndex = 30;
      this.label6.Text = "CurrentFilePath:";
      // 
      // lblCurrentFilePath
      // 
      this.lblCurrentFilePath.AutoSize = true;
      this.lblCurrentFilePath.Location = new System.Drawing.Point(120, 16);
      this.lblCurrentFilePath.Name = "lblCurrentFilePath";
      this.lblCurrentFilePath.Size = new System.Drawing.Size(89, 13);
      this.lblCurrentFilePath.TabIndex = 29;
      this.lblCurrentFilePath.Text = "lblCurrentFilePath";
      // 
      // label5
      // 
      this.label5.AutoSize = true;
      this.label5.Location = new System.Drawing.Point(18, 69);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(52, 13);
      this.label5.TabIndex = 28;
      this.label5.Text = "LastError:";
      // 
      // lblLastError
      // 
      this.lblLastError.AutoSize = true;
      this.lblLastError.Location = new System.Drawing.Point(120, 69);
      this.lblLastError.Name = "lblLastError";
      this.lblLastError.Size = new System.Drawing.Size(59, 13);
      this.lblLastError.TabIndex = 27;
      this.lblLastError.Text = "lblLastError";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(17, 42);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(83, 13);
      this.label2.TabIndex = 26;
      this.label2.Text = "Playback mode:";
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(19, 55);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(81, 13);
      this.label1.TabIndex = 25;
      this.label1.Text = "CurrentPosition:";
      // 
      // lblPlaybackMode
      // 
      this.lblPlaybackMode.AutoSize = true;
      this.lblPlaybackMode.Location = new System.Drawing.Point(120, 42);
      this.lblPlaybackMode.Name = "lblPlaybackMode";
      this.lblPlaybackMode.Size = new System.Drawing.Size(88, 13);
      this.lblPlaybackMode.TabIndex = 24;
      this.lblPlaybackMode.Text = "lblPlaybackMode";
      // 
      // lblCurrentPosition
      // 
      this.lblCurrentPosition.AutoSize = true;
      this.lblCurrentPosition.Location = new System.Drawing.Point(120, 55);
      this.lblCurrentPosition.Name = "lblCurrentPosition";
      this.lblCurrentPosition.Size = new System.Drawing.Size(88, 13);
      this.lblCurrentPosition.TabIndex = 23;
      this.lblCurrentPosition.Text = "lblCurrentPosition";
      // 
      // label7
      // 
      this.label7.AutoSize = true;
      this.label7.Location = new System.Drawing.Point(15, 135);
      this.label7.Name = "label7";
      this.label7.Size = new System.Drawing.Size(28, 13);
      this.label7.TabIndex = 24;
      this.label7.Text = "Log:";
      // 
      // PureAudioTest
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(750, 528);
      this.Controls.Add(this.label7);
      this.Controls.Add(this.groupBox1);
      this.Controls.Add(this.label4);
      this.Controls.Add(this.cboFileName);
      this.Controls.Add(this.btnGetFile);
      this.Controls.Add(this.tbLog);
      this.Controls.Add(this.label3);
      this.Controls.Add(this.tbEvents);
      this.Controls.Add(this.btnConfig);
      this.Controls.Add(this.btnToggleMode);
      this.Controls.Add(this.btnForward);
      this.Controls.Add(this.btnBack);
      this.Controls.Add(this.btnPause);
      this.Controls.Add(this.btnStop);
      this.Controls.Add(this.btnStart);
      this.Name = "PureAudioTest";
      this.Text = "PureAudio Tester";
      this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.PureAudioTest_FormClosed);
      this.Load += new System.EventHandler(this.PureAudioTest_Load);
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnStart;
		private System.Windows.Forms.Button btnStop;
    private System.Windows.Forms.Button btnPause;
		private System.Windows.Forms.Timer tmrStatus;
		private System.Windows.Forms.Button btnBack;
		private System.Windows.Forms.Button btnForward;
    private System.Windows.Forms.Button btnToggleMode;
    private System.Windows.Forms.Button btnConfig;
    private System.Windows.Forms.TextBox tbEvents;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.TextBox tbLog;
    private System.Windows.Forms.Button btnGetFile;
    private System.Windows.Forms.ComboBox cboFileName;
    private System.Windows.Forms.OpenFileDialog openFileDialog;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.Label label20;
    private System.Windows.Forms.Label lblCurrentFileType;
    private System.Windows.Forms.Label label6;
    private System.Windows.Forms.Label lblCurrentFilePath;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.Label lblLastError;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label lblPlaybackMode;
    private System.Windows.Forms.Label lblCurrentPosition;
    private System.Windows.Forms.Label label7;
	}
}

