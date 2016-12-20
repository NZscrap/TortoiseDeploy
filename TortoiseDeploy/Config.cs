using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TortoiseDeploy {
	public class Config {
		public string MergeToolPath;
		public string RepositoryRoot;

		private Dictionary<string, string> _deploymentMap;
		[JsonProperty(Order = 1)]	// Setting order to 1 makes this the last field in the serialized data.
		private Dictionary<string, string> DeploymentMap {
			get {
				return _deploymentMap;
			}
			set {
				_deploymentMap = value;
				// When we read in our config settings, we also want to generate the ProcessingMap
				// The ProcessingMap only stores full paths for source files - this makes GetDeploymentFolder simpler.
				// We want to copy DeploymentMap into ProcessingMap
				ProcessingMap = new Dictionary<string, string>(_deploymentMap);
				
				// Store a full path copy of all the relative paths in the ProcessingMap
				foreach(string relativePath in _deploymentMap.Keys.Where(path => path.StartsWith(Path.DirectorySeparatorChar.ToString()))) {
					ProcessingMap.Add((RepositoryRoot + relativePath), _deploymentMap[relativePath]);
				}
			}
		}
		private Dictionary<string, string> ProcessingMap;	// Initialized when DeploymentMap is created

		/// <summary>
		/// Find the relevant destination directory that we should deploy a given input file to.
		/// </summary>
		/// <param name="sourcePath">The FULL path of the file on disk that we'll be copying</param>
		/// <returns>The folder we should copy the file to, including a trailing slash. Note this doesn't include the file name.</returns>
		public string GetDeploymentFolder(string sourcePath) {

			string response = "";

			// First, check for an exact match on the file
			if (ProcessingMap.ContainsKey(sourcePath)) {
				response = ProcessingMap[sourcePath];
			}

			// If there was no exact match, look for a best-match source
			if (String.IsNullOrEmpty(response)) {
				// We define best-match as the most-specific folder path, ie the longest path name
				foreach (string potentialMatch in ProcessingMap.Keys.OrderByDescending(path => path.Length)) {
					// Since we're sorting from the longest path to the shortest, the first match we find is what we want!
					if (sourcePath.StartsWith(potentialMatch)) {
						// As this is a StartsWith check, it's possible that we have further directory nesting.
						// Eg, Assume sourcePath is ~/Desktop/TortoiseDeploy/config.json, and potentialMatch is ~/Deskop which maps to /var/svn
						//     We would expect to get back a deployment folder of /var/svn/TortoiseDeploy
						string postMatch = "";
						if (potentialMatch.Length < Path.GetDirectoryName(sourcePath).Length) {
							// We want to find the remaining folder structure beyond what we matched on, and include it in our response
							postMatch = Path.GetDirectoryName(sourcePath).Substring(potentialMatch.Length);
						}

						// Our response should be the deployment path that maps to the matched path, plus any postMatch directory structure
						response = ProcessingMap[potentialMatch] + Path.DirectorySeparatorChar + postMatch;
						break;
					}
				}
			}

			// Finally, fall back to suggesting the generic deployment folder (The folder mapped to *)
			if (String.IsNullOrEmpty(response)) {
				response = ProcessingMap["*"];
			}

			// Ensure that we return the path with a trailing seperator character
			return response.EndsWith(Path.DirectorySeparatorChar.ToString()) ? response : response + Path.DirectorySeparatorChar.ToString();
		}
	}
}
