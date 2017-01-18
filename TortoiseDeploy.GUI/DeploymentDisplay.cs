using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TortoiseDeploy.GUI {
	class DeploymentDisplay {
		DeploymentMapping mapping;
		public string Source { get; private set; }
		public string Destination { get; private set; }
		public string Display { get; private set; }

		public DeploymentDisplay(string source, string destination, string display) {
			this.Source = source;
			this.Destination = destination;
			this.Display = display;
		}

		public override string ToString() {
			return Display;
		}
	}
}


