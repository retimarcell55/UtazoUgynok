using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Hamkor
    {
        public List<Edge> ham(Graph g)
        {
            int x=g.VertexCount;
            int y=g.VertexCount;
            int i = 0;
            int j = 0;
            int kor = 1;
            int[] oszlop = new int[y];
            
            List<List<Edge>> HamEdgeList = new List<List<Edge>>();
            List<int> bp = new List<int>();
            double osszeg=0;
            double min = 0;
            g.BuildAdjacencyMatrix();
            for (int idx = 0; idx < y; idx++)
            {
                oszlop[idx] = -1;
            }

            while (j < y)
            {
                bool benne = false;
                bool ellentet = false;
                for (int elem = 0; elem < y; elem++)
                {
                    if (oszlop[elem] == j)
                    {
                        benne = true;
                    }
                }
                for (int e = 0; e < bp.Count; e += 2)
                {
                    if (i == bp[e + 1])
                    {
                        if (j == bp[e])
                        {
                            ellentet = true;
                        }

                    }
                }

                if (i != j && !benne && !ellentet)
                {
                    osszeg += g.AdjacencyMatrix[i, j];
                    oszlop[i] = j;
                    bp.Add(i);
                    bp.Add(j);
                    j = 0;
                    if (x - 1 > i)
                    {
                        i++;
                    }
                }
                else if (bp.Count / 2 < x && j + 1 == y)
                {
                    while (j + 1 == y)
                    {
                        osszeg -= g.AdjacencyMatrix[bp[bp.Count - 2], bp[bp.Count - 1]];
                        i = bp[(bp.Count - 2)];
                        j = bp[(bp.Count - 1)];
                        oszlop[i] = -1;
                        bp.RemoveAt(bp.Count - 1);
                        bp.RemoveAt(bp.Count - 1);
                    }
                    j++;
                }
                else
                {
                    j++;
                }
                if (bp.Count / 2 == x && bp[1] < y - 1)
                {
                    List<Edge> bejartelek = new List<Edge>();
                    for (int v=0;v<bp.Count;v+=2)
                    {
                        for (int n = 0; n < g.EdgeCount; n++)
                        {
                            if (g.Edges[n].StartVertex.Id == v && g.Edges[n].EndVertex.Id == v+1 || g.Edges[n].StartVertex.Id == v+1 && g.Edges[n].EndVertex.Id == v)
                            {
                                bejartelek.Add(g.Edges[n]);
                            }
                        }
                    }
                    HamEdgeList.Add(bejartelek);
                    // Console.WriteLine(osszeg);
                    if (kor == 1)
                    {
                        min = osszeg;
                     
                    }
                    else
                    {
                        if (osszeg < min)
                        {
                            min = osszeg;
                            HamEdgeList.RemoveAt(0);
                        }
                    }
                    i = 0;
                    j = bp[1] + 1;
                    osszeg = 0;
                    while (bp.Count != 0)
                    {
                        bp.RemoveAt(bp.Count - 1);
                    }
                    for (int idx = 0; idx < y; idx++)
                    {
                        oszlop[idx] = -1;
                    }
                    kor++;

                }
                else if (bp.Count / 2 == x && i + 1 == x && j + 1 == y)
                {
                    List<Edge> bejartelek = new List<Edge>();
                    for (int v = 0; v < bp.Count; v += 2)
                    {
                        for (int n = 0; n < g.EdgeCount; n++)
                        {
                            if (g.Edges[n].StartVertex.Id == v && g.Edges[n].EndVertex.Id == v + 1 || g.Edges[n].StartVertex.Id == v + 1 && g.Edges[n].EndVertex.Id == v)
                            {
                                bejartelek.Add(g.Edges[n]);
                            }
                        }
                    }
                    HamEdgeList.Add(bejartelek);
                    // Console.WriteLine(osszeg);
                    if (osszeg < min)
                    {
                        min = osszeg;
                        HamEdgeList.RemoveAt(0);
                    }
                    else
                    {
                        HamEdgeList.RemoveAt(1);
                       
                    }

                }

            }
            List<Edge> MinHam = new List<Edge>();
            MinHam=HamEdgeList[0];

            return MinHam;

        }
    }
}
