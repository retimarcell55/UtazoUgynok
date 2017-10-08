using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algo
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.ReadKey();
        }

        //https://hu.wikipedia.org/wiki/Prim-algoritmus
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

            #region utomunkalatok
            //utómunkálatok (a csúcsokban az éllistákat megfelelően beállítani)
            foreach (Vertex v in treeVertices)
            {
                foreach (Edge e in v.Edges)
                {
                    foreach (Vertex nonTreeV in nonTreeVertices)
                    {
                        if (e.StartVertex.Equals(v) && e.EndVertex.Equals(nonTreeV))
                        {
                            v.Edges.Remove(e);  //ha a kezdete fabeli, de a vége nem fa beli, akkor kiszedjük
                        }
                        else if (e.StartVertex.Equals(nonTreeV) && e.EndVertex.Equals(v))
                        {
                            v.Edges.Remove(e);  //ha a vége fabeli de a kezdete nem az, akkor is kiszedjük
                        }
                        //különben semmi, maradhat, mert mindkettő fabeli végződés
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
                intermediateEdges.Clear();  //mert újra kell számolni

                foreach (Vertex treeV in treeVertices)
                {
                    foreach (Edge e in treeV.Edges)
                    {
                        foreach (Vertex v in nonTreeVertices)
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
            minimumSpanningTree.Edges = treeEdges;
            minimumSpanningTree.Vertices = treeVertices;
            minimumSpanningTree.EdgeCount = minimumSpanningTree.Edges.Count;
            minimumSpanningTree.VertexCount = minimumSpanningTree.Vertices.Count;

            minimumSpanningTree.BuildAdjacencyMatrix();     //felépítjük a mátrixát is !!

            return minimumSpanningTree;
        }

        public Graph CereateCompleteGraphFromOddDegreeVertices(Graph g)
        {
            Graph originalGraph = g;
            List<Edge> newGraphEdges = new List<Edge>();
            List<Vertex> newGraphVertices = new List<Vertex>();
            foreach (Vertex v in originalGraph.Vertices)
            {
                if (v.Edges.Count % 2 != 0)  //páratlan fokszámú
                {
                    newGraphVertices.Add(v);//beletesszüka csúcshalmazba, (még teljes éllistával)
                    v.Used = true;          //Ha beletettük akkor használt
                }
                else
                {
                    v.Used = false;         //ha nem tettük bele akkor legyen nem használt
                }
            }
            foreach (Edge e in originalGraph.Edges)
            {
                if (e.StartVertex.Used && e.EndVertex.Used)  //ha az él két oldalán használt csúcs van akkor kell nekünk
                {
                    newGraphEdges.Add(e);
                }
                else    //ha legalább az egyik oldala nem használt akkor nem kell, kivesszük a csúcsinak a listájából
                {
                    e.EndVertex.Edges.Remove(e);
                    e.StartVertex.Edges.Remove(e);
                }
            }
            Graph newGraph = new Graph();
            newGraph.Vertices = newGraphVertices;
            newGraph.Edges = newGraphEdges;
            newGraph.EdgeCount = newGraph.Edges.Count;
            newGraph.VertexCount = newGraph.Vertices.Count;

            newGraph.BuildAdjacencyMatrix();    //felépítjük a mátrixát is !

            return newGraph;
        }

        public Graph CombineTwoGraphs(Graph g1, Graph g2)   //a csúcshalmaznak egyeznie kell !!!
        {
            //először az egyik gráfot beletesszük, aztán a másik gráf éleit újonnan létrehozzuk, hogy más legyen az ID, és úgy adjuk hozzá
            //FONTOS, itt elvileg már nem jó a szomszédossági mátrixxal számolni !!! (max ha megbeszéljük azt a háromszögeléses módszert)
            Graph combinedGraph = new Graph();
            combinedGraph = g1;
            foreach (Edge e in g2.Edges)
            {
                Edge newEdge = new Edge(e.StartVertex, e.EndVertex, e.Used, e.Weight);
                newEdge.StartVertex.Edges.Add(newEdge);
                newEdge.EndVertex.Edges.Add(newEdge);
                combinedGraph.Edges.Add(newEdge);
            }
            combinedGraph.EdgeCount = combinedGraph.Edges.Count;
            combinedGraph.VertexCount = combinedGraph.Vertices.Count;
            //elvileg a mátrixját meg lehet csinálni, dupla élek nélkül... :
            //combinedGraph.BuildAdjacencyMatrix();
            return combinedGraph;
        }

        //http://www.geeksforgeeks.org/hierholzers-algorithm-directed-graph/
        public List<Edge> CalculateEulerianCycle(Graph g)
        {
            Graph originalGraph = g;
            foreach (Edge e in originalGraph.Edges)
            {
                e.Used = false;
            }

            List<Edge> circuitEdges = new List<Edge>();         //Az euler út éleinek sorrendje
            List<Vertex> currentPath = new List<Vertex>();      //Egy pálya, addig megy egy úton amíg talál szabad élet
            List<Vertex> circuitVertices = new List<Vertex>();  //Az Euler kör csúcsainak a sorrendje
            List<Edge> unusedEdges = originalGraph.Edges;       //A használatlan csúcsok (elején az összes)

            currentPath.Add(originalGraph.Edges[0].StartVertex);    //egy random él eleje
            currentPath.Add(originalGraph.Edges[0].EndVertex);      //a random él vége
            unusedEdges.Remove(originalGraph.Edges[0]);             //kivesszük a használatlanok közül

            while (unusedEdges.Count != 0)
            {
                if (currentPath[0].Equals(currentPath.Last<Vertex>()))  //ha az első és az utolsó csúcs megegyezik (itt BIZTOS VAN használatlan csúcs)
                {
                    while (true)
                    {
                        bool haveUnusedEdge = false;
                        foreach (Edge e in unusedEdges)
                        {
                            if (e.StartVertex.Equals(currentPath.Last<Vertex>()) || e.EndVertex.Equals(currentPath.Last<Vertex>())) //ha a pálya végén van
                            {
                                haveUnusedEdge = true;
                                break;                  //kilép a foreachból
                            }
                        }
                        if (haveUnusedEdge)
                        {
                            break;      //kilép a while-ból, a currentPath utolsó tagjának van használatlan éle
                        }
                        else
                        {
                            circuitVertices.Add(currentPath.Last<Vertex>());    //a körhöz adjuk a pálya utolsóját
                            currentPath.Remove(currentPath.Last<Vertex>());     //elveszük a pályából az utolsót
                        }
                    }
                }
                else        //ha a pálya első és utolsó csúcsa nem egyezik meg
                {
                    foreach (Edge e in unusedEdges)
                    {
                        if (e.StartVertex.Equals(currentPath.Last<Vertex>()) && e.Used == false)    //ha egy használatlan élet találtunk
                        {
                            currentPath.Add(e.EndVertex);
                            unusedEdges.Remove(e);          //kiszedjük az élet a használatanok közül
                            break;                          //kilépünk a foreachból, csak egy új élet kértünk
                        }
                        else if (e.EndVertex.Equals(currentPath.Last<Vertex>()) && e.Used == false)
                        {
                            currentPath.Add(e.StartVertex);
                            unusedEdges.Remove(e);           //kiszedjük az élet a használatanok közül
                            break;                           //kilépünk a foreachból, csak egy új élet kértünk
                        }
                    }
                }
            }
            while (currentPath.Count != 0)   //a végén a pályát fordított sorrendben hozzáadjuk a körhöz
            {
                circuitVertices.Add(currentPath.Last<Vertex>());        //a körhöz adjuk a pálya utolsóját
                currentPath.Remove(currentPath.Last<Vertex>());         //elveszük a pályából az utolsót
            }
            //itt már kész a csúcsok sorrendje, kiszámoljuk az éleket is
            foreach (Edge e in originalGraph.Edges)
            {
                e.Used = false;
            }

            for (int i = 0; i < circuitVertices.Count - 1; i++)  // -1 mert élből eggyel kevesebb van mint csúcsból
            {
                foreach (Edge e in circuitVertices[i].Edges)
                {
                    //ha az egyik kezdete vagy a vége i+1 edik, és még nem hasznátuk
                    if ((e.EndVertex.Equals(circuitVertices[i + 1]) && e.Used == false) || (e.StartVertex.Equals(circuitVertices[i + 1]) && e.Used == false))
                    {
                        circuitEdges.Add(e);
                        e.Used = true;          //használjuk, dupla éleknél kell majd
                        break;                  //kilépünk a foreachból, mert csak egy kell nekünk, ha dupla él van akkor az később kell
                    }
                }
            }

            return circuitEdges;   //ennek SZÁMÍT a sorrendje, mert azt bejárva megkapjukaz euler kört
        }




        //a program osztály vége
    }
}
