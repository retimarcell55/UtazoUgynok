using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            Draw();
            algorithmStarted = true;
        }

        public void runAlgorithmNextMove()
        {
            if (algorithmStarted)
            {
                if (algorithm.hasAlgorithmNextMove())
                {
                    algorithm.NextTurn();
                    Draw();
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
                    Draw();
                    System.Threading.Thread.Sleep(500);
                }
                algorithmStarted = false;
            }
        }

        private void Draw()
        {
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

        public void testAlgorithm(string confName)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            algorithm.TestInitialize();
            algorithmStarted = true;

            if (algorithmStarted)
            {
                while (algorithm.hasAlgorithmNextMove())
                {
                    algorithm.NextTurn();
                }
                algorithmStarted = false;
            }

            stopwatch.Stop();
            long elapsed_time = stopwatch.ElapsedMilliseconds;

            FileManager fm = new FileManager();
            fm.CreateTestFolderIfItNotExists();
            fm.LogAlgorithmResults(algorithm.GetName(),confName,algorithm.TestParameters,elapsed_time.ToString(),algorithm.getActualResult().ToString());
        }
    }
}
