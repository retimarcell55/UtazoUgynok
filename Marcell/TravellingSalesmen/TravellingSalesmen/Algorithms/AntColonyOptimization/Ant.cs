using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravellingSalesmen.Algorithms.AntColonyOptimization
{
    class Ant
    {
        private int id;
        private int startPosition;
        private int actualPosition;
        private bool stopped;
        private string visitedNodes;
        private double distanceToNextNode;
        private int nextNode;
        private double totalDistanceTravelled;

        public int Id { get => id; }
        public int StartPosition { get => startPosition; }
        public int ActualPosition { get => actualPosition; set => actualPosition = value; }
        public bool Stopped { get => stopped; set => stopped = value; }
        public string VisitedNodes { get => visitedNodes; set => visitedNodes = value; }
        public double DistanceToNextNode { get => distanceToNextNode; set => distanceToNextNode = value; }
        public int NextNode { get => nextNode; set => nextNode = value; }
        public double TotalDistanceTravelled { get => totalDistanceTravelled; set => totalDistanceTravelled = value; }

        public Ant(int id, int startPosition)
        {
            this.id = id;
            this.startPosition = startPosition;
            this.actualPosition = startPosition;
            stopped = false;
            visitedNodes = startPosition.ToString();
            distanceToNextNode = 0;
            nextNode = -1;
            totalDistanceTravelled = 0;
        }

        public void Reset()
        {
            this.actualPosition = startPosition;
            stopped = false;
            visitedNodes = startPosition.ToString();
            distanceToNextNode = 0;
            nextNode = -1;
            totalDistanceTravelled = 0;
        }
    }
}
