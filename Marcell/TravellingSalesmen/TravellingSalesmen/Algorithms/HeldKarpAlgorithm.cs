using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravellingSalesmen.Algorithms
{
    class HeldKarpAlgorithm : Algorithm
    {
        private int depot;
        private Dictionary<int, Dictionary<string, double>> minimumDistanceToNode;
        private int result;

        public HeldKarpAlgorithm(CompleteGraph graph, AgentManager agentManager) : base(graph, agentManager)
        {
            depot = agentManager.Agents[0].StartPosition;
            minimumDistanceToNode = new Dictionary<int, Dictionary<string, double>>();
            result = -1;
            moreAgentCirclesToHighlight = new List<List<Edge>>();
            actualDrawingMode = DRAWING_MODE.MORE_AGENT_CIRCLES;
        }

        public override void Initialize()
        {
            this.NextTurn();
        }

        public override double getActualResult()
        {
            return result;
        }

        public override bool hasAlgorithmNextMove()
        {
            return false;
        }

        public override void NextTurn()
        {
            string nodesExceptDepot = "";

            //add finish nodes
            for (int i = 0 ; i < graph.AdjacencyMatrix.GetLength(0); i++)
            {
                if(i != depot)
                {
                    minimumDistanceToNode.Add(i, new Dictionary<string, double>());
                    nodesExceptDepot += i.ToString();
                }  
            }

            List<string> nodeSubsets = new List<string>();

            //add empty set
            nodeSubsets.Add("");

            FindAllSubsets(nodeSubsets, nodesExceptDepot, 0);

            for (int i = 0; i < nodesExceptDepot.Length - 1; i++)
            {
                GenerateMinimumPathLengthBasedOnSetSize(nodeSubsets, i);
            }


        }

        private void GenerateMinimumPathLengthBasedOnSetSize(List<string> nodeSubsets, int setSize)
        {
            if (setSize == 0)
            {
                foreach (var item in minimumDistanceToNode)
                {
                    double minimumDistance = graph.AdjacencyMatrix[depot, item.Key];
                    item.Value.Add("", minimumDistance);
                }
            }
            else
            {
                List<string> nodeSubsetsBasedOnSize = GetNodeSubsetsBasedOnSize(nodeSubsets, setSize);

                foreach (var item in minimumDistanceToNode)
                {
                    foreach (var subSet in nodeSubsetsBasedOnSize)
                    {
                        if(!subSet.Contains(item.Key.ToString()))
                        {
                            double minDistance = double.PositiveInfinity;
                            for (int i = 0; i < subSet.Length; i++)
                            {
                                char endOfSubset = subSet[i];
                                string remainingSubset = subSet.Remove(i, 1);
                                double previousDistance = minimumDistanceToNode[endOfSubset][remainingSubset];
                                double actualDistance = previousDistance + graph.AdjacencyMatrix[endOfSubset, item.Key];
                                if(minDistance > actualDistance)
                                {
                                    minDistance = actualDistance;
                                }
                            }
                            item.Value.Add(subSet, minDistance);
                        }

                    }
                }
            }
        }

        private List<string> GetNodeSubsetsBasedOnSize(List<string> nodeSubsets, int setSize)
        {
            List<string> sets = new List<string>();

            foreach (var item in nodeSubsets)
            {
                if(item.Length == setSize)
                {
                    sets.Add(item);
                }
            }
            return sets;
        }

        private void FindAllSubsets(List<string> nodeSubsets, string nodesExceptDepot, int currIndex)
        {
            if(currIndex == nodesExceptDepot.Length)
            {
                return;
            }

            int allSubSetsSize = nodeSubsets.Count;
            string newSubset = "";

            for (int i = 0; i < allSubSetsSize; i++)
            {
                newSubset = nodeSubsets[i];
                newSubset += nodesExceptDepot[currIndex];
                nodeSubsets.Add(newSubset);
            }

            FindAllSubsets(nodeSubsets, nodesExceptDepot, currIndex + 1);
        }
    }
}
