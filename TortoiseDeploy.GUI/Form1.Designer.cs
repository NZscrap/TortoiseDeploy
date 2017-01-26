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
			this.btnOpen = new System.Windows.Forms.Button();
			this.progressBar = new System.Windows.Forms.ProgressBar();
			this.richTextBox1 = new System.Windows.Forms.RichTextBox();
			this.txtLog = new System.Windows.Forms.RichTextBox();
			this.btnDeploy = new System.Windows.Forms.Button();
			this.chkBoxSelectAll = new System.Windows.Forms.CheckBox();
			this.btnMerge = new System.Windows.Forms.Button();
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.openConfigToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.reloadConfigToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.lvChangedPaths = new System.Windows.Forms.ListView();
			this.Source = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.Destination = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
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
			this.splitContainer1.Panel1.Controls.Add(this.lvChangedPaths);
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
			this.chkBoxSelectAll.Checked = true;
			this.chkBoxSelectAll.CheckState = System.Windows.Forms.CheckState.Checked;
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
			this.openConfigToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
			this.openConfigToolStripMenuItem.Text = "Edit Config";
			this.openConfigToolStripMenuItem.Click += new System.EventHandler(this.openConfigToolStripMenuItem_Click);
			// 
			// reloadConfigToolStripMenuItem
			// 
			this.reloadConfigToolStripMenuItem.Name = "reloadConfigToolStripMenuItem";
			this.reloadConfigToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
			this.reloadConfigToolStripMenuItem.Text = "Reload Config";
			this.reloadConfigToolStripMenuItem.Click += new System.EventHandler(this.reloadConfigToolStripMenuItem_Click);
			// 
			// exitToolStripMenuItem
			// 
			this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
			this.exitToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
			this.exitToolStripMenuItem.Text = "Exit";
			this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
			// 
			// contextMenuStrip1
			// 
			this.contextMenuStrip1.Name = "contextMenuStrip1";
			this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
			// 
			// lvChangedPaths
			// 
			this.lvChangedPaths.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lvChangedPaths.CheckBoxes = true;
			this.lvChangedPaths.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Source,
            this.Destination});
			this.lvChangedPaths.Location = new System.Drawing.Point(3, 3);
			this.lvChangedPaths.Name = "lvChangedPaths";
			this.lvChangedPaths.Size = new System.Drawing.Size(758, 247);
			this.lvChangedPaths.TabIndex = 3;
			this.lvChangedPaths.UseCompatibleStateImageBehavior = false;
			this.lvChangedPaths.View = System.Windows.Forms.View.Details;
			// 
			// Source
			// 
			this.Source.Text = "Source";
			this.Source.Width = 200;
			// 
			// Destination
			// 
			this.Destination.Text = "Destination";
			this.Destination.Width = 554;
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
			this.ResizeEnd += new System.EventHandler(this.Form1_ResizeEnd);
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
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem openConfigToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem reloadConfigToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
		private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
		private System.Windows.Forms.ListView lvChangedPaths;
		private System.Windows.Forms.ColumnHeader Source;
		private System.Windows.Forms.ColumnHeader Destination;
	}
}

