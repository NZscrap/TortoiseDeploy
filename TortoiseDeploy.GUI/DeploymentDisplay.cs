using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TortoiseDeploy.GUI {
	class DeploymentDisplay {
		public string Source { get; private set; }
		public string Destination { get; private set; }

		public DeploymentDisplay(string source, string destination) {
			this.Source = source;
			this.Destination = destination;
		}

		public override string ToString() {
			return Source;
		}
	}
}


