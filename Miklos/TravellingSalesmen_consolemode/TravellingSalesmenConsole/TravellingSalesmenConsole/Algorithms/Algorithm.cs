﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravellingSalesmenConsole
{
    public abstract class Algorithm
    {
        protected CompleteGraph graph;
        protected AgentManager agentManager;

        public CompleteGraph Graph { get => graph; set => graph = value; }
        public AgentManager AgentManager { get => agentManager; set => agentManager = value; }

        public Algorithm(CompleteGraph graph,AgentManager agentManager)
        {
            this.graph = graph;
            this.agentManager = agentManager;
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

        public virtual int getPopSize()
        {
            return 0;
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
        
    }
}
