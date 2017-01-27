using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TortoiseDeploy.GUI {
	class DeploymentDisplay {
		public string Source { get; private set; }
		public string Destination { get; private set; }
		public string DisplaySource { get; private set; }

		private bool _isDeployed = false;
		/// <summary>
		/// Whether this file has been deployed during this execution run or not.
		/// </summary>
		public bool IsDeployed {
			get {
				return _isDeployed;
			}
			set {
				_isDeployed = value;
				if (isDeployedCallback != null) {
					isDeployedCallback(this);
				}
			}
		}
		private Action<DeploymentDisplay> isDeployedCallback;

		public DeploymentDisplay(string source, string destination, string displaySource = "") {
			this.Source = source;
			this.Destination = destination;
			if (String.IsNullOrEmpty(displaySource)) {
				displaySource = source;
			}
			this.DisplaySource = displaySource;
		}

		public override string ToString() {
			return DisplaySource;
		}

		/// <summary>
		/// Register to be notified when the IsDeployed flag changes.
		/// </summary>
		/// <param name="callback">Method to call when IsDeployed changes.</param>
		public void registerIsDeployedCallback(Action<DeploymentDisplay> callback) {
			isDeployedCallback += callback;
		}
	}
}


