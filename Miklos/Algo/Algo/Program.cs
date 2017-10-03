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



        public Graph CereateCompleteGraphFromOddDegreeVertices(Graph g)
        {
            Graph originalGraph = g;
            List<Edge> newGraphEdges = new List<Edge>();
            List<Vertex> newGraphVertices = new List<Vertex>();
            foreach (Vertex v in originalGraph.Vertices)
            {
                int degree = 0;
                foreach (Edge e in originalGraph.Edges) //megszámolja a fokszámokat
                {
                    if (e.StartVertex.Equals(v))
                        degree++;
                    if (e.EndVertex.Equals(v))
                        degree++;
                }

                if(degree % 2 != 0)         //páratlan fokszámú
                {
                    newGraphVertices.Add(v);
                    v.Used = true;          //Ha beletettük akkor használt
                }
                else
                {
                    v.Used = false;         //h nem tettük bele akkor legyen nem használt
                }
            }
            foreach (Edge e in originalGraph.Edges) //ha az él két oldalán használt csúcs van akkor kell nekünk
            {
                if(e.StartVertex.Used && e.EndVertex.Used)
                {
                    newGraphEdges.Add(e);
                }
            }
            Graph newGraph = new Graph();
            newGraph.Vertices = newGraphVertices;
            newGraph.Edges = newGraphEdges;
            return newGraph;
        }

        //http://www.geeksforgeeks.org/hierholzers-algorithm-directed-graph/
        public List<Vertex> CalculateEulerianCycle(Graph g)
        {
            Graph originalGraph = g;
            foreach (Edge e in originalGraph.Edges)
            {
                e.Used = false;
            }

            //List<Edge> eulerianCircuit = new List<Edge>();  //At euler út éleinek sorrendje
            List<Vertex> currentPath = new List<Vertex>();  //Egy pálya, addig megy egy úton amíg talál egy szabad élet
            List<Vertex> circuit = new List<Vertex>();      //Az Euler kör csúcsainak a sorrendje
            List<Edge> unusedEdge = originalGraph.Edges;    //A használatlan csúcsok

            currentPath.Add(originalGraph.Edges[0].StartVertex);    //random edge's start- and endvertex
            currentPath.Add(originalGraph.Edges[0].EndVertex);      //random edge's start- and endvertex
            originalGraph.Edges[0].Used = true;                     //már használtuk
            unusedEdge.Remove(originalGraph.Edges[0]);              //kivesszük a használatlanok kzül

            while (unusedEdge.Count != 0)
            {
                if (currentPath[0].Equals(currentPath.Last<Vertex>()) )  //ha az első és az utolsó csúcs megegyezik, itt BIZTOS VAN használatlan csúcs
                {
                    while(true)
                    {
                        bool haveUnusedEdge = false;
                        foreach (Edge e in unusedEdge)
                        {
                            if (e.StartVertex.Equals(currentPath.Last<Vertex>()) || e.EndVertex.Equals(currentPath.Last<Vertex>()))
                            {
                                haveUnusedEdge = true;
                                break;                  //kilép a foreachból
                            }
                        }
                        if(haveUnusedEdge)
                        {
                            break;      //kilép a while-ból, a currentPath utolsó tagjának van használatlan éle
                        }
                        else
                        {
                            circuit.Add(currentPath.Last<Vertex>());        //a körhöz adjuk
                            currentPath.RemoveAt(currentPath.Count - 1);    //elveszük a pályából
                        }
                    }
                }
                else        //ha a pálya első és utolsó csúcsa nem egyezik meg
                {
                    foreach (Edge e in unusedEdge)
                    {
                        if (e.StartVertex.Equals(currentPath.Last<Vertex>()) && e.Used == false)
                        {
                            e.Used = true;
                            currentPath.Add(e.EndVertex);
                            unusedEdge.Remove(e);           //kiszedjük az élet a használatanok közül
                            break;                          //kilépünk a foreachból, csak egy új élet kértünk
                        }
                        else if (e.EndVertex.Equals(currentPath.Last<Vertex>()) && e.Used == false)
                        {
                            e.Used = true;
                            currentPath.Add(e.StartVertex);
                            unusedEdge.Remove(e);           //kiszedjük az élet a használatanok közül
                            break;                          //kilépünk a foreachból, csak egy új élet kértünk
                        }
                    }
                }
            }

            return circuit;   //ennek SZÁMÍT a sorrendje, mert azt bejárva megkapjukaz euler kört
        }


        
    }
}
