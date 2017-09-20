using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace HamiltonAlgorithm
{
    [Serializable]
    class AdjacencyMatrix
    {
        private int size;
        //private int totalWeight; (total / 2)
        private int[,] matrix;

        public int Size { get{ return size;} private set { size = value; } }

        public AdjacencyMatrix() {size=0;}
        public AdjacencyMatrix(string source)
        {
            int lines = File.ReadAllLines(source).Length;
            matrix = new int[lines, lines];
            Size = lines;

            using (StreamReader sr = new StreamReader(source))
            {
                for (int i = 0; i < lines; i++)
                {
                    String[] data = sr.ReadLine().Split(' ');
                    for (int j = 0; j < lines; j++)
                    {
                        matrix[i, j] = int.Parse(data[j]);
                    }
                }
            }
        }

        public void WriteConsole()
        {
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    Console.Write(matrix[i,j] + "\t");
                }
                Console.WriteLine();
            }
        }
    
    }
}
