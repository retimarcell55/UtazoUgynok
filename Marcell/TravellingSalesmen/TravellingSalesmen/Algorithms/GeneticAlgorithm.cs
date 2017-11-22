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
        private const int GENERATIONS = 200;

        //CAN BE DIVIDED BY 4!!
        private int POPULATION_SIZE;

        private bool FIRST_CHILD_MUTATE = false;
        private bool SECEND_CHILD_MUTATE = true;
        private double MUTATION_PROBABILITY = 0.5;
        private double WEAK_PARENT_RATE = 0.025;

        private int startCity;
        private int numberOfCities;
        private int numberOfSalesmen;

        private int actualGeneration;
        private List<Chromosome> population = new List<Chromosome>();

        public GeneticAlgorithm(CompleteGraph graph, AgentManager agentManager)
            : base(graph, agentManager)
        {
            POPULATION_SIZE = 4 * (graph.Vertices.Count / 2) * (graph.Vertices.Count / 2) * (graph.Vertices.Count / 2);
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
            population.Clear();
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
            List<int> unUsed = new List<int>();
            for (int i = 0; i < numberOfCities; i++)
                if (i != startCity)
                    unUsed.Add(i);

            for (int i = 0; i < numberOfCities - 1; i++)
            {
                int randomCity = Coordinator.rnd.Next(0, numberOfCities - i - 1);
                chromosome.Cities.Add(unUsed[randomCity]);
                unUsed.RemoveAt(randomCity);
            }

            //Add salesmen in random cuts
            //LESS SALESMEN THAN CITY!!

            int total = numberOfCities;

            chromosome.Salesmen = new List<int>();
            for (int i = 0; i < numberOfSalesmen - 1; i++)
            {
                //int randomPathLength = Coordinator.rnd.Next(numberOfCities/ numberOfSalesmen / numberOfSalesmen, numberOfCities - chromosome.Salesmen.Sum()-(numberOfCities / numberOfSalesmen / numberOfSalesmen* (numberOfSalesmen-chromosome.Salesmen.Count-1)));
                int randomPathLength = Coordinator.rnd.Next(1, numberOfCities - chromosome.Salesmen.Sum() - (1 * (numberOfSalesmen - chromosome.Salesmen.Count - 1)));
                chromosome.Salesmen.Add(randomPathLength);
            }
            chromosome.Salesmen.Add(numberOfCities - chromosome.Salesmen.Sum() - 1);
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
                time += graph.AdjacencyMatrix[startCity, chromosome.Cities[actualPosition]];
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
            int fistOnesToDieCount = (int)(POPULATION_SIZE / 2 * (1 - WEAK_PARENT_RATE));
            //Remove worst half of the population
            population.RemoveRange(POPULATION_SIZE - fistOnesToDieCount, fistOnesToDieCount);
            int protectedsCount = fistOnesToDieCount;
            while (population.Count > POPULATION_SIZE / 2)
                population.RemoveAt(protectedsCount + Coordinator.rnd.Next(0, population.Count - protectedsCount - 1));
            //Selection + Crossover
            //RandomSelection
            List<int> numbers0toPopSize = new List<int>();
            for (int i = 0; i < population.Count; i++)
            {
                numbers0toPopSize.Add(i);
            }

            int tmp;
            //Select parents randomly, once every chromosome
            while (numbers0toPopSize.Count != 0)
            {
                int parent1 = Coordinator.rnd.Next(0, numbers0toPopSize.Count - 1);
                int parent2 = Coordinator.rnd.Next(0, numbers0toPopSize.Count - 2);
                if (parent1 <= parent2)
                    parent2 = (parent2 + 1) % numbers0toPopSize.Count;
                if (parent1 < parent2)
                {
                    tmp = parent1;
                    parent1 = parent2;
                    parent2 = tmp;
                }

                Chromosome[] children = new Chromosome[2];
                children[0] = Crossover(population[numbers0toPopSize[parent1]], population[numbers0toPopSize[parent2]]);
                children[1] = Crossover(population[numbers0toPopSize[parent2]], population[numbers0toPopSize[parent1]]);
                if (Coordinator.rnd.NextDouble() > MUTATION_PROBABILITY && FIRST_CHILD_MUTATE)
                    Mutate(children[0]);
                if (Coordinator.rnd.NextDouble() > MUTATION_PROBABILITY && SECEND_CHILD_MUTATE)
                    Mutate(children[1]);
                population.Add(children[0]);
                population.Add(children[1]);
                numbers0toPopSize.RemoveAt(parent1);
                numbers0toPopSize.RemoveAt(parent2);
            }
        }

        private Chromosome Crossover(Chromosome mother, Chromosome father)
        {
            Chromosome child = new Chromosome();
            child.Salesmen = new List<int>(mother.Salesmen);

            //Is the Gene Fixed? We can Know it from here
            bool[] Fixed = new bool[numberOfCities - 1];
            for (int i = 0; i < numberOfCities - 1; i++)
                Fixed[i] = false;

            //Value of chosen Cities in the mainParents order
            List<int> chosenOnes = new List<int>();
            chosenOnes = new List<int>(mother.Cities);

            //Value of unChosen Cities in the secendParents order
            List<int> unChosenOnes = new List<int>();
            unChosenOnes = new List<int>(father.Cities);

            //select Unchosen and chosen Ones
            int indexOfChosen = 0;
            int jump = 1;
            int chosen;
            for (int i = 0; i < numberOfCities / 2; i++)
            {
                if (i % jump == 0)
                {
                    indexOfChosen = Coordinator.rnd.Next(0, unChosenOnes.Count - 1);
                    jump = Coordinator.rnd.Next(1, numberOfCities / 7 + 1);
                }
                else
                    indexOfChosen = indexOfChosen % (unChosenOnes.Count - 1);
                chosen = unChosenOnes[indexOfChosen];
                Fixed[mother.Cities.IndexOf(chosen)] = true;
                unChosenOnes.Remove(chosen);
            }

            //select Chosen Ones
            for (int i = 0; i < unChosenOnes.Count; i++)
                chosenOnes.Remove(unChosenOnes[i]);

            //Build Children's 
            for (int i = 0; i < numberOfCities - 1; i++)
                if (Fixed[i])
                {
                    child.Cities.Add(chosenOnes[0]);
                    chosenOnes.RemoveAt(0);
                }
                else
                {
                    child.Cities.Add(unChosenOnes[0]);
                    unChosenOnes.RemoveAt(0);
                }

            child.Fitness = GenerateFitnessForChromosome(child);
            return child;

        }

        /*private Chromosome Crossover(Chromosome mother, Chromosome father)
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

        }*/

        private void Mutate(Chromosome chromosome)
        {
            //select two random alleles
            int index1 = Coordinator.rnd.Next(0, numberOfCities - 2);
            int index2 = Coordinator.rnd.Next(0, numberOfCities - 3);
            if (index1 <= index2)
                index2 = (index2 + 1) % (numberOfCities - 1);

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
                chromosome.Cities[index3] = chromosome.Cities[(index3 + 1) % (numberOfCities - 1)];
                chromosome.Cities[(index3 + 1) % (numberOfCities - 1)] = tmp;
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
