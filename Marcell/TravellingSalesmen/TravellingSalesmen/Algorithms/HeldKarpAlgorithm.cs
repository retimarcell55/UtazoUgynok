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

            for (int i = 0; i < nodesExceptDepot.Length; i++)
            {
                GenerateMinimumPathLengthBasedOnSetSize(nodeSubsets, i);
            }

            //iterate through all subsets. Each subset represents how many agents strated to move and what was their end node
            List<string> routes = calculateTheBestRoutes(nodeSubsets, agentManager.Agents.Count);

            List<string> orderedRoutes = orderAgentRoutes(routes);

            //Add depot to orderedRoutes
            for (int i = 0; i < orderedRoutes.Count; i++)
            {
                orderedRoutes[i] += depot;
            }

            double maxRoute = -1;
            foreach (var path in orderedRoutes)
            {
                double distance = 0;
                for (int i = path.Length - 1; i > 0; i--)
                {
                    distance += graph.AdjacencyMatrix[int.Parse(path[i].ToString()), int.Parse(path[i - 1].ToString())];
                }

                if (distance > maxRoute)
                {
                    maxRoute = distance;
                }  
            }
            result = maxRoute;

            moreAgentCirclesToHighlight.Clear();

            foreach (var path in orderedRoutes)
            {
                List<Edge> edgeList = new List<Edge>();
                for (int i = path.Length - 1; i > 0; i--)
                {
                    Edge e = graph.Edges.Single(edge => (edge.StartVertex.Id.ToString() == path[i].ToString() && edge.EndVertex.Id.ToString() == path[i - 1].ToString()) || (edge.StartVertex.Id.ToString() == path[i - 1].ToString() && edge.EndVertex.Id.ToString() == path[i].ToString()));
                    edgeList.Add(e);
                }
                moreAgentCirclesToHighlight.Add(edgeList);
            }
        }

        private List<string> orderAgentRoutes(List<string> routes)
        {
            List<string> orderedRoutes = new List<string>();
            foreach (var route in routes)
            {
                string orderedRoute = backTrackRoute(route);
                orderedRoutes.Add(orderedRoute);
            }
            return orderedRoutes;
        }

        private string backTrackRoute(string route)
        {
            if (route.Length == 1)
            {
                return route[0].ToString();
            }

            char endNode = route[0];
            string set = String.Concat(route.Remove(0, 1).OrderBy(c => c));

            char successorEndNode = set[0];
            double min = double.PositiveInfinity;
            for (int i = 0; i < set.Length; i++)
            {
                double actualDistance = graph.AdjacencyMatrix[int.Parse(endNode.ToString()), int.Parse(set[i].ToString())];
                actualDistance += minimumDistanceToNode[int.Parse(set[i].ToString())][set.Replace(set[i].ToString(), "")];
                if (actualDistance <= min)
                {
                    successorEndNode = set[i];
                    min = actualDistance;
                }
            }
            route = route.Replace(endNode.ToString(), "").Replace(successorEndNode.ToString(), "").Insert(0, successorEndNode.ToString());
            return endNode + backTrackRoute(route);
        }

        private List<string> calculateTheBestRoutes(List<string> nodeSubsets, int agentCount)
        {
            List<string> routes = new List<string>();
            //The possible agent ending points are represented as subsets
            foreach (var subSet in nodeSubsets)
            {
                if (subSet.Length <= agentCount && subSet != "")
                {
                    FindBestAgentRoutesByEndingPointsByRecursion(routes, subSet, 0, "",-1, "");
                }
            }

            double bestResult = double.PositiveInfinity;
            List<string> bestRoutes = new List<string>();

            foreach (var route in routes)
            {
                List<string> actual = route.Split(',').ToList();
                double actualDistance = double.Parse(actual[actual.Count - 1]);
                if (actualDistance < bestResult)
                {
                    bestResult = actualDistance;
                    actual.RemoveAt(actual.Count - 1);
                    bestRoutes.Clear();
                    foreach (var item in actual)
                    {
                        bestRoutes.Add(item);
                    }
                    
                }
            }


            return bestRoutes;
        }

        private void FindBestAgentRoutesByEndingPointsByRecursion(List<string> routes, string subSet, int index, string visited, double actualDistance, string actualRoute)
        {
            if (index >= subSet.Length)
            {
                //If all nodes were visited
                if (visited.Length == nodesExceptDepot.Length)
                {
                    actualRoute += actualDistance;
                    routes.Add(actualRoute);
                    return;
                }
                //If not all nodes were visited
                else
                {
                    return;
                }
            }


            foreach (var item in minimumDistanceToNode[int.Parse(subSet[index].ToString())])
            {
                //Calculate remaining nodes, if there is none skip it!
                string remainingNodes = nodesExceptDepot;
                foreach (var node in visited)
                {
                    remainingNodes = remainingNodes.Replace(node.ToString(), "");
                }
                if (!remainingNodes.Contains(subSet[index].ToString()))
                {
                    continue;
                }
                remainingNodes = remainingNodes.Replace(subSet[index].ToString(), "");
                string visitedBySubset = visited + subSet[index].ToString();
                bool noElementFound = false;
                foreach (var actual in item.Key)
                {
                    if (!remainingNodes.Contains(actual.ToString()))
                    {
                        noElementFound = true;
                    }
                    remainingNodes = remainingNodes.Replace(actual.ToString(), "");
                    visitedBySubset += actual.ToString();
                }
                if (noElementFound)
                {
                    continue;
                }

                double distance = item.Value;
                if(distance < actualDistance)
                {
                    distance = actualDistance;
                }
                string route = subSet[index] + item.Key + ",";
                route = actualRoute + route;
                FindBestAgentRoutesByEndingPointsByRecursion(routes, subSet, index + 1, visitedBySubset, distance, route);

            }
            return;
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
                                double previousDistance = minimumDistanceToNode[int.Parse(endOfSubset.ToString())][remainingSubset];
                                double actualDistance = previousDistance + graph.AdjacencyMatrix[int.Parse(endOfSubset.ToString()), item.Key];
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
