using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravellingSalesmen
{
    [Serializable]
    public class CompleteGraph : Graph
    {
        public override List<Vertex> Vertices
        {
            get => vertices;
        }
        public override List<Edge> Edges
        {
            get => edges;
        }

        public CompleteGraph(List<Vertex> vertices)
        {
            this.Vertices = vertices;
            adjacencyMatrix = new double[vertices.Count, vertices.Count];
            vertexCount = vertices.Count;
            edgeCount = vertices.Count * vertices.Count;
            Edges = new List<Edge>();
            for (int i = 0; i < vertices.Count; i++)
            {
                for (int j = 0; j < vertices.Count; j++)
                {
                    double distance = 0;
                    if (i != j)
                    {
                        distance = Math.Sqrt(Math.Pow(vertices[j].Position.X - vertices[i].Position.X, 2) + Math.Pow(vertices[j].Position.Y - vertices[i].Position.Y, 2));
                        distance = Math.Round(distance, 2);
                        if (j >= i)
                        {
                            Edges.Add(new Edge(vertices[i], vertices[j], false, distance));
                        }
                    }
                    AdjacencyMatrix[i, j] = distance;
                }
            }
        }
    }
}
