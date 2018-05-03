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
    public partial class AntColonyOptimizationParams : ParamsWindow
    {
        public AntColonyOptimizationParams(Algorithm algorithm) : base(algorithm)
        {
            InitializeComponent();
        }

        private void EvaporationRateLabel_Click(object sender, EventArgs e)
        {
            information = "A párolgási tényező adja meg, hogy a feromonszint hány százaléka párolog el a gráf éleiről egy iteráció után.";
            ShowInformation();
        }

        private void IterationsLabel_Click(object sender, EventArgs e)
        {
            information = "Az iterációs paraméter adja meg, hogy az algoritmus hányiterációt végezzen el.";
            ShowInformation();
        }

        private void SpreadCountLabel_Click(object sender, EventArgs e)
        {
            information = "A Spread Count baraméter beállításával lehet beállítani, hogy egy adott iterációban az ágensszámú hangyák hányszor járják be a teljes gráfot.";
            ShowInformation();
        }

        private void PheromoneUpdateTurncountLabel_Click(object sender, EventArgs e)
        {
            information = "A paraméter beállításával meg lehet adni, hogy az adotzt iteráción belül hány hangyaáramoltatást követően legyenek frissítve a gráf éleinek feromonértékei.";
            ShowInformation();
        }

        private void TransitionInfluenceLabel_Click(object sender, EventArgs e)
        {
            information = "A tranzíciós faktor paramétermegadja, hogy milyen mértékben veszik figyelembe a hangyák a gráf bejárása közben az élek súlyozását. A paraméter értéke 0 és 1 között kell hogy legyen. A tranzíciós faktor és a feromonfaktor összege 1-et kell, hogy adjon.";
            ShowInformation();
        }

        private void PheromoneInfluenceLabel_Click(object sender, EventArgs e)
        {
            information = "A feromon faktor paramétermegadja, hogy milyen mértékben veszik figyelembe a hangyák a gráf bejárása közben az élek feromonszintjét. A paraméter értéke 0 és 1 között kell hogy legyen. A tranzíciós faktor és a feromonfaktor összege 1-et kell, hogy adjon.";
            ShowInformation();
        }

        private void MinimumPheromoneLabel_Click(object sender, EventArgs e)
        {
            information = "A paraméter megadja, hogy az adott gráfon mekkora a minimális feromonszint az éleken, ami alá az érték soha sem mehet.";
            ShowInformation();
        }

        private void Submit_Click(object sender, EventArgs e)
        {
            string parameters = IterationsTextBox.Text + "," + SpreadCountTextBox.Text + "," + PheromoneUpdateTurncountTextBox.Text + "," +
                TransitionInfluenceTextBox.Text + "," + PheromoneInfluenceTextBox.Text + "," + MinimumPheromoneTextBox.Text + "," + EvaporationRateTextBox.Text;
            PassParametersToAlgorithm(parameters);
            this.Close();
        }
    }
}
