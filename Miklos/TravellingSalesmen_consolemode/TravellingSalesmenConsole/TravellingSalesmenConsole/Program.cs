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
            //  Thread.CurrentThread.g


            Logger logger = new Logger();

            logger.AskUser();
            logger.Run();

            Console.WriteLine("vege");
            Console.ReadKey();
        }
    }
}
