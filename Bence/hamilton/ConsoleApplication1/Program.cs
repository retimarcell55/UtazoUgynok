using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        public List<Vertex> MinWeightHamiltonCircle(List<Vertex> list, int l, int r)
        {
            return getMinHam(list, l, r)[0];
        }
        public List<List<Vertex>> getMinHam(List<Vertex> list, int l, int r)
        {
            List<List<Vertex>> minhamcircle = new List<List<Vertex>>();
            double minweight = 0;
            permute(list, l, r, minhamcircle, minweight);

            return minhamcircle;
        }
        private void permute(List<Vertex> list, int l, int r, List<List<Vertex>> minhamcircle, double minweight)
        {
            if (l == r)
            {
                List<Vertex> tmp = new List<Vertex>(list);
                minhamcircle.Add(tmp);

                if (minhamcircle.Count == 1)
                {




                }
                else
                {
                    for (int i = 0; i < minhamcircle[0].Count - 1; i++)
                    {

                        minweight += Math.Sqrt(Math.Pow(minhamcircle[0][i + 1].Position.X - minhamcircle[0][i].Position.X, 2) + Math.Pow(minhamcircle[0][i + 1].Position.Y - minhamcircle[0][i].Position.Y, 2));
                    }
                    minweight += Math.Sqrt(Math.Pow(minhamcircle[0][minhamcircle[0].Count - 1].Position.X - minhamcircle[0][0].Position.X, 2) + Math.Pow(minhamcircle[0][minhamcircle[0].Count - 1].Position.Y - minhamcircle[0][0].Position.Y, 2));

                    Console.Write(minweight);
                    Console.Write(" ");
                    double d = 0;
                    for (int i = 0; i < minhamcircle[minhamcircle.Count - 1].Count - 1; i++)
                    {

                        d += Math.Sqrt(Math.Pow(minhamcircle[1][i + 1].Position.X - minhamcircle[1][i].Position.X, 2) + Math.Pow(minhamcircle[1][i + 1].Position.Y - minhamcircle[1][i].Position.Y, 2));
                    }
                    d += Math.Sqrt(Math.Pow(minhamcircle[1][minhamcircle[1].Count - 1].Position.X - minhamcircle[1][0].Position.X, 2) + Math.Pow(minhamcircle[1][minhamcircle[1].Count - 1].Position.Y - minhamcircle[1][0].Position.Y, 2));
                    Console.WriteLine(d);
                    if (d <= minweight)
                    {
                        minhamcircle.RemoveAt(0);
                    }
                    else
                    {
                        minhamcircle.RemoveAt(1);
                    }




                }


            }
            else
            {
                for (int i = l; i <= r; i++)
                {
                    list = swap(list, l, i);
                    permute(list, l + 1, r, minhamcircle, minweight);
                    list = swap(list, l, i);
                }
            }
        }
        public List<Vertex> swap(List<Vertex> a, int i, int j)
        {
            Vertex temp;
            List<Vertex> list;
            list = a;
            temp = a[i];
            list[i] = list[j];
            list[j] = temp;
            return list;
        }
        static void Main(string[] args)
        {
            
            
            Vertex a = new Vertex(1, new Coordinate(1, 1));
            Vertex b = new Vertex(2, new Coordinate(7, 7));
            Vertex c = new Vertex(3, new Coordinate(2, 2));
            Vertex d = new Vertex(4, new Coordinate(3, 2));
            Vertex e = new Vertex(5, new Coordinate(1, 2));
            Vertex f = new Vertex(6, new Coordinate(4, 3));
            Vertex g = new Vertex(7, new Coordinate(8, 2));
            Vertex h = new Vertex(8, new Coordinate(4, 4));

            List<Vertex> l = new List<Vertex>();
            l.Add(a);
            l.Add(b);
            l.Add(c);
            l.Add(d);
            l.Add(e);
            l.Add(f);
            l.Add(g);
            l.Add(h);
            /*
            Program p = new Program();

            List<Vertex> vl=p.MinWeightHamiltonCircle(l, 1, l.Count - 1);
            foreach (var item in vl)
            {
                Console.WriteLine(item.Id);
            }*/

            Vertex a1 = new Vertex(1, new Coordinate(1000, 13000));
            Vertex b1 = new Vertex(2, new Coordinate(6000, 13000));
            Vertex c1 = new Vertex(3, new Coordinate(2800, 17666));
            Vertex d1 = new Vertex(4, new Coordinate(1000, 1000));
            Vertex e1 = new Vertex(5, new Coordinate(2500, 16000));
            Vertex f1 = new Vertex(6, new Coordinate(3000, 17000));
            Vertex g1 = new Vertex(7, new Coordinate(2000, 15000));
            Vertex h1 = new Vertex(8, new Coordinate(1000, 15000));

            List<Vertex> negyes = new List<Vertex>();
            negyes.Add(a1);
            negyes.Add(b1);
            negyes.Add(c1);
            negyes.Add(d1);
            negyes.Add(e1);
            negyes.Add(f1);
            negyes.Add(g1);
            negyes.Add(h1);


            Mtsp p = new Mtsp();
            List<List<Vertex>> result = new List<List<Vertex>>();
            result=p.MultiTravel(2,negyes.Count,negyes,1,negyes.Count-1);
            Console.WriteLine("Vegeredmeny:");
            foreach (var item in result)
            {
                foreach (var i in item)
                {
                    Console.Write(i.Id+ " ");
                }
                Console.WriteLine();
            }
            Console.ReadKey();
        }
    }
}
