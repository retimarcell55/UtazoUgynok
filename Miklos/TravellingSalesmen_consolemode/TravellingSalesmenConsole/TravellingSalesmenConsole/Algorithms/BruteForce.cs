using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravellingSalesmenConsole
{
    class BruteForce : Algorithm
    {
        List<List<Edge>> solutionEdges;

        public BruteForce(CompleteGraph graph, AgentManager agentManager) : base(graph, agentManager)
        {
            solutionEdges = new List<List<Edge>>();
            List<List<Vertex>> solutionInOrder = MultiTravel(AgentManager.Agents[0].StartPosition,AgentManager.Agents.Count, graph.Vertices.Count, graph.Vertices, 1, graph.Vertices.Count - 1);
            
            foreach (var item in solutionInOrder)
            {
                List<Edge> actualEdges = new List<Edge>();
                for (int i = 0; i < item.Count - 1; i++)
                {
                    actualEdges.Add(graph.Edges.First(edge => (edge.StartVertex.Id == item[i].Id && edge.EndVertex.Id == item[i + 1].Id) || (edge.StartVertex.Id == item[i + 1].Id && edge.EndVertex.Id == item[i].Id)));
                }
                solutionEdges.Add(actualEdges);
            }
        }

        public override void NextTurn()
        {

        }

        public override double getActualResult()
        {
            if (solutionEdges.Count == 0)
            {
                return 0;
            }
            double max = 0;
            foreach (var edges in solutionEdges)
            {
                double sum = 0;
                foreach (var edge in edges)
                {
                    sum += graph.AdjacencyMatrix[edge.StartVertex.Id, edge.EndVertex.Id];
                }
                if (sum > max)
                {
                    max = sum;
                }
            }
            return max;
        }

        public override bool hasAlgorithmNextMove()
        {
            return true;
        }

        public List<List<Vertex>> MultiTravel(int index, int numberoftravelers, int size, List<Vertex> list, int l, int r)
        {
            kezdopont(list, index);
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
                                if (hamcircles[i][0].Id != hamcircles[i][k].Id)
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
                            //tmpWeight += SumWeight(tmp[j]);
                        }
                        for (int j = 0; j < tmp.Count; j++)
                        {
                            tmpWeight += SumWeight(tmp[j]);
                        }
                        
                        if (LongestRoute(tmp) < LongestRoute(optimal))
                        {
                            optimal.Clear();
                            optimal = new List<List<Vertex>>(tmp);
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
                sumweight += graph.AdjacencyMatrix[l[i].Id, l[i + 1].Id];
            }

            return sumweight;
        }
        public double LongestRoute(List<List<Vertex>> l)
        {
            double longestroute = 0;
            foreach (var item in l)
            {
                if (SumWeight(item) > longestroute)
                {
                    longestroute = SumWeight(item);
                }
            }
            return longestroute;
        }
        public void kezdopont(List<Vertex> l, int idx)
        {
            Vertex tmp = null;
            int ind = 0;
            for (int i = 0; i < l.Count; i++)
            {
                if (l[i].Id == idx)
                {
                    tmp = (l[i]);
                    ind = i;
                }
            }
            l.RemoveAt(ind);
            l.Insert(0, tmp);
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
