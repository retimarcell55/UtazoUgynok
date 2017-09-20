using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravellingSalesmen
{
    public class Vertex
    {
        private int id;
        private bool used;

        public int Id { get => id; set => id = value; }
        public bool Used { get => used; set => used = value; }

        public Vertex(int id)
        {
            this.id = id;
            this.used = false;
        }
    }
}
