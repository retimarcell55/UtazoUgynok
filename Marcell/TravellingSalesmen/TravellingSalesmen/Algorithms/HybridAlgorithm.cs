using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravellingSalesmen.AlgorithmParameters;

namespace TravellingSalesmen.Algorithms
{
    class HybridAlgorithm : Algorithm
    {
        GreedySearch greedySearch;
        GeneticAlgorithm geneticAlgorithm;

        public HybridAlgorithm(CompleteGraph graph, AgentManager agentManager) : base(graph, agentManager)
        {
            greedySearch = new GreedySearch(graph, agentManager);
            geneticAlgorithm = new GeneticAlgorithm(graph, agentManager);

            initParamsWindow(new HybridAlgorithmParams(this));
        }

        public override void Initialize()
        {
            //Can be called only in Test Mode!
            throw new NotImplementedException();
        }

        public override void NextTurn()
        {
            geneticAlgorithm.NextTurn();
        }

        public override bool hasAlgorithmNextMove()
        {
            return geneticAlgorithm.hasAlgorithmNextMove();
        }

        public override double getActualResult()
        {
            return geneticAlgorithm.getActualResult();
        }

        public override void TestInitialize()
        {
            greedySearch.TestParameters = this.testParameters;
            geneticAlgorithm.TestParameters = this.testParameters;
            greedySearch.TestInitialize();
            while (greedySearch.hasAlgorithmNextMove())
            {
                greedySearch.NextTurn();
            }
            List<List<int>> solution = greedySearch.GetOverallBestSolution();

            List<int> salesmen = new List<int>(solution.Count);
            List<int> cities = new List<int>(solution.Sum(route => route.Count));

            foreach (var currentSolution in solution)
            {
                salesmen.Add(currentSolution.Count);
            }

            foreach (var route in solution)
            {
                foreach (var allele in route)
                {
                    cities.Add(allele);
                }
            }

            GeneticAlgorithm.Chromosome initialChromosome = new GeneticAlgorithm.Chromosome(cities, salesmen, double.MaxValue);

            double fitness = geneticAlgorithm.GenerateFitnessForChromosome(initialChromosome);
            initialChromosome.Fitness = fitness;

            geneticAlgorithm.HybridInitialize(initialChromosome);
        }
    }
}
