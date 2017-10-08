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

        //Átírom hogy lehessen settere !!!!
        public virtual int VertexCount { set => vertexCount = value; get => vertexCount; }
        public virtual int EdgeCount { set => edgeCount = value; get => edgeCount;}
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
        
        //TODO: public legyen, ez a függvény az éllistából és a csúcslistából képes legyen mártixot építeni (frissíteni) !!
        public void BuildAdjacencyMatrix() 
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

        //TODO: egy public függvény ami mátrixból csinál (frissíti) él - és csúcslistát
        //FONTOS!! ez csak akkor jó ha a mátrix alsó/felső háromszögében -1 van !! különben mindenhol dupla élet csinálna !!!
        public void BuildEdgesAndVertices()
        {
            //a mátrixból felépítjük a listákat és frissítjük
            Edges.Clear();
            foreach (Vertex v in vertices)
            {
                v.Edges.Clear();
            }
            EdgeCount = 0;
            for(int i = 0; i < vertexCount; i++)
            {
                for(int j = i; j < vertexCount; j++)
                {
                    if(adjacencyMatrix[i,j] != 0)
                    {
                        Edge e = new Edge(vertices[i], vertices[j], false, adjacencyMatrix[i, j]);
                        e.StartVertex.Edges.Add(e);
                        e.EndVertex.Edges.Add(e);
                        Edges.Add(e);
                        EdgeCount++;
                    }
                }
            }
        }
        
        



    }
}
