using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TortoiseDeploy;

namespace TortoiseDeploy.GUI {
	public partial class Form1 : Form {

		TortoiseDeploy deployer;
		List<string> changedPaths;

		public Form1() {
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e) {

			// Create our deployer instance so that we can merge/deploy the files
			deployer = new TortoiseDeploy();

			changedPaths = new List<String>();
			changedPaths.Add("C:\\Users\\mitch\\Desktop\\local tortoiseDeploy tests\\other test.txt");
			changedPaths.Add("C:\\Users\\mitch\\Desktop\\local tortoiseDeploy tests\\test.txt");

			// Identify the longest display name in our list of changed paths
			int maxPathLength = changedPaths.Max(s => deployer.GetDisplayName(s).Length);

			// Create a source => destination display string for each file
			List<string> display = new List<string>();
			foreach(string path in changedPaths) {
				string displayPath = deployer.GetDisplayName(path);
				// Pad the displayname up to the longest one, so that our => symbol shows consistently
				displayPath += new string(' ', Math.Abs(displayPath.Length - maxPathLength));
				display.Add(displayPath + "\t=>\t" + deployer.GetDeploymentTarget(path));
			}

			// Set the list of files that changed
			((ListBox)this.checkListChangedPaths).DataSource = display;

			OverwriteLog(deployer.Log);
		}

		public void OverwriteLog(string log) {
			this.txtLog.Text = log;
		}

		private void btnMerge_Click(object sender, EventArgs e) {
			StringBuilder output = new StringBuilder();

			foreach(var i in this.checkListChangedPaths.CheckedItems) {
				output.AppendLine(i.ToString());
			}

			MessageBox.Show(output.ToString());

			// Update the log output
			OverwriteLog(deployer.Log);
		}

		private void chkBoxSelectAll_CheckedChanged(object sender, EventArgs e) {
			if (this.chkBoxSelectAll.Checked) {
				for(int i = 0; i < this.checkListChangedPaths.Items.Count; i++) {
					this.checkListChangedPaths.SetItemChecked(i, true);
				}
			} else {
				for (int i = 0; i < this.checkListChangedPaths.Items.Count; i++) {
					this.checkListChangedPaths.SetItemChecked(i, false);
				}
			}
		}

		private void btnDeploy_Click(object sender, EventArgs e) {
			// Update the log output
			OverwriteLog(deployer.Log);
		}
	}
}
