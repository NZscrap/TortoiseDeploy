using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TortoiseDeploy {
	public class DeploymentMapping {

		[JsonIgnore]
		public string WorkingSource;

		private string _source;
		public string Source {
			get {
				if (!String.IsNullOrEmpty(WorkingSource)) {
					return WorkingSource;
				}
				return _source;
			}
			set {
				_source = value;
			}
		}

		[JsonIgnore]
		public string WorkingDestination;

		private string _destination;
		public string Destination {
			get {
				if (!String.IsNullOrEmpty(WorkingDestination)) {
					return WorkingDestination;
				}
				return _destination;
			}
			set {
				_destination = value;
			}
		}
		public string Display {
			get {
				return _source + "\t->\t" + Destination;
			}
		}

		public override string ToString() {
			return "\t" + Display;
		}
	}
}
