using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TravellingSalesmen
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void SaveConfigurationButton_Click(object sender, EventArgs e)
        {
            if (ConfigurationName.Text == null)
            {
                MessageBox.Show("Please name your configuration!");
            }

            FileManager fm = new FileManager();

            Graph graph = fm.readGraphFromFile(AdjacencyPath.Text);
            AgentManager agentManager = fm.readAgentsFromFile(AgentPath.Text);

            Configuration conf = new Configuration(ConfigurationName.Text, graph, agentManager);

            if(conf != null)
            {
                MessageBox.Show("Configuration " + conf.Name + " saved!");
            }

            fm.saveConfiguration(conf);

        }

        private void LoadConfigurations_Click(object sender, EventArgs e)
        {
            FileManager fm = new FileManager();

            List<Configuration> configurations = fm.loadConfigurations();

            for (int i = 0; i < configurations.Count; i++)
            {
                ConfigurationsComboBox.Items.Add(configurations[i].Name);
            }

        }

        private void RunAlgorithm_Click(object sender, EventArgs e)
        {
            Coordinator coordinator = new Coordinator();
            switch (AlgorithmComboBox.SelectedItem.ToString())
            {
                case "GreedySearch":
                    coordinator.Algorithm = new GreedySearch();
                    break;
            }

            coordinator.Configuration = new FileManager().loadConfiguration(ConfigurationsComboBox.SelectedItem.ToString());

            coordinator.runAlgorithm();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadConfigurations_Click(null, null);
            ConfigurationsComboBox.SelectedIndex = 0;
            AlgorithmComboBox.SelectedIndex = 0;
            this.ActiveControl = label1;
        }

        private void AdjacencyPath_Enter(object sender, EventArgs e)
        {
            AdjacencyPath.Text = string.Empty;
        }

        private void AgentPath_Enter(object sender, EventArgs e)
        {
            AgentPath.Text = string.Empty;
        }

        private void ConfigurationName_Enter(object sender, EventArgs e)
        {
            ConfigurationName.Text = string.Empty;
        }
    }
}
