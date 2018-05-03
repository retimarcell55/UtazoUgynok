using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TravellingSalesmen.Algorithms;

namespace TravellingSalesmen.AlgorithmParameters
{

    public abstract class ParamsWindow : Form
    {
        protected string information;
        protected Algorithm algorithm;

        public ParamsWindow(Algorithm algorithm)
        {
            this.algorithm = algorithm;
        }

        protected void ShowInformation()
        {
            MessageBox.Show(information, "Parameter Details", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        protected void PassParametersToAlgorithm(string parameters)
        {
            algorithm.TestParameters = parameters;
        }
    }
}
