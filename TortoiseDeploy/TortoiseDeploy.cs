using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace TortoiseDeploy {
	public class TortoiseDeploy {

		private Config config;

		private StringBuilder _log;
		public string Log { get { return _log.ToString(); } }

		Dictionary<DeploymentGroup, List<string>> registeredDeployments;

		/// <summary>
		/// Instantiate a new TortoiseDeploy instance from the config file in the binary directory.
		/// The Ready attribute will be set to show whether the config loaded correctly or not.
		/// </summary>
		public TortoiseDeploy() {
			// Instantiate our log object, and log our start time
			this._log = new StringBuilder();
			this._log.AppendLine("**********************************************************************");
			this._log.AppendLine("TortoiseDeploy instance created at " + DateTime.Now.ToString());

			LoadConfig();
		}

		/// <summary>
		/// Load the config file from disk.
		/// </summary>
		public void LoadConfig() {
			string configPath = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "config.json");

			// Attempt to read the config file from disk
			string config;
			using (StreamReader reader = new StreamReader(configPath)) {
				config = reader.ReadToEnd();
			}

			// Try to load the config
			try {
				this.config = JsonConvert.DeserializeObject<Config>(config);
			} catch (Exception ex) {

				_log.AppendLine("Couldn't load config file! Aborting with error:");
				_log.AppendLine(ex.ToString());
			}
		}

		/// <summary>
		/// Destructor for the TortoiseDeploy object.
		/// When destroyed, our object will write to a log file.
		/// </summary>
		~TortoiseDeploy() {

			// Log our exit time
			_log.AppendLine("TortoiseDeploy instance destroyed at " + DateTime.Now.ToString());
			_log.AppendLine("\n");

			// Write out to a file named log.txt in the executable folder
			string logFilePath = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "log.txt");

			// Attempt to write out to the log file.
			try {
				using (StreamWriter writer = new StreamWriter(logFilePath, true)) {
					writer.Write(_log.ToString());
				}
			} catch { }	// Unfortunately we can't do anything but sink the exception
		}

		/// <summary>
		/// Copy the specified file from source to destination.
		/// </summary>
		/// <param name="source">Path on disk of source file</param>
		/// <param name="destination">Path on disk (or network) to copy the file to</param>
		/// <returns>Whether the deployment succeeded</returns>
		public bool Deploy(string source, string destination) {
			try {

				// Run the Pre-deployment step if it hasn't been run yet
				DeploymentGroup group = config.GetDeploymentGroup(source);
				if (group != null && !group.PreDeploymentHasRun && !String.IsNullOrEmpty(group.PreDeploymentScript)) {
					// Check whether the PreDeploymentScript exists. If not, we'll try looking in the same folder as our binary
					if (!File.Exists(group.PreDeploymentScript)) {
						group.PreDeploymentScript = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), group.PreDeploymentScript);

						// If the PreDeploymentScript file still doesn't exist, log an error and abort. DO NOT COPY THE FILE
						if (!File.Exists(group.PreDeploymentScript)) {
							LogMessage("The Pre-deployment script cannot be found! Aborted file copy.");
						}
					}

					LogMessage(String.Format("Launching Pre-deployment script ({0}) with arguments: {1}", group.PreDeploymentScript, group.PreDeploymentArguments));

					// Try to run the script until it's completed before continuing
					Process preDeploymentScript = Process.Start(group.PreDeploymentScript, group.PreDeploymentArguments);
					preDeploymentScript.WaitForExit();

					// Check the exit status. As is convention, a status of 0 is a success, anything else is some sort of failure
					if (preDeploymentScript.ExitCode == 0) {
						LogMessage("Pre-deployment script succeeded");
					} else {
						LogMessage("Pre-deployment script failed!");
					}

					// Mark our Pre-deployment step as having been run
					group.PreDeploymentHasRun = true;
				}

				File.Copy(source, destination, true);
				LogMessage(String.Format("Copied {0} to {1}", source, destination));

				// Remove the file from the list of registered deployment files
				DeploymentGroup deploymentGroup = config.GetDeploymentGroup(source);
				if (registeredDeployments != null
					&& registeredDeployments.ContainsKey(deploymentGroup)
					&& registeredDeployments[deploymentGroup].Contains(source)) {

					registeredDeployments[deploymentGroup].Remove(source);
				}


				// Run the post-deployment script if we have one configured, and we've deployed all the files registered for that group
				if (!String.IsNullOrEmpty(group.PostDeploymentScript) 
					&& !String.IsNullOrEmpty(group.PostDeploymentArguments)
					&& registeredDeployments != null
					&& (registeredDeployments.ContainsKey(deploymentGroup) && registeredDeployments[deploymentGroup].Count == 0)) {

					// Check whether the PoseDeploymentScript file exists. If not, we'll try looking in the same folder as our binary
					if (!File.Exists(group.PostDeploymentScript)) {
						group.PostDeploymentScript = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), group.PreDeploymentScript);

						// If the PreDeploymentScript file still doesn't exist, log an error and abort. DO NOT COPY THE FILE
						if (!File.Exists(group.PostDeploymentScript)) {
							LogMessage("The Post-deployment script cannot be found! Aborted file copy.");
						}
					}

					LogMessage(String.Format("Launching Post-deployment script ({0}) with arguments: {1}", group.PostDeploymentScript, group.PostDeploymentArguments));

					// Try to run the script until it's completed before continuing
					Process postDeploymentScript = Process.Start(group.PostDeploymentScript, group.PostDeploymentArguments);
					postDeploymentScript.WaitForExit();

					// Check the exit status. As is convention, a status of 0 is a success, anything else is some sort of failure
					if (postDeploymentScript.ExitCode == 0) {
						LogMessage("Post-deployment script succeeded");
					} else {
						LogMessage("Post-deployment script failed!");
					}
				}

				return true;
			} catch (Exception ex) {
				LogMessage(String.Format("\nError copying {0} to {1}:\n{2}\n", source, destination, ex.ToString()));
			}
			return false;
		}

		/// <summary>
		/// Launch the configured merge tool to compare source and destination files.
		/// </summary>
		/// <param name="source">Path on disk (or network) of first file</param>
		/// <param name="destination">Path on disk (or network) of second file</param>
		/// <returns>Whether we successfully launched the merge tool</returns>
		public bool Merge(string source, string destination) {

			// Create our arguments for calling the diff tool, ensuring we handle spaces in path names correctly
			string diffArguments = String.Format("\"{0}\" \"{1}\"", source, destination);

			// Ensure we have a diff tool configured
			if (!File.Exists(config.MergeToolPath)) {
				_log.AppendLine(String.Format("Configured merge tool ({0}) doesn't exist.", config.MergeToolPath));

				return false;
			}

			// Launch the diff tool
			try {
				_log.AppendLine(String.Format("Opening diff tool ({0}) with arguments {1}", config.MergeToolPath, diffArguments));
				Process diffTool = System.Diagnostics.Process.Start(config.MergeToolPath, diffArguments);

				return true;	// Return immediately, so the user doesn't have to close their diff tool before continuing with TortoiseDeploy
			} catch (Exception ex) {
				_log.AppendLine("Error running diff tool:");
				_log.AppendLine(ex.ToString());
			}

			// Assume we failed
			return false;
		}

		/// <summary>
		/// Identify and return the path that a given file should be deployed to
		/// </summary>
		/// <param name="source">The file that was committed</param>
		/// <returns>The destination path the file should be deployed to</returns>
		public string GetDeploymentTarget(string source) {
			return config.GetDeploymentFolder(source) + Path.GetFileName(source);
		}

		/// <summary>
		/// Get a user-friendly shortened display name.
		/// This name is the full path name with the RepositoryRoot removed from the start.
		/// </summary>
		/// <param name="source">Full path of the file on disk</param>
		/// <returns>Shortened user-friendly path for displaying</returns>
		public string GetDisplayName(string source) {
			return source.Substring(source.IndexOf(config.RepositoryRoot) + config.RepositoryRoot.Length + 1);
		}

		/// <summary>
		/// Register all the files that will be deployed by this instance.
		/// This MUST be called for post-deployment scripts to get executed, 
		/// as we run the post-deployment script after all registered files for a given group have been deployed.
		/// </summary>
		/// <param name="deploymentFiles">List of all files that are going to be deployed</param>
		public void RegisterDeploymentFiles(List<string> deploymentFiles) {
			// Reset our deployments map
			registeredDeployments = new Dictionary<DeploymentGroup, List<string>>();

			foreach(string file in deploymentFiles) {
				DeploymentGroup group = config.GetDeploymentGroup(file);

				// If the map doesn't have an entry for our group yet, create one
				if (!registeredDeployments.ContainsKey(group)) {
					registeredDeployments.Add(group, new List<string>());
				}

				// Add this file to the map
				registeredDeployments[group].Add(file);
			}
		}

		/// <summary>
		/// Add a message to the log file
		/// </summary>
		/// <param name="message">Message to add to the log file</param>
		public void LogMessage(string message) {
			_log.AppendLine(message);
		}

		/// <summary>
		/// Write the current config settings to disk.
		/// </summary>
		/// <param name="configPath">Path of the config file. If left blank, we'll use config.json in the same folder as the binary.</param>
		/// <returns>Whether the config was successfully written to disk or not</returns>
		public bool SaveConfig() {
			return config.Save();
		}
	}
}
