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
    public partial class GreedySearchParams : ParamsWindow
    {
        public GreedySearchParams(Algorithm algorithm) : base(algorithm)
        {
            InitializeComponent();
        }

        private void SubmitButton_Click(object sender, EventArgs e)
        {
            string parameters = PatienceNumberTextBox.Text + "," + NumerOfRunsTextBox.Text + "," + MaxRouteLengthPerAgentTextBox.Text;
            PassParametersToAlgorithm(parameters);
            this.Close();
        }

        private void PatienceNumberLabel_Click(object sender, EventArgs e)
        {
            information = "A PatienceNumber paraméter segítségével állítható, hogy az algoritmus hányszor próbáljon egy újabb jobb megoldást keresni az adott iterációban.\n" +
                "Minél nagyobb az érték, annál tovább vár egy jobb megoldásra és az adott iteráció leállítására.";
            ShowInformation();
        }

        private void NumberOfRunsLabel_Click(object sender, EventArgs e)
        {
            information = "A NumberOfRuns segítségével állítható, hogy hány iteráció után álljon le véglegesen az algoritmus.";
            ShowInformation();
        }


        private void MaxRouteLengthPerAgentLabel_Click(object sender, EventArgs e)
        {
            information = "A MaxRouteLengthPerAgent segítségével állítható, hogy egy ágens maximálisan hány lépést tehet meg.\n" +
                "A paraméter rossz beállításával a feladat megoldhatatlanná is válhat!";
            ShowInformation();
        }
    }
}
