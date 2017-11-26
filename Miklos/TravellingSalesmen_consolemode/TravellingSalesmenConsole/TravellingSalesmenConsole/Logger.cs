using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravellingSalesmenConsole
{
    class Logger
    {
        protected string algoName;
        protected string graphFileName;
        protected string agentFileName;
        protected int generationsNumber;
        protected int populationNumber;
        protected bool populationDefault;
        protected double mutationProbability;
        protected double weakParentRate;
        protected bool firstChildMutate;
        protected bool secondChildMutate;
        protected int runNumber;
        protected string outputFileName;
        protected enum ParameterToInterval {GenerationNumber, PopulationNumber, MutationProbability, WeakParentRate, FirstChildMutate, SecondChildMutate, Nothing};
        ParameterToInterval intervalEnum = new ParameterToInterval();
        string[] interval = { null, null };
        int intDoubleOrBool = -1; //0=nothing, 1=int, 2=double, 3=bool  (to the interval type)

        public string AlgoName { get => algoName; set => algoName = value; }
        public string GraphFileName { get => graphFileName; set => graphFileName = value; }
        public string AgentFileName { get => agentFileName; set => agentFileName = value; }

        public Logger() { }


        public void AskUser()
        {
            const string BASE_FOLDER_LOCATION = @"..\..\RawData";
            string readedDatas = null;  //ezt írja ki minden clear után
            string str;
            int numberInt;
            double numberDouble;
            bool isOk;

            string[] fileArrayGraphsFullName = Directory.GetFiles(BASE_FOLDER_LOCATION + @"\Graphs");
            string[] fileArrayGraphs = new string[fileArrayGraphsFullName.Length];
            for (int i = 0; i < fileArrayGraphsFullName.Length; i++)
            {
                string[] temp = fileArrayGraphsFullName[i].Split('\\');
                string[] temp2 = temp[temp.Length - 1].Split('.');
                fileArrayGraphs[i] = temp2[0];
            }
            
            string[] fileArrayAgentsFullName = Directory.GetFiles(BASE_FOLDER_LOCATION + @"\Agents");
            string[] fileArrayAgents = new string[fileArrayAgentsFullName.Length];
            for (int i = 0; i < fileArrayAgentsFullName.Length; i++)
            {
                string[] temp = fileArrayAgentsFullName[i].Split('\\');
                string[] temp2 = temp[temp.Length - 1].Split('.');
                fileArrayAgents[i] = temp2[0];
            }

            string[] fileArrayOutputsFullName = Directory.GetFiles(BASE_FOLDER_LOCATION + @"\Outputs");
            string[] fileArrayOutputs = new string[fileArrayOutputsFullName.Length];
            for (int i = 0; i < fileArrayOutputsFullName.Length; i++)
            {
                string[] temp = fileArrayOutputsFullName[i].Split('\\');
                string[] temp2 = temp[temp.Length - 1].Split('.');
                fileArrayOutputs[i] = temp2[0];
            }

            //kész
#region read Algo name
            isOk = false;
            Console.Clear();
            while (!isOk)
            {
                
                Console.WriteLine("Choose one algorithm:\n\t1 BruteForce\n\t2 Christofides\n\t3 Genetic\n\n");
                str = Console.ReadLine();
                switch (str)
                {
                    case "1":
                        algoName = "BruteForce";
                        readedDatas += "Algorithm: BruteForce\n";
                        isOk = true;
                        break;
                    case "2":
                        algoName = "Cristofides";
                        readedDatas += "Algorithm: Cristofides\n";
                        isOk = true;
                        break;
                    case "3":
                        algoName = "Genetic";
                        readedDatas += "Algorithm: Genetic\n";
                        isOk = true;
                        break;
                    default:
                        isOk = false;
                        Console.Clear();
                        Console.WriteLine("Wrong arnswer !\nType it again!\n");
                        break;
                }
            }
#endregion

            //kész
#region read Graph name
            isOk = false;
            Console.Clear();
            Console.WriteLine(readedDatas);
            while (!isOk)
            {
                Console.WriteLine("Type the grap name:\n");
                for (int i = 0; i < fileArrayGraphs.Length; i++)
                {
                    Console.WriteLine(fileArrayGraphs[i]);
                }
                Console.WriteLine("-----");
                str = Console.ReadLine();
                foreach (string row in fileArrayGraphs)
                {
                    if (row == str)
                    {
                        graphFileName = str;
                        readedDatas += "Graph: " + str + "\n";
                        isOk = true;
                        break;
                    }
                }
                if (!isOk)
                {
                    Console.Clear();
                    Console.WriteLine(readedDatas);
                    Console.WriteLine("Wrong arnswer !\nType it again!\n");
                }
            }
#endregion

            //kész
#region read Agents name
            isOk = false;
            Console.Clear();
            Console.WriteLine(readedDatas);
            while (!isOk)
            {
                Console.WriteLine("Type tha agents file name:\n");
                for (int i = 0; i < fileArrayAgents.Length; i++)
                {
                    Console.WriteLine(fileArrayAgents[i]);
                }
                Console.WriteLine("-----");
                str = Console.ReadLine();
                foreach (string row in fileArrayAgents)
                {
                    if (row == str)
                    {
                        agentFileName = str;
                        readedDatas += "Agents: " + str + "\n";
                        isOk = true;
                        break;
                    }
                }
                if (!isOk)
                {
                    Console.Clear();
                    Console.WriteLine(readedDatas);
                    Console.WriteLine("Wrong arnswer !\nType it again!\n");
                }
            }
            #endregion
            //kész
#region read Extras
            switch (algoName)
            { 
                case "BruteForce":
                    //nincs semmijen extra
                    break;
                case "Cristofides":
                    //nincs semmilyen extra
                    break;
                case "Genetic":

#region read Generation number
                    isOk = false;
                    Console.Clear();
                    Console.WriteLine(readedDatas);
                    while (!isOk)
                    {
                        
                        Console.WriteLine("Type the generatons number");
                        str = Console.ReadLine();
                        if (Int32.TryParse(str, out numberInt))
                        {
                            /* Yes input could be parsed and we can now use number in this code block 
                               scope */
                            generationsNumber = int.Parse(str);
                            readedDatas += "Generations number: " + generationsNumber + "\n";
                            isOk = true;
                        }
                        else
                        {
                            /* No, input could not be parsed to an integer */
                            Console.Clear();
                            Console.WriteLine(readedDatas);
                            Console.WriteLine("Wrong arnswer !\nType it again!\n");
                        }
                    }
#endregion

#region read Population number
                    isOk = false;
                    Console.Clear();
                    Console.WriteLine(readedDatas);
                    while (!isOk)
                    {
                        Console.WriteLine("Type the population number\nIf type 'Default' the number will be calculated.");
                        str = Console.ReadLine();

                        if(str == "default" || str == "Default")
                        {
                            populationDefault = true;
                            readedDatas += "Population number: " + "Default" + "\n";
                            isOk = true;
                        }
                        else
                        {
                            populationDefault = false;
                            if (Int32.TryParse(str, out numberInt))
                            {
                                /* Yes input could be parsed and we can now use number in this code block 
                                   scope */
                                populationNumber = int.Parse(str);
                                readedDatas += "Population number: " + populationNumber + "\n";
                                isOk = true;
                            }
                            else
                            {
                                /* No, input could not be parsed to an integer */
                                Console.Clear();
                                Console.WriteLine(readedDatas);
                                Console.WriteLine("Wrong arnswer !\nType it again!\n");
                            }
                        }

                    }
#endregion

#region read Mutation probability
                    isOk = false;
                    Console.Clear();
                    Console.WriteLine(readedDatas);
                    while (!isOk)
                    {

                        Console.WriteLine("Type the mutation probability");
                        str = Console.ReadLine();
                        if (Double.TryParse(str, out numberDouble))
                        {
                            /* Yes input could be parsed and we can now use number in this code block 
                               scope */
                            mutationProbability = Double.Parse(str);
                            readedDatas += "Mutation probability: " + mutationProbability + "\n";
                            isOk = true;
                        }
                        else
                        {
                            /* No, input could not be parsed to an integer */
                            Console.Clear();
                            Console.WriteLine(readedDatas);
                            Console.WriteLine("Wrong arnswer !\nType it again!\n");
                        }
                    }
#endregion

#region read Weak parent rate
                    isOk = false;
                    Console.Clear();
                    Console.WriteLine(readedDatas);
                    while (!isOk)
                    {

                        Console.WriteLine("Type the weak parent rate");
                        str = Console.ReadLine();

                        if (Double.TryParse(str, out numberDouble))
                        {
                            /* Yes input could be parsed and we can now use number in this code block 
                               scope */
                            weakParentRate = Double.Parse(str);
                            readedDatas += "Weak parent rate: " + weakParentRate + "\n";
                            isOk = true;
                        }
                        else
                        {
                            /* No, input could not be parsed to an double */
                            Console.Clear();
                            Console.WriteLine(readedDatas);
                            Console.WriteLine("Wrong arnswer !\nType it again!\n");
                        }
                    }
#endregion

#region read First child mutate
                    isOk = false;
                    Console.Clear();
                    Console.WriteLine(readedDatas);
                    while (!isOk)
                    {

                        Console.WriteLine("Type first child mutate!\n Type T if true, type F if false");
                        str = Console.ReadLine();
                        if (str == "T" || str == "t")
                        {
                            firstChildMutate = true;
                            readedDatas += "Firt child mutate: " + firstChildMutate + "\n";
                            isOk = true;
                        }
                        else if (str == "F" || str == "f")
                        {
                            firstChildMutate = false;
                            readedDatas += "Firt child mutate: " + firstChildMutate + "\n";
                            isOk = true;
                        }
                        else
                        {
                            Console.Clear();
                            Console.WriteLine(readedDatas);
                            Console.WriteLine("Wrong arnswer !\nType it again!\n");
                        }
                    }
#endregion

#region read Second child mutate
                    isOk = false;
                    Console.Clear();
                    Console.WriteLine(readedDatas);
                    while (!isOk)
                    {

                        Console.WriteLine("Type second child mutate!\n Type T if true, type F if false");
                        str = Console.ReadLine();
                        if (str == "T" || str == "t")
                        {
                            secondChildMutate = true;
                            readedDatas += "Second child mutate: " + secondChildMutate + "\n";
                            isOk = true;
                        }
                        else if (str == "F" || str == "f")
                        {
                            secondChildMutate = false;
                            readedDatas += "Second child mutate: " + secondChildMutate + "\n";
                            isOk = true;
                        }
                        else
                        {
                            Console.Clear();
                            Console.WriteLine(readedDatas);
                            Console.WriteLine("Wrong arnswer !\nType it again!\n");
                        }
                    }
                    #endregion

#region read Interval
                    isOk = false;
                    Console.Clear();
                    Console.WriteLine(readedDatas);

                    while (!isOk)
                    {
                        Console.WriteLine("Type the number of that parameter what you would like to run from...  ...to.:\n\n" +
                                            "1 GenerationNumber\n" +
                                            "2 PopulationNumber\n" +
                                            "3 MutationProbability\n" +
                                            "4 WeakParentRate\n" +
                                            "5 FirstChildMutate\n" +
                                            "6 SecondChildMutate\n" +
                                            "or type: 'nothing' if you dont want to run any parameter !");
                        str = Console.ReadLine();
                        str = str.ToUpper();
                        switch (str)
                        {
                            case "1":
                                intervalEnum = ParameterToInterval.GenerationNumber;
                                isOk = true;
                                intDoubleOrBool = 1;
                                break;
                            case "2":
                                intervalEnum = ParameterToInterval.PopulationNumber;
                                isOk = true;
                                intDoubleOrBool = 1;
                                break;
                            case "3":
                                intervalEnum = ParameterToInterval.MutationProbability;
                                isOk = true;
                                intDoubleOrBool = 2;
                                break;
                            case "4":
                                intervalEnum = ParameterToInterval.WeakParentRate;
                                isOk = true;
                                intDoubleOrBool = 2;
                                break;
                            case "5":
                                intervalEnum = ParameterToInterval.FirstChildMutate;
                                isOk = true;
                                intDoubleOrBool = 3;
                                break;
                            case "6":
                                intervalEnum = ParameterToInterval.SecondChildMutate;
                                isOk = true;
                                intDoubleOrBool = 3;
                                break;
                            case "NOTHING":
                                intervalEnum = ParameterToInterval.Nothing;
                                isOk = true;
                                intDoubleOrBool = 0;
                                break;

                            default:
                                Console.Clear();
                                Console.WriteLine(readedDatas);
                                Console.WriteLine("Wrong arnswer !\nType it again!\n");
                                isOk = false;
                                break;
                        }

                        if (intDoubleOrBool == 1)
                        {
#region read Int interval numbers
                            isOk = false;
                            Console.Clear();
                            Console.WriteLine(readedDatas);
                            while (!isOk)
                            {

                                Console.WriteLine("Type the interval minimum and maximum in this form: 423-653\n");
                                str = Console.ReadLine();
                                string[] str2 = str.Split('-');
                                if (Int32.TryParse(str2[0], out numberInt) && (Int32.TryParse(str2[1], out numberInt)))
                                {
                                    /* Yes input could be parsed and we can now use number in this code block 
                                       scope */
                                    interval[0] = str2[0];
                                    interval[1] = str2[1];
                                    readedDatas += intervalEnum.ToString() + " will run form " + interval[0] + " to " + interval[1] + "\n";
                                    isOk = true;
                                }
                                else
                                {
                                    /* No, input could not be parsed to an integer */
                                    Console.Clear();
                                    Console.WriteLine(readedDatas);
                                    Console.WriteLine("Wrong arnswer !\nType it again!\n");
                                }
                            }
#endregion
                        }

                        else if (intDoubleOrBool == 2)
                        {
#region read Double interval numbers
                            isOk = false;
                            Console.Clear();
                            Console.WriteLine(readedDatas);
                            while (!isOk)
                            {
                                Console.WriteLine("Type the interval minimum and maximum in this form: 0,21-0,452\n");
                                str = Console.ReadLine();
                                string[] str2 = str.Split('-');
                                if(str2.Length < 2)
                                {
                                    Console.Clear();
                                    Console.WriteLine(readedDatas);
                                    Console.WriteLine("Wrong arnswer !\nType it again!\n");
                                }
                                else
                                {
                                    if (Double.TryParse(str2[0], out numberDouble) && Double.TryParse(str2[1], out numberDouble))
                                    {
                                        /* Yes input could be parsed and we can now use number in this code block 
                                           scope */
                                        interval[0] = str2[0];
                                        interval[1] = str2[1];
                                        readedDatas += intervalEnum.ToString() + " will run form " + interval[0] + " to " + interval[1] + "\n";
                                        isOk = true;
                                    }
                                    else
                                    {
                                        /* No, input could not be parsed to an double */
                                        Console.Clear();
                                        Console.WriteLine(readedDatas);
                                        Console.WriteLine("Wrong arnswer !\nType it again!\n");
                                    }
                                }

                            }
#endregion
                        }

                        else if (intDoubleOrBool == 3)
                        {
                            readedDatas += intervalEnum.ToString() + " will run form " + "True" + " to " + "False" + "\n";
                            Console.Clear();
                            Console.WriteLine(readedDatas);
                        }

                        else if (intDoubleOrBool == 0)
                        {
                            readedDatas += intervalEnum.ToString() + " will run in interval!\n";
                            Console.Clear();
                            Console.WriteLine(readedDatas);
                        }
                    }
#endregion

                    break;
            }

            #endregion

#region read Run number
            isOk = false;
            Console.Clear();
            Console.WriteLine(readedDatas);
            while (!isOk)
            {
                Console.WriteLine("Type the run number");
                str = Console.ReadLine();
                if (Int32.TryParse(str, out numberInt))
                {
                    /* Yes input could be parsed and we can now use number in this code block 
                       scope */
                    runNumber = int.Parse(str);
                    readedDatas += "Run number: " + runNumber + "\n";
                    isOk = true;
                }
                else
                {
                    /* No, input could not be parsed to an integer */
                    Console.Clear();
                    Console.WriteLine(readedDatas);
                    Console.WriteLine("Wrong arnswer !\nType it again!\n");
                }
            }
            #endregion

#region Output file name
            isOk = false;
            Console.Clear();
            Console.WriteLine(readedDatas);
            while (!isOk)
            {
                Console.WriteLine("Type the output file name:\n");
                str = Console.ReadLine();
                isOk = true;
                foreach (string row in fileArrayOutputs)
                {
                    if (row == str)
                    {
                        isOk = false;
                    }
                }
                if (!isOk)
                {
                    Console.Clear();
                    Console.WriteLine(readedDatas);
                    Console.WriteLine("This filename is already exists\nType again !\n");
                }
                else
                {
                    outputFileName = str;
                    readedDatas += "Graph: " + outputFileName + "\n";
                    isOk = true;
                }
            }
            #endregion

            
        }

        public void Run()
        {
            
            Console.WriteLine("elkezdödött");
            Coordinator coordinator = new Coordinator();
            const string BASE_FOLDER_LOCATION = @"..\..\RawData";
            string outputPath = BASE_FOLDER_LOCATION + @"\Outputs\" + outputFileName + ".txt";
            string graphPath = BASE_FOLDER_LOCATION + @"\Graphs\" + graphFileName + ".txt";
            string agentPath = BASE_FOLDER_LOCATION + @"\Agents\" + agentFileName + ".txt";
            Stopwatch stopwatch = new Stopwatch();
            /*
            for(int i = 0; i <20; i++)
            {
                for(int j = 0; j < 20; j++)
                {
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(BASE_FOLDER_LOCATION + @"\Graphs\" + "graph400_1" + ".txt", true))
                    {
                        file.WriteLine("{0}" +" "+ "{1}",100+i*11, 100+j*10 );
                    }
                }

            }*/


            CompleteGraph cg = readGraphFromFile(graphPath);
            AgentManager am = readAgentsFromFile(agentPath);
            using (StreamWriter sw = (File.Exists(outputPath)) ? File.AppendText(outputPath) : File.CreateText(outputPath));
            double result = 0;            
            if (algoName == "BruteForce")
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(outputPath, true))
                {
                    file.WriteLine("Algorithm name" + "\t" + "Graph filename" + "\t" + "Agents filename" + "\t" + "Result" + "\t" + "Run time");
                }
                for (int i = 0; i < runNumber; i++)
                {
                    stopwatch.Restart();
                    BruteForce bf = new BruteForce(cg, am);
                    coordinator.Algorithm = bf;
                    coordinator.runAlgorithmThrough();
                    result = coordinator.Algorithm.getActualResult();
                    stopwatch.Stop();
                    TimeSpan ts = stopwatch.Elapsed;
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(outputPath, true))
                    {
                        file.WriteLine(this.algoName + "\t" + this.graphFileName + "\t" + this.agentFileName
                             + "\t" + result + "\t" + ts.TotalMilliseconds);
                    }
                }
            }
            else if (algoName == "Cristofides")
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(outputPath, true))
                {
                    file.WriteLine("Algorithm name" + "\t" + "Graph filename" + "\t" + "Agents filename" + "\t" + "Result" + "\t" + "Run time");
                }
                for (int i = 0; i < runNumber; i++)
                {
                    stopwatch.Restart();
                    Christofides c = new Christofides(cg, am);
                    coordinator.Algorithm = c;
                    coordinator.runAlgorithmThrough();
                    result = coordinator.Algorithm.getActualResult();
                    stopwatch.Stop();
                    TimeSpan ts = stopwatch.Elapsed;
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(outputPath, true))
                    {
                        file.WriteLine(this.algoName + "\t" + this.graphFileName + "\t" + this.agentFileName
                             + "\t" + result + "\t" + ts.TotalMilliseconds);
                    }
                }
            }
            else if (algoName == "Genetic")
            {
                double intervalStart = 0; ;
                double step = 0;
                if (interval[0] != null && interval[1] != null)
                {
                    if(intDoubleOrBool == 1)
                    {
                        int k1 = Int32.Parse(interval[0]);
                        intervalStart = k1;
                        int k2 = Int32.Parse(interval[1]);
                        int delta = Math.Abs(k1 - k2);
                        step = delta / runNumber;
                    }
                    else if(intDoubleOrBool == 2)
                    {
                        double k1 = Double.Parse(interval[0]);
                        intervalStart = k1;
                        double k2 = Double.Parse(interval[1]);
                        double delta = Math.Abs(k1 - k2);
                        step = delta / runNumber;
                    }
                }
                else
                {
                    step = 0;
                }

                using (System.IO.StreamWriter file = new System.IO.StreamWriter(outputPath, true))
                {
                    file.WriteLine("Algorithm name" + "\t" + "Graph filename" + "\t" + "Agents filename" + "\t" +
                        "Generation number" + "\t" + "Populatiom number" + "\t" + "Mutation probability" + "\t" +
                        "Weakparent rate" + "\t" + "First childmutate" + "\t" + "Second child mutate" + "\t" + 
                        "Result" + "\t" + "Run time");
                }
                for (int i = 0; i < runNumber; i++)
                {
                    GeneticAlgorithm ga = null;
                    int intervalIntStart = (int)intervalStart;
                    int stepInt = (int)step;
                    //GenerationNumber, PopulationNumber, MutationProbability, WeakParentRate, FirstChildMutate, SecondChildMutate, Nothing
                    switch (intervalEnum.ToString())
                    {
                        case "GenerationNumber":
                            generationsNumber = (intervalIntStart + i * stepInt);
                            ga = new GeneticAlgorithm(cg, am, generationsNumber, populationNumber,
                                        mutationProbability, weakParentRate,
                                        firstChildMutate, secondChildMutate, populationDefault);
                            break;
                        case "PopulationNumber":
                            populationNumber = (intervalIntStart + i * stepInt);
                            ga = new GeneticAlgorithm(cg, am, generationsNumber, populationNumber,
                                        mutationProbability, weakParentRate,
                                        firstChildMutate, secondChildMutate, populationDefault);
                            break;
                        case "MutationProbability":
                            mutationProbability = (intervalStart + i * step);
                            ga = new GeneticAlgorithm(cg, am, generationsNumber, populationNumber,
                                        mutationProbability, weakParentRate,
                                        firstChildMutate, secondChildMutate, populationDefault);
                            break;
                        case "WeakParentRate":
                            weakParentRate = (intervalStart + i * step);
                            ga = new GeneticAlgorithm(cg, am, generationsNumber, populationNumber,
                                        mutationProbability, weakParentRate,
                                        firstChildMutate, secondChildMutate, populationDefault);
                            break;
                        case "FirstChildMutate":
                            if (i < runNumber / 2)
                            {
                                firstChildMutate = true;
                                ga = new GeneticAlgorithm(cg, am, generationsNumber, populationNumber,
                                        mutationProbability, weakParentRate,
                                        firstChildMutate, secondChildMutate, populationDefault);
                            }
                            else
                            {
                                firstChildMutate = false;
                                ga = new GeneticAlgorithm(cg, am, generationsNumber, populationNumber,
                                        mutationProbability, weakParentRate,
                                        firstChildMutate, secondChildMutate, populationDefault);
                            }
                            break;
                        case "SecondChildMutate":
                            if (i < runNumber / 2)
                            {
                                secondChildMutate = true;
                                ga = new GeneticAlgorithm(cg, am, generationsNumber, populationNumber,
                                            mutationProbability, weakParentRate,
                                            firstChildMutate, secondChildMutate, populationDefault);
                            }
                            else
                            {
                                secondChildMutate = false;
                                ga = new GeneticAlgorithm(cg, am, generationsNumber, populationNumber,
                                            mutationProbability, weakParentRate,
                                            firstChildMutate, secondChildMutate, populationDefault);
                            }
                            break;
                        case "Nothing":
                            ga = new GeneticAlgorithm(cg, am, generationsNumber, populationNumber,
                                        mutationProbability, weakParentRate,
                                        firstChildMutate, secondChildMutate, populationDefault);
                            break;
                    }

                    
                    stopwatch.Stop();
                    TimeSpan ts = stopwatch.Elapsed;
                    stopwatch.Restart();
                    coordinator.Algorithm = ga;
                    coordinator.Algorithm.Initialize();
                    coordinator.runAlgorithmThrough();
                    result = coordinator.Algorithm.getActualResult();
                    
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(outputPath, true))
                    {
                        file.WriteLine(this.algoName + "\t" + this.graphFileName + "\t" + this.agentFileName
                             + "\t" + generationsNumber + "\t" + populationNumber + "\t" +mutationProbability
                             + "\t" + weakParentRate + "\t" + firstChildMutate + "\t" + secondChildMutate
                             +"\t" + result + "\t" + ts.TotalMilliseconds  );
                    }
                }
            }
        }

        public AgentManager readAgentsFromFile(String path)
        {

            AgentManager agentManager = new AgentManager();

            using (StreamReader sr = new StreamReader(path))
            {
                String[] data = sr.ReadLine().Split(' ');
                for (int i = 0; i < data.Length; i++)
                {
                    agentManager.Agents.Add(new Agent(i, int.Parse(data[i])));
                }
            }

            return agentManager;
        }

        public CompleteGraph readGraphFromFile(String path)
        {
            int lines = File.ReadAllLines(path).Length;
            List<Vertex> vertices = new List<Vertex>();

            using (StreamReader sr = new StreamReader(path))
            {
                for (int i = 0; i < lines; i++)
                {
                    String[] data = sr.ReadLine().Split(' ');
                    vertices.Add(new Vertex(i, new Coordinate(int.Parse(data[0]), int.Parse(data[1]))));
                }
            }

            return new CompleteGraph(vertices);
        }

    }
}





