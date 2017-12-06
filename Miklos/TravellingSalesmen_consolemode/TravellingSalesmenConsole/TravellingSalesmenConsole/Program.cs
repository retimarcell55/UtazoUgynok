using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TravellingSalesmenConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            bool runAgain = true;

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

            while (runAgain)
            {
                Logger logger = new Logger();
                logger.AskUser();
                logger.Run();

                bool goodKey = false;
                while (!goodKey)
                {
                    Console.Clear();
                    Console.WriteLine("Done.\nRun another algoritmh? (y/n)");
                    ConsoleKeyInfo keyInfo = Console.ReadKey();
                    if (keyInfo.Key.ToString() == "N" || keyInfo.Key.ToString() == "n")
                    {
                        runAgain = false;
                        goodKey = true;
                    }
                    else if(keyInfo.Key.ToString() == "Y" || keyInfo.Key.ToString() == "y")
                    {
                        runAgain = true;
                        goodKey = true;
                    }
                    else
                    {
                        goodKey = false;
                    }
                }
            }

        }
    }
}
