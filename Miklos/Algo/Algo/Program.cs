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

        public List<Edge> CalculateEulerianCycle(Graph originalGraph)
        {
            List<Edge> eulerianCycle = new List<Edge>();
            List<Edge> originalEdges = originalGraph.Edges;
            foreach (Edge e in originalEdges)
            {
                e.Used = false;
            }

            Vertex currentVertex = originalGraph.Vertices[0];   //a random vertex
            for (int i = 0; i < originalEdges.Count; i++)
            {
                foreach (Edge e in originalEdges)
                {
                    if( e.StartVertex.Equals(currentVertex)  && e.Used == false )
                    {
                        e.Used = true;
                        eulerianCycle.Add(e);
                        e.EndVertex = currentVertex;    //it is the cuttent place where we are
                        break;                          //break from foreach, we want add just one new edge
                    }
                    else if (e.EndVertex.Equals(currentVertex) && e.Used == false)
                    {
                        e.Used = true;
                        eulerianCycle.Add(e);
                        e.StartVertex = currentVertex;    //it is the cuttent place where we are
                        break;                  //break from foreach, we want add just one new edge
                    }
                }
            }

            return eulerianCycle;   //ennek SZÁMÍT a sorrendje, mert azt bejárva megkapjukaz euler kört
        }

    }
}
