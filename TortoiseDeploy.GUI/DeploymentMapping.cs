using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TortoiseDeploy.GUI {
	class DeploymentMapping {
		public string Source { get; private set;  }
		public string Display { get; private set; }

		public DeploymentMapping(string source, string display) {
			this.Source = source;
			this.Display = display;
		}

		public override string ToString() {
			return Display;
		}
	}
}
