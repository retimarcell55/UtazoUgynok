using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravellingSalesmen.Algorithms.AntColonyOptimization
{
    class AntColonyOptimization : Algorithm
    {

        private const int ITERATIONS = 10;
        private int currentIteration;
        private AntManager antManager;
        private double actualResult;
        private List<List<Edge>> iterationResult;

        public AntColonyOptimization(CompleteGraph graph, AgentManager agentManager) : base(graph, agentManager)
        {
            currentIteration = 1;
            actualResult = 0;
            actualDrawingMode = DRAWING_MODE.MORE_AGENT_CIRCLES;
            moreAgentCirclesToHighlight = new List<List<Edge>>();
            iterationResult = new List<List<Edge>>();
        }

        public override void Initialize()
        {
            base.Initialize();
            antManager = new AntManager(agentManager.Agents[0].StartPosition, agentManager.Agents.Count, graph);
        }

        public override void NextTurn()
        {
            iterationResult = antManager.SpreadAnts();
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
            return actualResult;
        }
    }
}
