using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravellingSalesmen
{
    class Christofides : Algorithm
    {
        public enum STAGES { MIN_SPANNING_TREE };
        private STAGES actualStage;
        private Graph minimumSpanningTree = null;

        public STAGES ActualStage { get => actualStage; set => actualStage = value; }
        public Graph MinimumSpanningTree { get => minimumSpanningTree; set => minimumSpanningTree = value; }

        public Christofides(CompleteGraph graph, AgentManager agentManager) : base(graph, agentManager)
        {
            actualStage = STAGES.MIN_SPANNING_TREE;
            actualDrawingMode = DRAWING_MODE.MIN_SPANNING_TREE;
        }

        public override void NextTurn()
        {
            switch (actualStage)
            {
                case STAGES.MIN_SPANNING_TREE:
                    Graph tmp = new Graph();
                    tmp.Vertices = new List<Vertex>(graph.Vertices);
                    tmp.Edges = new List<Edge>(graph.Edges);
                    MinimumSpanningTree mSP = new MinimumSpanningTree(tmp);
                    minimumSpanningTree = mSP.CalculateMinimumSpanningTree();
                    break;
                default:
                    break;
            }
        }

        public override double getActualResult()
        {
            double result = 0;
            if (actualStage == STAGES.MIN_SPANNING_TREE)
            {

                foreach (var edge in minimumSpanningTree.Edges)
                {
                    result += edge.Weight;

                }
            }

            return Math.Round(result, 2);
        }
    }
}
