namespace MediaPortal.Plugins.PureAudio.Configuration.Sections
{
  partial class Extensions
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
      this.groupBox2 = new System.Windows.Forms.GroupBox();
      this.chkUseForLastFMWebStream = new System.Windows.Forms.CheckBox();
      this.chkUseForWebStream = new System.Windows.Forms.CheckBox();
      this.chkUseForCDDA = new System.Windows.Forms.CheckBox();
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.mpLabel23 = new MediaPortal.UserInterface.Controls.MPLabel();
      this.lvExtensions = new System.Windows.Forms.ListView();
      this.tbExtension = new System.Windows.Forms.TextBox();
      this.btnDefault = new MediaPortal.UserInterface.Controls.MPButton();
      this.btnDeleteExt = new MediaPortal.UserInterface.Controls.MPButton();
      this.btnAddExt = new MediaPortal.UserInterface.Controls.MPButton();
      this.groupBox2.SuspendLayout();
      this.groupBox1.SuspendLayout();
      this.SuspendLayout();
      // 
      // groupBox2
      // 
      this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.groupBox2.Controls.Add(this.chkUseForLastFMWebStream);
      this.groupBox2.Controls.Add(this.chkUseForWebStream);
      this.groupBox2.Controls.Add(this.chkUseForCDDA);
      this.groupBox2.Location = new System.Drawing.Point(0, 222);
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.Size = new System.Drawing.Size(497, 86);
      this.groupBox2.TabIndex = 5;
      this.groupBox2.TabStop = false;
      this.groupBox2.Text = "Options";
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
      // groupBox1
      // 
      this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.groupBox1.Controls.Add(this.mpLabel23);
      this.groupBox1.Controls.Add(this.lvExtensions);
      this.groupBox1.Controls.Add(this.tbExtension);
      this.groupBox1.Controls.Add(this.btnDefault);
      this.groupBox1.Controls.Add(this.btnDeleteExt);
      this.groupBox1.Controls.Add(this.btnAddExt);
      this.groupBox1.Location = new System.Drawing.Point(0, 0);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(497, 216);
      this.groupBox1.TabIndex = 4;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Extensions this player will be used for";
      // 
      // mpLabel23
      // 
      this.mpLabel23.Location = new System.Drawing.Point(206, 20);
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
      this.lvExtensions.Size = new System.Drawing.Size(404, 165);
      this.lvExtensions.Sorting = System.Windows.Forms.SortOrder.Ascending;
      this.lvExtensions.TabIndex = 3;
      this.lvExtensions.UseCompatibleStateImageBehavior = false;
      this.lvExtensions.View = System.Windows.Forms.View.List;
      // 
      // tbExtension
      // 
      this.tbExtension.AcceptsReturn = true;
      this.tbExtension.Location = new System.Drawing.Point(335, 18);
      this.tbExtension.Name = "tbExtension";
      this.tbExtension.Size = new System.Drawing.Size(75, 20);
      this.tbExtension.TabIndex = 1;
      // 
      // btnDefault
      // 
      this.btnDefault.Location = new System.Drawing.Point(416, 74);
      this.btnDefault.Name = "btnDefault";
      this.btnDefault.Size = new System.Drawing.Size(75, 23);
      this.btnDefault.TabIndex = 5;
      this.btnDefault.Text = "Default";
      this.btnDefault.UseVisualStyleBackColor = true;
      this.btnDefault.Click += new System.EventHandler(this.btnDefault_Click);
      // 
      // btnDeleteExt
      // 
      this.btnDeleteExt.Location = new System.Drawing.Point(416, 45);
      this.btnDeleteExt.Name = "btnDeleteExt";
      this.btnDeleteExt.Size = new System.Drawing.Size(75, 23);
      this.btnDeleteExt.TabIndex = 4;
      this.btnDeleteExt.Text = "Delete";
      this.btnDeleteExt.UseVisualStyleBackColor = true;
      this.btnDeleteExt.Click += new System.EventHandler(this.btnDeleteExt_Click);
      // 
      // btnAddExt
      // 
      this.btnAddExt.Location = new System.Drawing.Point(416, 16);
      this.btnAddExt.Name = "btnAddExt";
      this.btnAddExt.Size = new System.Drawing.Size(75, 23);
      this.btnAddExt.TabIndex = 2;
      this.btnAddExt.Text = "Add";
      this.btnAddExt.UseVisualStyleBackColor = true;
      this.btnAddExt.Click += new System.EventHandler(this.btnAddExt_Click);
      // 
      // Extensions
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.groupBox2);
      this.Controls.Add(this.groupBox1);
      this.Name = "Extensions";
      this.Validating += new System.ComponentModel.CancelEventHandler(this.Extensions_Validating);
      this.groupBox2.ResumeLayout(false);
      this.groupBox2.PerformLayout();
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.GroupBox groupBox2;
    private System.Windows.Forms.CheckBox chkUseForLastFMWebStream;
    private System.Windows.Forms.CheckBox chkUseForWebStream;
    private System.Windows.Forms.CheckBox chkUseForCDDA;
    private System.Windows.Forms.GroupBox groupBox1;
    private MediaPortal.UserInterface.Controls.MPLabel mpLabel23;
    private System.Windows.Forms.ListView lvExtensions;
    private System.Windows.Forms.TextBox tbExtension;
    private MediaPortal.UserInterface.Controls.MPButton btnDefault;
    private MediaPortal.UserInterface.Controls.MPButton btnDeleteExt;
    private MediaPortal.UserInterface.Controls.MPButton btnAddExt;
  }
}
