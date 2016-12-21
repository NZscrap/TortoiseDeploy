using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TortoiseDeploy.GUI {
	public partial class Form1 : Form {
		public Form1() {
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e) {
			List<string> changedPaths = new List<String>();

			changedPaths.Add("Test one");
			changedPaths.Add("Test two");

			((ListBox)this.checkListChangedPaths).DataSource = changedPaths;
		}

		private void btnMerge_Click(object sender, EventArgs e) {
			StringBuilder output = new StringBuilder();

			foreach(var i in this.checkListChangedPaths.CheckedItems) {
				output.AppendLine(i.ToString());
			}

			MessageBox.Show(output.ToString());
		}
	}
}
