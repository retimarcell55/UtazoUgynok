using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravellingSalesmen.Algorithms
{
    class GeneticAlgorithm : Algorithm
    {
//tesztelésnél átírni ezeket lehet------------------------------
        static int generationsNumber = 10;
        static int populationSize = 3000;//CAN BE DIVIDED BY 4!!
        static double mutationProbability = 0.8;
        static int firstchildMutationTrue = 1;
        static int secondchildMutationTrue = 0;
//idáig módosíthatsz csak !!!-----------------------------------

        //Number of generations
        private int GENERATIONS = generationsNumber;

        //CAN BE DIVIDED BY 4!!
        private int POPULATION_SIZE = populationSize;

        private double MUTATION_PROBABILITY = mutationProbability;

        private int numberOfCities;
        private int numberOfSalesmen;

        private int actualGeneration;
        private List<Chromosome> population = new List<Chromosome>();

        public GeneticAlgorithm(CompleteGraph graph, AgentManager agentManager) : base(graph, agentManager)
        {
            numberOfCities = graph.Vertices.Count;
            numberOfSalesmen = agentManager.Agents.Count;
            actualGeneration = 1;
            actualDrawingMode = DRAWING_MODE.MORE_AGENT_CIRCLES;
        }

        public override void Initialize()
        {
            base.Initialize();
            GenerateInitialPopulation();
            OrderPopulationByFitness();

            SelectBestChromosomeEdges();

        }

        public override double getActualResult()
        {
            return population[0].Fitness;
        }

        public override void NextTurn()
        {
            MakeNewPopulation();
            OrderPopulationByFitness();
            actualGeneration++;

            SelectBestChromosomeEdges();

        }

        public override bool hasAlgorithmNextMove()
        {
            if (actualGeneration > GENERATIONS)
            {
                return false;
            }
            return true;
        }

        private void GenerateInitialPopulation()
        {
            for (int i = 0; i < POPULATION_SIZE; i++)
            {
                Chromosome newChromosome = GenerateChromosome();
                population.Add(newChromosome);
            }
        }

        private Chromosome GenerateChromosome()
        {
            Chromosome chromosome = new Chromosome();

            //addCitiesinRandomOrder

            for (int i = 0; i < numberOfCities; i++)
            {
                while (true)
                {
                    int randomCity = Coordinator.rnd.Next(0, numberOfCities);
                    if (!chromosome.Cities.Contains(randomCity))
                    {
                        chromosome.Cities.Add(randomCity);
                        break;
                    }
                }
            }

            //Add salesmen in random cuts
            //LESS SALESMEN THAN CITY!!

            int total = numberOfCities;
            while (true)
            {
                chromosome.Salesmen = new List<int>();
                int sumOfPathLengths = 0;
                for (int i = 0; i < numberOfSalesmen; i++)
                {
                    int randomPathLength = Coordinator.rnd.Next(1, numberOfCities - numberOfSalesmen + 2);
                    //int randomPathLength = Coordinator.rnd.Next(1, (numberOfCities /*- sumOfPathLengths*/ + 1) / (numberOfSalesmen - 1));
                    chromosome.Salesmen.Add(randomPathLength);
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

            chromosome.Fitness = GenerateFitnessForChromosome(chromosome);

            return chromosome;
        }

        private double GenerateFitnessForChromosome(Chromosome chromosome)
        {
            /*double fitness = 0;
            int actualPosition = 0;
            foreach (int salesman in chromosome.Salesmen)
            {
                int actualRouteLength = salesman;
                for (int j = actualPosition; j < (actualRouteLength + actualPosition - 1); j++)
                {
                    fitness += graph.AdjacencyMatrix[j, j + 1];
                }
                actualPosition += actualRouteLength;
            }*/

            int actualPosition = 0;
            double maxTime = 0;
            foreach (int salesman in chromosome.Salesmen)
            {
                int actualRouteLength = salesman;
                double time = 0;
                for (int j = actualPosition; j < (actualRouteLength + actualPosition) - 1; j++)
                {
                    time += graph.AdjacencyMatrix[chromosome.Cities[j], chromosome.Cities[j + 1]];
                }
                if (time > maxTime)
                {
                    maxTime = time;
                }
                actualPosition += actualRouteLength;
            }

            return maxTime;
        }

        private void OrderPopulationByFitness()
        {
            population = population.OrderBy(item => item.Fitness).ToList();
        }

        private void MakeNewPopulation()
        {
            //Remove worst half of the population
            population.RemoveRange((POPULATION_SIZE / 2), (POPULATION_SIZE / 2));

            //Selection + Crossover
            //RandomSelection
            List<int> numbers0toPopSize = new List<int>();
            for (int i = 0; i < population.Count; i++)
            {
                numbers0toPopSize.Add(i);
            }

            //Select parents randomly, once every chromosome
            while (numbers0toPopSize.Count != 0)
            {
                int parent1 = Coordinator.rnd.Next(0, population.Count);
                int parent2 = Coordinator.rnd.Next(0, population.Count);
                if (parent1 != parent2 && numbers0toPopSize.Contains(parent1) && numbers0toPopSize.Contains(parent2))
                {
                    numbers0toPopSize.Remove(parent1);
                    numbers0toPopSize.Remove(parent2);
                    Chromosome child1 = Crossover(population[parent1], population[parent2]);
                    if (Coordinator.rnd.NextDouble() > MUTATION_PROBABILITY && firstchildMutationTrue == 1)
                    {
                        Mutate(child1);
                    }
                    population.Add(child1);

                    Chromosome child2 = Crossover(population[parent2], population[parent1]);
                    if (Coordinator.rnd.NextDouble() > MUTATION_PROBABILITY && secondchildMutationTrue == 1)
                    {
                        Mutate(child2);
                    }
                    population.Add(child2);
                }
            }
        }

        private Chromosome Crossover(Chromosome mother, Chromosome father)
        {
            Chromosome child = new Chromosome();

            //Order Crossover

            //Random consecutive alleles from mother
            int allelStartIndex = Coordinator.rnd.Next(0, numberOfCities - 1);
            int allelEndIndex = Coordinator.rnd.Next(0, numberOfCities);
            while (allelEndIndex < allelStartIndex)
            {
                allelEndIndex = Coordinator.rnd.Next(0, numberOfCities);
            }

            //Search for the selected mother allel indexes in father
            List<int> FatherAllelIndexesToBeLeftOut = new List<int>();
            for (int i = allelStartIndex; i <= allelEndIndex; i++)
            {
                for (int j = 0; j < father.Cities.Count; j++)
                {
                    if (father.Cities[j] == mother.Cities[i])
                    {
                        FatherAllelIndexesToBeLeftOut.Add(j);
                        break;
                    }
                }
            }

            //Select remaining allel values
            List<int> fatherRemainingAlleles = new List<int>();
            for (int i = 0; i < father.Cities.Count; i++)
            {
                if (!FatherAllelIndexesToBeLeftOut.Contains(i))
                {
                    fatherRemainingAlleles.Add(father.Cities[i]);
                }
            }


            for (int i = 0; i < numberOfCities; i++)
            {
                if (i >= allelStartIndex && i <= allelEndIndex)
                {
                    //If selected mother allels 
                    child.Cities.Add(mother.Cities[i]);
                }
                else
                {
                    //Put father allel in order in to child
                    child.Cities.Add(fatherRemainingAlleles[0]);
                    //Remove the added father allel
                    fatherRemainingAlleles.RemoveAt(0);
                }
            }

            //Child gets salesmans from mother
            child.Salesmen = new List<int>(mother.Salesmen);

            child.Fitness = GenerateFitnessForChromosome(child);

            return child;

        }

        private void Mutate(Chromosome chromosome)
        {
            //select two random alleles
            int index1 = Coordinator.rnd.Next(0, numberOfCities);
            int index2 = Coordinator.rnd.Next(0, numberOfCities);
            while (index1 == index2)
            {
                index2 = Coordinator.rnd.Next(0, numberOfCities);
            }

            //swap two alleles
            int tmp = chromosome.Cities[index1];
            chromosome.Cities[index1] = chromosome.Cities[index2];
            chromosome.Cities[index2] = tmp;

            //Second random mutation probability: 50%
            if (Coordinator.rnd.Next(2) == 1)
            {
                //Reverse two alleles
                int index3 = Coordinator.rnd.Next(0, numberOfCities - 1);
                tmp = chromosome.Cities[index3];
                chromosome.Cities[index3] = chromosome.Cities[index3 + 1];
                chromosome.Cities[index3 + 1] = tmp;
            }

            chromosome.Fitness = GenerateFitnessForChromosome(chromosome);
        }

        private void SelectBestChromosomeEdges()
        {
            moreAgentCirclesToHighlight = new List<List<Edge>>();

            int actualIndex = 0;
            foreach (var item in population[0].Salesmen)
            {
                int actualLength = item;
                List<Edge> edges = new List<Edge>();
                for (int i = actualIndex; i < actualIndex + actualLength - 1; i++)
                {
                    edges.Add(graph.Edges.First(edge => (edge.StartVertex.Id == population[0].Cities[i] && edge.EndVertex.Id == population[0].Cities[i + 1]) || (edge.StartVertex.Id == population[0].Cities[i + 1] && edge.EndVertex.Id == population[0].Cities[i])));
                }
                moreAgentCirclesToHighlight.Add(edges);
                actualIndex += actualLength;
            }
        }

        public override string[] getInfos()
        {
            String[] temp = { generationsNumber.ToString(), populationSize.ToString(), mutationProbability.ToString(), firstchildMutationTrue.ToString(), secondchildMutationTrue.ToString() };
            return temp;
        }
    }

    class Chromosome
    {
        private List<int> cities;
        private List<int> salesmen;
        private double fitness;

        public List<int> Cities { get => cities; set => cities = value; }
        public List<int> Salesmen { get => salesmen; set => salesmen = value; }
        public double Fitness { get => fitness; set => fitness = value; }

        public Chromosome()
        {
            cities = new List<int>();
            salesmen = new List<int>();
            fitness = double.MaxValue;
        }

        public Chromosome(List<int> cities, List<int> salesmen, double fitness)
        {
            this.cities = cities;
            this.salesmen = salesmen;
            this.fitness = fitness;
        }
    }
}
