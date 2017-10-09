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
            for (int i = 0; i < 100; i++)
            {
                g.Vertices.Add(new Vertex(i, new Coordinate(i % 50, i % 2)));
            }
            Random rnd = new Random();

            foreach (Vertex item1 in g.Vertices)
            {
                foreach (Vertex item2 in g.Vertices)
                {
                    if (item1.Id < item2.Id)
                    {
                        g.Edges.Add(new Edge(item1, item2, false, rnd.Next(1, 101)));
                    }
                }
            }
            foreach (Edge item in g.Edges)
            {
                Console.WriteLine("(" + item.StartVertex.Id + ";" + item.EndVertex.Id + "):" + item.Id + "=" + item.Weight);
            }
            foreach (Vertex itemv in g.Vertices)
            {
                itemv.Edges = new List<Edge>();
                foreach (Edge iteme in g.Edges)
                {
                    if (iteme.StartVertex.Id == itemv.Id || iteme.EndVertex.Id == itemv.Id)
                        itemv.Edges.Add(iteme);
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
            g.Edges.OrderBy(x=>x.Weight);
            foreach (Vertex item in g.Vertices)
                item.Edges.OrderBy(x => x.Weight);
            

            //Itt Keszitjuk el magat a megoldast
            List<EdgeLeavCost> ELC = new List<EdgeLeavCost>();
            List<Edge> NeighboursOnStart = new List<Edge>();
            List<Edge> NeighboursOnEnd = new List<Edge>();
            List<Edge> Result = new List<Edge>();
            Edge[] Neighbours = new Edge[2];
            double Cost;
            double MinW;
            while (g.Vertices.FindAll(x=>x.Used==false).Count > 3)
            {
                //ELC feltoltese a naluk kisebb sulyuaktol fuggetlen elekkel, es a hozzajuk tartozo tavozasi koltseggel
                ELC.RemoveAll(x=>x.e.Used==true);
                foreach (Edge iteme in g.Edges)
                {
                    if(iteme.Used==false)
                        if (iteme.Weight == iteme.StartVertex.Edges.Where(x => x.Used == false).Min(x => x.Weight) && iteme.Weight == iteme.EndVertex.Edges.Where(x => x.Used == false).Min(x => x.Weight))
                        {
                            Console.WriteLine("Yolo" + iteme.Id);
                            Cost = 0;
                            iteme.StartVertex.Used = true;
                            iteme.EndVertex.Used = true;
                            iteme.StartVertex.Edges.ForEach(x=>x.Used=true);
                            iteme.EndVertex.Edges.ForEach(x => x.Used = true);
                            foreach (Vertex itemv in g.Vertices)
                            {
                                if (itemv.Used == false)
                                    Cost += itemv.Edges.First(x => x.Used == false).Weight - itemv.Edges[0].Weight;
                            }
                            ELC.Add(new EdgeLeavCost(iteme, Cost));
                            iteme.StartVertex.Used = false;
                            iteme.EndVertex.Used = false;
                            iteme.StartVertex.Edges.FindAll(x => x.EndVertex.Used == false && x.StartVertex.Used == false).ForEach(x => x.Used = false);
                            iteme.EndVertex.Edges.FindAll(x => x.EndVertex.Used == false && x.StartVertex.Used == false).ForEach(x => x.Used = false);
                        }
                }

                //Kikeressuk a legoptimalisabb kezdo elet
                ELC.OrderBy(x => x.e.Weight);
                ELC.OrderBy(x => x.LeavCost);

                //Leellenorizzuk, hogy van e jobb megoldas
                MinW = ELC.Max(x => x.e.Weight) * 2;
                NeighboursOnStart = ELC[0].e.StartVertex.Edges.FindAll(x=>x.Used=false);
                NeighboursOnEnd = ELC[0].e.EndVertex.Edges.FindAll(x => x.Used = false);
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
                if (MinW < ELC[0].e.Weight + g.Edges.FindAll(x => x.Used==false && !(NeighboursOnEnd.Contains(x) || NeighboursOnStart.Contains(x))).Min(x => x.Weight))
                {
                    Result.Add(Neighbours[0]);
                    Result.Add(Neighbours[1]);
                    Neighbours[0].StartVertex.Used=true;
                    Neighbours[1].StartVertex.Used = true;
                    Neighbours[0].EndVertex.Used = true;
                    Neighbours[1].EndVertex.Used = true;
                    Neighbours[0].EndVertex.Edges.ForEach(x => x.Used = true);
                    Neighbours[0].StartVertex.Edges.ForEach(x => x.Used = true);
                    Neighbours[1].EndVertex.Edges.ForEach(x => x.Used = true);
                    Neighbours[1].StartVertex.Edges.ForEach(x => x.Used = true);
                }
                else
                {
                    Result.Add(ELC[0].e);
                    ELC[0].e.EndVertex.Used=true;
                    ELC[0].e.StartVertex.Used=true;
                    ELC[0].e.EndVertex.Edges.ForEach(x => x.Used = true);
                    ELC[0].e.StartVertex.Edges.ForEach(x => x.Used = true);
                }
            }
            //ha maradt egy el, azt
            if (g.Edges.FindAll(x=>x.Used==false).Count != 0)
                Result.Add(g.Edges.First(x => x.Used==false && x.Weight == g.Edges.FindAll(y=>y.Used==false).Min(y => y.Weight)));
            return Result;
        }

    }
}
