using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Runtime.Serialization;
using System.Net.Http;
using Newtonsoft.Json;

namespace TortoiseDeploy {
	class Config {
		public string MergeToolPath;
		public string RepositoryRoot;

		public string RemoteDeploymentGroupURL;

		public List<DeploymentGroup> DeploymentGroups { get; set; }

		/// <summary>
		/// Get the DeploymentGroup that the specified sourcePath belongs to.
		/// </summary>
		/// <param name="sourcePath">Local file on disk</param>
		/// <returns>DeploymentGroup containing the DeploymentMapping for the specified source file</returns>
		public DeploymentGroup GetDeploymentGroup(string sourcePath) {
			return DeploymentGroups.Where(dg => dg.DeploymentMappings.Contains(GetDeploymentMapping(sourcePath))).FirstOrDefault();
		}

		/// <summary>
		/// Find the relevant DeploymentMapping for the specified source file.
		/// Note: Returns null if we can't find a mapping for this file
		/// </summary>
		/// <param name="sourcePath">Local file on disk</param>
		/// <returns>DeploymentMapping object that specifies where the sourcePath should be deployed.</returns>
		public DeploymentMapping GetDeploymentMapping(string sourcePath) {
			DeploymentMapping bestMapping = null;
			foreach (DeploymentGroup group in DeploymentGroups) {
				// First we'll check for an exact match on the file source
				DeploymentMapping exactMatchMapping = group.DeploymentMappings.Where(m => m.Source == sourcePath || this.RepositoryRoot + m.Source == sourcePath).FirstOrDefault<DeploymentMapping>();
				if (exactMatchMapping != null) {
					return exactMatchMapping;
				}

				// If there was no exact match, look for a best-match source
				foreach (DeploymentMapping mapping in group.DeploymentMappings.OrderByDescending(dm => dm.Source.Length)) {
					// Our mapping source might be a path relative to the RepositoryRoot. If that's the case, we need to put together the full path before comparing.
					if (mapping.Source.StartsWith(Path.DirectorySeparatorChar.ToString())) {
						mapping.Source = this.RepositoryRoot + mapping.Source;
					}

					// Since we're sorting from the longest path to the shortest, the first match we find is what we want!
					if (sourcePath.StartsWith(mapping.Source)) {
						// If we haven't mapped anything yet, this is our best mapping
						if (bestMapping == null) {
							bestMapping = mapping;
						} else if (bestMapping.Source.Length < mapping.Source.Length) {
							// See if the mapping in this group is the most-accurate map so far.
							bestMapping = mapping;
						}
					}
				}
			}

			// Return our best mapping, note that this will be null if we couldn't find a matching DeploymentMapping
			return bestMapping;
		}

		/// <summary>
		/// Find the relevant destination directory that we should deploy a given input file to.
		/// </summary>
		/// <param name="sourcePath">The FULL path of the file on disk that we'll be copying</param>
		/// <returns>The folder we should copy the file to, including a trailing slash. Note this doesn't include the file name.</returns>
		public string GetDeploymentFolder(string sourcePath) {

			string response = "";
			DeploymentMapping mapping = GetDeploymentMapping(sourcePath);

			if (mapping != null && sourcePath.StartsWith(mapping.Source)) {
				// It's possible that we have further directory nesting.
				// Eg, Assume sourcePath is ~/Desktop/TortoiseDeploy/config.json, and potentialMatch is ~/Deskop which maps to /var/svn
				//     We would expect to get back a deployment folder of /var/svn/TortoiseDeploy
				string postMatch = "";
				if (mapping.Source.Length < Path.GetDirectoryName(sourcePath).Length) {
					// We want to find the remaining folder structure beyond what we matched on, and include it in our response
					postMatch = Path.GetDirectoryName(sourcePath).Substring(mapping.Source.Length);
				}

				// Our response should be the deployment path that maps to the matched path, plus any postMatch directory structure
				response = mapping.Destination + Path.DirectorySeparatorChar + postMatch;
			}

			// Ensure that we return the path with a trailing seperator character (unless the deployment path is empty)
			return response.EndsWith(Path.DirectorySeparatorChar.ToString()) || String.IsNullOrEmpty(response) ? response : response + Path.DirectorySeparatorChar.ToString();
		}

		public Config() {
			DeploymentGroups = new List<DeploymentGroup>();
		}

		/// <summary>
		/// Serialize this config and write it to disk.
		/// </summary>
		/// <returns>Whether we successfully saved to disk or not</returns>
		public bool Save() {
			try {
				string configPath = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "config.json");

				// Backup the old config file, if there is one
				if (File.Exists(configPath)) {
					try {
						if (File.Exists(configPath + ".backup")) {
							File.Delete(configPath + ".backup");
						}
						File.Move(configPath, configPath + ".backup");
					} catch (Exception ex) {
						return false;
					}
				}

				// Write the current config to disk
				using (StreamWriter writer = new StreamWriter(configPath)) {
					writer.Write(JsonConvert.SerializeObject(this, Formatting.Indented));
				}

				return true;
			} catch (Exception ex) {
				// TODO - log errors
				return false;
			}
		}

		/// <summary>
		/// Load the DeploymentGroups section of our config from the configured remote URL.
		/// We replace the local DeploymentGroups config with the server version.
		/// Once loaded from the server, we save the config back to disk.
		/// Note: This method is called by the serializer once it finishes populating the behaviours.
		/// </summary>
		/// <param name="context"></param>
		[OnDeserialized]
		public void LoadRemoteDeploymentGroups(StreamingContext context) {
			// Once we've deserialized the config, check if we have a RemoteDeploymentGroupURL configured
			if (!String.IsNullOrEmpty(RemoteDeploymentGroupURL)) {
				try {
					// Try to connect to the remote url
					using (HttpClient client = new HttpClient()) {
						string remoteContent = client.GetAsync(RemoteDeploymentGroupURL).Result.Content.ReadAsStringAsync().Result;

						// If we managed to fetch the remote data, we'll try deserializing it into our config
						if (!String.IsNullOrEmpty(remoteContent)) {
							this.DeploymentGroups = JsonConvert.DeserializeObject<List<DeploymentGroup>>(remoteContent);

							// If we've successfully loaded the DeploymentGroups from the remote source, we'll save our config to disk.
							// This way, if the remote source goes down, we'll still have the info next time we start up
							this.Save();
						}
					}
				} catch {
					// TODO - log the exception into our general log file
				}
			}
		}
	}
}