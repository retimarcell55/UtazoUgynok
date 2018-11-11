using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravellingSalesmen.AlgorithmParameters;

namespace TravellingSalesmen.Algorithms
{
    class GreedySearch : Algorithm
    {

        private int PATIENCE_PARAMETER;
        private int NUMBER_OF_RUNS;
        private int MAX_ROUTE_LENGTH_PER_AGENT;

        private List<List<int>> OverallBest;
        List<List<int>> CurrentBest;
        private int startVertex;
        private int runCounter;

        public GreedySearch(CompleteGraph graph, AgentManager agentManager) : base(graph, agentManager)
        {
            OverallBest = new List<List<int>>();
            CurrentBest = new List<List<int>>();
            startVertex = agentManager.Agents[0].StartPosition;
            actualDrawingMode = DRAWING_MODE.MORE_AGENT_CIRCLES;
            moreAgentCirclesToHighlight = new List<List<Edge>>();
            runCounter = 0;

            initParamsWindow(new GreedySearchParams(this));
        }

        public override double getActualResult()
        {
            return generateFitnessForSolution(OverallBest);
        }

        public override bool hasAlgorithmNextMove()
        {
            if (runCounter == NUMBER_OF_RUNS)
            {
                return false;
            }
            return true;
        }


        public override void Initialize()
        {
            PATIENCE_PARAMETER = 50;
            NUMBER_OF_RUNS = 100;
            MAX_ROUTE_LENGTH_PER_AGENT = graph.Vertices.Count;

            OverallBest = GenerateRandomSolution();
            CurrentBest = new List<List<int>>(OverallBest);
            SelectEdgesToHighLight();
        }

        public override void TestInitialize()
        {
            string[] paramsArray = testParameters.Split(',');
            
            PATIENCE_PARAMETER = int.Parse(paramsArray[0]);
            NUMBER_OF_RUNS = int.Parse(paramsArray[1]);
            MAX_ROUTE_LENGTH_PER_AGENT = int.Parse(paramsArray[2]);

            OverallBest = GenerateRandomSolution();
            CurrentBest = new List<List<int>>(OverallBest);
            SelectEdgesToHighLight();
        }


        private List<List<int>> GenerateRandomSolution()
        {
            List<List<int>> solution = new List<List<int>>();

            List<int> citiesInRandomOrder = new List<int>();
            List<int> cities = new List<int>();
            for (int i = 0; i < graph.Vertices.Count; i++)
            {
                cities.Add(i);
            }

            cities.Remove(startVertex);

            while (citiesInRandomOrder.Count != graph.Vertices.Count - 1)
            {
                int random = Coordinator.rnd.Next(0, graph.Vertices.Count);
                if (cities.Contains(random))
                {
                    citiesInRandomOrder.Add(random);
                    cities.Remove(random);
                }
            }

            List<int> agentRouteLengths = new List<int>();
            while (agentRouteLengths.Count != agentManager.Agents.Count)
            {
                int overall = 0;
                for (int i = 0; i < agentManager.Agents.Count; i++)
                {
                    int random = Coordinator.rnd.Next(0, MAX_ROUTE_LENGTH_PER_AGENT + 1);
                    overall += random;
                    if (overall > citiesInRandomOrder.Count)
                    {
                        break;
                    }
                    agentRouteLengths.Add(random);
                }
                if (overall != citiesInRandomOrder.Count)
                {
                    agentRouteLengths.Clear();
                }
            }

            int actualindex = 0;
            foreach (var item in agentRouteLengths)
            {
                List<int> solutionRoute = new List<int>();
                for (int i = actualindex; i < actualindex + item; i++)
                {
                    solutionRoute.Add(citiesInRandomOrder[i]);
                }
                actualindex += item;
                solution.Add(solutionRoute);
            }

            return solution;
        }

        public double generateFitnessForSolution(List<List<int>> solution)
        {
            double ret = 0;

            foreach (var route in solution)
            {
                double actual = 0;
                for (int i = 0; i < route.Count - 1; i++)
                {
                    if (i == 0)
                    {
                        actual += graph.AdjacencyMatrix[startVertex, route[i]];
                    }
                    actual += graph.AdjacencyMatrix[route[i], route[i + 1]];
                }

                if (actual > ret)
                {
                    ret = actual;
                }
            }

            return ret;
        }

        private void SelectEdgesToHighLight()
        {
            moreAgentCirclesToHighlight = new List<List<Edge>>();

            foreach (var item in OverallBest)
            {
                List<Edge> edges = new List<Edge>();
                if(item.Count > 0)
                {
                    edges.Add(graph.Edges.First(edge => (edge.StartVertex.Id == startVertex && edge.EndVertex.Id == item[0]) || (edge.EndVertex.Id == startVertex && edge.StartVertex.Id == item[0])));
                }
                for (int i = 0; i < item.Count - 1; i++)
                {
                    edges.Add(graph.Edges.First(edge => (edge.StartVertex.Id == item[i] && edge.EndVertex.Id == item[i + 1]) || (edge.StartVertex.Id == item[i + 1] && edge.EndVertex.Id == item[i])));
                }
                moreAgentCirclesToHighlight.Add(edges);
            }
        }

        public List<List<int>> GetOverallBestSolution()
        {
            return OverallBest;
        }

        public override void NextTurn()
        {
            int patienceCounter = 0;
            while (true)
            {
                List<List<int>> neighbor = new List<List<int>>(GenerateRandomNeighbor(Clone(CurrentBest)));
                if(generateFitnessForSolution(neighbor) < generateFitnessForSolution(CurrentBest))
                {
                    CurrentBest = neighbor;
                }
                else if(generateFitnessForSolution(neighbor) < generateFitnessForSolution(OverallBest))
                {
                    OverallBest = neighbor;
                }

                if(CurrentBest != neighbor)
                {
                    patienceCounter++;
                }
                if(patienceCounter == PATIENCE_PARAMETER)
                {
                    break;
                }
            }

            SelectEdgesToHighLight();
            CurrentBest = GenerateRandomSolution();
            runCounter++;
        }

        private List<List<int>> GenerateRandomNeighbor(List<List<int>> solution)
        {
            List<List<int>> neighbor = new List<List<int>>();

            int random = Coordinator.rnd.Next(0, 5);

            switch (random)
            {
                case 0:
                    neighbor = IntrarouteInversion(solution);
                    break;
                case 1:
                    neighbor = IntrarouteSwitching(solution);
                    break;
                case 2:
                    neighbor = IntrarouteInsertion(solution);
                    break;
                case 3:
                    neighbor = InterrouteSwitching(solution);
                    break;
                case 4:
                    neighbor = InterrouteTransfer(solution);
                    break;
            }

            return neighbor;
        }

        //Pick a section of a tour and invert its order
        private List<List<int>> IntrarouteInversion(List<List<int>> neighbor)
        {
            int tour = Coordinator.rnd.Next(0, agentManager.Agents.Count);
            int sectionStart;
            int sectionEnd;
            if (neighbor[tour].Count < 2)
            {
                return neighbor;
            }
            while (true)
            {
                sectionStart = Coordinator.rnd.Next(0, neighbor[tour].Count);
                sectionEnd = Coordinator.rnd.Next(0, neighbor[tour].Count);
                if (sectionStart < sectionEnd)
                {
                    break;
                }
            }

            List<int> savedTour = new List<int>(neighbor[tour]);
            List<int> reversed = savedTour.GetRange(sectionStart, sectionEnd - sectionStart + 1);
            reversed.Reverse();

            neighbor[tour].Clear();
            for (int i = 0; i < savedTour.Count; i++)
            {
                if (i < sectionStart || i > sectionEnd)
                {
                    neighbor[tour].Add(savedTour[i]);
                }
                else
                {
                    neighbor[tour].AddRange(reversed);
                    i += reversed.Count - 1;
                }
            }
            return neighbor;
        }

        //Pick two sections within a tour and exchange their locations.
        private List<List<int>> IntrarouteSwitching(List<List<int>> neighbor)
        {
            int tour = Coordinator.rnd.Next(0, agentManager.Agents.Count);
            int section1Start;
            int section1End;
            int section2Start;
            int section2End;
            if (neighbor[tour].Count < 2)
            {
                return neighbor;
            }
            while (true)
            {
                section1Start = Coordinator.rnd.Next(0, neighbor[tour].Count);
                section1End = Coordinator.rnd.Next(0, neighbor[tour].Count);
                section2Start = Coordinator.rnd.Next(0, neighbor[tour].Count);
                section2End = Coordinator.rnd.Next(0, neighbor[tour].Count);
                if (section1Start <= section1End && section2Start <= section2End && section1End < section2Start)
                {
                    break;
                }
            }

            List<int> section1 = neighbor[tour].GetRange(section1Start, section1End - section1Start + 1);
            List<int> section2 = neighbor[tour].GetRange(section2Start, section2End - section2Start + 1);
            List<int> savedTour = new List<int>(neighbor[tour]);

            neighbor[tour].Clear();
            for (int i = 0; i < savedTour.Count; i++)
            {
                if (i < section1Start)
                {
                    neighbor[tour].Add(savedTour[i]);
                }
                else if (i >= section1Start && i <= section1End)
                {
                    neighbor[tour].AddRange(section2);
                    i += section1.Count - 1;
                }
                else if (i > section1End && i < section2Start)
                {
                    neighbor[tour].Add(savedTour[i]);
                }
                else if (i >= section2Start && i <= section2End)
                {
                    neighbor[tour].AddRange(section1);
                    i += section2.Count - 1;
                }
                else if (i > section2End)
                {
                    neighbor[tour].Add(savedTour[i]);
                }
            }
            return neighbor;
        }

        //Pick a section within a tour and put it in another place within that tour
        private List<List<int>> IntrarouteInsertion(List<List<int>> neighbor)
        {
            int tour = Coordinator.rnd.Next(0, agentManager.Agents.Count);
            int section1Start;
            int section1End;
            int section2Start;
            if (neighbor[tour].Count < 2)
            {
                return neighbor;
            }
            while (true)
            {
                section1Start = Coordinator.rnd.Next(0, neighbor[tour].Count);
                section1End = Coordinator.rnd.Next(0, neighbor[tour].Count);
                section2Start = Coordinator.rnd.Next(0, neighbor[tour].Count);
                if (section1Start <= section1End && section1End < section2Start)
                {
                    break;
                }
            }
            List<int> section1 = neighbor[tour].GetRange(section1Start, section1End - section1Start + 1);
            List<int> savedTour = new List<int>(neighbor[tour]);

            neighbor[tour].Clear();
            for (int i = 0; i < savedTour.Count; i++)
            {
                if (i != section2Start && (i < section1Start || i > section1End))
                {
                    neighbor[tour].Add(savedTour[i]);
                }
                else if (i == section2Start)
                {
                    neighbor[tour].Add(savedTour[i]);
                    neighbor[tour].AddRange(section1);
                }

            }
            return neighbor;
        }

        //Pick a section within one tour and a section within another and exchange their locations.These sections are selected to ensure that no route ends up with less than 1 or more than(p − 1) nodes on it
        private List<List<int>> InterrouteSwitching(List<List<int>> neighbor)
        {
            if(neighbor.Count <= 1)
            {
                return neighbor;
            }
            int tour = Coordinator.rnd.Next(0, agentManager.Agents.Count);
            int tour2;
            do
            {
                tour2 = Coordinator.rnd.Next(0, agentManager.Agents.Count);
            } while (tour2 == tour);
            if (neighbor[tour].Count < 1 || neighbor[tour2].Count < 1 || neighbor[tour].Count == MAX_ROUTE_LENGTH_PER_AGENT || neighbor[tour2].Count == MAX_ROUTE_LENGTH_PER_AGENT)
            {
                return neighbor;
            }

            int section1Start;
            int section1End;
            int section2Start;
            int section2End;

            while (true)
            {
                section1Start = Coordinator.rnd.Next(0, neighbor[tour].Count);
                section1End = Coordinator.rnd.Next(0, neighbor[tour].Count);
                section2Start = Coordinator.rnd.Next(0, neighbor[tour2].Count);
                section2End = Coordinator.rnd.Next(0, neighbor[tour2].Count);
                if (section1Start <= section1End
                    && section2Start <= section2End
                    && neighbor[tour].Count - (section1Start - section1End + 1) + (section2Start - section2End + 1) <= MAX_ROUTE_LENGTH_PER_AGENT
                    && neighbor[tour2].Count - (section2Start - section2End + 1) + (section1Start - section1End + 1) <= MAX_ROUTE_LENGTH_PER_AGENT)
                {
                    break;
                }
            }
            List<int> section1 = neighbor[tour].GetRange(section1Start, section1End - section1Start + 1);
            List<int> section2 = neighbor[tour2].GetRange(section2Start, section2End - section2Start + 1);

            neighbor[tour].RemoveRange(section1Start, section1End - section1Start + 1);
            neighbor[tour].InsertRange(section1Start, section2);
            neighbor[tour2].RemoveRange(section2Start, section2End - section2Start + 1);
            neighbor[tour2].InsertRange(section2Start, section1);

            return neighbor;
        }

        //Pick a section within one tour and put it at the end of another tour. This section and the receiving route are selected to ensure that no route ends up with less than 1 or more than(p − 1) nodes on it.
        private List<List<int>> InterrouteTransfer(List<List<int>> neighbor)
        {
            if (neighbor.Count <= 1)
            {
                return neighbor;
            }
            int tour = Coordinator.rnd.Next(0, agentManager.Agents.Count);
            int tour2;
            do
            {
                tour2 = Coordinator.rnd.Next(0, agentManager.Agents.Count);
            } while (tour2 == tour);

            if (neighbor[tour].Count < 1 || neighbor[tour2].Count == MAX_ROUTE_LENGTH_PER_AGENT)
            {
                return neighbor;
            }
            int section1Start;
            int section1End;
            while (true)
            {
                section1Start = Coordinator.rnd.Next(0, neighbor[tour].Count);
                section1End = Coordinator.rnd.Next(0, neighbor[tour].Count);
                if (section1Start <= section1End
                    && neighbor[tour2].Count + (section1Start - section1End + 1) <= MAX_ROUTE_LENGTH_PER_AGENT)
                {
                    break;
                }
            }
            List<int> section1 = neighbor[tour].GetRange(section1Start, section1End - section1Start + 1);
            neighbor[tour].RemoveRange(section1Start, section1End - section1Start + 1);
            neighbor[tour2].AddRange(section1);

            return neighbor;
        }

        private List<List<int>> Clone(List<List<int>> oldList)
        {
            List<List<int>> newList = new List<List<int>>();
            foreach (var item in oldList)
            {
                newList.Add(new List<int>(item));
            }
            return newList;
        }
    }
}
