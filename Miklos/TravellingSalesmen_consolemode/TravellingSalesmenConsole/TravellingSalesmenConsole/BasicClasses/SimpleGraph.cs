using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravellingSalesmenConsole
{
    [Serializable]
    public class SimpleGraph : AbstractGraph
    {
        public SimpleGraph() :base()
        {

        }

        public SimpleGraph(List<Vertex> vertices, List<Edge> edges) : base()
        {
            foreach (var item in vertices)
            {
                addVertex(item);
            }

            foreach (var item in edges)
            {
                addEdge(item);
            }
        }

        public override void addEdge(Edge e)
        {
            if (!edges.Any(item => e.StartVertex == item.StartVertex && e.EndVertex == item.EndVertex))
            {
                if(vertices.Any(item => item.Id == e.StartVertex.Id) && vertices.Any(item => item.Id == e.EndVertex.Id))
                {
                    edges.Add(new Edge(e.StartVertex, e.EndVertex, false, e.Weight));
                }   
            }
            updateMapping();
            BuildAdjacencyMatrix();
        }

        public override void removeEdge(int startId, int endId)
        {
            if (edges.Any(item => startId == item.StartVertex.Id && endId == item.EndVertex.Id))
            {
                edges.Remove((Edge)edges.Where(item => startId == item.StartVertex.Id && endId == item.EndVertex.Id));
            }
            updateMapping();
            BuildAdjacencyMatrix();
        }
    }
}
