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
        public static Random rnd = new Random();
        private Algorithm algorithm;
        private bool algorithmStarted;
        private MainForm mainForm;

        public Algorithm Algorithm { get => algorithm; set => algorithm = value; }

        public Coordinator(MainForm mainForm)
        {
            this.mainForm = mainForm;
            algorithmStarted = false;
            
        }

        public Coordinator(MainForm mainForm, Algorithm algorithm)
        {
            this.mainForm = mainForm;
            this.Algorithm = algorithm;
            algorithmStarted = false;
        }

        public void startAlgorithm()
        {
            algorithm.Initialize();

            mainForm.DrawGraph(algorithm.Graph, algorithm.AgentManager);
            switch (algorithm.ActualDrawingMode)
            {
                case Algorithm.DRAWING_MODE.MORE_AGENT_CIRCLES:
                    mainForm.MoreCirclesToHighlight(algorithm.MoreAgentCirclesToHighlight);
                    mainForm.UpdateResult(algorithm.getActualResult().ToString());
                    break;
            }
            algorithmStarted = true;
        }

        public void runAlgorithmNextMove()
        {
            if (algorithmStarted)
            {
                if (algorithm.hasAlgorithmNextMove())
                {
                    algorithm.NextTurn();
                    switch (algorithm.ActualDrawingMode)
                    {
                        case Algorithm.DRAWING_MODE.GRAPH:
                            mainForm.DrawGraph(algorithm.Graph, algorithm.AgentManager);
                            break;
                        case Algorithm.DRAWING_MODE.MIN_SPANNING_TREE:
                            mainForm.HighLightEdges(algorithm.EdgesToHighlight, Algorithm.DRAWING_COLOR.BLUE);
                            break;
                        case Algorithm.DRAWING_MODE.INDEPENDENT_EDGE_SET:
                            mainForm.HighLightEdges(algorithm.EdgesToHighlight, Algorithm.DRAWING_COLOR.RED);
                            break;
                        case Algorithm.DRAWING_MODE.MORE_AGENT_CIRCLES:
                            mainForm.DrawGraph(algorithm.Graph, algorithm.AgentManager);
                            mainForm.MoreCirclesToHighlight(algorithm.MoreAgentCirclesToHighlight);
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
                while (algorithm.hasAlgorithmNextMove())
                {
                    algorithm.NextTurn();
                    mainForm.DrawGraph(algorithm.Graph, algorithm.AgentManager);
                    mainForm.UpdateResult(algorithm.getActualResult().ToString());

                    System.Threading.Thread.Sleep(500);
                }
                algorithmStarted = false;
            }
        }
    }
}
