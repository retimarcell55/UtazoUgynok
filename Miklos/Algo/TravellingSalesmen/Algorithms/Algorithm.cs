﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravellingSalesmen.Algorithms
{
    public abstract class Algorithm
    {
        public enum DRAWING_MODE { GRAPH, MIN_SPANNING_TREE, INDEPENDENT_EDGE_SET };
        public enum DRAWING_COLOR { RED,GREEN,BLUE };
        protected CompleteGraph graph;
        protected AgentManager agentManager;
        protected DRAWING_MODE actualDrawingMode;

        protected List<Edge> edgesToHighlight;
        protected List<Vertex> verticesToHighlight;

        public CompleteGraph Graph { get => graph; set => graph = value; }
        public AgentManager AgentManager { get => agentManager; set => agentManager = value; }
        public DRAWING_MODE ActualDrawingMode { get => actualDrawingMode;}
        public List<Edge> EdgesToHighlight { get => edgesToHighlight;}
        public List<Vertex> VerticesToHighlight { get => verticesToHighlight; }

        public Algorithm(CompleteGraph graph,AgentManager agentManager)
        {
            this.graph = graph;
            this.agentManager = agentManager;
            actualDrawingMode = DRAWING_MODE.GRAPH;

            edgesToHighlight = new List<Edge>();
            verticesToHighlight = new List<Vertex>();
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

        public bool hasNonVisitedVertexLeft()
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

    }
}