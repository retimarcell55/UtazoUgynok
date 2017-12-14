using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravellingSalesmenConsole
{
    class RandomSearch : Algorithm
    {
        public RandomSearch(CompleteGraph graph, AgentManager agentManager) : base(graph, agentManager)
        {

        }

        public override void NextTurn()
        {
            throw new NotImplementedException();
        }
    }
}