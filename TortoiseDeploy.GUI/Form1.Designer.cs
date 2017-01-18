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
			this.components = new System.ComponentModel.Container();
			this.fileCopyWorker = new System.ComponentModel.BackgroundWorker();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.listChangedPaths = new System.Windows.Forms.ListBox();
			this.btnOpen = new System.Windows.Forms.Button();
			this.progressBar = new System.Windows.Forms.ProgressBar();
			this.richTextBox1 = new System.Windows.Forms.RichTextBox();
			this.txtLog = new System.Windows.Forms.RichTextBox();
			this.btnDeploy = new System.Windows.Forms.Button();
			this.chkBoxSelectAll = new System.Windows.Forms.CheckBox();
			this.btnMerge = new System.Windows.Forms.Button();
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.openConfigToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.reloadConfigToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.menuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// fileCopyWorker
			// 
			this.fileCopyWorker.WorkerReportsProgress = true;
			this.fileCopyWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker_DoWork);
			this.fileCopyWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker_ProgressChanged);
			this.fileCopyWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.fileCopyWorker_RunWorkerCompleted);
			// 
			// splitContainer1
			// 
			this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.splitContainer1.Location = new System.Drawing.Point(0, 27);
			this.splitContainer1.Name = "splitContainer1";
			this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.listChangedPaths);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.btnOpen);
			this.splitContainer1.Panel2.Controls.Add(this.progressBar);
			this.splitContainer1.Panel2.Controls.Add(this.richTextBox1);
			this.splitContainer1.Panel2.Controls.Add(this.txtLog);
			this.splitContainer1.Panel2.Controls.Add(this.btnDeploy);
			this.splitContainer1.Panel2.Controls.Add(this.chkBoxSelectAll);
			this.splitContainer1.Panel2.Controls.Add(this.btnMerge);
			this.splitContainer1.Size = new System.Drawing.Size(766, 527);
			this.splitContainer1.SplitterDistance = 263;
			this.splitContainer1.TabIndex = 8;
			// 
			// listChangedPaths
			// 
			this.listChangedPaths.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.listChangedPaths.FormattingEnabled = true;
			this.listChangedPaths.Location = new System.Drawing.Point(-1, -1);
			this.listChangedPaths.Name = "listChangedPaths";
			this.listChangedPaths.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.listChangedPaths.Size = new System.Drawing.Size(762, 251);
			this.listChangedPaths.TabIndex = 2;
			// 
			// btnOpen
			// 
			this.btnOpen.Location = new System.Drawing.Point(331, 26);
			this.btnOpen.Name = "btnOpen";
			this.btnOpen.Size = new System.Drawing.Size(155, 46);
			this.btnOpen.TabIndex = 14;
			this.btnOpen.Text = "Open Selected";
			this.btnOpen.UseVisualStyleBackColor = true;
			this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
			// 
			// progressBar
			// 
			this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.progressBar.Location = new System.Drawing.Point(125, 3);
			this.progressBar.Name = "progressBar";
			this.progressBar.Size = new System.Drawing.Size(628, 17);
			this.progressBar.TabIndex = 13;
			// 
			// richTextBox1
			// 
			this.richTextBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.richTextBox1.Location = new System.Drawing.Point(492, 26);
			this.richTextBox1.Name = "richTextBox1";
			this.richTextBox1.ReadOnly = true;
			this.richTextBox1.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
			this.richTextBox1.Size = new System.Drawing.Size(261, 45);
			this.richTextBox1.TabIndex = 12;
			this.richTextBox1.Text = "Contribute on Github! https://github.com/MitchReidNZ/TortoiseDeploy";
			// 
			// txtLog
			// 
			this.txtLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtLog.Location = new System.Drawing.Point(3, 78);
			this.txtLog.Name = "txtLog";
			this.txtLog.ReadOnly = true;
			this.txtLog.Size = new System.Drawing.Size(750, 180);
			this.txtLog.TabIndex = 11;
			this.txtLog.Text = "";
			// 
			// btnDeploy
			// 
			this.btnDeploy.Location = new System.Drawing.Point(170, 26);
			this.btnDeploy.Name = "btnDeploy";
			this.btnDeploy.Size = new System.Drawing.Size(155, 46);
			this.btnDeploy.TabIndex = 10;
			this.btnDeploy.Text = "Deploy Selected";
			this.btnDeploy.UseVisualStyleBackColor = true;
			this.btnDeploy.Click += new System.EventHandler(this.btnDeploy_Click);
			// 
			// chkBoxSelectAll
			// 
			this.chkBoxSelectAll.AutoSize = true;
			this.chkBoxSelectAll.Location = new System.Drawing.Point(12, 3);
			this.chkBoxSelectAll.Name = "chkBoxSelectAll";
			this.chkBoxSelectAll.Size = new System.Drawing.Size(107, 17);
			this.chkBoxSelectAll.TabIndex = 9;
			this.chkBoxSelectAll.Text = "Select All / None";
			this.chkBoxSelectAll.UseVisualStyleBackColor = true;
			this.chkBoxSelectAll.CheckedChanged += new System.EventHandler(this.chkBoxSelectAll_CheckedChanged);
			// 
			// btnMerge
			// 
			this.btnMerge.Location = new System.Drawing.Point(9, 26);
			this.btnMerge.Name = "btnMerge";
			this.btnMerge.Size = new System.Drawing.Size(155, 46);
			this.btnMerge.TabIndex = 8;
			this.btnMerge.Text = "Merge Selected";
			this.btnMerge.UseVisualStyleBackColor = true;
			this.btnMerge.Click += new System.EventHandler(this.btnMerge_Click);
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(763, 24);
			this.menuStrip1.TabIndex = 9;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// contextMenuStrip1
			// 
			this.contextMenuStrip1.Name = "contextMenuStrip1";
			this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
			// 
			// fileToolStripMenuItem
			// 
			this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openConfigToolStripMenuItem,
            this.reloadConfigToolStripMenuItem,
            this.exitToolStripMenuItem});
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
			this.fileToolStripMenuItem.Text = "File";
			// 
			// openConfigToolStripMenuItem
			// 
			this.openConfigToolStripMenuItem.Name = "openConfigToolStripMenuItem";
			this.openConfigToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.openConfigToolStripMenuItem.Text = "Edit Config";
			this.openConfigToolStripMenuItem.Click += new System.EventHandler(this.openConfigToolStripMenuItem_Click);
			// 
			// reloadConfigToolStripMenuItem
			// 
			this.reloadConfigToolStripMenuItem.Name = "reloadConfigToolStripMenuItem";
			this.reloadConfigToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.reloadConfigToolStripMenuItem.Text = "Reload Config";
			this.reloadConfigToolStripMenuItem.Click += new System.EventHandler(this.reloadConfigToolStripMenuItem_Click);
			// 
			// exitToolStripMenuItem
			// 
			this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
			this.exitToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.exitToolStripMenuItem.Text = "Exit";
			this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(763, 550);
			this.Controls.Add(this.splitContainer1);
			this.Controls.Add(this.menuStrip1);
			this.MainMenuStrip = this.menuStrip1;
			this.Name = "Form1";
			this.Text = "TortoiseDeploy";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.Panel2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.ComponentModel.BackgroundWorker fileCopyWorker;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.Button btnOpen;
		private System.Windows.Forms.ProgressBar progressBar;
		private System.Windows.Forms.RichTextBox richTextBox1;
		private System.Windows.Forms.RichTextBox txtLog;
		private System.Windows.Forms.Button btnDeploy;
		private System.Windows.Forms.CheckBox chkBoxSelectAll;
		private System.Windows.Forms.Button btnMerge;
		private System.Windows.Forms.ListBox listChangedPaths;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem openConfigToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem reloadConfigToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
		private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
	}
}

