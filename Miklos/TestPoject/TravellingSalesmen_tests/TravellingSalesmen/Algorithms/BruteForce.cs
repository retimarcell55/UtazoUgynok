using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravellingSalesmen.Algorithms
{
    class BruteForce : Algorithm
    {
        List<List<Vertex>> solutionInOrder;

        public BruteForce(CompleteGraph graph, AgentManager agentManager) : base(graph, agentManager)
        {
            solutionInOrder = MultiTravel(2,graph.Vertices.Count,graph.Vertices, 1, graph.Vertices.Count - 1);
            //solutionInOrder.Remove(solutionInOrder[0]);
            foreach (var item in solutionInOrder)
            {
                item.RemoveAt(0);
            }
        }

        public override void NextTurn()
        {
            for (int i = 0; i < agentManager.Agents.Count; i++)
            {
                if(solutionInOrder[i].Count != 0)
                {
                    int nextVertex = solutionInOrder[i][0].Id;
                    solutionInOrder[i].RemoveAt(0);
                    graph.Vertices.Single(item => item.Id == nextVertex).Used = true;
                    ((Edge)graph.Edges.Single(edge => (edge.StartVertex.Id == agentManager.Agents[i].ActualPosition && edge.EndVertex.Id == nextVertex)
                                                        || (edge.EndVertex.Id == agentManager.Agents[i].ActualPosition && edge.StartVertex.Id == nextVertex))).Used = true;
                    agentManager.Agents[i].ActualPosition = nextVertex;
                }
            }
        }

        public List<List<Vertex>> MultiTravel(int numberoftravelers, int size, List<Vertex> list, int l, int r)
        {
            List<List<Vertex>> hamcircles = new List<List<Vertex>>();
            permute(list, l, r, hamcircles);
            List<List<Vertex>> optimal = new List<List<Vertex>>();
            int[] travelers = new int[numberoftravelers];
            for (int i = 0; i < numberoftravelers; i++)
            {
                travelers[i] = i + 1;
            }
            int[] indexes = new int[size];
            int total = (int)Math.Pow(numberoftravelers, size);

            int[] snapshot = new int[size];

            while (total-- > 0)
            {
                for (int i = 0; i < size; i++)
                {
                    snapshot[i] = travelers[indexes[i]];
                }
                //A két permutáció találkozása
                List<List<Vertex>> tmp = new List<List<Vertex>>();
                //Végig az összes hamkörön
                for (int i = 0; i < hamcircles.Count; i++)
                {
                    tmp.Clear();
                    //Szétoszom a pontokat az utazók között
                    for (int j = 1; j <= numberoftravelers; j++)
                    {
                        //Kezdőpontot minden utazó listába belerakom
                        List<Vertex> rout = new List<Vertex>();
                        rout.Add(hamcircles[i][0]);
                        //külön listába az egyes utak
                        for (int k = 0; k < snapshot.Length; k++)
                        {
                            if (snapshot[k] == j)
                            {
                                if (hamcircles[i][0] != hamcircles[i][k])
                                {
                                    rout.Add(hamcircles[i][k]);
                                }

                            }

                        }
                        tmp.Add(rout);
                    }
                    if (optimal.Count == 0)
                    {
                        optimal = new List<List<Vertex>>(tmp);
                    }
                    else
                    {
                        double optimalWeight = 0;
                        double tmpWeight = 0;
                        for (int j = 0; j < optimal.Count; j++)
                        {
                            optimalWeight += SumWeight(optimal[j]);
                            tmpWeight += SumWeight(tmp[j]);
                        }
                        if (optimalWeight > tmpWeight)
                        {
                            optimal = tmp;
                        }
                    }

                }

                for (int i = 0; i < size; i++)
                {
                    if (indexes[i] >= numberoftravelers - 1)
                    {
                        indexes[i] = 0;
                    }
                    else
                    {
                        indexes[i]++;
                        break;
                    }
                }
            }
            return optimal;
        }
        public void Kiir(int[] snapshot)
        {
            for (int i = 0; i < snapshot.Length; i++)
            {
                Console.Write(snapshot[i]);

            }
            Console.WriteLine();

        }
        public double SumWeight(List<Vertex> l)
        {
            double sumweight = 0;
            for (int i = 0; i < l.Count - 1; i++)
            {
                sumweight += Math.Sqrt(Math.Pow(l[i + 1].Position.X - l[i].Position.X, 2) + Math.Pow(l[i + 1].Position.Y - l[i].Position.Y, 2));
            }
            sumweight += Math.Sqrt(Math.Pow(l[l.Count - 1].Position.X - l[0].Position.X, 2) + Math.Pow(l[l.Count - 1].Position.Y - l[0].Position.Y, 2));

            return sumweight;
        }
        private void permute(List<Vertex> list, int l, int r, List<List<Vertex>> hamcircles)
        {
            if (l == r)
            {
                List<Vertex> tmp = new List<Vertex>(list);
                hamcircles.Add(tmp);
            }
            else
            {
                for (int i = l; i <= r; i++)
                {
                    list = swap(list, l, i);
                    permute(list, l + 1, r, hamcircles);
                    list = swap(list, l, i);
                }
            }
        }
        public List<Vertex> swap(List<Vertex> a, int i, int j)
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
