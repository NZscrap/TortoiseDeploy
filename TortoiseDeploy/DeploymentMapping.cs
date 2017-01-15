namespace TortoiseDeploy {
	public class DeploymentMapping {

		public string Source { get; set; }
		public string Destination { get; set; }

		public string Display {
			get {
				return Source + "\t->\t" + Destination;
			}
		}

		public override string ToString() {
			return "\t" + Display;
		}
	}
}
