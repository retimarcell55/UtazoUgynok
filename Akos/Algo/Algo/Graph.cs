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
        protected double[,] adjacencyMatrix;
        protected List<Vertex> vertices;
        protected List<Edge> edges;
        protected Dictionary<int, List<Edge>> vertexIdAndEdges;

        public double[,] AdjacencyMatrix { get => adjacencyMatrix;}
        public Dictionary<int, List<Edge>> VertexIdAndEdges { get => vertexIdAndEdges; set => vertexIdAndEdges = value; }
        public  List<Vertex> Vertices{ get => vertices; }
        public List<Edge> Edges { get => edges; }

        public Graph()
        {
            this.vertices = new List<Vertex>();
            adjacencyMatrix = new double[vertices.Count, vertices.Count];
            edges = new List<Edge>();
            vertexIdAndEdges = new Dictionary<int, List<Edge>>();
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

        protected List<Edge> getEdgesByVertex(Vertex v)
        {
            List<Edge> list = new List<Edge>();
            foreach (var item in edges)
            {
                if(item.StartVertex.Id == v.Id || item.EndVertex.Id == v.Id)
                {
                    list.Add(item);
                }
            }
            return list;
        }

        public virtual void addVertex(Vertex v)
        {
            if(!vertices.Any(item => item.Id == v.Id))
            {
                vertices.Add(v);
            }
            updateMapping();
            BuildAdjacencyMatrix();
        }

        public virtual void removeVertex(int id)
        {
            if(vertices.Any(item => item.Id == id))
            {
                edges.RemoveAll(item => ((item.StartVertex.Id == id) || (item.EndVertex.Id == id)));
                vertices.Remove((Vertex)vertices.Where(item2 => item2.Id == id));
            }
            updateMapping();
            BuildAdjacencyMatrix();
        }

        public virtual void addEdge(Edge e)
        {
            if (!edges.Any(item => e.StartVertex == item.StartVertex && e.EndVertex == item.EndVertex))
            {
                if(vertices.Any(item => item.Id == e.StartVertex.Id) && vertices.Any(item => item.Id == e.EndVertex.Id))
                {
                    edges.Add(e);
                }   
            }
            updateMapping();
            BuildAdjacencyMatrix();
        }

        public virtual void removeEdge(int startId, int endId)
        {
            if (edges.Any(item => startId == item.StartVertex.Id && endId == item.EndVertex.Id))
            {
                edges.Remove((Edge)edges.Where(item => startId == item.StartVertex.Id && endId == item.EndVertex.Id));
            }
            updateMapping();
            BuildAdjacencyMatrix();
        }

        protected void updateMapping()
        {
            vertexIdAndEdges = new Dictionary<int, List<Edge>>();
            foreach (var item in vertices)
            {
                vertexIdAndEdges.Add(item.Id, edges.Where(item2 => ((item2.StartVertex.Id == item.Id) || (item2.EndVertex.Id == item.Id))).ToList());
            }
        }

        public List<Edge> getEdgesByVertex(int id)
        {
            return vertexIdAndEdges[id];
        }
    }
}
