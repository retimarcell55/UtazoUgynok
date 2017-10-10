using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravellingSalesmen
{
    [Serializable]
    public class Configuration
    {
        public CompleteGraph Graph;
        public AgentManager AgentManager;
        public string Name;

        public Configuration(string name, CompleteGraph graph, AgentManager agentManager)
        {
            Name = name;
            Graph = graph;
            AgentManager = agentManager;
        }
    }
}
