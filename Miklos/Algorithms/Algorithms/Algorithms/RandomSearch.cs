using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravellingSalesmen
{
    class RandomSearch : Algorithm
    {
        Random rnd;
        public RandomSearch(Graph graph, AgentManager agentManager) : base(graph, agentManager)
        {
            rnd = new Random();
        }

        public override void NextTurn()
        {
            for (int i = 0; i < agentManager.Agents.Count; i++)
            {
                int nextVertex = rnd.Next(0, graph.Vertices.Count);
                while (graph.Vertices.Exists(vertex => !vertex.Used))
                {
                    if (graph.Vertices[nextVertex].Used)
                    {
                        nextVertex = rnd.Next(0, graph.Vertices.Count);
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
