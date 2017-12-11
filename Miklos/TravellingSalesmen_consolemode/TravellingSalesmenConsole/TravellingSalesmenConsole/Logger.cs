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
        protected int algorithmRunNumber;
        protected string outputFileName;

        string[] interval = { null, null };
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
        int GA_intDoubleOrBool = -1; //0=nothing, 1=int, 2=double, 3=bool  (to the interval type)
        //GS extras
        protected int GS_patienceParameter;
        protected int GS_numberOfRuns;
        protected int GS_maxRouteLengthPerAgent;
        protected enum GS_ParameterToInterval { PatienceParameter, NumberOfRuns, MaxRouteLengthPerAgent, Nothing};
        GS_ParameterToInterval GS_intervalEnum = new GS_ParameterToInterval();
        //......................

        public string AlgoName { get => algoName; set => algoName = value; }
        public string GraphFileName { get => graphFileName; set => graphFileName = value; }
        public string AgentFileName { get => agentFileName; set => agentFileName = value; }

        public Logger() { }


        public void AskUser()
        {
            const string BASE_FOLDER_LOCATION = @"..\..\RawData";
            string readedDatas = null;  //to write after clear
            string str;
            int numberInt;
            double numberDouble;
            bool isOk;

#region read files from folders
            string[] fileArrayGraphsFullName = Directory.GetFiles(BASE_FOLDER_LOCATION + @"\Graphs");
            string[] fileArrayGraphs = new string[fileArrayGraphsFullName.Length];
            for (int i = 0; i < fileArrayGraphsFullName.Length; i++)
            {
                string[] temp = fileArrayGraphsFullName[i].Split('\\');
                string[] temp2 = temp[temp.Length - 1].Split('.');
                fileArrayGraphs[i] = temp2[0];
            }
            
            string[] fileArrayAgentsFullName_multi = Directory.GetFiles(BASE_FOLDER_LOCATION + @"\Agents\MultiAgents");
            string[] fileArrayAgents_multi = new string[fileArrayAgentsFullName_multi.Length];
            for (int i = 0; i < fileArrayAgentsFullName_multi.Length; i++)
            {
                string[] temp = fileArrayAgentsFullName_multi[i].Split('\\');
                string[] temp2 = temp[temp.Length - 1].Split('.');
                fileArrayAgents_multi[i] = temp2[0];
            }

            string[] fileArrayAgentsFullName_single = Directory.GetFiles(BASE_FOLDER_LOCATION + @"\Agents\SingleAgent");
            string[] fileArrayAgents_single = new string[fileArrayAgentsFullName_single.Length];
            for (int i = 0; i < fileArrayAgentsFullName_single.Length; i++)
            {
                string[] temp = fileArrayAgentsFullName_single[i].Split('\\');
                string[] temp2 = temp[temp.Length - 1].Split('.');
                fileArrayAgents_single[i] = temp2[0];
            }

            string[] fileArrayOutputsFullName_genetic = Directory.GetFiles(BASE_FOLDER_LOCATION + @"\Outputs\Genetic");
            string[] fileArrayOutputs_genetic = new string[fileArrayOutputsFullName_genetic.Length];
            for (int i = 0; i < fileArrayOutputsFullName_genetic.Length; i++)
            {
                string[] temp = fileArrayOutputsFullName_genetic[i].Split('\\');
                string[] temp2 = temp[temp.Length - 1].Split('.');
                fileArrayOutputs_genetic[i] = temp2[0];
            }
            string[] fileArrayOutputsFullName_greedy = Directory.GetFiles(BASE_FOLDER_LOCATION + @"\Outputs\GreedySearch");
            string[] fileArrayOutputs_greedy = new string[fileArrayOutputsFullName_greedy.Length];
            for (int i = 0; i < fileArrayOutputsFullName_greedy.Length; i++)
            {
                string[] temp = fileArrayOutputsFullName_greedy[i].Split('\\');
                string[] temp2 = temp[temp.Length - 1].Split('.');
                fileArrayOutputs_greedy[i] = temp2[0];
            }
            string[] fileArrayOutputsFullName_christofides = Directory.GetFiles(BASE_FOLDER_LOCATION + @"\Outputs\Christofides");
            string[] fileArrayOutputs_christofides = new string[fileArrayOutputsFullName_christofides.Length];
            for (int i = 0; i < fileArrayOutputsFullName_christofides.Length; i++)
            {
                string[] temp = fileArrayOutputsFullName_christofides[i].Split('\\');
                string[] temp2 = temp[temp.Length - 1].Split('.');
                fileArrayOutputs_christofides[i] = temp2[0];
            }
            string[] fileArrayOutputsFullName_bruteSingle = Directory.GetFiles(BASE_FOLDER_LOCATION + @"\Outputs\BruteForceSingleAgent");
            string[] fileArrayOutputs_bruteSingle = new string[fileArrayOutputsFullName_bruteSingle.Length];
            for (int i = 0; i < fileArrayOutputsFullName_bruteSingle.Length; i++)
            {
                string[] temp = fileArrayOutputsFullName_bruteSingle[i].Split('\\');
                string[] temp2 = temp[temp.Length - 1].Split('.');
                fileArrayOutputs_bruteSingle[i] = temp2[0];
            }
            string[] fileArrayOutputsFullName_bruteMulti = Directory.GetFiles(BASE_FOLDER_LOCATION + @"\Outputs\BruteForceMultiAgents");
            string[] fileArrayOutputs_bruteMulti = new string[fileArrayOutputsFullName_bruteMulti.Length];
            for (int i = 0; i < fileArrayOutputsFullName_bruteMulti.Length; i++)
            {
                string[] temp = fileArrayOutputsFullName_bruteMulti[i].Split('\\');
                string[] temp2 = temp[temp.Length - 1].Split('.');
                fileArrayOutputs_bruteMulti[i] = temp2[0];
            }
#endregion

            
#region read Algo name
            isOk = false;
            Console.Clear();
            while (!isOk)
            {
                Console.WriteLine("Choose one algorithm:\n" +
                    "\t1 BruteForceSingleAgent\n" +
                    "\t2 BruteForceMultiAgent\n" +
                    "\t3 Christofides\n" +
                    "\t4 Genetic\n" +
                    "\t5 GreedySearch\n\n");
                str = Console.ReadLine();
                switch (str)
                {
                    case "1":
                        algoName = "BruteForceSingleAgent";
                        readedDatas += "Algorithm: BruteForceSingleAgent\n";
                        isOk = true;
                        break;
                    case "2":
                        algoName = "BruteForceMultiAgent";
                        readedDatas += "Algorithm: BruteForceMultiAgent\n";
                        isOk = true;
                        break;
                    case "3":
                        algoName = "Christofides";
                        readedDatas += "Algorithm: Christofides\n";
                        isOk = true;
                        break;
                    case "4":
                        algoName = "Genetic";
                        readedDatas += "Algorithm: Genetic\n";
                        isOk = true;
                        break;
                    case "5":
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

            
#region read Agents name
            isOk = false;
            Console.Clear();
            Console.WriteLine(readedDatas);
            while (!isOk)
            {
                if(algoName == "BruteForceSingleAgent" || algoName == "Christofides")   //SingleAgents
                {
                    Console.WriteLine("Type the agents file name:\n");
                    for (int i = 0; i < fileArrayAgents_single.Length; i++)
                    {
                        Console.WriteLine(fileArrayAgents_single[i]);
                    }
                    Console.WriteLine("-----");
                    str = Console.ReadLine();
                    foreach (string row in fileArrayAgents_single)
                    {
                        if (row == str)
                        {
                            agentFileName = @"SingleAgent\" + str;
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
                else //MultiAgents
                {
                    Console.WriteLine("Type the agents file name:\n");
                    for (int i = 0; i < fileArrayAgents_multi.Length; i++)
                    {
                        Console.WriteLine(fileArrayAgents_multi[i]);
                    }
                    Console.WriteLine("-----");
                    str = Console.ReadLine();
                    foreach (string row in fileArrayAgents_multi)
                    {
                        if (row == str)
                        {
                            agentFileName = @"MultiAgents\" + str;
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

                
            }
            #endregion
            //kész
#region read Extras
            switch (algoName)
            { 
                case "BruteForceSingleAgent":
                    //nincs semmijen extra
                    break;
                case "BruteForceMultiAgents":
                    //nincs semmijen extra
                    break;
                case "Christofides":
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
                        Console.WriteLine("Type the population number\nIf type -1 the number will be calculated.");
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
#region read Patience paramter
                    isOk = false;
                    Console.Clear();
                    Console.WriteLine(readedDatas);
                    while (!isOk)
                    {
                        Console.WriteLine("Type the patience parameter.");
                        str = Console.ReadLine();

                        if (Int32.TryParse(str, out numberInt))
                        {
                            /* Yes input could be parsed and we can now use number in this code block 
                                scope */
                           GS_patienceParameter = int.Parse(str);
                            readedDatas += "Patience parameter: " + GS_patienceParameter + "\n";
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

#region read Number of run
                    isOk = false;
                    Console.Clear();
                    Console.WriteLine(readedDatas);
                    while (!isOk)
                    {
                        Console.WriteLine("Type the number of runs.");
                        str = Console.ReadLine();

                        if (Int32.TryParse(str, out numberInt))
                        {
                            /* Yes input could be parsed and we can now use number in this code block 
                                scope */
                            GS_numberOfRuns = int.Parse(str);
                            readedDatas += "Number of Runs: " + GS_numberOfRuns + "\n";
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

#region read Max Route Length Per Agent
                    isOk = false;
                    Console.Clear();
                    Console.WriteLine(readedDatas);
                    while (!isOk)
                    {
                        Console.WriteLine("Type the max route length per agent.");
                        str = Console.ReadLine();

                        if (Int32.TryParse(str, out numberInt))
                        {
                            /* Yes input could be parsed and we can now use number in this code block 
                                scope */
                            GS_maxRouteLengthPerAgent = int.Parse(str);
                            readedDatas += "Max route length per agent: " + GS_maxRouteLengthPerAgent + "\n";
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

#region read interval in greedy
                    isOk = false;
                    Console.Clear();
                    Console.WriteLine(readedDatas);
                    bool nothing = false;
                    while (!isOk)
                    {
                        Console.WriteLine("Type the number of that parameter what you would like to run from...  ...to.:\n\n" +
                                            "1 PatienceParameter\n" +
                                            "2 NumberOfRuns\n" +
                                            "3 MaxLengthPerAgent\n" +
                                            "or type: 'nothing' if you dont want to run any parameter !");
                        str = Console.ReadLine();
                        str = str.ToUpper();
                        switch (str)
                        {
                            case "1":
                                GS_intervalEnum = GS_ParameterToInterval.PatienceParameter;
                                isOk = true;
                                break;
                            case "2":
                                GS_intervalEnum = GS_ParameterToInterval.NumberOfRuns;
                                isOk = true;
                                break;
                            case "3":
                                GS_intervalEnum = GS_ParameterToInterval.MaxRouteLengthPerAgent;
                                isOk = true;
                                GA_intDoubleOrBool = 2;
                                break;
                            case "NOTHING":
                                GS_intervalEnum = GS_ParameterToInterval.Nothing;
                                isOk = true;
                                nothing = true;
                                GA_intDoubleOrBool = 0;
                                break;
                            default:
                                Console.Clear();
                                Console.WriteLine(readedDatas);
                                Console.WriteLine("Wrong arnswer !\nType it again!\n");
                                isOk = false;
                                break;
                        }
                    }

                    isOk = false;
                    Console.Clear();
                    Console.WriteLine(readedDatas);
                    while (!isOk)
                    {
                        if (nothing)
                        {
                            isOk = true;
                        }
                        else
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
                                readedDatas += GS_intervalEnum.ToString() + " will run form " + interval[0] + " to " + interval[1] + "\n";
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
                    algorithmRunNumber = int.Parse(str);
                    readedDatas += "Run number: " + algorithmRunNumber + "\n";
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

            switch (algoName)
            {
                case "BruteForceSingleAgent":
                    while (!isOk)
                    {
                        Console.WriteLine("Type the output file name:\n");
                        str = Console.ReadLine();
                        isOk = true;
                        foreach (string row in fileArrayOutputsFullName_bruteSingle)
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
                            outputFileName = @"BruteForceSingleAgent\" + str;
                            readedDatas += "Output file name: " + outputFileName + "\n";
                            isOk = true;
                        }
                    }
                    break;
                case "BruteForceMultiAgents":
                    while (!isOk)
                    {
                        Console.WriteLine("Type the output file name:\n");
                        str = Console.ReadLine();
                        isOk = true;
                        foreach (string row in fileArrayOutputsFullName_bruteMulti)
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
                            outputFileName = @"BruteForceultiAgents\" + str;
                            readedDatas += "Output file name: " + outputFileName + "\n";
                            isOk = true;
                        }
                    }
                    break;
                case "Christofides":
                    while (!isOk)
                    {
                        Console.WriteLine("Type the output file name:\n");
                        str = Console.ReadLine();
                        isOk = true;
                        foreach (string row in fileArrayOutputsFullName_christofides)
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
                            outputFileName = @"Christofides\" + str;
                            readedDatas += "Output file name: " + outputFileName + "\n";
                            isOk = true;
                        }
                    }
                    break;
                case "Genetic":
                    while (!isOk)
                    {
                        Console.WriteLine("Type the output file name:\n");
                        str = Console.ReadLine();
                        isOk = true;
                        foreach (string row in fileArrayOutputsFullName_genetic)
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
                            outputFileName = @"Genetic\" + str;
                            readedDatas += "Output file name: " + outputFileName + "\n";
                            isOk = true;
                        }
                    }
                    break;
                case "GreedySearch":
                    while (!isOk)
                    {
                        Console.WriteLine("Type the output file name:\n");
                        str = Console.ReadLine();
                        isOk = true;
                        foreach (string row in fileArrayOutputsFullName_greedy)
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
                            outputFileName = @"GreedySearch\" + str;
                            readedDatas += "Output file name: " + outputFileName + "\n";
                            isOk = true;
                        }
                    }
                    break;
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
            stopwatch.Start();
            stopwatch.Stop();


            CompleteGraph cg = readGraphFromFile(graphPath);
            AgentManager am = readAgentsFromFile(agentPath);
            using (StreamWriter sw = (File.Exists(outputPath)) ? File.AppendText(outputPath) : File.CreateText(outputPath));
            double result = 0;
            if (algoName == "BruteForceSingleAgent" || algoName == "BruteForceMultiAgents")
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(outputPath, true))
                {
                    file.WriteLine("Algorithm name" + "\t" + "Graph filename" + "\t" + "Agents filename" + "\t" + "Result" + "\t" + "Run time");
                }
                for (int i = 0; i < algorithmRunNumber; i++)
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
                    Console.WriteLine("Algorithm is already {0}/{1} done.",i,algorithmRunNumber);
                }
            }
            else if (algoName == "Christofides")
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(outputPath, true))
                {
                    file.WriteLine("Algorithm name" + "\t" + "Graph filename" + "\t" + "Agents filename" + "\t" + "Result" + "\t" + "Run time");
                }
                for (int i = 0; i < algorithmRunNumber; i++)
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
                    Console.WriteLine("Algorithm is already {0}/{1} done.", i, algorithmRunNumber);
                }
            }
            else if (algoName == "GreedySearch")
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(outputPath, true))
                {
                    file.WriteLine("Algorithm name" + "\t" + "Graph filename" + "\t" + "Agents filename" + "\t" +
                        "Patience parameter" + "\t" + "Number of runs" + "\t" + "Max route length per agent" + "\t" +
                        "Result" + "\t" + "Run time");
                }
                for (int i = 0; i < algorithmRunNumber; i++)
                {
                    double intervalStart = 0;
                    double step = 0;
                    if (interval[0] != null && interval[1] != null)
                    {
                            int k1 = Int32.Parse(interval[0]);
                            intervalStart = k1;
                            int k2 = Int32.Parse(interval[1]);
                            int delta = Math.Abs(k1 - k2);
                            step = delta / algorithmRunNumber;
                    }

                    GreedySearch gs = null;
                    int intervalIntStart = (int)intervalStart;
                    int stepInt = (int)step;
                    
                    switch (GS_intervalEnum.ToString())
                    {
                        case "PatienceParameter":
                            GS_patienceParameter = (intervalIntStart + i * stepInt);
                            gs = new GreedySearch(cg, am, GS_patienceParameter, GS_numberOfRuns, GS_maxRouteLengthPerAgent);
                            break;
                        case "NumberOfRuns":
                            GS_numberOfRuns = (intervalIntStart + i * stepInt);
                            gs = new GreedySearch(cg, am, GS_patienceParameter, GS_numberOfRuns, GS_maxRouteLengthPerAgent);
                            break;
                        case "MaxRouteLengthPerAgent":
                            GS_maxRouteLengthPerAgent = (intervalIntStart + i * stepInt);
                            gs = new GreedySearch(cg, am, GS_patienceParameter, GS_numberOfRuns, GS_maxRouteLengthPerAgent);
                            break;
                        case "Nothing":
                            gs = new GreedySearch(cg, am, GS_patienceParameter, GS_numberOfRuns, GS_maxRouteLengthPerAgent);
                            break;
                    }
                    
                    stopwatch.Restart();
                    coordinator.Algorithm = gs;
                    coordinator.startAlgorithm();
                    coordinator.runAlgorithmThrough();
                    result = coordinator.Algorithm.getActualResult();
                    stopwatch.Stop();
                    TimeSpan ts = stopwatch.Elapsed;
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(outputPath, true))
                    {
                        file.WriteLine(this.algoName + "\t" + this.graphFileName + "\t" + this.agentFileName
                             + "\t" + GS_patienceParameter + "\t" + GS_numberOfRuns + "\t" + GS_maxRouteLengthPerAgent
                             + "\t" + result + "\t" + ts.TotalMilliseconds);
                    }
                    Console.Clear();
                    Console.WriteLine("Algorithm is already {0}/{1} done.", i, algorithmRunNumber);
                }
            }
            else if (algoName == "Genetic")
            {
                double intervalStart = 0;
                double step = 0;
                if (interval[0] != null && interval[1] != null)
                {
                    if (GA_intDoubleOrBool == 1)
                    {
                        int k1 = Int32.Parse(interval[0]);
                        intervalStart = k1;
                        int k2 = Int32.Parse(interval[1]);
                        int delta = Math.Abs(k1 - k2);
                        step = delta / algorithmRunNumber;
                    }
                    else if (GA_intDoubleOrBool == 2)
                    {
                        double k1 = Double.Parse(interval[0]);
                        intervalStart = k1;
                        double k2 = Double.Parse(interval[1]);
                        double delta = Math.Abs(k1 - k2);
                        step = delta / algorithmRunNumber;
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
                for (int i = 0; i < algorithmRunNumber; i++)
                {
                    GeneticAlgorithm ga = null;
                    int intervalIntStart = (int)intervalStart;
                    int stepInt = (int)step;
                    //GenerationNumber, PopulationNumber, MutationProbability, WeakParentRate, FirstChildMutate, SecondChildMutate, Nothing
                    switch (GA_intervalEnum.ToString().ToString())
                    {
                        case "GenerationNumber":
                            GA_generationsNumber = (intervalIntStart + i * stepInt);
                            ga = new GeneticAlgorithm(cg, am, GA_generationsNumber, GA_populationNumber,
                                        GA_mutationProbability, GA_weakParentRate,
                                        GA_firstChildMutate, GA_secondChildMutate);
                            break;
                        case "PopulationNumber":
                            GA_populationNumber = (intervalIntStart + i * stepInt);
                            ga = new GeneticAlgorithm(cg, am, GA_generationsNumber, GA_populationNumber,
                                        GA_mutationProbability, GA_weakParentRate,
                                        GA_firstChildMutate, GA_secondChildMutate);
                            break;
                        case "MutationProbability":
                            GA_mutationProbability = (intervalStart + i * step);
                            ga = new GeneticAlgorithm(cg, am, GA_generationsNumber, GA_populationNumber,
                                        GA_mutationProbability, GA_weakParentRate,
                                        GA_firstChildMutate, GA_secondChildMutate);
                            break;
                        case "WeakParentRate":
                            GA_weakParentRate = (intervalStart + i * step);
                            ga = new GeneticAlgorithm(cg, am, GA_generationsNumber, GA_populationNumber,
                                        GA_mutationProbability, GA_weakParentRate,
                                        GA_firstChildMutate, GA_secondChildMutate);
                            break;
                        case "FirstChildMutate":
                            if (i < algorithmRunNumber / 2)
                            {
                                GA_firstChildMutate = true;
                                ga = new GeneticAlgorithm(cg, am, GA_generationsNumber, GA_populationNumber,
                                        GA_mutationProbability, GA_weakParentRate,
                                        GA_firstChildMutate, GA_secondChildMutate);
                            }
                            else
                            {
                                GA_firstChildMutate = false;
                                ga = new GeneticAlgorithm(cg, am, GA_generationsNumber, GA_populationNumber,
                                        GA_mutationProbability, GA_weakParentRate,
                                        GA_firstChildMutate, GA_secondChildMutate);
                            }
                            break;
                        case "SecondChildMutate":
                            if (i < algorithmRunNumber / 2)
                            {
                                GA_secondChildMutate = true;
                                ga = new GeneticAlgorithm(cg, am, GA_generationsNumber, GA_populationNumber,
                                            GA_mutationProbability, GA_weakParentRate,
                                            GA_firstChildMutate, GA_secondChildMutate);
                            }
                            else
                            {
                                GA_secondChildMutate = false;
                                ga = new GeneticAlgorithm(cg, am, GA_generationsNumber, GA_populationNumber,
                                            GA_mutationProbability, GA_weakParentRate,
                                            GA_firstChildMutate, GA_secondChildMutate);
                            }
                            break;
                        case "Nothing":
                            ga = new GeneticAlgorithm(cg, am, GA_generationsNumber, GA_populationNumber,
                                        GA_mutationProbability, GA_weakParentRate,
                                        GA_firstChildMutate, GA_secondChildMutate);
                            break;
                    }

                    GA_populationNumber = ga.getPopSize();
                    
                    stopwatch.Restart();
                    coordinator.Algorithm = ga;
                    coordinator.startAlgorithm();
                    coordinator.runAlgorithmThrough();
                    result = coordinator.Algorithm.getActualResult();
                    stopwatch.Stop();
                    TimeSpan ts = stopwatch.Elapsed;

                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(outputPath, true))
                    {
                        file.WriteLine(this.algoName + "\t" + this.graphFileName + "\t" + this.agentFileName
                             + "\t" + GA_generationsNumber + "\t" + GA_populationNumber + "\t" + GA_mutationProbability
                             + "\t" + GA_weakParentRate + "\t" + GA_firstChildMutate + "\t" + GA_secondChildMutate
                             + "\t" + result + "\t" + ts.TotalMilliseconds);
                    }
                    Console.Clear();
                    Console.WriteLine("Algorithm is already {0}/{1} done.", i, algorithmRunNumber);
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





