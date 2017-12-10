using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravellingSalesmenConsole
{
    class Christofides : Algorithm
    {
        private SimpleGraph minimumSpanningTree = null;
        private List<Edge> independentMinimunEdges = null;
        private List<Vertex> hamiltonVertices = null;
        private bool hasnext;

        public Christofides(CompleteGraph graph, AgentManager agentManager) : base(graph, agentManager)
        {
            hasnext = true;
        }

        public override bool hasAlgorithmNextMove()
        {
            return hasnext;
        }

        public override void NextTurn()
        {
            CompleteGraph tmp = new CompleteGraph(graph.Vertices);
            minimumSpanningTree = CreateMinimumSpanningTree(tmp);
            //-------------
            List<Vertex> oddVertices = new List<Vertex>();

            foreach (var item in minimumSpanningTree.VertexIdAndEdges)
            {
                if (item.Value.Count % 2 != 0)
                {
                    oddVertices.Add(graph.Vertices.Single(x => x.Id == item.Key));
                }
            }
            CompleteGraph fromOddVertices = new CompleteGraph(oddVertices);
            independentMinimunEdges = CalculateIndependentEdges(fromOddVertices);
            //--------------
            if (hamiltonVertices == null)
            {
                hamiltonVertices = CalculateHamiltonCircuit(minimumSpanningTree.Edges, independentMinimunEdges, graph, agentManager.Agents[0].StartPosition);
                hamiltonVertices.Remove(hamiltonVertices[0]);
                foreach (var item in graph.Edges)
                {
                    item.Used = false;
                }
                foreach (var item in graph.Vertices)
                {
                    item.Used = false;
                }
                foreach (var item in agentManager.Agents)
                {
                    graph.Vertices.Single(x => x.Id == item.StartPosition).Used = true;
                }
            }
            /*for (int i = 0; i < agentManager.Agents.Count; i++)
            {

                int nextVertex = hamiltonVertices[0].Id;
                hamiltonVertices.Remove(hamiltonVertices[0]);
                graph.Vertices.Single(item => item.Id == nextVertex).Used = true;
                ((Edge)graph.Edges.Single(edge => (edge.StartVertex.Id == agentManager.Agents[i].ActualPosition && edge.EndVertex.Id == nextVertex)
                                                    || (edge.EndVertex.Id == agentManager.Agents[i].ActualPosition && edge.StartVertex.Id == nextVertex))).Used = true;
                agentManager.Agents[i].ActualPosition = nextVertex;



            }*/
            //EZ !!!!!!!!!
            graph.Edges.Single(e => (e.StartVertex.Id == hamiltonVertices[0].Id && e.EndVertex.Id == agentManager.Agents[0].StartPosition)
                                 || (e.EndVertex.Id == hamiltonVertices[0].Id && e.StartVertex.Id == agentManager.Agents[0].StartPosition)).Used = true;
            
            //IDÁIG!!!!!!!
            for(int i = 0; i < hamiltonVertices.Count-1; i++)
            {
                
                foreach (var e in graph.Edges)
                {
                    if((e.StartVertex.Id == hamiltonVertices[i].Id && e.EndVertex.Id == hamiltonVertices[i+1].Id) || (e.EndVertex.Id == hamiltonVertices[i].Id && e.StartVertex.Id == hamiltonVertices[i + 1].Id))
                    {
                        e.Used = true;
                            break;
                    }
                }
            }

            hasnext = false;

        }

        public override double getActualResult()
        {
            double result = 0;
            
                    foreach (var edge in graph.Edges)
                    {
                        if (edge.Used)
                        {
                            result += edge.Weight;
                        }
                    }
            return Math.Round(result, 2);
        }

        public SimpleGraph CreateMinimumSpanningTree(CompleteGraph g)
        {
            #region inicializálások
            //inizializálások:
            SimpleGraph originalGraph = new SimpleGraph(g.Vertices,g.Edges);

            List<Vertex> treeVertices = new List<Vertex>();     //a fa csúcsai (semmiből építkezünk)
            List<Vertex> nonTreeVertices = new List<Vertex>(originalGraph.Vertices); //azok a csúcsok melyek nem a fában vannak        //az összes egyelőre
            List<Edge> treeEdges = new List<Edge>();            //az éllista

            List<Edge> intermediateEdges = new List<Edge>();    //azok az élek melyek fa és nemfa csúcsok között mennek
            double maxEdgeWeight = 0;                           //max súly
            foreach (Edge e in originalGraph.Edges)             //kiszámoljuk a max súlyt
            {
                if (e.Weight > maxEdgeWeight)
                    maxEdgeWeight = e.Weight;
            }
            #endregion

            #region az_algoritmus_törzse

            MoveVertex(originalGraph.Vertices[0]);              //egy random csúcsot átadunk

            while (nonTreeVertices.Count > 0)
            {
                RecalcIntermediateEdges();
                Edge min = SearchMinimumWeightIntermediateEdge();
                treeEdges.Add(min);
                foreach (Vertex v in treeVertices)
                {
                    if (min.StartVertex.Id == v.Id)
                    {
                        MoveVertex(min.EndVertex);
                        break;
                    }
                    if (min.EndVertex.Id == v.Id)
                    {
                        MoveVertex(min.StartVertex);
                        break;
                    }
                }
            }

            #endregion

            #region függvények
            //függvények
            void MoveVertex(Vertex v)
            {
                if (nonTreeVertices.Count > 0)
                {
                    nonTreeVertices.Remove(nonTreeVertices.Single(x => x.Id == v.Id));
                    treeVertices.Add(v);
                }
            };

            void RecalcIntermediateEdges()
            {
                intermediateEdges.Clear();

                foreach (Vertex treeV in treeVertices)
                {
                    foreach (Vertex v in nonTreeVertices)
                    {
                        foreach (Edge e in originalGraph.Edges)
                        {
                            if ((e.StartVertex.Id == treeV.Id && e.EndVertex.Id == v.Id) || (e.StartVertex.Id == v.Id && e.EndVertex.Id == treeV.Id))
                            {
                                intermediateEdges.Add(e);
                            }
                        }
                    }
                }
            };

            Edge SearchMinimumWeightIntermediateEdge()
            {
                double minimumWeight = maxEdgeWeight + 1;   //ennél az összes kisebb lesz, így biztos kiválasztódik legalább egy él
                Edge temp = null;
                foreach (Edge e in intermediateEdges)
                {
                    if (e.Weight < minimumWeight)
                    {
                        minimumWeight = e.Weight;
                        temp = e;
                    }
                }
                return temp;
            };
            #endregion

            SimpleGraph minimumSpanningTree = new SimpleGraph();
            foreach (var item in treeVertices)
            {
                minimumSpanningTree.addVertex(item);
            }
            foreach (var item in treeEdges)
            {
                minimumSpanningTree.addEdge(item);
            }

            return minimumSpanningTree;
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

        static public List<Edge> CalculateIndependentEdges(CompleteGraph g)
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

        //http://www.geeksforgeeks.org/hierholzers-algorithm-directed-graph/
        private List<Vertex> CalculateHamiltonCircuit(List<Edge> minSpanningTree, List<Edge> perfectMaching, CompleteGraph fullOriginalGraph, int startPointId)
        {
            //eddig mindent kinulláztunk
            List<Edge> combinedEdgeList = new List<Edge>();
            combinedEdgeList.AddRange(minSpanningTree);
            combinedEdgeList.AddRange(perfectMaching);

            foreach (var item in combinedEdgeList)
            {
                item.Used = false;
            }

            List<Vertex> currentPathVertices = new List<Vertex>();      //Egy pálya a csúcsokknak, addig megy egy úton amíg talál szabad élet
            List<Edge> currentPathEdges = new List<Edge>();             //Egy pálya az éleknek, addig megy egy úton amíg talál szabad élet


            //egy random él hozzáadása (a csúcsokat is, mindkettővel együtt fogunk számolni)
            foreach (Edge item in combinedEdgeList)
            {
                if (item.EndVertex.Id == startPointId)
                {
                    item.Used = true;
                    combinedEdgeList.Single(x => (x.StartVertex.Id == item.StartVertex.Id && x.EndVertex.Id == item.EndVertex.Id) ||
                                (x.EndVertex.Id == item.StartVertex.Id && x.StartVertex.Id == item.EndVertex.Id)).Used = true;
                    currentPathVertices.Add(item.EndVertex);      //a random él vége
                    currentPathVertices.Add(item.StartVertex);    //egy random él eleje

                    break;
                }
                else if (item.StartVertex.Id == startPointId)
                {
                    item.Used = true;
                    combinedEdgeList.Single(x => (x.StartVertex.Id == item.StartVertex.Id && x.EndVertex.Id == item.EndVertex.Id) ||
                                (x.EndVertex.Id == item.StartVertex.Id && x.StartVertex.Id == item.EndVertex.Id)).Used = true;
                    currentPathVertices.Add(item.StartVertex);    //egy random él eleje
                    currentPathVertices.Add(item.EndVertex);      //a random él vége
                    break;
                }
            }


            int usedEdgeCount = 1;  //mert egy már van
            List<Edge> freeableEdges = new List<Edge>();
            while (usedEdgeCount != combinedEdgeList.Count)
            {
                if (currentPathVertices[0].Equals(currentPathVertices.Last<Vertex>()))  //ha az első és az utolsó csúcs megegyezik a pályában (itt BIZTOS VAN használatlan él)
                {
                    while (true)
                    {
                        bool haveUnusedEdge = false;
                        foreach (Edge e in combinedEdgeList.Where(x => x.StartVertex.Id == currentPathVertices.Last<Vertex>().Id || x.EndVertex.Id == currentPathVertices.Last<Vertex>().Id))/*g.getEdgesByVertex(currentPathVertices.Last<Vertex>().Id*/
                        {
                            if (e.Used == false)
                            {
                                haveUnusedEdge = true;
                                break;
                            }
                        }
                        if (haveUnusedEdge)
                        {
                            break;      //kilép a while-ból, a currentPath utolsó tagjának van használatlan éle
                        }
                        else
                        {
                            Vertex last = currentPathVertices.Last<Vertex>();
                            currentPathVertices.RemoveAt(currentPathVertices.Count - 1);     //elveszük a pályából az utolsót 
                            Vertex beforelast = currentPathVertices.Last<Vertex>();
                            Edge edgetoAdd = combinedEdgeList.Where(x => (x.StartVertex.Id == last.Id && x.EndVertex.Id == beforelast.Id) || (x.EndVertex.Id == last.Id && x.StartVertex.Id == beforelast.Id)).ToList()[0];
                            freeableEdges.Add(edgetoAdd);
                            usedEdgeCount--;
                        }
                    }
                }
                else        //ha a pálya első és utolsó csúcsa nem egyezik meg
                {
                    foreach (Edge e in combinedEdgeList.Where(x => x.StartVertex.Id == currentPathVertices.Last<Vertex>().Id || x.EndVertex.Id == currentPathVertices.Last<Vertex>().Id).OrderBy(item => Coordinator.rnd.Next()))/*g.getEdgesByVertex(currentPathVertices.Last<Vertex>().Id*/
                    {
                        if (e.StartVertex.Equals(currentPathVertices.Last<Vertex>()) && e.Used == false)    //ha egy használatlan élet találtunk
                        {
                            e.Used = true;
                            currentPathVertices.Add(e.EndVertex);
                            
                            usedEdgeCount++;
                            foreach (var item in freeableEdges)
                            {
                                item.Used = false;
                            }
                            freeableEdges.Clear();
                            break;                          //kilépünk a foreachból, csak egy új élet kértünk
                        }
                        else if (e.EndVertex.Equals(currentPathVertices.Last<Vertex>()) && e.Used == false)
                        {
                            e.Used = true;
                            currentPathVertices.Add(e.StartVertex);
                           
                            usedEdgeCount++;
                            foreach (var item in freeableEdges)
                            {
                                item.Used = false;
                            }
                            freeableEdges.Clear();
                            break;                           //kilépünk a foreachból, csak egy új élet kértünk
                        }
                    }
                }
            }
            List<int> foundedIds = new List<int>();
            List<Vertex> hamiltonVertices = new List<Vertex>();
            foreach (var item in currentPathVertices)
            {
                if (!foundedIds.Contains(item.Id))
                {
                    foundedIds.Add(item.Id);
                    hamiltonVertices.Add(item);
                }
            }
            return hamiltonVertices;
        }
    }
}
