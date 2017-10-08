using System;
using System.Collections.Generic;
using System.Linq;


namespace Algo
{
    class Program
    {
        static void Main(string[] args)
        {
            Graph g=new Graph();
            for (int i = 0; i < 8; i++)
            {
                g.Vertices.Add(new Vertex(i, new Coordinate(i%4, i%2)));
            }
            Random rnd = new Random();

            foreach (Vertex item1 in g.Vertices)
            {
                foreach (Vertex item2 in g.Vertices)
                {
                    if (item1.Id < item2.Id)
                    {
                        g.Edges.Add(new Edge(item1,item2,false,rnd.Next(1,7)));
                    }
                }
            }
            foreach (Edge item in g.Edges)
            {
                Console.WriteLine("("+item.StartVertex.Id+";"+item.EndVertex.Id+"):"+item.Id+"="+item.Weight);
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
        static public List<Edge> CalculateIndependentEdges(Graph g) {
            Graph OriginalG = g;
            Graph Gtmp= new Graph();
            OriginalG.Edges.OrderBy(x => x.Weight);

            //Itt Keszitjuk el magat a megoldast
            List<EdgeLeavCost> ELC = new List<EdgeLeavCost>();
            List<Edge> NeighboursOnStart = new List<Edge>();
            List<Edge> NeighboursOnEnd = new List<Edge>();
            List<Edge> Result = new List<Edge>();
            Edge[] Neighbours = new Edge[2];
            double Cost;
            double MinW;
            while (OriginalG.Edges.Count>3)
            {
                //ELC feltoltese a naluk kisebb sulyuaktol fuggetlen elekkel, es a hozzajuk tartozo tavozasi koltseggel
                ELC.Clear();
                foreach (Edge iteme in OriginalG.Edges)
                {
                    if (iteme.Weight == iteme.StartVertex.Edges.Min(x=>x.Weight) && iteme.Weight == iteme.EndVertex.Edges.Min(x=>x.Weight))
                    {
                        Cost = 0;
                        Gtmp.Edges.Clear();
                        Gtmp.Edges.AddRange(OriginalG.Edges);
                        Gtmp.Vertices.Clear();
                        Gtmp.Vertices.AddRange(OriginalG.Vertices);
                        foreach (Vertex itemv2 in Gtmp.Vertices)
                        {
                            itemv2.Edges.Clear();
                            foreach (Edge iteme2 in Gtmp.Edges)
                                if (iteme2.StartVertex.Id == itemv2.Id || iteme2.EndVertex.Id == itemv2.Id)
                                    itemv2.Edges.Add(iteme2);
                        }
                        Gtmp.Edges.RemoveAll(x => x.EndVertex.Id == iteme.EndVertex.Id || x.StartVertex.Id == iteme.StartVertex.Id || x.EndVertex.Id == iteme.StartVertex.Id || x.StartVertex.Id == iteme.EndVertex.Id);
                        Gtmp.Vertices.Remove(iteme.StartVertex);
                        Gtmp.Vertices.Remove(iteme.EndVertex);
                        foreach (Vertex itemv in Gtmp.Vertices)
                        {
                            itemv.Edges.RemoveAll(x => (Gtmp.Edges.FindAll(y => y.StartVertex.Id == x.StartVertex.Id && y.EndVertex.Id == x.EndVertex.Id).ToList().Count != 1));
                            if(itemv.Edges.Count!=0)
                            Cost += itemv.Edges.Min(x=>x.Weight) - OriginalG.Vertices.Find(x => x.Id == itemv.Id).Edges.Min(x=>x.Weight);
                        }
                        ELC.Add(new EdgeLeavCost(iteme, Cost));
                    }
                }

                //Kikeressuk a legoptimalisabb kezdo elet
                ELC.OrderBy(x => x.e.Weight);
                ELC.OrderBy(x => x.LeavCost);

                //Leellenorizzuk, hogy van e jobb megoldas
                MinW = OriginalG.Edges.Max(x=>x.Weight)*2;
                NeighboursOnStart = OriginalG.Edges.FindAll(x => x.StartVertex.Id == ELC[0].e.StartVertex.Id || x.EndVertex.Id == ELC[0].e.StartVertex.Id);
                NeighboursOnEnd = OriginalG.Edges.FindAll(x =>  x.EndVertex.Id == ELC[0].e.EndVertex.Id  || x.StartVertex.Id == ELC[0].e.EndVertex.Id);
                foreach (Edge itemE in NeighboursOnEnd)
                {
                    foreach (Edge itemS in NeighboursOnStart)
                    {
                        if(itemE.StartVertex.Id!=itemS.StartVertex.Id&& itemE.EndVertex.Id != itemS.StartVertex.Id &&
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
                if (MinW < ELC[0].e.Weight + OriginalG.Edges.FindAll(x => !(NeighboursOnEnd.Contains(x) || NeighboursOnStart.Contains(x))).ToList().Min(x=>x.Weight))
                {
                    Result.Add(Neighbours[0]);
                    Result.Add(Neighbours[1]);
                    OriginalG.Edges.RemoveAll(x=>x.StartVertex==Neighbours[0].StartVertex|| x.StartVertex == Neighbours[0].EndVertex || x.EndVertex == Neighbours[0].StartVertex || x.EndVertex == Neighbours[0].EndVertex ||
                    x.StartVertex == Neighbours[1].StartVertex || x.StartVertex == Neighbours[1].EndVertex || x.EndVertex == Neighbours[1].StartVertex || x.EndVertex == Neighbours[1].EndVertex );
                    OriginalG.Vertices.Remove(Neighbours[0].StartVertex);
                    OriginalG.Vertices.Remove(Neighbours[1].StartVertex);
                    OriginalG.Vertices.Remove(Neighbours[0].EndVertex);
                    OriginalG.Vertices.Remove(Neighbours[1].EndVertex);
                }
                else
                {
                    Result.Add(ELC[0].e);
                    OriginalG.Edges.RemoveAll(x => x.StartVertex == ELC[0].e.StartVertex || x.StartVertex == ELC[0].e.EndVertex || x.EndVertex == ELC[0].e.StartVertex || x.EndVertex == ELC[0].e.EndVertex);
                    OriginalG.Vertices.Remove(ELC[0].e.StartVertex);
                    OriginalG.Vertices.Remove(ELC[0].e.StartVertex);
                }
            }
            //ha maradt egy el, azt
            if (OriginalG.Edges.Count != 0)
                Result.Add(OriginalG.Edges.Find(x=>x.Weight== OriginalG.Edges.Min(y=>y.Weight)));
            return Result;
        }
        
    }
}
