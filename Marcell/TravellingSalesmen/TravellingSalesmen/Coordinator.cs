using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravellingSalesmen.Algorithms;

namespace TravellingSalesmen
{
    class Coordinator
    {
        private Configuration configuration;
        private Algorithm algorithm;
        private bool algorithmStarted;
        private MainForm mainForm;

        public Configuration Configuration { get => configuration; set => configuration = value; }
        public Algorithm Algorithm { get => algorithm; set => algorithm = value; }

        public Coordinator(MainForm mainForm)
        {
            this.mainForm = mainForm;
            algorithmStarted = false;
        }

        public Coordinator(MainForm mainForm, Configuration configuration, Algorithm algorithm)
        {
            this.mainForm = mainForm;
            this.Configuration = configuration;
            this.Algorithm = algorithm;
            algorithmStarted = false;
        }

        public void startAlgorithm()
        {
            algorithm.Initialize();

            mainForm.DrawGraph(configuration.Graph, configuration.AgentManager);

            algorithmStarted = true;
        }

        public void runAlgorithmNextMove()
        {
            if (algorithmStarted)
            {
                if (algorithm.hasNonVisitedVertexLeft())
                {
                    algorithm.NextTurn();
                    switch (algorithm.ActualDrawingMode)
                    {
                        case Algorithm.DRAWING_MODE.GRAPH:
                            mainForm.DrawGraph(configuration.Graph, configuration.AgentManager);
                            break;
                        case Algorithm.DRAWING_MODE.MIN_SPANNING_TREE:
                            mainForm.HighLightEdges(algorithm.EdgesToHighlight, Algorithm.DRAWING_COLOR.RED);
                            break;
                        default:
                            break;
                    }

                    mainForm.UpdateResult(algorithm.getActualResult().ToString());
                }
                else
                {
                    algorithmStarted = false;
                }
            }
        }

        public void runAlgorithmThrough()
        {
            if (algorithmStarted)
            {
                while (algorithm.hasNonVisitedVertexLeft())
                {
                    algorithm.NextTurn();
                    mainForm.DrawGraph(configuration.Graph, configuration.AgentManager);
                    mainForm.UpdateResult(algorithm.getActualResult().ToString());

                    System.Threading.Thread.Sleep(500);
                }
                algorithmStarted = false;
            }
        }
    }
}
