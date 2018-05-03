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
    public partial class GeneticAlgorithmParams : ParamsWindow
    {
        public GeneticAlgorithmParams(Algorithm algorithm) : base(algorithm)
        {
            InitializeComponent();
        }

        private void GenerationsLabel_Click(object sender, EventArgs e)
        {
            information = "A generációk számával állítható be hogy hány iterációt végezzen az algoritmus, hány új generációt hozzon létre.";
            ShowInformation();
        }

        private void PopulationSizeLabel_Click(object sender, EventArgs e)
        {
            information = "A populáció számával meghatározható, hogy egy-egy generáció hány egyedet tartalmazzon. A populáció száma a generációk előrehaladtával nem változik.";
            ShowInformation();
        }

        private void FistChildMutateLabel_Click(object sender, EventArgs e)
        {
            information = "A paraméter segítségével beállítható, hogy egy keresztezésből származó első egyed mutálódhat-e vagy sem.";
            ShowInformation();
        }

        private void SecondChildMutateLabel_Click(object sender, EventArgs e)
        {
            information = "A paraméter segítségével beállítható, hogy egy keresztezésből származó második egyed mutálódhat-e vagy sem.";
            ShowInformation();
        }

        private void MutationProbabilityLabel_Click(object sender, EventArgs e)
        {
            information = "A mutációs probabilitás segítségével állítható, hogy egy újonnan létrejött egyed milyen eséllyel mutálódhat. A paraméter értéke 0.0 és 1.0 közötti szám.";
            ShowInformation();
        }

        private void WeakParentRateLabel_Click(object sender, EventArgs e)
        {
            information = "A paraméter segítségével állítható, hogy a populáció hányad részét lehet keresztezésre használni. A paraméter értéke 0.0 és 1.0 közötti szám. A felhasznált egyenlet a kieső kromoszómák számára: (int)(population.Length / 2 * (1 - WEAK_PARENT_RATE))";
            ShowInformation();
        }

        private void Submit_Click(object sender, EventArgs e)
        {
            string parameters = GenerationsTextBox.Text + "," + PopulationSizeTextBox.Text + ",";
            if(FirstChildMutateCheckBox.Checked)
            {
                parameters += "true,";
            }
            else
            {
                parameters += "false,";
            }
            if (SecondCildMutateCheckBox.Checked)
            {
                parameters += "true,";
            }
            else
            {
                parameters += "false,";
            }
            parameters += MutationProbabilityTextBox.Text + "," + WeakParentRateTextBox.Text;
            PassParametersToAlgorithm(parameters);
            this.Close();
        }
    }
}
