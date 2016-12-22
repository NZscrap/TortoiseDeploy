using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Linq;
using Newtonsoft.Json;

namespace TortoiseDeploy.CLI {
	class Program {

		public static TortoiseDeploy deployer;

		static void Main(string[] args) {
			// Used to write to an error file
			StringBuilder errors = new StringBuilder();

			// For paranoia's sake, we're wrapping everything in a try/catch.
			// This should only be used as a last resort, to log that we had a completely unexpected exception.
			try {
				bool hasError = false;

				#region Validation

				// Do some validation first up.
				// TortoiseSVN passes 6 parameters for a Post-commit hook. When we call ourselves, we add a seventh parameter.
				// To learn why we call ourselves, read beyond the validation
				if (args.Length != 6 && args.Length != 7) {
					errors.AppendLine("Incorrect number of parameters - are you sure you're calling from TortoiseSVNs Post-Commit hook?");
					errors.AppendLine("Called with: " + String.Join(" ", args));

					hasError = true;
				}

				// The first parameter is a file containing all the changed paths.
				// We read it during the Process method, but we should check that it exists now.
				if (!File.Exists(args[0])) {
					hasError = true;
					errors.AppendLine("Temp file containing changes (" + args[0] + ") doesn't exist on disk");
				}

				// The fifth parameter is an error file, which will be empty if the Commit succeeded.
				// If there's an error of any sort, we don't want to run
				try {
					using (StreamReader reader = new StreamReader(args[4])) {
						// If the file actually has contents, we want to exit early.
						if (!reader.EndOfStream) {
							hasError = true;
							errors.AppendLine("An error occurred during the commit operation:");
							errors.AppendLine("-----------------------------------------------");
							errors.Append(reader.ReadToEnd());
							errors.AppendLine("-----------------------------------------------");
						}
					}
				} catch {
					hasError = true;
					errors.AppendLine("Couldn't open error file - aborting.");
				}

				#endregion

				// Do our processing, unless SVN had some sort of error trying to commit
				if (!hasError) {
					// TortoiseSVN redirects output when calling commit hooks, so that it can read the output and use it for error detection in the hook script.
					// Unfortunately, I really want to interact with the user, so I need to be able to output things.
					// The solution is to execute ourself again, adding an extra parameter to the end of all the TortoiseSVN arguments.
					if (args.Length == 6) {
						// Launch ourselves again, with an extra 7th parameter. Once the child returns, we will exit (returning the child exit code)
						Process child = System.Diagnostics.Process.Start(System.Reflection.Assembly.GetExecutingAssembly().Location, "\"" + String.Join("\" \"", args) + "\" LaunchAsChild");
						child.WaitForExit();
						Environment.Exit(child.ExitCode);
					}

					// Do the processing
					Process(args);
				}
			} catch (Exception ex) {
				// This should never happen!
				// If we're winding up in this block, we really want to know about it - and fix the underlying code!
				errors.AppendLine("*********** FATAL EXCEPTION ***********");
				errors.AppendLine("Error logged at " + DateTime.Now.ToString());
				errors.AppendLine(ex.ToString());
			} finally {
				// Write to the error log if we've got any errors
				if (!String.IsNullOrEmpty(errors.ToString())) {

					// Our errors log is errors.txt in the same folder as the executable
					string logFilePath = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "errors.txt");
					using (StreamWriter writer = new StreamWriter(logFilePath, true)) {
						writer.Write(errors.ToString());
					}
				}
			}

			// If we've got any errors during this execution, output them to the user at the end
			if (!String.IsNullOrEmpty(errors.ToString())) {
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("Errors occurred:");
				Console.WriteLine(errors.ToString());
				Console.ResetColor();
			}

			// Don't close until prompted to by the user
			Console.WriteLine("\n\nPress any key to exit..");
			Console.ReadLine();
		}

		/// <summary>
		/// Process the committed files.
		/// </summary>
		/// <param name="args">Commandline arguments, as passed by TortoiseSVN's post-commit hook.</param>
		static void Process(string[] args) {
			// We'll log the complete list of changed files, as reported by TortoiseSVN
			StringBuilder output = new StringBuilder();
			output.AppendLine("Paths changed:");

			// The first parameter from TortoiseSVN is a temp file containing a list of the files that changed (one changed file per line)
			// We read this into a list of files that changed, which we will process individually
			List<string> changedFiles = new List<string>();
			using (StreamReader reader = new StreamReader(args[0])) {
				while (!reader.EndOfStream) {
					string changedFile = reader.ReadLine();
					// TortoiseSVN gives us the changed path linux-style (ie C:/folder/path/to/file.txt)
					// We want to get the windows style (ie, C:\\folder\\path\\to\\file.txt)
					// We're going to consistently deal in windows style paths
					changedFile = Path.GetFullPath(changedFile);
					changedFiles.Add(changedFile);
					output.AppendLine(changedFile); // Add the changed path to our log output
				}
				output.AppendLine();    // Add a line of whitespace after listing our files, to make the logs a little easier to read
			}

			// Instantiate our TortoiseDeploy engine - it will be used to merge/deploy the files
			deployer = new TortoiseDeploy();

			// Check that the config was correctly loaded, and start processing the files if it was
			if (deployer.Ready) {
				// Add our log of changed paths
				deployer.LogMessage(output.ToString());

				// For each file that changed, we want to prompt the user for an action
				foreach (string changedFile in changedFiles) {
					ProcessPath(changedFile);
				}
			} else { 
				// Display an error to the user - they're going to have to manually deploy everything
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("Error loading deployment config. You will need to manually deploy.");
				Console.ResetColor();
			}			
		}

		/// <summary>
		/// Prompt the user for how they want to do for a particular file.
		/// We currently support Merging, Deploying, and Skipping.
		/// </summary>
		/// <param name="changedFile">Local path of the file that changed</param>
		private static void ProcessPath(string changedFile) {

			// Figure out where we will be deploying the file
			string destination = deployer.GetDeploymentTarget(changedFile);

			// Ask the user how we should proceed. They will be prompted until they either Deploy or Skip
			bool hasProcessed = false;
			while (!hasProcessed) {
				string sourceDisplay = deployer.GetDisplayName(changedFile);
				string prompt = String.Format("Deploying {0} to {1}\nNote: Merging won't move to the next file, you'll still have a chance to deploy.\n[M]erge, [D]eploy, [S]kip:", sourceDisplay, destination);
				string response = PromptUser(prompt, new string[] { "m", "d", "s" });

				switch (response) {
					case "m":   // Merge
						// Launch merge tool, and show an error if it fails
						if (!deployer.Merge(changedFile, destination)) {
							Console.ForegroundColor = ConsoleColor.Red;
							Console.WriteLine("Error opening diff tool");
							Console.ResetColor();
						}
						break;
					case "d":	// Deploy
						// Attempt to deploy the file
						if (deployer.Deploy(changedFile, destination)) {
							// Show a success message in Yellow
							Console.ForegroundColor = ConsoleColor.Yellow;
							Console.WriteLine(String.Format("Copied {0} to {1}", changedFile, destination));
							Console.ResetColor();
						} else {
							// Show an error in red
							Console.ForegroundColor = ConsoleColor.Red;
							Console.WriteLine("ERROR - Failed to copy file. You will need to manually deploy.");
							Console.ResetColor();
						}

						hasProcessed = true;
						break;
					case "s":	// Skip
						Console.WriteLine("You will need to deploy this manually");
						hasProcessed = true;
						break;
				}
			}
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
