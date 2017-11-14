using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravellingSalesmen.Algorithms
{
    class GeneticAlgorithm : Algorithm
    {
        private const int GENERATIONS = 5;
        private const double MUTATION_PROBABILITY = 0.5;
        private const int POPULATION_SIZE = 3;

        private int numberOfCities;
        private int numberOfSalesmen;

        private Random rnd = new Random();

        private int actualGeneration;
        private List<List<int>> population = new List<List<int>>();
        private List<double> fitness = new List<double>();

        public GeneticAlgorithm(CompleteGraph graph, AgentManager agentManager) : base(graph, agentManager)
        {
            numberOfCities = graph.Vertices.Count;
            numberOfSalesmen = agentManager.Agents.Count;
            actualGeneration = 1;
        }

        public override void Initialize()
        {
            base.Initialize();
            GenerateInitialPopulation();
        }

        public override double getActualResult()
        {
            return base.getActualResult();
        }

        public override void NextTurn()
        {
            population = MakeNewPopulation();
            actualGeneration++;
        }

        public override bool hasNonVisitedVertexLeft()
        {
            if (actualGeneration > GENERATIONS)
            {
                return true;
            }
            return false;
        }

        private void GenerateInitialPopulation()
        {
            for (int i = 0; i < POPULATION_SIZE; i++)
            {
                List<int> newPopulation = GenerateIndividual();
                population.Add(newPopulation);
                fitness.Add(GenerateFitnessForPopulation(newPopulation));
            }
        }

        private List<int> GenerateIndividual()
        {
            List<int> individual = new List<int>();

            //addCitiesinRandomOrder
            for (int i = 0; i < numberOfCities; i++)
            {
                while (true)
                {
                    int randomCity = rnd.Next(0, 10);
                    if (!individual.Contains(randomCity))
                    {
                        individual.Add(randomCity);
                    }
                }
            }

            //Add salesmen in random cuts
            //LESS SALESMEN THAN CITY!!

            int total = numberOfCities;
            List<int> salesmen = new List<int>();
            while (true)
            {
                salesmen = new List<int>();
                int sumOfPathLengths = 0;
                for (int i = 0; i < numberOfSalesmen; i++)
                {
                    int randomPathLength = rnd.Next(1, numberOfCities + 1);
                    salesmen.Add(randomPathLength);
                    sumOfPathLengths += randomPathLength;
                    if (sumOfPathLengths > total)
                    {
                        break;
                    }
                }
                if (total == sumOfPathLengths)
                {
                    break;
                }
            }

            individual.AddRange(salesmen);
            return individual;
        }

        private double GenerateFitnessForPopulation(List<int> pop)
        {
            double fitness = 0;
            int actualPosition = 0;
            for (int i = (population.Count - numberOfSalesmen); i < population.Count; i++)
            {
                int actualRouteLength = pop[i];
                for (int j = actualPosition; j < (actualRouteLength + actualPosition - 1); j++)
                {
                    fitness += graph.AdjacencyMatrix[j, j + 1];
                }
                actualPosition += actualRouteLength;
            }

            return fitness;
        }

        private void MakeNewPopulation()
        {
            List<List<int>> children = new List<List<int>>();

            //Selection + Crossover
            //RandomSelection
            List<int> numbers0toPopSize = new List<int>();
            for (int i = 0; i < population.Count; i++)
            {
                numbers0toPopSize.Add(i);
            }

            //Select parents randomly
            while (numbers0toPopSize.Count >= 1)
            {
                int parent1 = rnd.Next(0, numbers0toPopSize.Count);
                int parent2 = rnd.Next(0, numbers0toPopSize.Count);
                if (parent1 != parent2 && numbers0toPopSize.Contains(parent1) && numbers0toPopSize.Contains(parent2))
                {
                    numbers0toPopSize.Remove(parent1);
                    numbers0toPopSize.Remove(parent2);
                    List<int> child = Crossover(population[parent1], population[parent2]);
                }
            }

            //Mutation
        }

        private List<int> Crossover(List<int> mother, List<int> father)
        {
            List<int> child = new List<int>();

            return child;

        }
    }
}
