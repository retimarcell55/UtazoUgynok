using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravellingSalesmen
{
    [Serializable]
    public class Edge
    {
        private static int IdCounter = 0;
        private int id;
        private Vertex startVertex;
        private Vertex endVertex;
        private bool used;
        private double weight;

        public int Id { get => id; }
        public bool Used { get => used; set => used = value; }
        public double Weight { get => weight; set => weight = value; }
        public Vertex StartVertex { get => startVertex; set => startVertex = value; }
        public Vertex EndVertex { get => endVertex; set => endVertex = value; }


        public Edge(Vertex startVertex, Vertex endVertex, bool used, double weight)
        {
            this.startVertex = startVertex;
            this.endVertex = endVertex;
            this.used = used;
            this.weight = weight;
            id = IdCounter;
            IdCounter++;
        }

    }
}
