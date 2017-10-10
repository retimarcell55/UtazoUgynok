using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravellingSalesmen.Algorithms
{
    class BruteForce : Algorithm
    {
        List<Vertex> solutionInOrder;

        public BruteForce(CompleteGraph graph, AgentManager agentManager) : base(graph, agentManager)
        {
            solutionInOrder = getMinHam(graph.Vertices, 1, graph.Vertices.Count);
        }

        public override void NextTurn()
        {
            for (int i = 0; i < agentManager.Agents.Count; i++)
            {
                int nextVertex = solutionInOrder[0].Id;
                solutionInOrder.Remove(solutionInOrder[0]);
                graph.Vertices.Single(item => item.Id == nextVertex).Used = true;
                ((Edge)graph.Edges.Single(edge => (edge.StartVertex.Id == agentManager.Agents[i].ActualPosition && edge.EndVertex.Id == nextVertex)
                                                    || (edge.EndVertex.Id == agentManager.Agents[i].ActualPosition && edge.StartVertex.Id == nextVertex))).Used = true;
                agentManager.Agents[i].ActualPosition = nextVertex;

            }
        }
        private List<Vertex> getMinHam(List<Vertex> list, int l, int r)
        {
            List<Vertex> minhamcircle = new List<Vertex>();
            double minweight = 0;
            permute(list, l, r, minhamcircle, minweight);

            return minhamcircle;
        }

        private void permute(List<Vertex> list, int l, int r, List<Vertex> minhamcircle, double minweight)
        {
            if (l == r)
            {
                if (minhamcircle.Count == 0)
                {
                    minhamcircle = list;
                    for (int i = 1; i < list.Count; i++)
                    {
                        minweight += Math.Sqrt(Math.Pow(list[i].Position.X - list[i - 1].Position.X, 2) + Math.Pow(list[i].Position.Y - list[i - 1].Position.Y, 2));
                    }
                }
                else
                {
                    double d = 0;
                    for (int i = 1; i < list.Count; i++)
                    {
                        d += Math.Sqrt(Math.Pow(list[i].Position.X - list[i - 1].Position.X, 2) + Math.Pow(list[i].Position.Y - list[i - 1].Position.Y, 2));
                    }
                    if (d < minweight)
                    {
                        minhamcircle = list;
                    }
                }

            }
            else
            {
                for (int i = l; i < r; i++)
                {
                    list = swap(list, l, i);
                    permute(list, l + 1, r, minhamcircle, minweight);
                    list = swap(list, l, i);
                }
            }
        }

        private List<Vertex> swap(List<Vertex> a, int i, int j)
        {
            Vertex temp;
            List<Vertex> list = new List<Vertex>(a);
            temp = list[i];
            list[i] = list[j];
            list[j] = temp;
            return list;
        }

    }
}
