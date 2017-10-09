using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algo
{
    [Serializable]
    public class AgentManager
    {
        private List<Agent> agents;

        internal List<Agent> Agents { get => agents; set => agents = value; }

        public AgentManager()
        {
            agents = new List<Agent>(0);
        }

        public AgentManager(List<Agent> agents)
        {
            this.agents = agents;
        }
    }
}
