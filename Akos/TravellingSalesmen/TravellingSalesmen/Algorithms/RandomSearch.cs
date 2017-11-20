using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravellingSalesmen.Algorithms
{
    class RandomSearch : Algorithm
    {
        public RandomSearch(CompleteGraph graph, AgentManager agentManager) : base(graph, agentManager)
        {
        }

        public override void NextTurn()
        {
            for (int i = 0; i < agentManager.Agents.Count; i++)
            {
                int nextVertex = Coordinator.rnd.Next(0, graph.Vertices.Count);
                while (graph.Vertices.Exists(vertex => !vertex.Used))
                {
                    if (graph.Vertices[nextVertex].Used)
                    {
                        nextVertex = Coordinator.rnd.Next(0, graph.Vertices.Count);
                    }
                    else
                    {
                        graph.Vertices[nextVertex].Used = true;
                        ((Edge)graph.Edges.Single(edge => (edge.StartVertex.Id == agentManager.Agents[i].ActualPosition && edge.EndVertex.Id == nextVertex)
                                                     || (edge.EndVertex.Id == agentManager.Agents[i].ActualPosition && edge.StartVertex.Id == nextVertex))).Used = true;
                        agentManager.Agents[i].ActualPosition = nextVertex;
                        break;
                    }
                }

            }

        }
    }
}
