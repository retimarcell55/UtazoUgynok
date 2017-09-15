using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravellingSalesmen
{
    class Coordinator
    {
        private Configuration configuration;
        private Algorithm algorithm;
        private bool algorithmStarted;

        public Configuration Configuration { get => configuration; set => configuration = value; }
        public Algorithm Algorithm { get => algorithm; set => algorithm = value; }

        public Coordinator()
        {
            algorithmStarted = false;
        }

        public Coordinator(Configuration configuration,Algorithm algorithm)
        {
            this.Configuration = configuration;
            this.Algorithm = algorithm;
            algorithmStarted = false;
        }

        public void runAlgorithm()
        {

        }
    }
}
