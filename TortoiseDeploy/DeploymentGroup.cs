using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TortoiseDeploy {
	public class DeploymentGroup {
		/// <summary>
		/// User-friendly name for the deployment group - this is displayed in the GUI
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Command to be executed before deploying any files in this DeploymentGroup.
		/// The command will be called once per run.
		/// </summary>
		public string PreDeploymentScript { get; set; }

		/// <summary>
		/// Additional arguments for the PreDeploymentScript
		/// </summary>
		public string PreDeploymentArguments { get; set; }

		/// <summary>
		/// Whether we've run the Pre Deployment Script or not.
		/// </summary>
		[JsonIgnore]
		public bool PreDeploymentHasRun = false;

		/// <summary>
		/// Command to be executed after deploying files from this DeploymentGroup.
		/// The command will be called once per deployment.
		/// </summary>
		public string PostDeploymentScript { get; set; }

		/// <summary>
		/// Additional arguments for the PostDeploymentScript.
		/// </summary>
		public string PostDeploymentArguments { get; set; }

		/// <summary>
		/// Source -> Destination mappings
		/// </summary>
		public List<DeploymentMapping> DeploymentMappings { get; set; }

		public DeploymentGroup() {
			DeploymentMappings = new List<DeploymentMapping>();
		}

		public override string ToString() {
			return Name;
		}

	}
}
