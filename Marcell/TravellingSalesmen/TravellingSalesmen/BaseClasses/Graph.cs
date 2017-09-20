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
        private int vertexCount;
        private int edgeCount;
        private int[,] adjacencyMatrix;

        public int VertexCount { get => vertexCount; }
        public int EdgeCount { get => edgeCount; }
        public int[,] AdjacencyMatrix { get => adjacencyMatrix; }

        public Graph(int[,] adjacencyMatrix)
        {
            this.adjacencyMatrix = adjacencyMatrix;
            this.vertexCount = adjacencyMatrix.GetLength(0);
            this.edgeCount = vertexCount * vertexCount;
        }
    }
}
