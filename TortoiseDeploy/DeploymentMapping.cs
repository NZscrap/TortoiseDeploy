namespace TortoiseDeploy {
	public class DeploymentMapping {

		public string Source { get; set; }
		public string Destination { get; set; }

		public override string ToString() {
			return Source + "\t->\t" + Destination; ;
		}
	}
}
