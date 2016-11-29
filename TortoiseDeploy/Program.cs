using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace TortoiseDeploy {
	class Program {
		static void Main(string[] args) {

			bool hasError = false;

			// We want to log everything that the program does, for auditting purposes if anything goes wrong.
			StringBuilder output = new StringBuilder();
			output.AppendLine("\n\n**********************************************************************");
			output.AppendLine("Execution started at " + DateTime.Now.ToString());

			#region Validation

			// Do some validation first up.
			// TortoiseSVN passes 6 parameters for a Post-commit hook. When we call ourselves, we add a seventh parameter.
			// To learn why we call ourselves, read beyond the validation
			if (args.Length != 6 && args.Length != 7) {
				output.AppendLine("Incorrect number of parameters - are you sure you're calling from TortoiseSVNs Post-Commit hook?");
				output.AppendLine("Called with: " + String.Join(" ", args));

				// Write the log file
				using (StreamWriter writer = new StreamWriter("out.txt", true)) {
					writer.Write(output.ToString());
				}

				// Exit early
				Environment.Exit(-1);
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
			//TODO - Test the "repo out of date" error condition to ensure this actually works
			if (!hasError) {
				// TortoiseSVN redirects output when calling commit hooks, so that it can read the output and use it for error detection in the hook script.
				// Unfortunately, I really want to interact with the user, so I need to be able to output things.
				// The solution is to execute ourself again, adding an extra parameter to the end of all the TortoiseSVN arguments.
				if (args.Length == 6) {
					output.AppendLine("Launching child process.");

					// We'll write our logging here, since after the child process exits we'll just exit.
					using (StreamWriter writer = new StreamWriter("out.txt", true)) {
						writer.Write(output.ToString());
					}

					// Launch ourselves again, with an extra 7th parameter
					Process child = System.Diagnostics.Process.Start(System.Reflection.Assembly.GetExecutingAssembly().Location, String.Join(" ", args) + " LaunchAsChild");
					child.WaitForExit();
					Environment.Exit(child.ExitCode);
				}

				// Do the processing
				string processingOutput = Process(args);
				output.Append(processingOutput);
			}

			// Finally, we always want to write to the log file (Appending if one already exists)
			using (StreamWriter writer = new StreamWriter("out.txt", true)) {
				writer.Write(output.ToString());
			}
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
					changedFiles.Add(changedFile);
					output.AppendLine(changedFile); // Add the changed path to our log output
				}
			}

			return output.ToString();
		}
	}
}
