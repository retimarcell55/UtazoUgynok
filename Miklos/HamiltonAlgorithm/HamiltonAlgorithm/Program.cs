using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HamiltonAlgorithm
{
    class Program
    {
        private const string BASE_FOLDER_LOCATION = @"C:\Users\mikilos\Documents\BME_orak\Felev05\Témalabor";
        private const string fileSource = @"\Miklos\Matrices\02.txt";

        static void Main(string[] args)
        {
            AdjacencyMatrix am = new AdjacencyMatrix(BASE_FOLDER_LOCATION + fileSource);

            am.WriteConsole();
            Console.ReadKey();
        }
    }
}
