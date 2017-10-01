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



        public Graph CereateCompleteGraphFromOddDegreeVertices(Graph originalGraph)
        {
            List<Vertex> newGraphVertices = new List<Vertex>();
            foreach (Vertex v in originalGraph.Vertices)
            {
                int degree = 0;
                foreach (Edge e in originalGraph.Edges)
                {
                    if (e.StartVertex.Equals(v))
                        degree++;
                    if (e.EndVertex.Equals(v))
                        degree++;
                }

                if(degree % 2 != 0) //degree is odd
                {
                    newGraphVertices.Add(v);
                }
            }
            Graph newGraph = new Graph();
            newGraph.Vertices = newGraphVertices;
            return newGraph;//TODO
        }
    }
}
