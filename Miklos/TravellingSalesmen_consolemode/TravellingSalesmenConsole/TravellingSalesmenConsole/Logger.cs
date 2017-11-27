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
        protected int runNumber;
        protected string outputFileName;
        //GA extras
        protected int GA_generationsNumber;
        protected int GA_populationNumber;
        protected bool GA_populationDefault;
        protected double GA_mutationProbability;
        protected double GA_weakParentRate;
        protected bool GA_firstChildMutate;
        protected bool GA_secondChildMutate;
        protected enum GA_ParameterToInterval {GenerationNumber, PopulationNumber, MutationProbability, WeakParentRate, FirstChildMutate, SecondChildMutate, Nothing};
        GA_ParameterToInterval GA_intervalEnum = new GA_ParameterToInterval();
        string[] interval = { null, null };
        int GA_intDoubleOrBool = -1; //0=nothing, 1=int, 2=double, 3=bool  (to the interval type)
        //
        //GS extras


        //......................

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
                Console.WriteLine("Choose one algorithm:\n\t1 BruteForce\n\t2 Christofides\n\t3 Genetic\n\t4 GreedySearch\n\n");
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
                    case "4":
                        algoName = "GreedySearch";
                        readedDatas += "Algorithm: GreedySearch\n";
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
                            GA_generationsNumber = int.Parse(str);
                            readedDatas += "Generations number: " + GA_generationsNumber + "\n";
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
                            GA_populationDefault = true;
                            readedDatas += "Population number: " + "Default" + "\n";
                            isOk = true;
                        }
                        else
                        {
                            GA_populationDefault = false;
                            if (Int32.TryParse(str, out numberInt))
                            {
                                /* Yes input could be parsed and we can now use number in this code block 
                                   scope */
                                GA_populationNumber = int.Parse(str);
                                readedDatas += "Population number: " + GA_populationNumber + "\n";
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
                            GA_mutationProbability = Double.Parse(str);
                            readedDatas += "Mutation probability: " + GA_mutationProbability + "\n";
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
                            GA_weakParentRate = Double.Parse(str);
                            readedDatas += "Weak parent rate: " + GA_weakParentRate + "\n";
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

                        Console.WriteLine("Type first child mutate!\nType T if true, type F if false");
                        str = Console.ReadLine();
                        if (str == "T" || str == "t")
                        {
                            GA_firstChildMutate = true;
                            readedDatas += "Firt child mutate: " + GA_firstChildMutate + "\n";
                            isOk = true;
                        }
                        else if (str == "F" || str == "f")
                        {
                            GA_firstChildMutate = false;
                            readedDatas += "Firt child mutate: " + GA_firstChildMutate + "\n";
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

                        Console.WriteLine("Type second child mutate!\nType T if true, type F if false");
                        str = Console.ReadLine();
                        if (str == "T" || str == "t")
                        {
                            GA_secondChildMutate = true;
                            readedDatas += "Second child mutate: " + GA_secondChildMutate + "\n";
                            isOk = true;
                        }
                        else if (str == "F" || str == "f")
                        {
                            GA_secondChildMutate = false;
                            readedDatas += "Second child mutate: " + GA_secondChildMutate + "\n";
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
                                GA_intervalEnum = GA_ParameterToInterval.GenerationNumber;
                                isOk = true;
                                GA_intDoubleOrBool = 1;
                                break;
                            case "2":
                                GA_intervalEnum = GA_ParameterToInterval.PopulationNumber;
                                isOk = true;
                                GA_intDoubleOrBool = 1;
                                break;
                            case "3":
                                GA_intervalEnum = GA_ParameterToInterval.MutationProbability;
                                isOk = true;
                                GA_intDoubleOrBool = 2;
                                break;
                            case "4":
                                GA_intervalEnum = GA_ParameterToInterval.WeakParentRate;
                                isOk = true;
                                GA_intDoubleOrBool = 2;
                                break;
                            case "5":
                                GA_intervalEnum = GA_ParameterToInterval.FirstChildMutate;
                                isOk = true;
                                GA_intDoubleOrBool = 3;
                                break;
                            case "6":
                                GA_intervalEnum = GA_ParameterToInterval.SecondChildMutate;
                                isOk = true;
                                GA_intDoubleOrBool = 3;
                                break;
                            case "NOTHING":
                                GA_intervalEnum = GA_ParameterToInterval.Nothing;
                                isOk = true;
                                GA_intDoubleOrBool = 0;
                                break;

                            default:
                                Console.Clear();
                                Console.WriteLine(readedDatas);
                                Console.WriteLine("Wrong arnswer !\nType it again!\n");
                                isOk = false;
                                break;
                        }

                        if (GA_intDoubleOrBool == 1)
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
                                    readedDatas += GA_intervalEnum.ToString() + " will run form " + interval[0] + " to " + interval[1] + "\n";
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

                        else if (GA_intDoubleOrBool == 2)
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
                                        readedDatas += GA_intervalEnum.ToString() + " will run form " + interval[0] + " to " + interval[1] + "\n";
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

                        else if (GA_intDoubleOrBool == 3)
                        {
                            readedDatas += GA_intervalEnum.ToString() + " will run form " + "True" + " to " + "False" + "\n";
                            Console.Clear();
                            Console.WriteLine(readedDatas);
                        }

                        else if (GA_intDoubleOrBool == 0)
                        {
                            readedDatas += GA_intervalEnum.ToString() + " will run in interval!\n";
                            Console.Clear();
                            Console.WriteLine(readedDatas);
                        }
                    }
#endregion

                    break;
                case "GreedySearch":
                    //todo greedy paraméterei beolvasás
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
            
            Console.WriteLine("Algorithm is started");
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
                    coordinator.startAlgorithm();
                    coordinator.runAlgorithmThrough();
                    result = coordinator.Algorithm.getActualResult();
                    stopwatch.Stop();
                    TimeSpan ts = stopwatch.Elapsed;
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(outputPath, true))
                    {
                        file.WriteLine(this.algoName + "\t" + this.graphFileName + "\t" + this.agentFileName
                             + "\t" + result + "\t" + ts.TotalMilliseconds);
                    }
                    Console.Clear();
                    Console.WriteLine("Algorithm is already {0}/{1} done.",i,runNumber);
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
                    coordinator.startAlgorithm();
                    coordinator.runAlgorithmThrough();
                    result = coordinator.Algorithm.getActualResult();
                    stopwatch.Stop();
                    TimeSpan ts = stopwatch.Elapsed;
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(outputPath, true))
                    {
                        file.WriteLine(this.algoName + "\t" + this.graphFileName + "\t" + this.agentFileName
                             + "\t" + result + "\t" + ts.TotalMilliseconds);
                    }
                    Console.Clear();
                    Console.WriteLine("Algorithm is already {0}/{1} done.", i, runNumber);
                }
            }
            else if (algoName == "GreedySearch")
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(outputPath, true))
                {
                    file.WriteLine("Algorithm name" + "\t" + "Graph filename" + "\t" + "Agents filename" + "\t" + "Result" + "\t" + "Run time");
                }
                for (int i = 0; i < runNumber; i++)
                {
                    stopwatch.Restart();
                    GreedySearch gs = new GreedySearch(cg, am);
                    coordinator.Algorithm = gs;
                    coordinator.startAlgorithm();
                    coordinator.runAlgorithmThrough();
                    result = coordinator.Algorithm.getActualResult();
                    stopwatch.Stop();
                    TimeSpan ts = stopwatch.Elapsed;
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(outputPath, true))
                    {
                        file.WriteLine(this.algoName + "\t" + this.graphFileName + "\t" + this.agentFileName
                             + "\t" + result + "\t" + ts.TotalMilliseconds);
                    }
                    Console.Clear();
                    Console.WriteLine("Algorithm is already {0}/{1} done.", i, runNumber);
                }
            }
            else if (algoName == "Genetic")
            {
                double intervalStart = 0; ;
                double step = 0;
                if (interval[0] != null && interval[1] != null)
                {
                    if (GA_intDoubleOrBool == 1)
                    {
                        int k1 = Int32.Parse(interval[0]);
                        intervalStart = k1;
                        int k2 = Int32.Parse(interval[1]);
                        int delta = Math.Abs(k1 - k2);
                        step = delta / runNumber;
                    }
                    else if (GA_intDoubleOrBool == 2)
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
                    switch (GA_intervalEnum.ToString())
                    {
                        case "GenerationNumber":
                            GA_generationsNumber = (intervalIntStart + i * stepInt);
                            ga = new GeneticAlgorithm(cg, am, GA_generationsNumber, GA_populationNumber,
                                        GA_mutationProbability, GA_weakParentRate,
                                        GA_firstChildMutate, GA_secondChildMutate, GA_populationDefault);
                            break;
                        case "PopulationNumber":
                            GA_populationNumber = (intervalIntStart + i * stepInt);
                            ga = new GeneticAlgorithm(cg, am, GA_generationsNumber, GA_populationNumber,
                                        GA_mutationProbability, GA_weakParentRate,
                                        GA_firstChildMutate, GA_secondChildMutate, GA_populationDefault);
                            break;
                        case "MutationProbability":
                            GA_mutationProbability = (intervalStart + i * step);
                            ga = new GeneticAlgorithm(cg, am, GA_generationsNumber, GA_populationNumber,
                                        GA_mutationProbability, GA_weakParentRate,
                                        GA_firstChildMutate, GA_secondChildMutate, GA_populationDefault);
                            break;
                        case "WeakParentRate":
                            GA_weakParentRate = (intervalStart + i * step);
                            ga = new GeneticAlgorithm(cg, am, GA_generationsNumber, GA_populationNumber,
                                        GA_mutationProbability, GA_weakParentRate,
                                        GA_firstChildMutate, GA_secondChildMutate, GA_populationDefault);
                            break;
                        case "FirstChildMutate":
                            if (i < runNumber / 2)
                            {
                                GA_firstChildMutate = true;
                                ga = new GeneticAlgorithm(cg, am, GA_generationsNumber, GA_populationNumber,
                                        GA_mutationProbability, GA_weakParentRate,
                                        GA_firstChildMutate, GA_secondChildMutate, GA_populationDefault);
                            }
                            else
                            {
                                GA_firstChildMutate = false;
                                ga = new GeneticAlgorithm(cg, am, GA_generationsNumber, GA_populationNumber,
                                        GA_mutationProbability, GA_weakParentRate,
                                        GA_firstChildMutate, GA_secondChildMutate, GA_populationDefault);
                            }
                            break;
                        case "SecondChildMutate":
                            if (i < runNumber / 2)
                            {
                                GA_secondChildMutate = true;
                                ga = new GeneticAlgorithm(cg, am, GA_generationsNumber, GA_populationNumber,
                                            GA_mutationProbability, GA_weakParentRate,
                                            GA_firstChildMutate, GA_secondChildMutate, GA_populationDefault);
                            }
                            else
                            {
                                GA_secondChildMutate = false;
                                ga = new GeneticAlgorithm(cg, am, GA_generationsNumber, GA_populationNumber,
                                            GA_mutationProbability, GA_weakParentRate,
                                            GA_firstChildMutate, GA_secondChildMutate, GA_populationDefault);
                            }
                            break;
                        case "Nothing":
                            ga = new GeneticAlgorithm(cg, am, GA_generationsNumber, GA_populationNumber,
                                        GA_mutationProbability, GA_weakParentRate,
                                        GA_firstChildMutate, GA_secondChildMutate, GA_populationDefault);
                            break;
                    }


                    stopwatch.Stop();
                    TimeSpan ts = stopwatch.Elapsed;
                    stopwatch.Restart();
                    coordinator.Algorithm = ga;
                    coordinator.startAlgorithm();
                    coordinator.runAlgorithmThrough();
                    result = coordinator.Algorithm.getActualResult();

                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(outputPath, true))
                    {
                        file.WriteLine(this.algoName + "\t" + this.graphFileName + "\t" + this.agentFileName
                             + "\t" + GA_generationsNumber + "\t" + GA_populationNumber + "\t" + GA_mutationProbability
                             + "\t" + GA_weakParentRate + "\t" + GA_firstChildMutate + "\t" + GA_secondChildMutate
                             + "\t" + result + "\t" + ts.TotalMilliseconds);
                    }
                    Console.Clear();
                    Console.WriteLine("Algorithm is already {0}/{1} done.", i, runNumber);
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





