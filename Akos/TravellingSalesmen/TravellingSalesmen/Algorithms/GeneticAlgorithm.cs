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
        private int GENERATIONS;

        private bool FIRST_CHILD_MUTATE;
        private bool SECEND_CHILD_MUTATE;
        private double MUTATION_PROBABILITY;
        private double WEAK_PARENT_RATE;

        private int startCity;
        private static int numberOfCities;
        private static int numberOfSalesmen;

        private int actualGeneration;
        private Chromosome[] population;

        public GeneticAlgorithm(CompleteGraph graph, AgentManager agentManager,
                int generationsNuber = 200, int populationNumber = -1,
                double mutationProbability = 0.5, double weakParentRate = 0.025,
                bool firstChildMutate = false, bool SecondChildMutate = true)
            : base(graph, agentManager)
        {

            GENERATIONS = generationsNuber;
            FIRST_CHILD_MUTATE = firstChildMutate;
            SECEND_CHILD_MUTATE = SecondChildMutate;
            MUTATION_PROBABILITY = mutationProbability;
            WEAK_PARENT_RATE = weakParentRate;

            if (populationNumber == -1)
            {
                double chromsize = System.Math.Round((Math.Log(graph.Vertices.Count)));
                double length = chromsize * (graph.Vertices.Count +agentManager.Agents.Count);
                population = new Chromosome[(int)(Math.Round((length*Math.Pow(2.0,chromsize))/chromsize))];
            }
            else
                population = new Chromosome[populationNumber];

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
            return population[0].fitness;
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
            for (int i = 0; i < population.Length; i++)
            {
                population[i] = new Chromosome();
                GenerateChromosome(population[i]);
            }
        }

        private void GenerateChromosome(Chromosome chromosome) 
        {
            //addCitiesinRandomOrder
            List<int> unUsed = new List<int>();
            for (int i = 0; i < numberOfCities; i++)
                if (i != startCity)
                    unUsed.Add(i);
                
            for (int i = 0; i < numberOfCities-1; i++)
            {
                int randomCity = Coordinator.rnd.Next(0, numberOfCities-i-1);
                chromosome.cities[i]=unUsed[randomCity];
                unUsed.RemoveAt(randomCity);
            }

            //Add salesmen in random cuts
            //LESS SALESMEN THAN CITY!!

            int total = numberOfCities;

            /*int lastSalesMan = 1;

            chromosome.salesmen = new List<int>();
            for (int i = 0; i < numberOfSalesmen - 1; i++)
            {
                lastSalesMan = Coordinator.rnd.Next(lastSalesMan, (numberOfCities-1-chromosome.salesmen.Sum())/(numberOfSalesmen-chromosome.salesmen.Count));
                chromosome.salesmen.Add(lastSalesMan);
            }*/
            
            for (int i = 0; i < numberOfSalesmen - 1; i++)
            {
                int randomPathLength = Coordinator.rnd.Next(1, numberOfCities - chromosome.SalesSum(i) - (1* (numberOfSalesmen-(i+1))));
                chromosome.salesmen[i]=randomPathLength;
            }
            chromosome.salesmen[numberOfSalesmen-1]=(numberOfCities-chromosome.SalesSum(numberOfSalesmen-1)-1);
            chromosome.fitness = GenerateFitnessForChromosome(chromosome);
        }

        private double GenerateFitnessForChromosome(Chromosome chromosome)
        {

            int actualPosition = 0;
            double maxTime = 0;
            foreach (int salesman in chromosome.salesmen)
            {
                int actualRouteLength = salesman;
                double time = 0;
                time+= graph.AdjacencyMatrix[startCity, chromosome.cities[actualPosition]];
                for (int j = actualPosition; j < (actualRouteLength + actualPosition) - 1; j++)
                {
                    time += graph.AdjacencyMatrix[chromosome.cities[j], chromosome.cities[j + 1]];
                }
                if (time > maxTime)
                    maxTime = time;

                actualPosition += actualRouteLength;
            }

            return maxTime;
        }

        private void OrderPopulationByFitness()
        {
            population = population.OrderBy(item => item.fitness).ToArray();
        }

        private void MakeNewPopulation()
        {
            //Count of the worst ones we kill
            int fistOnesToDieCount = (int)(population.Length / 2 * (1 - WEAK_PARENT_RATE));

            //List of Survivors who we gonna use as Parents
            List<int> listSurPar = new List<int>();
            for (int i = 0; i < (population.Length - fistOnesToDieCount); i++)
                listSurPar.Add(i);
            //List of Dead ones whose memory space we gonna use for the new Children
            List<int> listDeChil = new List<int>();
            for (int i = (population.Length - fistOnesToDieCount); i < population.Length; i++)
                listDeChil.Add(i);

            //The same amount of best is protected as many worst was killed
            int protectedsCount = fistOnesToDieCount;

            int chosensIndex;
            //Kill random unprotected ones to reach the ideal populationSize
            while (listSurPar.Count > population.Length / 2)
            {
                chosensIndex = protectedsCount + Coordinator.rnd.Next(0, listSurPar.Count - protectedsCount - 1);
                listDeChil.Add(listSurPar[chosensIndex]);
                listSurPar.RemoveAt(chosensIndex);
            }

            int tmp;
            //Select parents randomly, once every chromosome
            while (listSurPar.Count != 0)
            {
                int parent1 = Coordinator.rnd.Next(0, listSurPar.Count-1);
                int parent2 = Coordinator.rnd.Next(0, listSurPar.Count-1);
                if (parent1 <= parent2)
                    parent2 = (parent2 + 1) % listSurPar.Count;
                if (parent1 < parent2) {
                    tmp = parent1;
                    parent1 = parent2;
                    parent2 = tmp;
                }

                Crossover(population[listSurPar[parent1]], population[listSurPar[parent2]],population[listDeChil[0]]);
                Crossover(population[listSurPar[parent2]], population[listSurPar[parent1]],population[listDeChil[1]]);
                if (Coordinator.rnd.NextDouble() > MUTATION_PROBABILITY && FIRST_CHILD_MUTATE)
                    Mutate(population[listDeChil[0]]);
                if (Coordinator.rnd.NextDouble() > MUTATION_PROBABILITY && SECEND_CHILD_MUTATE)
                    Mutate(population[listDeChil[1]]);
                listDeChil.RemoveAt(0);
                listDeChil.RemoveAt(0);
                listSurPar.RemoveAt(parent1);
                listSurPar.RemoveAt(parent2);
            }
        }

        private void Crossover(Chromosome mother, Chromosome father,Chromosome child)
        {
            for (int i = 0; i < child.salesmen.Length; i++)
                child.salesmen[i] = mother.salesmen[i];

            //Is the Gene Fixed? We can Know it from here
            bool[] Fixed = new bool[numberOfCities - 1];
            for (int i = 0; i < numberOfCities - 1; i++)
                Fixed[i] = false;

            //Value of chosen Cities in the mainParents order
            List<int> chosenOnes = new List<int>();
            chosenOnes = new List<int>(mother.cities);

            //Value of unChosen Cities in the secendParents order
            List<int> unChosenOnes = new List<int>();
            unChosenOnes = new List<int>(father.cities);

            //select Unchosen and chosen Ones
            int indexOfChosen=0;
            int jump = 1;
            int chosen;
            for (int i = 0; i < numberOfCities / 2; i++)
            {
                if (i % jump == 0)
                {
                    indexOfChosen = Coordinator.rnd.Next(0, unChosenOnes.Count - 1);
                    jump = Coordinator.rnd.Next(1, numberOfCities/7+1);
                }
                else
                    indexOfChosen = indexOfChosen % (unChosenOnes.Count - 1);
                chosen = unChosenOnes[indexOfChosen];
                bool found = false;
                for (int j = 0; j < mother.cities.Length&&!found; j++)
                    if (mother.cities[j] == chosen)
                    {
                        indexOfChosen = j;
                        found = true;
                    }
                Fixed[indexOfChosen] = true;
                unChosenOnes.Remove(chosen);
            }

            //select Chosen Ones
            for (int i = 0; i < unChosenOnes.Count; i++)
                chosenOnes.Remove(unChosenOnes[i]);

            //Build Children's 
            for (int i = 0; i < numberOfCities-1; i++)
                if (Fixed[i])
                {
                    child.cities[i]=chosenOnes[0];
                    chosenOnes.RemoveAt(0);
                }
                else
                {
                    child.cities[i]=unChosenOnes[0];
                    unChosenOnes.RemoveAt(0);
                }

            child.fitness=GenerateFitnessForChromosome(child);
        }

        private void Mutate(Chromosome chromosome)
        {
            //select two random alleles
            int index1 = Coordinator.rnd.Next(0, numberOfCities-2);
            int index2 = Coordinator.rnd.Next(0, numberOfCities-3);
            if (index1 <= index2)
                index2 = (index2 + 1) % (numberOfCities - 1);

            //swap two alleles
            int tmp = chromosome.cities[index1];
            chromosome.cities[index1] = chromosome.cities[index2];
            chromosome.cities[index2] = tmp;

            //Second random mutation probability: 50%
            if (Coordinator.rnd.Next(2) == 1)
            {
                //Reverse two alleles
                int index3 = Coordinator.rnd.Next(0, numberOfCities - 1);
                tmp = chromosome.cities[index3];
                chromosome.cities[index3] = chromosome.cities[(index3 + 1)%(numberOfCities-1)];
                chromosome.cities[(index3 + 1) % (numberOfCities - 1)] = tmp;
            }

            chromosome.fitness = GenerateFitnessForChromosome(chromosome);
        }

        private void SelectBestChromosomeEdges()
        {
            moreAgentCirclesToHighlight = new List<List<Edge>>();

            int actualIndex = 0;
            foreach (var item in population[0].salesmen)
            {
                int actualLength = item;
                List<Edge> edges = new List<Edge>();
                edges.Add(graph.Edges.First(edge => (edge.StartVertex.Id == population[0].cities[actualIndex] && edge.EndVertex.Id == startCity) || (edge.StartVertex.Id == startCity && edge.EndVertex.Id == population[0].cities[actualIndex])));
                for (int i = actualIndex; i < actualIndex + actualLength - 1; i++)
                {
                    edges.Add(graph.Edges.First(edge => (edge.StartVertex.Id == population[0].cities[i] && edge.EndVertex.Id == population[0].cities[i + 1]) || (edge.StartVertex.Id == population[0].cities[i + 1] && edge.EndVertex.Id == population[0].cities[i])));
                }
                moreAgentCirclesToHighlight.Add(edges);
                actualIndex += actualLength;
            }
        }

        private class Chromosome
        {
            public int[] cities;
            public int[] salesmen;
            public double fitness;

            public Chromosome()
            {
                cities = new int[numberOfCities-1];
                salesmen = new int[numberOfSalesmen];
                fitness = double.MaxValue;
            }

            public Chromosome(int[] cities, int[] salesmen, double fitness, bool alive)
            {
                this.cities = cities;
                this.salesmen = salesmen;
                this.fitness = fitness;
            }

            public int SalesSum(int n)
            {
                int result=0;
                if (n < numberOfSalesmen)
                    for (int i = 0; i < n; i++)
                        result += salesmen[i];
                return result;
            }
        }
    }
}
