using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravellingSalesmen.Algorithms
{
    class GeneticAlgorithm : Algorithm
    {
        //Number of generations
        private const int GENERATIONS = 100;

        //CAN BE DIVIDED BY 4!!
        private int POPULATION_SIZE;

        private const double MUTATION_PROBABILITY = 0.8;

        private int startCity;
        private int numberOfCities;
        private int numberOfSalesmen;

        private int actualGeneration;
        private List<Chromosome> population = new List<Chromosome>();

        public GeneticAlgorithm(CompleteGraph graph, AgentManager agentManager)
            : base(graph, agentManager) {
            POPULATION_SIZE=3000;
            startCity = agentManager.Agents[0].StartPosition;
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

            for (int i = 0; i < numberOfCities-1; i++)
            {
                while (true)
                {
                    int randomCity = Coordinator.rnd.Next(0, numberOfCities);
                    if (!(chromosome.Cities.Contains(randomCity) || randomCity == startCity))
                        chromosome.Cities.Add(randomCity);
                    else if (chromosome.Cities.Count == numberOfCities - 1)
                        break;
                }
            }

            //Add salesmen in random cuts
            //LESS SALESMEN THAN CITY!!

            int total = numberOfCities;

            chromosome.Salesmen = new List<int>();
            for (int i = 0; i < numberOfSalesmen - 1; i++)
            {
                int randomPathLength = Coordinator.rnd.Next(1, numberOfCities - (numberOfSalesmen - chromosome.Salesmen.Count) - chromosome.Salesmen.Sum()+1);
                chromosome.Salesmen.Add(randomPathLength);
            }
            chromosome.Salesmen.Add(numberOfCities-chromosome.Salesmen.Sum()-1);
            chromosome.Fitness = GenerateFitnessForChromosome(chromosome);

            return chromosome;
        }

        private double GenerateFitnessForChromosome(Chromosome chromosome)
        {

            int actualPosition = 0;
            double maxTime = 0;
            foreach (int salesman in chromosome.Salesmen)
            {
                int actualRouteLength = salesman;
                double time = 0;
                time+= graph.AdjacencyMatrix[startCity, chromosome.Cities[actualPosition]];
                for (int j = actualPosition; j < (actualRouteLength + actualPosition) - 1; j++)
                {
                    time += graph.AdjacencyMatrix[chromosome.Cities[j], chromosome.Cities[j + 1]];
                }
                if (time > maxTime)
                    maxTime = time;

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
                    /*if (Coordinator.rnd.NextDouble() > MUTATION_PROBABILITY)
                    {
                        Mutate(child1);
                    }*/
                    population.Add(child1);

                    Chromosome child2 = Crossover(population[parent2], population[parent1]);
                    if (Coordinator.rnd.NextDouble() > MUTATION_PROBABILITY)
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
            int allelStartIndex = Coordinator.rnd.Next(0, numberOfCities - 2);
            int allelEndIndex = Coordinator.rnd.Next(0, numberOfCities-1);
            while (allelEndIndex < allelStartIndex)
            {
                allelEndIndex = Coordinator.rnd.Next(0, numberOfCities-1);
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


            for (int i = 0; i < numberOfCities-1; i++)
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
            int index1 = Coordinator.rnd.Next(0, numberOfCities-1);
            int index2 = Coordinator.rnd.Next(0, numberOfCities-1);
            while (index1 == index2)
            {
                index2 = Coordinator.rnd.Next(0, numberOfCities-1);
            }

            //swap two alleles
            int tmp = chromosome.Cities[index1];
            chromosome.Cities[index1] = chromosome.Cities[index2];
            chromosome.Cities[index2] = tmp;

            //Second random mutation probability: 50%
            if (Coordinator.rnd.Next(2) == 1)
            {
                //Reverse two alleles
                int index3 = Coordinator.rnd.Next(0, numberOfCities - 2);
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
                edges.Add(graph.Edges.First(edge => (edge.StartVertex.Id == population[0].Cities[actualIndex] && edge.EndVertex.Id == startCity) || (edge.StartVertex.Id == startCity && edge.EndVertex.Id == population[0].Cities[actualIndex])));
                for (int i = actualIndex; i < actualIndex + actualLength - 1; i++)
                {
                    edges.Add(graph.Edges.First(edge => (edge.StartVertex.Id == population[0].Cities[i] && edge.EndVertex.Id == population[0].Cities[i + 1]) || (edge.StartVertex.Id == population[0].Cities[i + 1] && edge.EndVertex.Id == population[0].Cities[i])));
                }
                moreAgentCirclesToHighlight.Add(edges);
                actualIndex += actualLength;
            }
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
