using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        public List<Vertex> getMinHam(List<Vertex> list, int l, int r)
        {
            List<Vertex> minhamcircle = new List<Vertex>();
            double minweight = new double();
            permute(list,l,r,minhamcircle,minweight);

            return minhamcircle;
        }
        private void permute(List<Vertex> list, int l, int r, List<Vertex> minhamcircle, double minweight)
        {
            if (l == r)
            {
                if(minhamcircle.Count==0)
                {
                    minhamcircle = list;
                    for (int i = 1; i < list.Count; i++)
                    {
                        minweight += Math.Sqrt(Math.Pow(list[i].Position.X - list[i - 1].Position.X, 2) + Math.Pow(list[i].Position.Y - list[i - 1].Position.Y, 2));
                    }
                }
                else
                {
                    double d = 0;
                    for (int i = 1; i < list.Count; i++)
                    {
                        d += Math.Sqrt(Math.Pow(list[i].Position.X - list[i - 1].Position.X, 2) + Math.Pow(list[i].Position.Y - list[i - 1].Position.Y, 2));
                    }
                    if(d<minweight)
                    {
                        minhamcircle = list;
                    }
                }

            }
            else
            {
                for (int i = l; i <= r; i++)
                {
                    list = swap(list, l, i);
                    permute(list, l + 1, r,minhamcircle, minweight);
                    list = swap(list, l, i);
                }
            }  
        }
        public List<Vertex> swap(List<Vertex> a, int i, int j)
        {
            Vertex temp;
            List<Vertex> list = new List<Vertex>();
            list = a;
            temp = a[i];
            list[i] = list[j];
            list[j] = temp;
            return list;
        }
        static void Main(string[] args)
        {
           /* Program a = new Program();
            Graph g = new Graph();
            List<Vertex> vertices= a.getMinHam(g.Vertices,1,g.Vertices.Count); */
            Console.ReadKey();
        }
    }
}
