using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravellingSalesmen
{
    public class Edge
    {
        private int start;
        private int end;
        private bool used;
        private int weight;

        public int Start { get => start; set => start = value; }
        public int End { get => end; set => end = value; }
        public bool Used { get => used; set => used = value; }
        public int Weight { get => weight; set => weight = value; }

        public Edge(int start, int end, int weight)
        {
            Start = start;
            End = end;
            Weight = weight;
            Used = false;
        }
    }
}
