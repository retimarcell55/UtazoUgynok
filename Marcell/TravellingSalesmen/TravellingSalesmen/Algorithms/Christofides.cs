using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravellingSalesmen.Algorithms
{
    class Christofides : Algorithm
    {
        public enum STAGES { MIN_SPANNING_TREE, INDEPENDENT_EDGE_SET };
        private STAGES actualStage;
        private Graph minimumSpanningTree = null;

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
                    Dictionary<int, int> vertexAndOccurence = new Dictionary<int, int>();
                    foreach (var item in minimumSpanningTree.Edges)
                    {
                        if(vertexAndOccurence.ContainsKey(item.StartVertex.Id))
                        {
                            vertexAndOccurence[item.StartVertex.Id]++;
                        }
                        else
                        {
                            vertexAndOccurence.Add(item.StartVertex.Id, 1);
                        }

                        if (vertexAndOccurence.ContainsKey(item.EndVertex.Id))
                        {
                            vertexAndOccurence[item.StartVertex.Id]++;
                        }
                        else
                        {
                            vertexAndOccurence.Add(item.EndVertex.Id, 1);
                        }
                    }

                    List<Vertex> oddVertices = new List<Vertex>();

                    foreach (var item in vertexAndOccurence)
                    {
                        if(item.Value % 2 != 0)
                        {
                            oddVertices.Add(graph.Vertices.Single(x => x.Id == item.Key));
                        }
                    }
                    CompleteGraph fromOddVertices = new CompleteGraph(oddVertices);
                    CalculateIndependentEdges(fromOddVertices);//TODO

                    break;
                default:
                    break;
            }
        }

        public override double getActualResult()
        {
            double result = 0;
            if (actualStage == STAGES.MIN_SPANNING_TREE)
            {

                foreach (var edge in minimumSpanningTree.Edges)
                {
                    result += edge.Weight;

                }
            }

            return Math.Round(result, 2);
        }

        public Graph CreateMinimumSpanningTree(Graph g)
        {
            #region inicializálások
            //inizializálások:
            Graph originalGraph = g;                            //az eredeti gráf másolata
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

        private List<Edge> CalculateIndependentEdges(Graph g)
        {
            throw new NotImplementedException();
        }
    }
}
