using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        List<Vertex> Minham = new List<Vertex>();
        private List<Vertex> permute(List<Vertex> list, int l, int r)
        {
            if (l == r)
            {
                double distance = 0;
                for (int i = 1; i < list.Count; i++)
                {
                    distance += Math.Sqrt(Math.Pow(list[i].Position.X - list[i - 1].Position.X, 2) + Math.Pow(list[i].Position.Y - list[i - 1].Position.Y, 2)); 
                }
                Minham = list;
            }
            else
            {
                for (int i = l; i <= r; i++)
                {
                    list = swap(list, l, i);
                    permute(list, l + 1, r);
                    list = swap(list, l, i);
                }
            }
            return Minham;
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
           // Program a = new Program();
           /* Graph g = new Graph();
            a.permute(g.Vertices,1,g.VertexCount);*/
            Console.ReadKey();
        }
    }
}
