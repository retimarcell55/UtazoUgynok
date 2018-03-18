using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravellingSalesmen.Algorithms.AntColonyOptimization
{
    class AntManager
    {
        private const int SPREAD_COUNT = 100;
        private const int PHEROMONE_UPDATE_TURNCOUNT = 10;
        private const double TRANSITION_INFLUENCE = 0.4;
        private const double PHEROMONE_INFLUENCE = 0.6;
        private const double MINIMUM_PHEROMONE = 0.001;
        private const double EVAPORATION = 0.96;

        private List<Ant> ants;
        private double[,] pheromoneMatrix;
        double[,] temporalPheromoneMatrix;
        private CompleteGraph graph;
        private List<Vertex> unusedNodes;


        public List<Ant> Ants { get => ants;}

        public AntManager(int startPosition, int count, CompleteGraph graph)
        {
            ants = new List<Ant>();
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
                    pheromoneMatrix[i, j] = EVAPORATION * pheromoneMatrix[i, j] + temporalPheromoneMatrix[i,j];
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
            Ant bestAnt = ants.Single(ant => ant.TotalDistanceTravelled == ants.Min(ant2 => ant2.TotalDistanceTravelled));
            for (int i = 0; i < bestAnt.VisitedNodes.Length - 1; i++)
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

        public List<string> SpreadAnts()
        {
            for (int i = 1; i <= SPREAD_COUNT; i++)
            {
                ResetAnts();
                FillUnusedNodes();
                foreach (var ant in ants)
                {
                    AntMakeDecision(ant);
                }
                while(isEnabledAntInCollection())
                {
                    Ant selectedAnt = SelectAntToMove();
                    MoveAntAndUpdateOthers(selectedAnt);
                    AntMakeDecision(selectedAnt);
                }
                UpdateTemporalPheromoneMatrix();
                if(SPREAD_COUNT % PHEROMONE_UPDATE_TURNCOUNT == 0)
                {
                    UpdatePheromones();
                }  
            }
            List<string> result = new List<string>();
            foreach (var ant in ants)
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
            selectedAnt.VisitedNodes += selectedAnt.ActualPosition;
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

        private bool isEnabledAntInCollection()
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
    }
}
