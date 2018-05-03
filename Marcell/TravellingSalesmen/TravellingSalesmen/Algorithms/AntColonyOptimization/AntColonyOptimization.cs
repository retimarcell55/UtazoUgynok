using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravellingSalesmen.AlgorithmParameters;

namespace TravellingSalesmen.Algorithms.AntColonyOptimization
{
    class AntColonyOptimization : Algorithm
    {

        private int ITERATIONS = 10;
        private int currentIteration;
        private AntManager antManager;

        public AntColonyOptimization(CompleteGraph graph, AgentManager agentManager) : base(graph, agentManager)
        {
            currentIteration = 1;
            actualDrawingMode = DRAWING_MODE.MORE_AGENT_CIRCLES;
            moreAgentCirclesToHighlight = new List<List<Edge>>();
            antManager = new AntManager(agentManager.Agents[0].StartPosition, agentManager.Agents.Count, graph);

            initParamsWindow(new AntColonyOptimizationParams(this));
        }

        public override void Initialize()
        {
            ITERATIONS = 10;
        }

        public override void TestInitialize()
        {
            string[] paramsArray = testParameters.Split(',');

            ITERATIONS = int.Parse(paramsArray[0]);
            antManager.SetParameters(testParameters);
        }

        public override void NextTurn()
        {
            List<string> result = antManager.SpreadAnts();
            moreAgentCirclesToHighlight.Clear();
            
            foreach (var path in result)
            {
                List<Edge> edgeList = new List<Edge>();
                for (int i = 0; i < path.Length - 1; i++)
                {
                    Edge e = graph.Edges.Single(edge => (edge.StartVertex.Id.ToString() == path[i].ToString() && edge.EndVertex.Id.ToString() == path[i + 1].ToString()) || (edge.StartVertex.Id.ToString() == path[i + 1].ToString() && edge.EndVertex.Id.ToString() == path[i].ToString()));
                    edgeList.Add(e);
                }
                moreAgentCirclesToHighlight.Add(edgeList);
            }

            currentIteration++;
        }

        public override bool hasAlgorithmNextMove()
        {
            if(currentIteration > ITERATIONS)
            {
                return false;
            }
            return true;
        }

        public override double getActualResult()
        {
            double max = 0;
            foreach (var path in moreAgentCirclesToHighlight)
            {
                double local = 0;
                foreach (var edge in path)
                {
                    local += edge.Weight;
                }
                if(local > max)
                {
                    max = local;
                }
            }
            return max;
        }
    }
}
