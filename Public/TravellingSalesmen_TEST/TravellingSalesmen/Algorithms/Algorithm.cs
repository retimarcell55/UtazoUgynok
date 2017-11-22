using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravellingSalesmen.Algorithms
{
    public abstract class Algorithm
    {
        protected string selectedConf;
        public string SelectedConf { get => selectedConf; set => selectedConf = value; }


        public enum DRAWING_MODE { GRAPH, MIN_SPANNING_TREE, INDEPENDENT_EDGE_SET, MORE_AGENT_CIRCLES };
        public enum DRAWING_COLOR { RED,GREEN,BLUE };
        protected CompleteGraph graph;
        protected AgentManager agentManager;
        protected DRAWING_MODE actualDrawingMode;

        protected List<Edge> edgesToHighlight;
        protected List<Vertex> verticesToHighlight;
        protected List<List<Edge>> moreAgentCirclesToHighlight;

        public CompleteGraph Graph { get => graph; set => graph = value; }
        public AgentManager AgentManager { get => agentManager; set => agentManager = value; }
        public DRAWING_MODE ActualDrawingMode { get => actualDrawingMode;}
        public List<Edge> EdgesToHighlight { get => edgesToHighlight;}
        public List<Vertex> VerticesToHighlight { get => verticesToHighlight; }
        public List<List<Edge>> MoreAgentCirclesToHighlight { get => moreAgentCirclesToHighlight;}

        public Algorithm(CompleteGraph graph,AgentManager agentManager)
        {
            this.graph = graph;
            this.agentManager = agentManager;
            actualDrawingMode = DRAWING_MODE.GRAPH;

            edgesToHighlight = new List<Edge>();
            verticesToHighlight = new List<Vertex>();
            moreAgentCirclesToHighlight = new List<List<Edge>>();
        }

        public string GetName()
        {
            return this.GetType().Name;
        }

        public virtual void Initialize()
        {
            foreach (var vertex in graph.Vertices)
            {
                vertex.Used = false;
            }
            foreach (var edge in graph.Edges)
            {
                edge.Used = false;
            }
            for (int i = 0; i < agentManager.Agents.Count; i++)
            {
                agentManager.Agents[i].ActualPosition = agentManager.Agents[i].StartPosition;
                graph.Vertices[agentManager.Agents[i].StartPosition].Used = true;
            }
        }

        abstract public void NextTurn();

        public virtual bool hasAlgorithmNextMove()
        {
            if(graph.Vertices.Exists(vertex => !vertex.Used))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public virtual double getActualResult()
        {
            double result = 0;

            foreach (var edge in graph.Edges)
            {
                if(edge.Used)
                {
                    result += edge.Weight;
                }
            }
            return Math.Round(result,2);
        }

        public virtual String[] getInfos()
        {
            return null;
        }

    }
}
