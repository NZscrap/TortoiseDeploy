using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TortoiseDeploy.GUI {
	static class Program {
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args) {
			try {
				// Load up the list of files that changed
				List<string> changedFiles = new List<string>();
				if (args.Length >= 1) {
					using (StreamReader reader = new StreamReader(args[0])) {
						while (!reader.EndOfStream) {
							string changedFile = reader.ReadLine();
							// TortoiseSVN gives us the changed path linux-style (ie C:/folder/path/to/file.txt)
							// We want to get the windows style (ie, C:\\folder\\path\\to\\file.txt)
							// We're going to consistently deal in windows style paths
							changedFile = Path.GetFullPath(changedFile);
							changedFiles.Add(changedFile);
						}
					}
				}

				// Check to make sure TortoiseSVN didn't record some sort of error, and exit early if it did
				StringBuilder errors = new StringBuilder();
				try {
					using (StreamReader reader = new StreamReader(args[4])) {
						// If the file actually has contents, we want to exit early.
						if (!reader.EndOfStream) {
							errors.AppendLine("An error occurred during the commit operation:");
							errors.AppendLine("-----------------------------------------------");
							errors.Append(reader.ReadToEnd());
							errors.AppendLine("-----------------------------------------------");
						}
					}
				} catch { }

				Application.EnableVisualStyles();
				Application.SetCompatibleTextRenderingDefault(false);
				Application.Run(new Form1(changedFiles, errors.ToString()));
			} catch(Exception ex) {
				try {
					string logFilePath = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "log.txt");
					using (StreamWriter writer = new StreamWriter(logFilePath, true)) {
						writer.WriteLine("FATAL EXCEPTION:");
						writer.Write(ex.ToString());
					}
				} catch {
					// Sink, things have gone really bad at this stage
				}
			}
		}
	}
}
