using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
            int sleepTime = 500;
#if TEST
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            int counter = 0;
#endif

            if (algorithmStarted)
            {
                while (algorithm.hasAlgorithmNextMove())
                {
                    algorithm.NextTurn();
                    mainForm.DrawGraph(algorithm.Graph, algorithm.AgentManager);
                    mainForm.UpdateResult(algorithm.getActualResult().ToString());

                    System.Threading.Thread.Sleep(sleepTime);
#if TEST
                    counter++;
#endif
                }
                algorithmStarted = false;
#if TEST
                //time
                stopWatch.Stop();
                TimeSpan ts = stopWatch.Elapsed;
                
                //write
                writeTestOutput(algorithm, ts, counter, sleepTime/1000);    
#endif

            }

        }


        public static void writeTestOutput(Algorithm algo, TimeSpan time, int counter, double sleepperoid)
        {
            string[] str = algo.getInfos();
            
            const string BASE_FOLDER_LOCATION = @"..\..\..\Tests";

            

            string path = BASE_FOLDER_LOCATION + @"\" + algo.GetName() + "_" + algo.SelectedConf + "_" ;
            int i;
            for(i = 0; i < str.Length - 1; i++)
            {
                path += str[i] + "_";
            }
            path += str[i] + ".txt";

            

            using (StreamWriter sw = (File.Exists(path)) ? File.AppendText(path) : File.CreateText(path));

            int prevnum;
            if (File.ReadAllLines(path).Length != 0)
            {
                String lastLine = File.ReadLines(path).Last();
                if (lastLine.Contains("."))
                {

                    String[] prevResult = lastLine.Split('.');
                    prevnum = int.Parse(prevResult[0]) + 1;
                }
                else prevnum = 1;
            }
            else
            {
                prevnum = 1;
            }

            double finalResult = algo.getActualResult();
            //write
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(path, true))
            {
                file.WriteLine(prevnum + ". run:\tWeight: " + finalResult + "\tTime: " + (time.TotalSeconds - (counter * sleepperoid)));
            }

        }
    }
}
