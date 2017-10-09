using System;
using System.Collections.Generic;
using System.Linq;


namespace Algo
{
    class Program
    {
        static void Main(string[] args)
        {
            Graph g = new Graph();
            for (int i = 0; i < 1000; i++)
            {
                g.Vertices.Add(new Vertex(i, new Coordinate(i % 500, i % 2)));
            }
            Random rnd = new Random();

            foreach (Vertex item1 in g.Vertices)
            {
                foreach (Vertex item2 in g.Vertices)
                {
                    if (item1.Id < item2.Id)
                    {
                        g.Edges.Add(new Edge(item1, item2, false, rnd.Next(1, 1001)));
                    }
                }
            }
            foreach (Edge item in g.Edges)
            {
                Console.WriteLine("(" + item.StartVertex.Id + ";" + item.EndVertex.Id + "):" + item.Id + "=" + item.Weight);
            }
            foreach (Vertex itemv in g.Vertices)
            {
                g.VertexIdAndEdges.Add(itemv.Id,new List<Edge>());
                foreach (Edge iteme in g.Edges)
                {
                    if (iteme.StartVertex.Id == itemv.Id || iteme.EndVertex.Id == itemv.Id)
                        g.VertexIdAndEdges[itemv.Id].Add(iteme);
                }
            }
            List<Edge> result = CalculateIndependentEdges(g);
            foreach (Edge item in result)
            {
                Console.WriteLine("(" + item.StartVertex.Id + ";" + item.EndVertex.Id + "):" + item.Id + "=" + item.Weight);
            }
            Console.ReadKey();
        }

        struct EdgeLeavCost
        {
            public Edge e;
            public double LeavCost;
            public EdgeLeavCost(Edge ed, double Lc)
            {
                e = ed;
                LeavCost = Lc;
            }
        };
        static public List<Edge> CalculateIndependentEdges(Graph g)
        {
            g.Edges.OrderBy(x => x.Weight);
            foreach (Vertex item in g.Vertices)
                g.VertexIdAndEdges[item.Id].OrderBy(x => x.Weight);


            //Itt Keszitjuk el magat a megoldast
            List<EdgeLeavCost> ELC = new List<EdgeLeavCost>();
            List<Edge> NeighboursOnStart = new List<Edge>();
            List<Edge> NeighboursOnEnd = new List<Edge>();
            List<Edge> Result = new List<Edge>();
            Edge[] Neighbours = new Edge[2];
            double Cost;
            double MinW;
            while (g.Vertices.FindAll(x => x.Used == false).Count > 3)
            {
                //ELC feltoltese a naluk kisebb sulyuaktol fuggetlen elekkel, es a hozzajuk tartozo tavozasi koltseggel
                ELC.RemoveAll(x => x.e.Used == true);
                foreach (Edge iteme in g.Edges)
                {
                    if (iteme.Used == false)
                        if (iteme.Weight == g.VertexIdAndEdges[iteme.StartVertex.Id].Where(x => x.Used == false).Min(x => x.Weight) && iteme.Weight == g.VertexIdAndEdges[iteme.EndVertex.Id].Where(x => x.Used == false).Min(x => x.Weight))
                        {
                            Console.WriteLine(Result.Count+"Yolo" + iteme.Id);
                            Cost = 0;
                            iteme.StartVertex.Used = true;
                            iteme.EndVertex.Used = true;
                            g.VertexIdAndEdges[iteme.StartVertex.Id].ForEach(x => x.Used = true);
                            g.VertexIdAndEdges[iteme.EndVertex.Id].ForEach(x => x.Used = true);
                            foreach (Vertex itemv in g.Vertices)
                            {
                                if (itemv.Used == false)
                                    Cost += g.VertexIdAndEdges[itemv.Id].First(x => x.Used == false).Weight - g.VertexIdAndEdges[itemv.Id][0].Weight;
                            }
                            ELC.Add(new EdgeLeavCost(iteme, Cost));
                            iteme.StartVertex.Used = false;
                            iteme.EndVertex.Used = false;
                            g.VertexIdAndEdges[iteme.StartVertex.Id].FindAll(x => x.EndVertex.Used == false && x.StartVertex.Used == false).ForEach(x => x.Used = false);
                            g.VertexIdAndEdges[iteme.EndVertex.Id].FindAll(x => x.EndVertex.Used == false && x.StartVertex.Used == false).ForEach(x => x.Used = false);
                        }
                }

                //Kikeressuk a legoptimalisabb kezdo elet
                ELC.OrderBy(x => x.e.Weight);
                ELC.OrderBy(x => x.LeavCost);

                //Leellenorizzuk, hogy van e jobb megoldas
                MinW = ELC.Max(x => x.e.Weight) * 2;
                NeighboursOnStart = g.VertexIdAndEdges[ELC[0].e.StartVertex.Id].FindAll(x => x.Used = false);
                NeighboursOnEnd = g.VertexIdAndEdges[ELC[0].e.EndVertex.Id].FindAll(x => x.Used = false);
                foreach (Edge itemE in NeighboursOnEnd)
                {
                    foreach (Edge itemS in NeighboursOnStart)
                    {
                        if (itemE.StartVertex.Id != itemS.StartVertex.Id && itemE.EndVertex.Id != itemS.StartVertex.Id &&
                            itemE.StartVertex.Id != itemS.EndVertex.Id && itemE.EndVertex.Id != itemS.EndVertex.Id &&
                            itemE.Weight + itemS.Weight <= MinW)
                        {
                            MinW = itemE.Weight + itemS.Weight;
                            Neighbours[0] = itemS;
                            Neighbours[1] = itemE;
                        }

                    }
                }
                //Ellenorzes kiertekelese es El Kivalasztasa, Eredmenylistaba illesztese, Az eredeti graf egyszerusitese
                if (MinW < ELC[0].e.Weight + g.Edges.FindAll(x => x.Used == false && !(NeighboursOnEnd.Contains(x) || NeighboursOnStart.Contains(x))).Min(x => x.Weight))
                {
                    Result.Add(Neighbours[0]);
                    Result.Add(Neighbours[1]);
                    Neighbours[0].StartVertex.Used = true;
                    Neighbours[1].StartVertex.Used = true;
                    Neighbours[0].EndVertex.Used = true;
                    Neighbours[1].EndVertex.Used = true;
                    g.VertexIdAndEdges[Neighbours[0].EndVertex.Id].ForEach(x => x.Used = true);
                    g.VertexIdAndEdges[Neighbours[0].StartVertex.Id].ForEach(x => x.Used = true);
                    g.VertexIdAndEdges[Neighbours[1].EndVertex.Id].ForEach(x => x.Used = true);
                    g.VertexIdAndEdges[Neighbours[1].StartVertex.Id].ForEach(x => x.Used = true);
                }
                else
                {
                    Result.Add(ELC[0].e);
                    ELC[0].e.EndVertex.Used = true;
                    ELC[0].e.StartVertex.Used = true;
                    g.VertexIdAndEdges[ELC[0].e.EndVertex.Id].ForEach(x => x.Used = true);
                    g.VertexIdAndEdges[ELC[0].e.StartVertex.Id].ForEach(x => x.Used = true);
                }
            }
            //ha maradt egy el, azt
            if (g.Edges.FindAll(x => x.Used == false).Count != 0)
                Result.Add(g.Edges.First(x => x.Used == false && x.Weight == g.Edges.FindAll(y => y.Used == false).Min(y => y.Weight)));
            return Result;
        }

    }
}
