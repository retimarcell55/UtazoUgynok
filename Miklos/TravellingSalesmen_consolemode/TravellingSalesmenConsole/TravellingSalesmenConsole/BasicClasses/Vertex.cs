using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravellingSalesmenConsole
{
    [Serializable]
    public class Vertex
    {
        private Coordinate position;
        private int id;
        private bool used;

        public int Id { get => id; set => id = value; }
        public bool Used { get => used; set => used = value; }
        public Coordinate Position { get => position; set => position = value; }

        public Vertex(int id, Coordinate position)
        {
            this.id = id;
            this.position = position;
            this.used = false;
        }
    }
}
