using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravellingSalesmen.Algorithms.AntColonyOptimization
{
    class AntManager
    {
        private const int SPRED_COUNT = 10;
        private const double TRANSITION_INFLUENCE = 0.5;
        private const double PHEROMONE_INFLUENCE = 0.5;
        private const double MINIMUM_PHEROMONE = 0.1;

        private List<Ant> ants;
        private double[,] pheromoneMatrix;
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
            FillUnusedNodes();
        }

        private void FillUnusedNodes()
        {
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

        }

        public List<List<Edge>> SpreadAnts()
        {
            for (int i = 0; i < SPRED_COUNT; i++)
            {
                foreach (var ant in ants)
                {
                    AntMakeDecision(ant);
                }
                while(unusedNodes.Count != 0)
                {
                    Ant selectedAnd = SelectAntToMove();

                }
                ResetAnts();
                UpdatePheromones();
                FillUnusedNodes();
            }
        }

        private void AntMakeDecision(Ant ant)
        {
            if(unusedNodes.Count != 0)
            {
                int nodeId = SelectNextNode(ant);
                //updateAnt
            }
        }

        private int SelectNextNode(Ant ant)
        {
            int nodeId = 0;
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
                if(rnd <= ceiling)
                {
                    nodeId = entry.Key;
                }
                ceiling += entry.Value;
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

        private void UpdateAntsByTheSelectedMove(Ant selectedAnt)
        {

        }
    }
}
