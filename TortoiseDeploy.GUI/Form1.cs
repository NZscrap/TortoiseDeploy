using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TortoiseDeploy;

namespace TortoiseDeploy.GUI {
	public partial class Form1 : Form {

		TortoiseDeploy deployer;
		List<string> changedPaths;
		List<DeploymentDisplay> deploymentMappings;
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
				LoadDeploymentMappings();
			} else {
				// If there were any errors, add them to the deployer log.
				// This will ensure they're shown in the GUI, and written to the log file.
				deployer.LogMessage(errors);
			}

			// Update the GUI log box
			OverwriteLog(deployer.Log);
		}

		private void LoadDeploymentMappings() {
			// Create a source => destination display string for each file
			deploymentMappings = new List<DeploymentDisplay>();
			foreach (string path in changedPaths) {
				string destination = deployer.GetDeploymentTarget(path);

				// Create our display object
				deploymentMappings.Add(new DeploymentDisplay(path, destination));
			}

			// Populate the list of files that changed.
			foreach(var i in deploymentMappings) {
				ListViewItem entry = new ListViewItem(new string[] { i.Source, i.Destination });
				entry.Checked = true;
				this.lvChangedPaths.Items.Add(entry);
			}
			resizeListView();
		}

		/// <summary>
		/// Replace the text in the log output text box
		/// </summary>
		/// <param name="log">Text to display</param>
		private void OverwriteLog(string log) {
			// First, we clear the old log message
			this.txtLog.Text = "";

			// Richtextbox's will auto-scroll to the end if they have focus and we call AppendText
			this.txtLog.Focus();
			this.txtLog.AppendText(log);
		}

		/// <summary>
		/// Get the list of DeploymentMapping objects representing all of the selected files
		/// </summary>
		/// <returns>List of selected DeploymentMapping items</returns>
		private List<DeploymentDisplay> getSelected() {
			List<DeploymentDisplay> returnVal = new List<DeploymentDisplay>();

			foreach(var selected in this.lvChangedPaths.CheckedItems) {
				returnVal.Add(this.deploymentMappings.Where(m => m.Source == ((ListViewItem)selected).Text).First());
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

			// Ensure the progressbar settings are reset
			this.progressBar.Minimum = 0;
			this.progressBar.Maximum = getSelected().Count;
			this.progressBar.Value = 0;

			// Loop through each of the selected files individually and launch the merge tool
			foreach (var i in getSelected()) {
				deployer.Merge(i.Source, i.Destination);
				this.progressBar.Value++;	// Update the progressbar
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

			// Ensure the progressbar settings are reset
			this.progressBar.Minimum = 0;
			this.progressBar.Maximum = getSelected().Count;
			this.progressBar.Value = 0;

			// Disable the deploy button until the files are deployed
			this.btnDeploy.Enabled = false;

			// Use the background worker to merge the files
			this.fileCopyWorker.RunWorkerAsync(getSelected());

			// Update the log output
			OverwriteLog(deployer.Log);
		}

		/// <summary>
		/// Handle the click event of the Open button. This launches the default tool to handle these files, as configured by the OS
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnOpen_Click(object sender, EventArgs e) {
			// Ensure the progressbar settings are reset
			this.progressBar.Minimum = 0;
			this.progressBar.Maximum = getSelected().Count;
			this.progressBar.Value = 0;

			foreach (DeploymentDisplay i in getSelected()) {
				System.Diagnostics.Process.Start(i.Source);
				this.progressBar.Value++;   // Update the progressbar
			}
		}

		/// <summary>
		/// Handle click events on the "Select All / None" checkbox.
		/// This will select / deselect all of the files in the list.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void chkBoxSelectAll_CheckedChanged(object sender, EventArgs e) {
			foreach(ListViewItem item in this.lvChangedPaths.Items) {
				item.Checked = this.chkBoxSelectAll.Checked;
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

		/// <summary>
		/// Use the background worker to deploy files.
		/// </summary>
		/// <param name="sender">Reference to the background worker object</param>
		/// <param name="e">e.Argument contains the list of DeploymentMappings we'll be deploying</param>
		private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e) {
			BackgroundWorker worker = sender as BackgroundWorker;

			// Cast our argument to a list
			List<DeploymentDisplay> deploymentFiles = ((List<DeploymentDisplay>)e.Argument);

			// Register all of our fiels with the deployer, so that post-deployment scripts can be executed.
			deployer.RegisterDeploymentFiles(deploymentFiles.Select(df => df.Source).ToList());

			// Figure out the exact percentage, so we can report our progress
			int percentagePerFile = 100 / deploymentFiles.Count;

			// Copy each file, using the deployer
			int i = 0;
			foreach(DeploymentDisplay file in deploymentFiles) {
				deployer.Deploy(file.Source, file.Destination);
				worker.ReportProgress(i * percentagePerFile);
				i++;
			}
		}

		/// <summary>
		/// Update the progress bar to show the user how far through our file deployments we've been.
		/// This is called after each file is copied, from backgroundWorker_DoWork
		/// </summary>
		/// <param name="sender">Reference to the background worker object</param>
		/// <param name="e">Arguments</param>
		private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e) {
			// Update our progress bar
			if (this.progressBar.Value < this.progressBar.Maximum) {
				this.progressBar.Value++;
			}

			// Update the output log, so that it's refreshed after each file is copied.
			OverwriteLog(deployer.Log);
		}

		/// <summary>
		/// Update the output log once the background worker has finished.
		/// </summary>
		/// <param name="sender">Reference to the background worker object</param>
		/// <param name="e">Arguments</param>
		private void fileCopyWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
			// Update the log output
			OverwriteLog(deployer.Log);

			// Re-enable the Deploy button
			this.btnDeploy.Enabled = true;
		}

		/// <summary>
		/// File menu option to exit the application
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void exitToolStripMenuItem_Click(object sender, EventArgs e) {
			Application.Exit();
		}

		/// <summary>
		/// File menu option to open the config file with the default application for .json files
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void openConfigToolStripMenuItem_Click(object sender, EventArgs e) {
			System.Diagnostics.Process.Start(Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "config.json"));
		}

		/// <summary>
		/// File menu option to reload the config file from disk, and update the application to use the new config.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void reloadConfigToolStripMenuItem_Click(object sender, EventArgs e) {
			// Reload the config file from disk
			deployer.LoadConfig();

			// Reload the deployment mappings
			this.LoadDeploymentMappings();
		}

		/// <summary>
		/// Called whenever the form is resized.
		/// We call off to the resizeListView method, to resize the columns.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Form1_ResizeEnd(object sender, EventArgs e) {
			resizeListView();
		}

		/// <summary>
		/// Resize the columns in the ListView to match their content.
		/// </summary>
		private void resizeListView() {
			// Ensure the list of changed paths resizes appropriately
			for (int i = 0; i < this.lvChangedPaths.Columns.Count - 1; i++) {
				this.lvChangedPaths.AutoResizeColumn(i, ColumnHeaderAutoResizeStyle.ColumnContent);
			}
			this.lvChangedPaths.Columns[this.lvChangedPaths.Columns.Count - 1].Width = -2;	// -2 is a magic number that tells the last column to use all remaining space
		}
	}
}
