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
        private double result;
        string nodesExceptDepot;

        public HeldKarpAlgorithm(CompleteGraph graph, AgentManager agentManager) : base(graph, agentManager)
        {
            depot = agentManager.Agents[0].StartPosition;
            minimumDistanceToNode = new Dictionary<int, Dictionary<string, double>>();
            result = -1;
            moreAgentCirclesToHighlight = new List<List<Edge>>();
            actualDrawingMode = DRAWING_MODE.MORE_AGENT_CIRCLES;
            nodesExceptDepot = "";
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


            //add finish nodes
            for (int i = 0; i < graph.AdjacencyMatrix.GetLength(0); i++)
            {
                if (i != depot)
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

            //iterate through all subsets. Each subset represents how many agents strated to move and what was their end node
            List<string> routes = calculateTheBestRoutes(nodeSubsets, agentManager.Agents.Count);

            List<string> orderedRoutes = orderAgentRoutes(routes);


            foreach (var route in orderedRoutes)
            {
                double minRoute = graph.AdjacencyMatrix[0, int.Parse(route[0].ToString())];
                char endNode = route[0];
                for (int i = 1; i < route.Length; i++)
                {
                    double distance = graph.AdjacencyMatrix[int.Parse(route[i].ToString()), int.Parse(route[0].ToString())] + minimumDistanceToNode[int.Parse(route[i].ToString())][route.Remove(0, 1).Remove(i, 1)];
                    if(distance < minRoute)
                    {
                        minRoute = distance;
                    }
                }
                if(result < minRoute)
                {
                    result = minRoute;
                }
            }
        }

        private List<string> orderAgentRoutes(List<string> routes)
        {
            List<string> orderedRoutes = new List<string>();
            foreach (var route in routes)
            {
                string orderedRoute = backTrackRoute(route);

            }
            return orderedRoutes;
        }

        private string backTrackRoute(string route)
        {
            if (route == "")
            {
                return "";
            }

            char endNode = route[0];
            string set = String.Concat(route.Remove(0, 1).OrderBy(c => c));

            char successorEndNode = set[0];
            double min = double.PositiveInfinity;
            for (int i = 0; i < set.Length; i++)
            {
                double actualDistance = graph.AdjacencyMatrix[int.Parse(endNode.ToString()), int.Parse(set[i].ToString())];
                actualDistance += minimumDistanceToNode[endNode][set.Replace(set[i].ToString(), "")];
                if (actualDistance <= min)
                {
                    successorEndNode = set[i];
                    min = actualDistance;
                }
            }
            route = route.Replace(endNode.ToString(), "").Insert(0, successorEndNode.ToString());
            return successorEndNode + backTrackRoute(route);
        }

        private List<string> calculateTheBestRoutes(List<string> nodeSubsets, int agentCount)
        {
            List<string> routes = new List<string>();
            foreach (var subSet in nodeSubsets)
            {
                if (subSet.Length <= agentCount)
                {
                    FindBestAgentRoutesByEndingPointsByRecursion(routes, subSet, 0, "");
                }
            }

            return routes;
        }

        private double FindBestAgentRoutesByEndingPointsByRecursion(List<string> routes, string subSet, int index, string visited)
        {
            if (index >= subSet.Length)
            {
                //If all nodes were visited
                if (visited.Length == nodesExceptDepot.Length)
                {
                    return -1;
                }
                //If not all nodes were visited
                else
                {
                    return -2;
                }
            }

            double minDistance = double.PositiveInfinity;
            string bestSubset = "";
            foreach (var item in minimumDistanceToNode[int.Parse(subSet[index].ToString())])
            {
                //Calculate remaining nodes, if there is none skip it!
                string remainingNodes = nodesExceptDepot.Replace(visited, "");
                if (!remainingNodes.Contains(subSet[index].ToString()))
                {
                    continue;
                }
                remainingNodes = remainingNodes.Replace(subSet[index].ToString(), "");
                visited += subSet[index].ToString();
                bool noElementFound = false;
                foreach (var actual in item.Key)
                {
                    if (!remainingNodes.Contains(actual.ToString()))
                    {
                        noElementFound = true;
                    }
                    remainingNodes = remainingNodes.Replace(actual.ToString(), "");
                    visited += actual.ToString();
                }
                if (noElementFound)
                {
                    continue;
                }

                double distance = item.Value;

                double feedback = FindBestAgentRoutesByEndingPointsByRecursion(routes, subSet, index + 1, visited);
                if (feedback == -2)
                {
                    continue;
                }
                else if (feedback != -1 && feedback > distance)
                {
                    distance = feedback;
                }
                if (distance < minDistance)
                {
                    minDistance = distance;
                    bestSubset = subSet[index] + item.Key;
                }
            }

            if (routes.ElementAtOrDefault(index) == null)
            {
                routes.Add("");
            }

            routes[index] = bestSubset;
            return minDistance;
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
                        if (!subSet.Contains(item.Key.ToString()))
                        {
                            double minDistance = double.PositiveInfinity;
                            for (int i = 0; i < subSet.Length; i++)
                            {
                                char endOfSubset = subSet[i];
                                string remainingSubset = subSet.Remove(i, 1);
                                double previousDistance = minimumDistanceToNode[endOfSubset][remainingSubset];
                                double actualDistance = previousDistance + graph.AdjacencyMatrix[endOfSubset, item.Key];
                                if (minDistance > actualDistance)
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
                if (item.Length == setSize)
                {
                    sets.Add(item);
                }
            }
            return sets;
        }

        private void FindAllSubsets(List<string> nodeSubsets, string nodesExceptDepot, int currIndex)
        {
            if (currIndex == nodesExceptDepot.Length)
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
