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
        private Coordinator coordinator;
        private const int VERTEX_RADIUS = 10;
        private const int CIRCLE_RADIUS = 200;

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

            Graph graph = fm.readGraphFromFile(AdjacencyPath.Text);
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
            coordinator.Configuration = new FileManager().loadConfiguration(ConfigurationsComboBox.SelectedItem.ToString());

            switch (AlgorithmComboBox.SelectedItem.ToString())
            {
                case "RandomSearch":
                    coordinator.Algorithm = new RandomSearch(coordinator.Configuration.Graph, coordinator.Configuration.AgentManager);
                    break;
            }

            Restart.Enabled = true;
            RunThrough.Enabled = true;
            NextMove.Enabled = true;

            coordinator.startAlgorithm();
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

        public void DrawGraph(int graphVertexCount, AgentManager agentManager, List<Vertex> usedVertices, List<Edge> usedEdges)
        {

            System.Drawing.Graphics graphics = visualizer.CreateGraphics();
            graphics.Clear(Color.YellowGreen);
            Point center = new Point(visualizer.Width / 2, visualizer.Height / 2);
            PointF[] nPoints = CalculateVertices(graphVertexCount, CIRCLE_RADIUS, 0, center);

            for (int i = 0; i < nPoints.Length; i++)
            {
                System.Drawing.Rectangle rectangle = new System.Drawing.Rectangle(
                 (int)nPoints[i].X - (VERTEX_RADIUS / 2), (int)nPoints[i].Y - (VERTEX_RADIUS / 2), VERTEX_RADIUS, VERTEX_RADIUS);
                if (agentManager.Agents.Exists(agent => agent.ActualPosition == i))
                {
                    graphics.FillEllipse(new System.Drawing.SolidBrush(System.Drawing.Color.Red), rectangle);
                }
                else if (usedVertices.Any(vertex => vertex.Id == i && vertex.Used))
                {
                    graphics.FillEllipse(new System.Drawing.SolidBrush(System.Drawing.Color.Yellow), rectangle);
                }
                else
                {
                    graphics.FillEllipse(new System.Drawing.SolidBrush(System.Drawing.Color.Black), rectangle);
                }
            }

            for (int i = 0; i < nPoints.Length - 1; i++)
            {
                for (int j = i + 1; j < nPoints.Length; j++)
                {
                    if (usedEdges.Exists(edge => edge.Start == i && edge.End == j && edge.Used) || usedEdges.Exists(edge => edge.End == i && edge.Start == j && edge.Used))
                    {
                        graphics.DrawLine(System.Drawing.Pens.Yellow, new Point((int)nPoints[i].X, (int)nPoints[i].Y), new Point((int)nPoints[j].X, (int)nPoints[j].Y));
                    }
                    else
                    {
                        graphics.DrawLine(System.Drawing.Pens.Black, new Point((int)nPoints[i].X, (int)nPoints[i].Y), new Point((int)nPoints[j].X, (int)nPoints[j].Y));
                    }
                }
            }
        }

        private PointF[] CalculateVertices(int sides, int radius, float startingAngle, Point center)
        {

            List<PointF> points = new List<PointF>();
            float step = 360.0f / sides;

            float angle = startingAngle; //starting angle
            for (double i = startingAngle; i < startingAngle + 360.0; i += step) //go in a circle
            {
                points.Add(DegreesToXY(angle, radius, center));
                angle += step;
            }

            return points.ToArray();
        }

        private PointF DegreesToXY(float degrees, float radius, Point origin)
        {
            PointF xy = new PointF();
            double radians = degrees * Math.PI / 180.0;

            xy.X = (int)(Math.Cos(radians) * radius + origin.X);
            xy.Y = (int)(Math.Sin(-radians) * radius + origin.Y);

            return xy;
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
            coordinator.startAlgorithm();
        }
    }
}
