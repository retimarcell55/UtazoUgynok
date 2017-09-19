using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravellingSalesmen
{
    abstract class Algorithm
    {
        protected Graph graph;
        protected AgentManager agentManager;
        protected List<Edge> edges;
        protected List<Vertex> vertices;

        public Graph Graph { get => graph; set => graph = value; }
        public AgentManager AgentManager { get => agentManager; set => agentManager = value; }
        public List<Edge> Edges { get => edges; set => edges = value; }
        public List<Vertex> Vertices { get => vertices; set => vertices = value; }

        public Algorithm(Graph graph,AgentManager agentManager)
        {
            this.graph = graph;
            this.agentManager = agentManager;
        }

        abstract public string GetName();

        public virtual void Initialize()
        {
            edges = new List<Edge>();

            for (int i = 0; i < graph.AdjacencyMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < graph.AdjacencyMatrix.GetLength(1); j++)
                {
                    if(graph.AdjacencyMatrix[i,j] != 0)
                    {
                        edges.Add(new Edge(i, j));
                    }
                }
            }

            vertices = new List<Vertex>();

            for (int i = 0; i < graph.AdjacencyMatrix.GetLength(0); i++)
            {
                vertices.Add(new Vertex(i));
            }

            for (int i = 0; i < agentManager.Agents.Count; i++)
            {
                vertices[agentManager.Agents[i].StartPosition].Used = true;
            }
        }

        abstract public void NextTurn();

        public bool hasNonVisitedVertexLeft()
        {
            if(vertices.Exists(vertex => !vertex.Used))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
