using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravellingSalesmenConsole
{
    class Coordinator
    {
        public static Random rnd = new Random();
        private Algorithm algorithm;
        private bool algorithmStarted;

        public Algorithm Algorithm { get => algorithm; set => algorithm = value; }

        public Coordinator()
        {
            algorithmStarted = false;
        }

        public Coordinator(Algorithm algorithm)
        {
            this.Algorithm = algorithm;
            algorithmStarted = false;
        }

        public void startAlgorithm()
        {
            algorithm.Initialize();

            algorithmStarted = true;
        }

        public void runAlgorithmNextMove()
        {
            if (algorithmStarted)
            {
                if (algorithm.hasAlgorithmNextMove())
                {
                    algorithm.NextTurn();
                }
                else
                {
                    algorithmStarted = false;
                }
            }
        }

        public void runAlgorithmThrough()
        {
            if (algorithmStarted)
            {
                while (algorithm.hasAlgorithmNextMove())
                {
                    algorithm.NextTurn();
                }
                algorithmStarted = false;
            }
        }
    }
}
