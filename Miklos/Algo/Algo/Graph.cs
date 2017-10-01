using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algo
{
    [Serializable]
    public class Graph
    {
        protected int vertexCount;
        protected int edgeCount;
        protected double[,] adjacencyMatrix;
        protected List<Vertex> vertices;
        protected List<Edge> edges;

        public virtual int VertexCount { get => vertexCount;}
        public virtual int EdgeCount { get => edgeCount;}
        public virtual double[,] AdjacencyMatrix { get => adjacencyMatrix;}
        public virtual List<Vertex> Vertices
        {
            get => vertices;
            set
            {
                vertices = value;
                vertexCount = vertices.Count;
                BuildAdjacencyMatrix();
            }
        }
        public virtual List<Edge> Edges
        {
            get => edges;
            set
            {
                edges = value;
                edgeCount = edges.Count;
                BuildAdjacencyMatrix();
            }
        }

        public Graph()
        {
            this.Vertices = new List<Vertex>();
            adjacencyMatrix = new double[vertices.Count, vertices.Count];
            vertexCount = vertices.Count;
            edgeCount = vertices.Count * vertices.Count;
            edges = new List<Edge>();
        }

        private void BuildAdjacencyMatrix()
        {
            adjacencyMatrix = new double[vertices.Count, vertices.Count];
            for (int i = 0; i < vertices.Count; i++)
            {
                for (int j = 0; j < vertices.Count; j++)
                {
                    double distance = 0;
                    if (i != j && Edges.Exists(edge => (edge.StartVertex.Id == i && edge.EndVertex.Id == j) || edge.StartVertex.Id == j && edge.EndVertex.Id == i))
                    {
                        distance = Math.Sqrt(Math.Pow(vertices[j].Position.X - vertices[i].Position.X, 2) + Math.Pow(vertices[j].Position.Y - vertices[i].Position.Y, 2));
                        distance = Math.Round(distance, 2);
                    }
                    AdjacencyMatrix[i, j] = distance;
                }
            }
        }
    }
}
