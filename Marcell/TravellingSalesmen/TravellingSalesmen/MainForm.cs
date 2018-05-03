using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TravellingSalesmen.AlgorithmParameters;
using TravellingSalesmen.Algorithms;
using TravellingSalesmen.Algorithms.AntColonyOptimization;

namespace TravellingSalesmen
{
    public partial class MainForm : Form
    {
        private Coordinator coordinator;
        private const int VERTEX_RADIUS = 10;
        private const int CIRCLE_RADIUS = 270;

        public MainForm()
        {
            InitializeComponent();
            coordinator = new Coordinator(this);
        }

        private void SaveConfigurationButton_Click(object sender, EventArgs e)
        {
            if (ConfigurationName.Text == null)
            {
                MessageBox.Show("Please name your configuration!");
            }

            FileManager fm = new FileManager();

            CompleteGraph graph = fm.readGraphFromFile(VertexCoordPath.Text);
            AgentManager agentManager = fm.readAgentsFromFile(AgentPath.Text);

            Configuration conf = new Configuration(ConfigurationName.Text, graph, agentManager);

            if (conf != null)
            {
                MessageBox.Show("Configuration " + conf.Name + " saved!");
            }

            fm.saveConfiguration(conf);

        }

        private void LoadConfigurations_Click(object sender, EventArgs e)
        {
            FileManager fm = new FileManager();

            List<Configuration> configurations = fm.loadConfigurations();

            ConfigurationsComboBox.Items.Clear();

            for (int i = 0; i < configurations.Count; i++)
            {
                ConfigurationsComboBox.Items.Add(configurations[i].Name);
            }

        }

        private void RunAlgorithm_Click(object sender, EventArgs e)
        {
            ActualResult.Text = string.Empty;
            Configuration conf = new FileManager().loadConfiguration(ConfigurationsComboBox.SelectedItem.ToString());

            switch (AlgorithmComboBox.SelectedItem.ToString())
            { 
                case "GeneticAlgorithm":
                    coordinator.Algorithm = new GeneticAlgorithm(conf.Graph, conf.AgentManager);
                    break;
                case "GreedySearch":
                    coordinator.Algorithm = new GreedySearch(conf.Graph, conf.AgentManager);
                    break;
                case "AntColonyOptimization":
                    coordinator.Algorithm = new AntColonyOptimization(conf.Graph, conf.AgentManager);
                    break;
                case "HeldKarpAlgorithm":
                    coordinator.Algorithm = new HeldKarpAlgorithm(conf.Graph, conf.AgentManager);
                    break;
            }

            Restart.Enabled = true;
            RunThrough.Enabled = true;
            NextMove.Enabled = true;

            coordinator.startAlgorithm();
        }


        private void TestAlgorithm_Click(object sender, EventArgs e)
        {
            ActualResult.Text = string.Empty;
            Configuration conf = new FileManager().loadConfiguration(ConfigurationsComboBox.SelectedItem.ToString());

            switch (AlgorithmComboBox.SelectedItem.ToString())
            {
                case "GeneticAlgorithm":
                    coordinator.Algorithm = new GeneticAlgorithm(conf.Graph, conf.AgentManager);
                    break;
                case "GreedySearch":
                    coordinator.Algorithm = new GreedySearch(conf.Graph, conf.AgentManager);
                    break;
                case "AntColonyOptimization":
                    coordinator.Algorithm = new AntColonyOptimization(conf.Graph, conf.AgentManager);
                    break;
                case "HeldKarpAlgorithm":
                    coordinator.Algorithm = new HeldKarpAlgorithm(conf.Graph, conf.AgentManager);
                    break;
            }

            coordinator.Algorithm.ParamsWindow.Show();
            coordinator.Algorithm.ParamsWindow.FormClosed += new FormClosedEventHandler(ParamsWindowClosed);
        }

        void ParamsWindowClosed(object sender, FormClosedEventArgs e)
        {
            coordinator.testAlgorithm(ConfigurationsComboBox.SelectedItem.ToString());
            MessageBox.Show("Testing Finished!");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadConfigurations_Click(null, null);
            if (ConfigurationsComboBox.Items.Count != 0)
            {
                ConfigurationsComboBox.SelectedIndex = 0;
            }
            if (AlgorithmComboBox.Items.Count != 0)
            {
                AlgorithmComboBox.SelectedIndex = 0;
            }
            this.ActiveControl = label1;

            visualizer.BackColor = Color.YellowGreen;

            Restart.Enabled = false;
            RunThrough.Enabled = false;
            NextMove.Enabled = false;

            ActualResult.Font = new Font("Arial", 18, FontStyle.Bold);
        }

        private void AdjacencyPath_Enter(object sender, EventArgs e)
        {
            VertexCoordPath.Text = string.Empty;
        }

        private void AgentPath_Enter(object sender, EventArgs e)
        {
            AgentPath.Text = string.Empty;
        }

        private void ConfigurationName_Enter(object sender, EventArgs e)
        {
            ConfigurationName.Text = string.Empty;
        }

        public void DrawGraph(CompleteGraph graph, AgentManager agentManager)
        {

            Graphics graphics = visualizer.CreateGraphics();
            graphics.Clear(Color.YellowGreen);

            Font drawFont = new Font("Arial", 12);
            SolidBrush drawBrush = new SolidBrush(Color.Black);
            StringFormat drawFormat = new StringFormat();

            foreach (var vertex in graph.Vertices)
            {
                Rectangle rectangle = new Rectangle(
                 vertex.Position.X - (VERTEX_RADIUS / 2), vertex.Position.Y - (VERTEX_RADIUS / 2), VERTEX_RADIUS, VERTEX_RADIUS);
                if (agentManager.Agents.Exists(agent => agent.ActualPosition == vertex.Id))
                {
                    graphics.FillEllipse(new SolidBrush(Color.Red), rectangle);
                }
                else if (vertex.Used)
                {
                    graphics.FillEllipse(new SolidBrush(Color.Yellow), rectangle);
                }
                else
                {
                    graphics.FillEllipse(new SolidBrush(Color.Black), rectangle);
                }
                string drawString = ((char)(vertex.Id + 65)).ToString();
                graphics.DrawString(drawString, drawFont, drawBrush, vertex.Position.X, vertex.Position.Y, drawFormat);
            }

            Pen pen = null;

            foreach (var edge in graph.Edges)
            {
                if (edge.Used)
                {
                    drawBrush = new SolidBrush(Color.Yellow);
                    pen = Pens.Yellow;
                }
                else
                {
                    drawBrush = new SolidBrush(Color.Black);
                    pen = Pens.Black;
                }
                graphics.DrawLine(pen, new Point(edge.StartVertex.Position.X, edge.StartVertex.Position.Y), new Point(edge.EndVertex.Position.X, edge.EndVertex.Position.Y));
                string drawString = edge.Weight.ToString();

                graphics.DrawString(drawString, drawFont, drawBrush, (edge.StartVertex.Position.X + edge.EndVertex.Position.X) / 2, (edge.StartVertex.Position.Y + edge.EndVertex.Position.Y) / 2, drawFormat);

            }
        }

        public void HighLightEdges(List<Edge> edges, Algorithm.DRAWING_COLOR color)
        {
            Graphics graphics = visualizer.CreateGraphics();
            Pen pen = null;
            switch (color)
            {
                case Algorithm.DRAWING_COLOR.RED:
                    pen = new Pen(Color.FromArgb(125, 255, 0, 0), 7);
                    break;
                case Algorithm.DRAWING_COLOR.GREEN:
                    pen = new Pen(Color.FromArgb(125, 0, 255, 0), 7);
                    break;
                case Algorithm.DRAWING_COLOR.BLUE:
                    pen = new Pen(Color.FromArgb(125, 0, 0, 255), 7);
                    break;
                default:
                    break;
            }
            foreach (var edge in edges)
            {
                graphics.DrawLine(pen, new Point(edge.StartVertex.Position.X, edge.StartVertex.Position.Y), new Point(edge.EndVertex.Position.X, edge.EndVertex.Position.Y));
            }
        }


        internal void MoreCirclesToHighlight(List<List<Edge>> moreAgentCirclesToHighlight)
        {
            Graphics graphics = visualizer.CreateGraphics();
            foreach (var item in moreAgentCirclesToHighlight)
            {
                Color randomColor = Color.FromArgb(Coordinator.rnd.Next(256), Coordinator.rnd.Next(256), Coordinator.rnd.Next(256));
                Pen pen = new Pen(randomColor,7);
                foreach (var edge in item)
                {
                    graphics.DrawLine(pen, new Point(edge.StartVertex.Position.X, edge.StartVertex.Position.Y), new Point(edge.EndVertex.Position.X, edge.EndVertex.Position.Y));
                }
            }
            
        }

        public void UpdateResult(string result)
        {
            ActualResult.Text = result;
        }

        private void NextMove_Click(object sender, EventArgs e)
        {
            coordinator.runAlgorithmNextMove();
        }

        private void RunThrough_Click(object sender, EventArgs e)
        {
            coordinator.runAlgorithmThrough();
        }

        private void Restart_Click(object sender, EventArgs e)
        {
            ActualResult.Text = string.Empty;
            coordinator.startAlgorithm();
        }
    }
}
