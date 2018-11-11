using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravellingSalesmen.Algorithms.AntColonyOptimization
{
    class AntManager
    {
        private int SPREAD_COUNT = 100;
        private int PHEROMONE_UPDATE_TURNCOUNT = 10;
        private double TRANSITION_INFLUENCE = 0.4;
        private double PHEROMONE_INFLUENCE = 0.6;
        private double MINIMUM_PHEROMONE = 0.001;
        private double EVAPORATION = 0.04;

        private List<Ant> ants;
        private double[,] pheromoneMatrix;
        double[,] temporalPheromoneMatrix;
        private CompleteGraph graph;
        private List<Vertex> unusedNodes;
        private double bestSolution;
        private List<Ant> bestAntsolution;

        public List<Ant> Ants { get => ants;}

        public AntManager(int startPosition, int count, CompleteGraph graph)
        {
            ants = new List<Ant>();
            bestAntsolution = new List<Ant>();
            for (int i = 0; i < count; i++)
            {
                Ant ant = new Ant(i, startPosition);
                ants.Add(ant);
            }
            this.graph = graph;
            pheromoneMatrix = new double[graph.AdjacencyMatrix.GetLength(0),graph.AdjacencyMatrix.GetLength(1)];
            for (int i = 0; i < pheromoneMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < pheromoneMatrix.GetLength(1); j++)
                {
                    pheromoneMatrix[i, j] = MINIMUM_PHEROMONE;
                }
            }
            temporalPheromoneMatrix = new double[graph.AdjacencyMatrix.GetLength(0), graph.AdjacencyMatrix.GetLength(1)];
            unusedNodes = new List<Vertex>();
            FillUnusedNodes();

            bestSolution = double.PositiveInfinity;
        }

        public void SetParameters(string testParameters)
        {
            string[] paramsArray = testParameters.Split(',');

            SPREAD_COUNT = int.Parse(paramsArray[1]);
            PHEROMONE_UPDATE_TURNCOUNT = int.Parse(paramsArray[2]);
            TRANSITION_INFLUENCE = double.Parse(paramsArray[3]);
            PHEROMONE_INFLUENCE = double.Parse(paramsArray[4]);
            MINIMUM_PHEROMONE = double.Parse(paramsArray[5]);
            EVAPORATION = double.Parse(paramsArray[6]);
        }

        private void FillUnusedNodes()
        {
            unusedNodes.Clear();
            foreach (var vertex in graph.Vertices)
            {
                if (vertex.Id != ants[0].StartPosition)
                {
                    unusedNodes.Add(vertex);
                }
            }
        }

        private void ResetAnts()
        {
            foreach (var ant in ants)
            {
                ant.Reset();
            }
        }

        private void UpdatePheromones()
        {

            for (int i = 0; i < pheromoneMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < pheromoneMatrix.GetLength(1); j++)
                {
                    pheromoneMatrix[i, j] = (1 - EVAPORATION) * pheromoneMatrix[i, j] + temporalPheromoneMatrix[i,j];
                    if(pheromoneMatrix[i, j] < MINIMUM_PHEROMONE)
                    {
                        pheromoneMatrix[i, j] = MINIMUM_PHEROMONE;
                    }
                }
            }

            Array.Clear(temporalPheromoneMatrix, 0, temporalPheromoneMatrix.Length);
        }

        private void UpdateTemporalPheromoneMatrix()
        {
            Ant bestAnt = ants.First(ant => ant.TotalDistanceTravelled == ants.Min(ant2 => ant2.TotalDistanceTravelled));
            for (int i = 0; i < bestAnt.VisitedNodes.Count - 1; i++)
            {
                temporalPheromoneMatrix[int.Parse(bestAnt.VisitedNodes[i].ToString()), int.Parse(bestAnt.VisitedNodes[i + 1].ToString())] += (1 / bestAnt.TotalDistanceTravelled);
                temporalPheromoneMatrix[int.Parse(bestAnt.VisitedNodes[i + 1].ToString()), int.Parse(bestAnt.VisitedNodes[i].ToString())] += (1 / bestAnt.TotalDistanceTravelled);
            }
            /*foreach (var ant in ants)
            {
                for (int i = 0; i < ant.VisitedNodes.Length - 1; i++)
                {
                    temporalPheromoneMatrix[int.Parse(ant.VisitedNodes[i].ToString()), int.Parse(ant.VisitedNodes[i + 1].ToString())] += (1 / ant.TotalDistanceTravelled);
                    temporalPheromoneMatrix[int.Parse(ant.VisitedNodes[i + 1].ToString()), int.Parse(ant.VisitedNodes[i].ToString())] += (1 / ant.TotalDistanceTravelled);
                }
            }*/
        }

        public List<List<int>> SpreadAnts()
        {
            for (int i = 1; i <= SPREAD_COUNT; i++)
            {
                ResetAnts();
                FillUnusedNodes();
                foreach (var ant in ants)
                {
                    AntMakeDecision(ant);
                }
                while(IsEnabledAntInCollection())
                {
                    Ant selectedAnt = SelectAntToMove();
                    MoveAntAndUpdateOthers(selectedAnt);
                    AntMakeDecision(selectedAnt);
                }
                UpdateTemporalPheromoneMatrix();
                if(SPREAD_COUNT % PHEROMONE_UPDATE_TURNCOUNT == 0)
                {
                    if(bestSolution > GetActualSolutionResult())
                    {
                        bestAntsolution = CloneAnts();
                        bestSolution = GetActualSolutionResult();   
                    }
                    UpdatePheromones();
                }  
            }
            List<List<int>> result = new List<List<int>>();
            foreach (var ant in bestAntsolution)
            {
                result.Add(ant.VisitedNodes);
            }
            return result;
        }

        private void MoveAntAndUpdateOthers(Ant selectedAnt)
        {
            foreach (var ant in ants)
            {
                if(ant != selectedAnt && !ant.Stopped)
                {
                    ant.TotalDistanceTravelled += selectedAnt.DistanceToNextNode;
                    ant.DistanceToNextNode -= selectedAnt.DistanceToNextNode;
                }
            }
            selectedAnt.ActualPosition = selectedAnt.NextNode;
            selectedAnt.TotalDistanceTravelled += selectedAnt.DistanceToNextNode;
            selectedAnt.DistanceToNextNode = 0;
            selectedAnt.VisitedNodes.Add(selectedAnt.ActualPosition);
        }

        private void AntMakeDecision(Ant ant)
        {
            if(unusedNodes.Count != 0)
            {
                int nodeId = SelectNextNode(ant);
                ant.NextNode = nodeId;
                ant.DistanceToNextNode = graph.AdjacencyMatrix[ant.ActualPosition, nodeId];
                unusedNodes.Remove(unusedNodes.Single(node => node.Id == nodeId));
            }
            else
            {
                ant.Stopped = true;
                ant.DistanceToNextNode = double.PositiveInfinity;
            }
        }

        private int SelectNextNode(Ant ant)
        {
            int nodeId = -1;
            Dictionary<int, double> nodeAndPossibility = new Dictionary<int, double>();

            double total = 0;
            foreach (var node in unusedNodes)
            {
                double distance = graph.AdjacencyMatrix[ant.ActualPosition, node.Id];
                double pheromone = pheromoneMatrix[ant.ActualPosition, node.Id];
                double actual = Math.Pow(pheromone, PHEROMONE_INFLUENCE) * Math.Pow(1 / distance, TRANSITION_INFLUENCE);
                total += actual;
            }

            foreach (var node in unusedNodes)
            {
                double distance = graph.AdjacencyMatrix[ant.ActualPosition, node.Id];
                double pheromone = pheromoneMatrix[ant.ActualPosition, node.Id];
                double actual = Math.Pow(pheromone, PHEROMONE_INFLUENCE) * Math.Pow(1 / distance, TRANSITION_INFLUENCE);
                double possibility = actual / total;
                nodeAndPossibility.Add(node.Id, possibility);
            }

            double rnd = Coordinator.rnd.NextDouble();
            double ceiling = 0;
            foreach (var entry in nodeAndPossibility)
            {
                ceiling += entry.Value;
                if (rnd <= ceiling)
                {
                    nodeId = entry.Key;
                    break;
                }
            }
            return nodeId;
        }

        private Ant SelectAntToMove()
        {
            double minDistance = ants[0].DistanceToNextNode;
            Ant selected = ants[0];
            foreach (var ant in ants)
            {
                if(ant.DistanceToNextNode < minDistance)
                {
                    minDistance = ant.DistanceToNextNode;
                    selected = ant;
                }
            }
            return selected;
        }

        private bool IsEnabledAntInCollection()
        {
            foreach (var ant in ants)
            {
                if(!ant.Stopped)
                {
                    return true;
                }
            }
            return false;
        }

        private double GetActualSolutionResult()
        {
           return ants.Max(ant => ant.TotalDistanceTravelled);
        }

        private List<Ant> CloneAnts()
        {
            List<Ant> clonedAnts = new List<Ant>();
            foreach (var ant in ants)
            {
                Ant newAnt = new Ant(ant.Id,ant.StartPosition);
                newAnt.NextNode = ant.NextNode;
                newAnt.Stopped = ant.Stopped;
                newAnt.TotalDistanceTravelled = ant.TotalDistanceTravelled;
                newAnt.VisitedNodes = new List<int>();
                foreach (var item in ant.VisitedNodes)
                {
                    newAnt.VisitedNodes.Add(item);
                }
                newAnt.ActualPosition = ant.ActualPosition;
                newAnt.DistanceToNextNode = ant.DistanceToNextNode;
                clonedAnts.Add(newAnt);
            }
            return clonedAnts;
        }
    }
}
