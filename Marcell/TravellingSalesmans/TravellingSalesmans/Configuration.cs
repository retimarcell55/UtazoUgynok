using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravellingSalesmans
{
    [Serializable]
    public class Configuration
    {
        public Graph Graph;
        public AgentManager AgentManager;
        public string Name;

        public Configuration(string name, Graph graph, AgentManager agentManager)
        {
            Name = name;
            Graph = graph;
            AgentManager = agentManager;
        }
    }
}
