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
			this.SuspendLayout();
			// 
			// checkListChangedPaths
			// 
			this.checkListChangedPaths.FormattingEnabled = true;
			this.checkListChangedPaths.Location = new System.Drawing.Point(12, 42);
			this.checkListChangedPaths.Name = "checkListChangedPaths";
			this.checkListChangedPaths.Size = new System.Drawing.Size(333, 559);
			this.checkListChangedPaths.TabIndex = 0;
			// 
			// btnMerge
			// 
			this.btnMerge.Location = new System.Drawing.Point(382, 54);
			this.btnMerge.Name = "btnMerge";
			this.btnMerge.Size = new System.Drawing.Size(224, 46);
			this.btnMerge.TabIndex = 1;
			this.btnMerge.Text = "Merge Selected";
			this.btnMerge.UseVisualStyleBackColor = true;
			this.btnMerge.Click += new System.EventHandler(this.btnMerge_Click);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(864, 606);
			this.Controls.Add(this.btnMerge);
			this.Controls.Add(this.checkListChangedPaths);
			this.Name = "Form1";
			this.Text = "TortoiseDeploy";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.CheckedListBox checkListChangedPaths;
		private System.Windows.Forms.Button btnMerge;
	}
}

