using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TortoiseDeploy {
	class Config {
		public string MergeToolPath;
		public Dictionary<string, string> DeploymentMap;
		public string RepositoryRoot;

		public Config() {
			DeploymentMap = new Dictionary<string, string>();
		}

		/// <summary>
		/// Find the relevant destination directory that we should deploy a given input file to.
		/// </summary>
		/// <param name="sourcePath">The FULL path of the file we'll be copying</param>
		/// <returns>The folder we should copy the file to, including a trailing slash. Note this doesn't include the file name.</returns>
		public string GetDeploymentFolder(string sourcePath) {
			return DeploymentMap["*"].EndsWith("/") ? DeploymentMap["*"] : DeploymentMap["*"] + "/";
		}
	}
}
