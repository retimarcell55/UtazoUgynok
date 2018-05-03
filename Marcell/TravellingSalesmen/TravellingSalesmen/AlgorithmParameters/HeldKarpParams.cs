using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TravellingSalesmen.Algorithms;

namespace TravellingSalesmen.AlgorithmParameters
{
    public partial class HeldKarpParams : ParamsWindow
    {
        public HeldKarpParams(Algorithm algorithm) : base(algorithm)
        {
            InitializeComponent();
        }

        private void Submit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
