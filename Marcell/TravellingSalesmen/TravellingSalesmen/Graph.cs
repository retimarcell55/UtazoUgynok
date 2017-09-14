using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravellingSalesmen
{
    [Serializable]
    public class Graph
    {
        private int vertices;
        private int edges;
        private int[,] adjacencyMatrix;

        public int Vertices { get => vertices; }
        public int Edges { get => edges; }
        public int[,] AdjacencyMatrix { get => adjacencyMatrix; }

        public Graph(int[,] adjacencyMatrix)
        {
            this.adjacencyMatrix = adjacencyMatrix;
            this.vertices = adjacencyMatrix.GetLength(0);
            this.edges = vertices * vertices;
        }
    }
}
