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

        public int Start { get => start; set => start = value; }
        public int End { get => end; set => end = value; }
        public bool Used { get => used; set => used = value; }

        public Edge(int start, int end)
        {
            Start = start;
            End = end;
            Used = false;
        }
    }
}
