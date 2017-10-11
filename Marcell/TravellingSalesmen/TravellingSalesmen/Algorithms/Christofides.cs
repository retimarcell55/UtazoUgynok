using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravellingSalesmen.Algorithms
{
    class Christofides : Algorithm
    {
        public enum STAGES { MIN_SPANNING_TREE, INDEPENDENT_EDGE_SET, HAMILTON };
        private STAGES actualStage;
        private Graph minimumSpanningTree = null;
        private List<Edge> independentMinimunEdges = null;
        private List<Vertex> hamiltonVertices = null;

        public STAGES ActualStage { get => actualStage; set => actualStage = value; }

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
                    foreach (var item in graph.Vertices)
                    {
                        tmp.addVertex(item);
                    }
                    foreach (var item in graph.Edges)
                    {
                        tmp.addEdge(item);
                    }
                    minimumSpanningTree = CreateMinimumSpanningTree(graph);
                    edgesToHighlight = minimumSpanningTree.Edges;
                    actualStage = STAGES.INDEPENDENT_EDGE_SET;
                    break;
                case STAGES.INDEPENDENT_EDGE_SET:

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
                    edgesToHighlight = independentMinimunEdges;
                    actualDrawingMode = DRAWING_MODE.INDEPENDENT_EDGE_SET;
                    actualStage = STAGES.HAMILTON;
                    break;
                case STAGES.HAMILTON:
                    actualDrawingMode = DRAWING_MODE.INDEPENDENT_EDGE_SET;
                    if (hamiltonVertices == null)
                    {
                        hamiltonVertices = CalculateHamiltonCircuit(minimumSpanningTree.Edges, independentMinimunEdges, graph, agentManager.Agents[0].StartPosition);
                        hamiltonVertices.Remove(hamiltonVertices[0]);
                    }
                    actualDrawingMode = DRAWING_MODE.GRAPH;

                    for (int i = 0; i < agentManager.Agents.Count; i++)
                    {

                        int nextVertex = hamiltonVertices[0].Id;
                        hamiltonVertices.Remove(hamiltonVertices[0]);
                        graph.Vertices.Single(item => item.Id == nextVertex).Used = true;
                        ((Edge)graph.Edges.Single(edge => (edge.StartVertex.Id == agentManager.Agents[i].ActualPosition && edge.EndVertex.Id == nextVertex)
                                                            || (edge.EndVertex.Id == agentManager.Agents[i].ActualPosition && edge.StartVertex.Id == nextVertex))).Used = true;
                        agentManager.Agents[i].ActualPosition = nextVertex;
                    }
                    break;
                default:
                    break;
            }
        }

        /*public override double getActualResult()
        {
            double result = 0;
            switch (actualStage)
            {
                case STAGES.MIN_SPANNING_TREE:
                    foreach (var edge in minimumSpanningTree.Edges)
                    {
                        result += edge.Weight;
                    }
                    break;
                case STAGES.INDEPENDENT_EDGE_SET:
                    foreach (var edge in independentMinimunEdges)
                    {
                        result += edge.Weight;
                    }
                    break;
                case STAGES.HAMILTON:
                    foreach (var edge in graph.Edges)
                    {
                        if (edge.Used)
                        {
                            result += edge.Weight;
                        }
                    }
                    break;
                default:
                    break;
            }

            return Math.Round(result, 2);
        }*/

        public Graph CreateMinimumSpanningTree(Graph g)
        {
            #region inicializálások
            //inizializálások:
            Graph originalGraph = new Graph();
            foreach (var item in graph.Vertices)
            {
                originalGraph.addVertex(item);
            }
            foreach (var item in graph.Edges)
            {
                originalGraph.addEdge(item);
            }
            List<Vertex> treeVertices = new List<Vertex>();     //a fa csúcsai (semmiből építkezünk)
            List<Vertex> nonTreeVertices = new List<Vertex>(); //azok a csúcsok melyek nem a fában vannak
            nonTreeVertices = originalGraph.Vertices;           //az összes egyelőre
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
                    if (min.StartVertex.Equals(v))
                    {
                        MoveVertex(min.EndVertex);
                        break;
                    }
                    if (min.EndVertex.Equals(v))
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
                    nonTreeVertices.Remove(v);
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
                            if ((e.StartVertex.Equals(treeV) && e.EndVertex.Equals(v)) || (e.StartVertex.Equals(v) && e.EndVertex.Equals(treeV)))
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

            Graph minimumSpanningTree = new Graph();
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
                            Console.WriteLine(Result.Count + "Yolo" + iteme.Id);
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
        private List<Vertex> CalculateHamiltonCircuit(List<Edge> minSpanningTree, List<Edge> perfectMaching, Graph fullOriginalGraph, int startPointId)
        {
            foreach (Edge e in minSpanningTree)
            {
                e.Used = false;
            }
            foreach (Edge e in perfectMaching)
            {
                e.Used = false;
            }
            //eddig mindent kinulláztunk
            List<Edge> combinedEdgeList = new List<Edge>();
            combinedEdgeList.AddRange(minSpanningTree);
            combinedEdgeList.AddRange(perfectMaching);

            Graph g = new Graph();
            foreach (var item in fullOriginalGraph.Vertices)
            {
                g.addVertex(item);
            }
            foreach (var item in fullOriginalGraph.Edges)
            {
                g.addEdge(item);
            }
            //nem értem teljesen még a dolgokat....


            List<Edge> circuitEdges = new List<Edge>();                 //Az euler út éleinek sorrendje
            List<Vertex> circuitVertices = new List<Vertex>();          //Az Euler kör csúcsainak a sorrendje

            List<Vertex> currentPathVertices = new List<Vertex>();      //Egy pálya a csúcsokknak, addig megy egy úton amíg talál szabad élet
            List<Edge> currentPathEdges = new List<Edge>();             //Egy pálya az éleknek, addig megy egy úton amíg talál szabad élet


            //egy randol él hozzáadása (a csúcsokat is, mindkettővel együtt fogunk számolni)
            foreach (Edge item in g.Edges)
            {
                if (item.EndVertex.Id == startPointId)
                {
                    item.Used = true;
                    currentPathVertices.Add(item.EndVertex);      //a random él vége
                    currentPathVertices.Add(item.StartVertex);    //egy random él eleje

                    break;
                }
                else if (item.StartVertex.Id == startPointId)
                {
                    item.Used = true;
                    currentPathVertices.Add(item.StartVertex);    //egy random él eleje
                    currentPathVertices.Add(item.EndVertex);      //a random él vége
                    break;
                }
            }


            int usedEdgeCount = 1;  //mert egy már van
            while (usedEdgeCount != combinedEdgeList.Count)
            {
                if (currentPathVertices[0].Equals(currentPathVertices.Last<Vertex>()))  //ha az első és az utolsó csúcs megegyezik a pályában (itt BIZTOS VAN használatlan él)
                {
                    while (true)
                    {
                        bool haveUnusedEdge = false;
                        foreach (Edge e in g.getEdgesByVertex(currentPathVertices.Last<Vertex>().Id))
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
                            circuitVertices.Add(currentPathVertices.Last<Vertex>());    //a körhöz adjuk a pálya utolsóját
                            currentPathVertices.Remove(currentPathVertices.Last<Vertex>());     //elveszük a pályából az utolsót

                        }
                    }
                }
                else        //ha a pálya első és utolsó csúcsa nem egyezik meg
                {
                    foreach (Edge e in combinedEdgeList.Where(x => x.StartVertex.Id == currentPathVertices.Last<Vertex>().Id || x.EndVertex.Id == currentPathVertices.Last<Vertex>().Id))/*g.getEdgesByVertex(currentPathVertices.Last<Vertex>().Id*/
                    {
                        if (e.StartVertex.Equals(currentPathVertices.Last<Vertex>()) && e.Used == false)    //ha egy használatlan élet találtunk
                        {
                            currentPathVertices.Add(e.EndVertex);
                            e.Used = true;
                            usedEdgeCount++;
                            break;                          //kilépünk a foreachból, csak egy új élet kértünk
                        }
                        else if (e.EndVertex.Equals(currentPathVertices.Last<Vertex>()) && e.Used == false)
                        {
                            currentPathVertices.Add(e.StartVertex);
                            e.Used = true;
                            usedEdgeCount++;
                            break;                           //kilépünk a foreachból, csak egy új élet kértünk
                        }
                    }
                }
            }

            while (currentPathVertices.Count != 0)   //a végén a pályát fordított sorrendben hozzáadjuk a körhöz
            {
                circuitVertices.Add(currentPathVertices.Last<Vertex>());        //a körhöz adjuk a pálya utolsóját
                currentPathVertices.Remove(currentPathVertices.Last<Vertex>());         //elveszük a pályából az utolsót
            }



            List<int> foundedIds = new List<int>();
            List<Vertex> hamiltonVertices = new List<Vertex>();
            foreach (var item in circuitVertices)
            {
                if (!foundedIds.Contains(item.Id))
                {
                    foundedIds.Add(item.Id);
                    hamiltonVertices.Add(item);
                }
            }
            //hamiltonVertices.Add(circuitVertices[0]);
            return hamiltonVertices;
        }
    }
}
