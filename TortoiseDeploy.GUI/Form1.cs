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
		string errors;

		public Form1(List<String> changedPaths, string errors) {
			this.changedPaths = changedPaths;
			this.errors = errors;

			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e) {

			// Create our deployer instance so that we can merge/deploy the files
			deployer = new TortoiseDeploy();

			// Only do our initial setup if there weren't any errors on startup
			if (String.IsNullOrEmpty(errors)) {
				// Identify the longest display name in our list of changed paths
				int maxPathLength = 0;
				if (changedPaths.Count > 0) {
					changedPaths.Max(s => deployer.GetDisplayName(s).Length);
				}

				// Create a source => destination display string for each file
				deploymentMappings = new List<DeploymentMapping>();
				foreach (string path in changedPaths) {
					string displayPath = deployer.GetDisplayName(path);
					// Pad the displayname up to the longest one, so that our => symbol shows consistently
					displayPath += new string(' ', Math.Abs(displayPath.Length - maxPathLength));
					displayPath += "\t=>\t" + deployer.GetDeploymentTarget(path);
					deploymentMappings.Add(new DeploymentMapping(path, displayPath));
				}

				// Set the list of files that changed.
				// If there weren't any, we'll show our usage message
				if (deploymentMappings.Count > 0) {
					((ListBox)this.checkListChangedPaths).DataSource = deploymentMappings;
				}
			} else {
				// If there were any errors, add them to the deployer log.
				// This will ensure they're shown in the GUI, and written to the log file.
				deployer.LogMessage(errors);
			}

			// Update the GUI log box
			OverwriteLog(deployer.Log);
		}

		/// <summary>
		/// Replace the text in the log output text box
		/// </summary>
		/// <param name="log">Text to display</param>
		private void OverwriteLog(string log) {
			this.txtLog.Text = log;
		}

		/// <summary>
		/// Get the list of DeploymentMapping objects representing all of the selected files
		/// </summary>
		/// <returns>List of selected DeploymentMapping items</returns>
		private List<DeploymentMapping> getSelected() {
			List<DeploymentMapping> returnVal = new List<DeploymentMapping>();

			foreach(var selected in this.checkListChangedPaths.CheckedItems) {
				returnVal.Add(this.deploymentMappings.Where(m => m.Display == selected.ToString()).First());
			}

			return returnVal;
		}

		/// <summary>
		/// Handle the click event of the Merge button.
		/// This uses the deployer to launch the configured merge tool for all selected files.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnMerge_Click(object sender, EventArgs e) {
			
			// Loop through each of the selected files individually and launch the merge tool
			foreach(var i in getSelected()) {
				deployer.Merge(i.Source, deployer.GetDeploymentTarget(i.Source));
			}

			// Update the log output
			OverwriteLog(deployer.Log);
		}

		/// <summary>
		/// Handle the click event of the Deploy button.
		/// This uses the deployer to deploy all the selected files
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnDeploy_Click(object sender, EventArgs e) {
			
			// Loop through all of our selected files, and deploy them one at a time.
			foreach (var i in getSelected()) {
				deployer.Deploy(i.Source, deployer.GetDeploymentTarget(i.Source));
			}

			// Update the log output
			OverwriteLog(deployer.Log);
		}

		/// <summary>
		/// Handle click events on the "Select All / None" checkbox.
		/// This will select / deselect all of the files in the list.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
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

		/// <summary>
		/// Handle clicks on links in the rich text box.
		/// When a link is clicked, we launch the users default browser to navigate
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void richTextBox1_LinkClicked(object sender, LinkClickedEventArgs e) {
			System.Diagnostics.Process.Start(e.LinkText.ToString());
		}
	}
}
