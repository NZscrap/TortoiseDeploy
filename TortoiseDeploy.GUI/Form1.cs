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
		List<DeploymentMapping> deploymentMappings;

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
			deploymentMappings = new List<DeploymentMapping>();
			foreach(string path in changedPaths) {
				string displayPath = deployer.GetDisplayName(path);
				// Pad the displayname up to the longest one, so that our => symbol shows consistently
				displayPath += new string(' ', Math.Abs(displayPath.Length - maxPathLength));
				displayPath += "\t=>\t" + deployer.GetDeploymentTarget(path);
				deploymentMappings.Add(new DeploymentMapping(path, displayPath));
			}

			// Set the list of files that changed
			((ListBox)this.checkListChangedPaths).DataSource = deploymentMappings;

			OverwriteLog(deployer.Log);
		}

		public void OverwriteLog(string log) {
			this.txtLog.Text = log;
		}

		private List<DeploymentMapping> getSelected() {
			List<DeploymentMapping> returnVal = new List<DeploymentMapping>();

			foreach(var selected in this.checkListChangedPaths.CheckedItems) {
				returnVal.Add(this.deploymentMappings.Where(m => m.Display == selected.ToString()).First());
			}

			return returnVal;
		}

		private void btnMerge_Click(object sender, EventArgs e) {
			foreach(var i in getSelected()) {
				deployer.Merge(i.Source, deployer.GetDeploymentTarget(i.Source));
			}

			// Update the log output
			OverwriteLog(deployer.Log);
		}

		private void btnDeploy_Click(object sender, EventArgs e) {
			foreach (var i in getSelected()) {
				deployer.Deploy(i.Source, deployer.GetDeploymentTarget(i.Source));
			}

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

		private void richTextBox1_LinkClicked(object sender, LinkClickedEventArgs e) {
			System.Diagnostics.Process.Start(e.LinkText.ToString());
		}
	}
}
