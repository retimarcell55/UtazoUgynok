using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algo
{
    [Serializable]
    public class Agent
    {
        private int id;
        private int startPosition;
        private int actualPosition;

        public int Id { get => id; }
        public int StartPosition { get => startPosition; }
        public int ActualPosition { get => actualPosition; set => actualPosition = value; }

        public Agent(int id, int startPosition)
        {
            this.id = id;
            this.startPosition = startPosition;
            this.actualPosition = startPosition;
        }
    }
}
