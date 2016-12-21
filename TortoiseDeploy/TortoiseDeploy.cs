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

		public StringBuilder Log { get; private set; }
		public Boolean Ready { get; private set; }

		/// <summary>
		/// Instantiate a new TortoiseDeploy instance from the specified config file.
		/// The Ready attribute will be set to show whether the config loaded correctly or not.
		/// </summary>
		/// <param name="configPath">Path on disk to a config file that can be deserialized.</param>
		public TortoiseDeploy(string configPath) {
			// Instantiate our log object, and log our start time
			this.Log = new StringBuilder();
			this.Log.AppendLine("\n\n**********************************************************************");
			this.Log.AppendLine("TortoiseDeploy instance created at " + DateTime.Now.ToString());

			// Attempt to load the config file
			using (StreamReader reader = new StreamReader(configPath)) {
				try {
					this.config = JsonConvert.DeserializeObject<Config>(reader.ReadToEnd());
					this.Ready = true;
				} catch (Exception ex) {

					Log.AppendLine("Couldn't load config file! Aborting with error:");
					Log.AppendLine(ex.ToString());

					this.Ready = false;
				}
			}
		}

		/// <summary>
		/// Destructor for the TortoiseDeploy object.
		/// When destroyed, our object will write to a log file.
		/// </summary>
		~TortoiseDeploy() {

			// Log our exit time
			Log.AppendLine("TortoiseDeploy instance destroyed at " + DateTime.Now.ToString());

			// Write out to a file named log.txt in the executable folder
			string logFilePath = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "log.txt");

			// Attempt to write out to the log file.
			try {
				using (StreamWriter writer = new StreamWriter(logFilePath, true)) {
					writer.Write(Log.ToString());
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
				File.Copy(source, destination, true);
				Log.AppendLine(String.Format("Copied {0} to {1}", source, destination));
				return true;
			} catch (Exception ex) {
				Log.AppendLine(String.Format("\nError copying {0} to {1}:\n{2}\n", source, destination, ex.ToString()));
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
				Log.AppendLine(String.Format("Configured merge tool ({0}) doesn't exist.", config.MergeToolPath));

				return false;
			}

			// Launch the diff tool
			try {
				Log.AppendLine(String.Format("Opening diff tool ({0}) with arguments {1}", config.MergeToolPath, diffArguments));
				Process diffTool = System.Diagnostics.Process.Start(config.MergeToolPath, diffArguments);

				return true;	// Return immediately, so the user doesn't have to close their diff tool before continuing with TortoiseDeploy
			} catch (Exception ex) {
				Log.AppendLine("Error running diff tool:");
				Log.AppendLine(ex.ToString());
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
			Log.AppendLine(message);
		}
	}
}
