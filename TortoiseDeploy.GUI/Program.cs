using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TortoiseDeploy.GUI {
	static class Program {
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args) {

			// Load up the list of files that changed
			List<string> changedFiles = new List<string>();
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

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new Form1(changedFiles));
		}
	}
}
