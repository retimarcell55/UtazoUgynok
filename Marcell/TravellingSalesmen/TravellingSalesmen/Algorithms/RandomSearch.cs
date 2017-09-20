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
                int nextVertex = rnd.Next(0, vertices.Count);
                while (vertices.Exists(vertex => !vertex.Used))
                {
                    if (vertices[nextVertex].Used)
                    {
                        nextVertex = rnd.Next(0, vertices.Count);
                    }
                    else
                    {
                        vertices[nextVertex].Used = true;
                        for (int j = 0; j < edges.Count; j++)
                        {
                            if (edges[j].Start == agentManager.Agents[i].ActualPosition
                                && edges[j].End == vertices[nextVertex].Id)
                            {
                                edges[j].Used = true;
                            }
                        }
                        agentManager.Agents[i].ActualPosition = nextVertex;
                        break;
                    }
                }
                
            }
   
        }
    }
}
