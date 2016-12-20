using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Linq;

namespace TortoiseDeploy {
	class Program {

		private static Config config;

		static void Main(string[] args) {
			// We want to log everything, so that it can be auditted later if necessary.
			StringBuilder output = new StringBuilder();

			// For paranoia's sake, we're wrapping everything in a try/catch.
			// This should only be used as a last resort, to log that we had a completely unexpected exception.
			try {
				bool hasError = false;

				// Log that we've started an execution run
				output.AppendLine("\n\n**********************************************************************");
				output.AppendLine("Execution started at " + DateTime.Now.ToString());

				#region Validation

				// Do some validation first up.
				// TortoiseSVN passes 6 parameters for a Post-commit hook. When we call ourselves, we add a seventh parameter.
				// To learn why we call ourselves, read beyond the validation
				if (args.Length != 6 && args.Length != 7) {
					output.AppendLine("Incorrect number of parameters - are you sure you're calling from TortoiseSVNs Post-Commit hook?");
					output.AppendLine("Called with: " + String.Join(" ", args));

					hasError = true;
				}

				// The first parameter is a file containing all the changed paths.
				// We read it during the Process method, but we should check that it exists now.
				if (!File.Exists(args[0])) {
					hasError = true;
					output.AppendLine("Temp file containing changes (" + args[0] + ") doesn't exist on disk");
				}

				// The fifth parameter is an error file, which will be empty if the Commit succeeded.
				// If there's an error of any sort, we don't want to run
				try {
					using (StreamReader reader = new StreamReader(args[4])) {
						// If the file actually has contents, we want to exit early.
						if (!reader.EndOfStream) {
							hasError = true;
							output.AppendLine("An error occurred during the commit operation:");
							output.AppendLine("-----------------------------------------------");
							output.Append(reader.ReadToEnd());
							output.AppendLine("-----------------------------------------------");
						}
					}
				} catch {
					hasError = true;
					output.AppendLine("Couldn't open error file - aborting.");
				}

				#endregion

				// Do our processing, unless SVN had some sort of error trying to commit
				if (!hasError) {
					// TortoiseSVN redirects output when calling commit hooks, so that it can read the output and use it for error detection in the hook script.
					// Unfortunately, I really want to interact with the user, so I need to be able to output things.
					// The solution is to execute ourself again, adding an extra parameter to the end of all the TortoiseSVN arguments.
					if (args.Length == 6) {
						output.AppendLine("Launching child process.");

						// Ensure the parent process write out to the log
						string logFilePath = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "log.txt");
						using (StreamWriter writer = new StreamWriter(logFilePath, true)) {
							writer.Write(output.ToString());
						}

						// Launch ourselves again, with an extra 7th parameter. Once the child returns, we will exit (returning the child exit code)
						Process child = System.Diagnostics.Process.Start(System.Reflection.Assembly.GetExecutingAssembly().Location, "\"" + String.Join("\" \"", args) + "\" LaunchAsChild");
						child.WaitForExit();
						Environment.Exit(child.ExitCode);
					}

					// Do the processing
					string processingOutput = Process(args);
					output.Append(processingOutput);
				}
			} catch (Exception ex) {
				// This should never happen!
				// If we're winding up in this block, we really want to know about it - and fix the underlying code!
				output.AppendLine("*********** FATAL EXCEPTION ***********");
				output.AppendLine(ex.ToString());
			} finally {

				output.AppendLine("Execution ended at " + DateTime.Now.ToString());

				// We always want to write to the log file (Appending if one already exists)
				string logFilePath = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "log.txt");
				using (StreamWriter writer = new StreamWriter(logFilePath, true)) {
					writer.Write(output.ToString());
				}
			}

			// Don't close until prompted to by the user
			Console.WriteLine("\n\nPress any key to exit..");
			Console.ReadLine();
		}

		static string Process(string[] args) {
			StringBuilder output = new StringBuilder();

			// The first parameter from TortoiseSVN is a temp file containing a list of the files that changed (one changed file per line)
			// We read this into a list of files that changed, which we will process individually
			List<string> changedFiles = new List<string>();
			using (StreamReader reader = new StreamReader(args[0])) {
				output.AppendLine("Paths changed:");
				while (!reader.EndOfStream) {
					string changedFile = reader.ReadLine();
					// TortoiseSVN gives us the changed path linux-style (ie C:/folder/path/to/file.txt)
					// We want to get the windows style (ie, C:\\folder\\path\\to\\file.txt)
					// We're going to consistently deal in windows style paths
					changedFile = Path.GetFullPath(changedFile);
					changedFiles.Add(changedFile);
					output.AppendLine(changedFile); // Add the changed path to our log output
				}
				output.AppendLine();	// Add a line of whitespace after listing our files, to make the logs a little easier to read
			}

			// Load up our config file
			string configFilePath = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "config.json");
			output.AppendLine("Loading config from: " + configFilePath);
			using (StreamReader reader = new StreamReader(configFilePath)) {
				try {
					config = JsonConvert.DeserializeObject<Config>(reader.ReadToEnd());
				} catch (Exception ex) {
					// Display an error to the user - they're going to have to manually deploy everything
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine("Error loading deployment config. You will need to manually deploy.");
					Console.ResetColor();

					output.AppendLine("Couldn't load config file! Aborting with error:");
					output.AppendLine(ex.ToString());
					return output.ToString();
				}
			}

			// Loop through each changed path, and launch the merge tool
			foreach(string changedFile in changedFiles) {
				output.Append(ProcessPath(changedFile));
			}

			return output.ToString();
		}

		private static string ProcessPath(string changedFile) {

			StringBuilder output = new StringBuilder();
			string destination = config.GetDeploymentFolder(changedFile) + Path.GetFileName(changedFile);

			// Ask the user how we should proceed. They will be prompted until they either Deploy or Skip
			bool hasProcessed = false;
			while (!hasProcessed) {
				string sourceDisplay = changedFile.Substring(changedFile.IndexOf(config.RepositoryRoot) + config.RepositoryRoot.Length + 1);
				string prompt = String.Format("Deploying {0} to {1}\nNote: Merging won't move to the next file, you'll still have a chance to deploy.\n[M]erge, [D]eploy, [S]kip, [U]pdate deployment folder:", sourceDisplay, destination);
				string response = PromptUser(prompt, new string[] { "m", "d", "s", "u" });

				switch (response) {
					case "m":   // Merge
								// Prep our arguments for the diff tool. We wrap the file paths in speech marks in case they contain spaces.
						string diffArguments = String.Format("\"{0}\" \"{1}\"", changedFile, destination);

						// Ensure a diff tool is configured / the file exists
						if (!File.Exists(config.MergeToolPath)) {
							output.AppendLine("Invalid merge tool");

							Console.ForegroundColor = ConsoleColor.Red;
							Console.WriteLine("Invalid merge tool. You will need to manually deploy the files.");   // TODO - Allow updating of merge tool location
							Console.ResetColor();

						} else {

							// Try firing up the diff tool
							try {
								output.AppendLine(String.Format("Opening diff tool ({0}) with arguments {1}", config.MergeToolPath, diffArguments));
								Process diffTool = System.Diagnostics.Process.Start(config.MergeToolPath, diffArguments);
							} catch (Exception ex) {
								output.AppendLine("Error opening diff tool:");
								output.AppendLine(ex.ToString());

								Console.ForegroundColor = ConsoleColor.Red;
								Console.WriteLine("Error opening diff tool");
								Console.ResetColor();
							}
						}
						break;
					case "d":
						try {
							File.Copy(changedFile, destination, true);
							output.AppendLine(String.Format("Copied {0} to {1}", changedFile, destination));

							// Show a success message in Yellow
							Console.ForegroundColor = ConsoleColor.Yellow;
							Console.WriteLine(String.Format("Copied {0} to {1}", changedFile, destination));
							Console.ResetColor();
						} catch (Exception ex) {
							output.AppendLine(String.Format("\nError copying {0} to {1}:\n{2}\n", changedFile, destination, ex.ToString()));

							// Show an error in red
							Console.ForegroundColor = ConsoleColor.Red;
							Console.WriteLine("ERROR - Failed to copy file. You will need to manually deploy.");
							Console.ResetColor();
						}
						hasProcessed = true;
						break;
					case "s":
						Console.WriteLine("You will need to deploy this manually");
						hasProcessed = true;	// Move on to the next file
						break;
					case "u":
						// TODO - allow the user to specify a new deployment mapping
						break;
				}
			}

			return output.ToString();
		}

		/// <summary>
		/// Show the specified prompt to the user, and show them an error until they specify a valid input.
		/// </summary>
		/// <param name="prompt">The prompt to show the user, for example "[Y]es or [N]o"</param>
		/// <param name="validInputs">A case-insensitive list of inputs that are valid.</param>
		/// <returns>The valid selection made by the user</returns>
		private static string PromptUser(string prompt, string[] validInputs) {

			// Prompt the user
			Console.Write(prompt);

			// Read their response
			string userInput = Console.ReadLine();

			// If there are only certain validInputs, loop until the user enters one of them
			if (validInputs != null && validInputs.Length > 0) {
				while (!validInputs.Contains(userInput, StringComparer.OrdinalIgnoreCase)) {
					// Show a red error message, prompting the user to enter a valid options
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine("Please specify an option surrounded by []");
					Console.ResetColor();

					// Prompt the user again
					Console.Write(prompt);
					userInput = Console.ReadLine();
				}
			}

			return userInput;
		}
	}
}
