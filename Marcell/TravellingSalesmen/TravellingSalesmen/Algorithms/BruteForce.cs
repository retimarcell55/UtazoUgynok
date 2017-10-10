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
            solutionInOrder = MinWeightHamiltonCircle(graph.Vertices, 1, graph.Vertices.Count - 1);
            solutionInOrder.Remove(solutionInOrder[0]);
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

        private List<Vertex> MinWeightHamiltonCircle(List<Vertex> list, int l, int r)
        {
            return getMinHam(list, l, r)[0];
        }

        private List<List<Vertex>> getMinHam(List<Vertex> list, int l, int r)
        {
            List<List<Vertex>> minhamcircle = new List<List<Vertex>>();
            double minweight = 0;
            permute(list, l, r, minhamcircle, minweight);

            return minhamcircle;
        }

        private void permute(List<Vertex> list, int l, int r, List<List<Vertex>> minhamcircle, double minweight)
        {
            if (l == r)
            {
                List<Vertex> tmp = new List<Vertex>(list);
                minhamcircle.Add(tmp);

                if (minhamcircle.Count == 1)
                {




                }
                else
                {
                    for (int i = 0; i < minhamcircle[0].Count - 1; i++)
                    {

                        minweight += Math.Sqrt(Math.Pow(minhamcircle[0][i + 1].Position.X - minhamcircle[0][i].Position.X, 2) + Math.Pow(minhamcircle[0][i + 1].Position.Y - minhamcircle[0][i].Position.Y, 2));
                    }
                    minweight += Math.Sqrt(Math.Pow(minhamcircle[0][minhamcircle[0].Count - 1].Position.X - minhamcircle[0][0].Position.X, 2) + Math.Pow(minhamcircle[0][minhamcircle[0].Count - 1].Position.Y - minhamcircle[0][0].Position.Y, 2));

                    Console.Write(minweight);
                    Console.Write(" ");
                    double d = 0;
                    for (int i = 0; i < minhamcircle[minhamcircle.Count - 1].Count - 1; i++)
                    {

                        d += Math.Sqrt(Math.Pow(minhamcircle[1][i + 1].Position.X - minhamcircle[1][i].Position.X, 2) + Math.Pow(minhamcircle[1][i + 1].Position.Y - minhamcircle[1][i].Position.Y, 2));
                    }
                    d += Math.Sqrt(Math.Pow(minhamcircle[1][minhamcircle[1].Count - 1].Position.X - minhamcircle[1][0].Position.X, 2) + Math.Pow(minhamcircle[1][minhamcircle[1].Count - 1].Position.Y - minhamcircle[1][0].Position.Y, 2));
                    Console.WriteLine(d);
                    if (d <= minweight)
                    {
                        minhamcircle.RemoveAt(0);
                    }
                    else
                    {
                        minhamcircle.RemoveAt(1);
                    }




                }


            }
            else
            {
                for (int i = l; i <= r; i++)
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
            List<Vertex> list;
            list = a;
            temp = a[i];
            list[i] = list[j];
            list[j] = temp;
            return list;
        }

    }
}
