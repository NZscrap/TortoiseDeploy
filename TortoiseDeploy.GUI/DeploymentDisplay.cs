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
	}
}


