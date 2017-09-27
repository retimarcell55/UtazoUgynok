using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravellingSalesmen.Algorithms
{
    class BruteForce : Algorithm
    {
        private int bestResult;
        private List<Edge> bestAgentRoute;

        public int BestResult { get => bestResult; set => bestResult = value; }

        public BruteForce(CompleteGraph graph, AgentManager agentManager) : base(graph, agentManager)
        {

        }

        public override void Initialize()
        {
            base.Initialize();
            bestResult = 0;
            bestAgentRoute = new List<Edge>();
        }

        private void computeBestRoute()
        {
            for (int i = 0; i < graph.Edges.Count; i++)
            {
                List<Edge> actualRoute = new List<Edge>();
                for (int j = 0; j < graph.Edges.Count; j++)
                {

                }
            }
        }

        public override void NextTurn()
        {
            throw new NotImplementedException();
        }
    }
}
