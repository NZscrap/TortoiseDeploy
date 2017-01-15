using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TortoiseDeploy {
	public class TortoiseDeploy {

		private Config config;

		private StringBuilder _log;
		public string Log { get { return _log.ToString(); } }
		public Boolean Ready { get; private set; }

		public TortoiseDeploy() : this(Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "config.json")) { }

		/// <summary>
		/// Instantiate a new TortoiseDeploy instance from the specified config file.
		/// The Ready attribute will be set to show whether the config loaded correctly or not.
		/// </summary>
		/// <param name="configPath">Path on disk to a config file that can be deserialized.</param>
		public TortoiseDeploy(string configPath) {
			// Instantiate our log object, and log our start time
			this._log = new StringBuilder();
			this._log.AppendLine("**********************************************************************");
			this._log.AppendLine("TortoiseDeploy instance created at " + DateTime.Now.ToString());

			// Attempt to load the config file
			using (StreamReader reader = new StreamReader(configPath)) {
				try {
					this.config = JsonConvert.DeserializeObject<Config>(reader.ReadToEnd());
					this.Ready = true;
				} catch (Exception ex) {

					_log.AppendLine("Couldn't load config file! Aborting with error:");
					_log.AppendLine(ex.ToString());

					this.Ready = false;
				}
			}

			//generateConfig();//TODO debug hack
		}

		private void generateConfig() {
			// We're going to make a CR UAT group
			DeploymentGroup crGroup = new DeploymentGroup() {
				Name = "Mitch testing",
				PreDeploymentScript = "connectToServer.bat",
				PreDeploymentArguments = "\\\\crvweb6.estaronline.local",
				PostDeploymentScript = "recycleSite.bat",
				PostDeploymentArguments = "\\\\crvweb6.estaronline.local\\websites\\isams_9_countryroad_uat\\website"
			};
			DeploymentMapping mapping = new DeploymentMapping() {
				Source = "C:\\Users\\mitch\\Desktop\\local tortoiseDeploy tests",
				Destination = "C:\\Users\\mitch\\Desktop\\TortoiseDeploy-Deploy\\svn"
			};
			crGroup.DeploymentMappings.Add(mapping);
			mapping = new DeploymentMapping() {
				Source = "\\abc.txt",
				Destination = "C:\\Users\\mitch\\Desktop\\TortoiseDeploy-Deploy\\svn\\gui-abc.txt"
			};
			crGroup.DeploymentMappings.Add(mapping);

			config = new Config() {
				MergeToolPath = "C:\\Program Files (x86)\\Beyond Compare 4\\BCompare.exe",
				RepositoryRoot = "C:\\Users\\mitch\\Desktop\\local tortoiseDeploy tests"
			};
			config.DeploymentGroups.Add(crGroup);

			// Write the config to disk, so we can see what it looks like
			SaveConfig();
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
		public bool SaveConfig(string configPath = "") {
			try {
				// Use the default config path if we weren't passed one
				if (String.IsNullOrEmpty(configPath)) {
					configPath = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "config.json");
				}

				// Backup the old config file, if there is one
				if (File.Exists(configPath)) {
					try {
						if (File.Exists(configPath + ".backup")) {
							File.Delete(configPath + ".backup");
						}
						File.Move(configPath, configPath + ".backup");
					} catch {
						return false;
					}
				}

				// Write the current config to disk
				using (StreamWriter writer = new StreamWriter(configPath)) {
					writer.Write(JsonConvert.SerializeObject(this.config));
				}

				return true;
			} catch(Exception ex) {
				LogMessage("Exception saving config: " + ex.ToString());

				return false;
			}
		}
	}
}
