namespace TortoiseDeploy.GUI {
	partial class Form1 {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.checkListChangedPaths = new System.Windows.Forms.CheckedListBox();
			this.btnMerge = new System.Windows.Forms.Button();
			this.chkBoxSelectAll = new System.Windows.Forms.CheckBox();
			this.btnDeploy = new System.Windows.Forms.Button();
			this.txtLog = new System.Windows.Forms.RichTextBox();
			this.richTextBox1 = new System.Windows.Forms.RichTextBox();
			this.progressBar = new System.Windows.Forms.ProgressBar();
			this.SuspendLayout();
			// 
			// checkListChangedPaths
			// 
			this.checkListChangedPaths.CheckOnClick = true;
			this.checkListChangedPaths.FormattingEnabled = true;
			this.checkListChangedPaths.Items.AddRange(new object[] {
            "TortoiseDeploy needs to be called as a TortoiseSVN Post Commit hook.",
            "Check out the documentation at https://tortoisesvn.net/docs/release/TortoiseSVN_e" +
                "n/tsvn-dug-settings.html for further information"});
			this.checkListChangedPaths.Location = new System.Drawing.Point(12, 12);
			this.checkListChangedPaths.Name = "checkListChangedPaths";
			this.checkListChangedPaths.Size = new System.Drawing.Size(769, 259);
			this.checkListChangedPaths.TabIndex = 0;
			// 
			// btnMerge
			// 
			this.btnMerge.Location = new System.Drawing.Point(12, 300);
			this.btnMerge.Name = "btnMerge";
			this.btnMerge.Size = new System.Drawing.Size(224, 46);
			this.btnMerge.TabIndex = 1;
			this.btnMerge.Text = "Merge Selected";
			this.btnMerge.UseVisualStyleBackColor = true;
			this.btnMerge.Click += new System.EventHandler(this.btnMerge_Click);
			// 
			// chkBoxSelectAll
			// 
			this.chkBoxSelectAll.AutoSize = true;
			this.chkBoxSelectAll.Location = new System.Drawing.Point(12, 277);
			this.chkBoxSelectAll.Name = "chkBoxSelectAll";
			this.chkBoxSelectAll.Size = new System.Drawing.Size(107, 17);
			this.chkBoxSelectAll.TabIndex = 2;
			this.chkBoxSelectAll.Text = "Select All / None";
			this.chkBoxSelectAll.UseVisualStyleBackColor = true;
			this.chkBoxSelectAll.CheckedChanged += new System.EventHandler(this.chkBoxSelectAll_CheckedChanged);
			// 
			// btnDeploy
			// 
			this.btnDeploy.Location = new System.Drawing.Point(242, 300);
			this.btnDeploy.Name = "btnDeploy";
			this.btnDeploy.Size = new System.Drawing.Size(224, 46);
			this.btnDeploy.TabIndex = 3;
			this.btnDeploy.Text = "Deploy Selected";
			this.btnDeploy.UseVisualStyleBackColor = true;
			this.btnDeploy.Click += new System.EventHandler(this.btnDeploy_Click);
			// 
			// txtLog
			// 
			this.txtLog.Location = new System.Drawing.Point(12, 352);
			this.txtLog.Name = "txtLog";
			this.txtLog.ReadOnly = true;
			this.txtLog.Size = new System.Drawing.Size(769, 242);
			this.txtLog.TabIndex = 4;
			this.txtLog.Text = "";
			// 
			// richTextBox1
			// 
			this.richTextBox1.Location = new System.Drawing.Point(479, 300);
			this.richTextBox1.Name = "richTextBox1";
			this.richTextBox1.ReadOnly = true;
			this.richTextBox1.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
			this.richTextBox1.Size = new System.Drawing.Size(301, 45);
			this.richTextBox1.TabIndex = 5;
			this.richTextBox1.Text = "Contribute on Github! https://github.com/MitchReidNZ/TortoiseDeploy";
			this.richTextBox1.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.richTextBox1_LinkClicked);
			// 
			// progressBar
			// 
			this.progressBar.Location = new System.Drawing.Point(125, 277);
			this.progressBar.Name = "progressBar";
			this.progressBar.Size = new System.Drawing.Size(655, 17);
			this.progressBar.TabIndex = 6;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(794, 606);
			this.Controls.Add(this.progressBar);
			this.Controls.Add(this.richTextBox1);
			this.Controls.Add(this.txtLog);
			this.Controls.Add(this.btnDeploy);
			this.Controls.Add(this.chkBoxSelectAll);
			this.Controls.Add(this.btnMerge);
			this.Controls.Add(this.checkListChangedPaths);
			this.Name = "Form1";
			this.Text = "TortoiseDeploy";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.CheckedListBox checkListChangedPaths;
		private System.Windows.Forms.Button btnMerge;
		private System.Windows.Forms.CheckBox chkBoxSelectAll;
		private System.Windows.Forms.Button btnDeploy;
		private System.Windows.Forms.RichTextBox txtLog;
		private System.Windows.Forms.RichTextBox richTextBox1;
		private System.Windows.Forms.ProgressBar progressBar;
	}
}

