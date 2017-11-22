﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Mtsp
    {
        public List<List<Vertex>> MultiTravel(int numberoftravelers, int size, List<Vertex> list, int l, int r)
        {
           
            List<List<Vertex>> hamcircles = new List<List<Vertex>>();
            permute(list, l, r, hamcircles);

            List<List<Vertex>> optimal = new List<List<Vertex>>();
            int[] travelers = new int[numberoftravelers];
            for (int i = 0; i < numberoftravelers; i++)
            {
                travelers[i] = i + 1;
            }
            int[] indexes = new int[size];
          
            int total = (int)Math.Pow(numberoftravelers, size);

            int[] snapshot = new int[size];

            while (total-- > 0)
            {
                for (int i = 0; i < size; i++)
                {
                    
                    snapshot[i] = travelers[indexes[i]];
                    //Console.Write(snapshot[i] + " ");
                }
                //Console.WriteLine();
                //A két permutáció találkozása
                List<List<Vertex>> tmp = new List<List<Vertex>>();
                //Végig az összes hamkörön
                for (int i = 0; i < hamcircles.Count; i++)
                {
                    tmp.Clear();
                    //Szétoszom a pontokat az utazók között
                    for (int j = 1; j <= numberoftravelers; j++)
                    {
                        //Kezdőpontot minden utazó listába belerakom
                        List<Vertex> rout = new List<Vertex>();
                        rout.Add(hamcircles[i][0]);
                        //külön listába az egyes utak
                        for (int k = 0; k < snapshot.Length; k++)
                        {
                            //Console.Write(snapshot[k]+ " ");
                            if (snapshot[k] == j)
                            {
                                if (hamcircles[i][0].Id != hamcircles[i][k].Id)
                                {
                                    
                                    rout.Add(hamcircles[i][k]);
                                    
                                }
                                
                            }

                        }
                        /*int []pr= new int[] { 2, 2, 1, 1, 2, 2, 1, 1 };
                        int jo = 0;
                        for(int a=0;a<8;a++)
                        {
                            if(pr[a]==snapshot[a])
                            {
                               jo++;
                            }
                        }
                        if(jo==8 && i==5)
                        {
                            //Kiirom az aktális gráf bejárást (2szer fogja, mert 2 route lesz)
                            for (int hm = 0; hm < hamcircles[5].Count; hm++)
                            {
                                Console.Write(hamcircles[5][hm].Id + " ");
                            }
                            Console.WriteLine();
                            ///Kiirom a két routeot
                            for (int rci = 0; rci < rout.Count; rci++)
                            {
                                Console.Write(rout[rci].Id+ " ");
                            }
                            Console.WriteLine();
                        }*/

                        tmp.Add(rout);
                    }
                    /*if (tmp[0].Count > 1 && tmp[1].Count > 1)
                    {
                        foreach (var item in tmp)
                        {
                            foreach (var tem in item)
                            {
                                Console.Write(tem.Id + " ");
                            }
                            Console.WriteLine();
                        }
                    }*/

                    if (optimal.Count == 0)
                    {
                        
                        optimal = new List<List<Vertex>>(tmp);
                    }
                    else
                    {

                        double optimalWeight = 0;
                        double tmpWeight = 0;
                        for (int j = 0; j < optimal.Count; j++)
                        {
                            optimalWeight += SumWeight(optimal[j]);
                            //tmpWeight += SumWeight(tmp[j]);
                        }
                        for (int j = 0; j < tmp.Count; j++)
                        {
                            
                            tmpWeight += SumWeight(tmp[j]);
                        }
                        if (tmpWeight<15)
                        {
                            /*
                            foreach (var item in tmp)
                            {
                                foreach (var tem in item)
                                {
                                    Console.Write(tem.Id + " ");
                                }
                                Console.WriteLine();
                            }*/
                           /* for (int kk = 0; kk < size; kk++)
                            {
                                Console.Write(snapshot[kk] + " ");
                            }
                            Console.WriteLine();*/

                        }

                       /* if (optimalWeight >= tmpWeight)
                        {
                           Console.WriteLine(tmpWeight);
                            optimal.Clear();
                            optimal =new List<List<Vertex>>(tmp);
                            
                        }*/
                        if(LongestRoute(tmp)<LongestRoute(optimal))
                        {
                            optimal.Clear();
                            optimal = new List<List<Vertex>>(tmp);
                        }
                    }

                }

                for (int i = 0; i < size; i++)
                {
                    if (indexes[i] >= numberoftravelers - 1)
                    {
                        indexes[i] = 0;
                    }
                    else
                    {
                        indexes[i]++;
                        break;
                    }
                }
            }
            
            return optimal;
            
        }
        public void Kiir(int[] snapshot)
        {
            for (int i = 0; i < snapshot.Length; i++)
            {
                Console.Write(snapshot[i]);

            }
            Console.WriteLine();

        }
        public double SumWeight(List<Vertex> l)
        {
            double sumweight = 0;
            for (int i = 0; i < l.Count - 1; i++)
            {
                sumweight += Math.Sqrt(Math.Pow(l[i + 1].Position.X - l[i].Position.X, 2) + Math.Pow(l[i + 1].Position.Y - l[i].Position.Y, 2));
            }
            //sumweight += Math.Sqrt(Math.Pow(l[l.Count - 1].Position.X - l[0].Position.X, 2) + Math.Pow(l[l.Count - 1].Position.Y - l[0].Position.Y, 2));

            return sumweight;
        }
        public double LongestRoute(List<List<Vertex>> l)
        {
            double longestroute = 0;
            foreach (var item in l)
            {
                if(SumWeight(item)>longestroute)
                {
                    longestroute = SumWeight(item);
                }
            }
            return longestroute;
        }
        private void permute(List<Vertex> list, int l, int r, List<List<Vertex>> hamcircles)
        {
            if (l == r)
            {
                List<Vertex> tmp = new List<Vertex>(list);
                hamcircles.Add(tmp);
            }
            else
            {
                for (int i = l; i <= r; i++)
                {
                    list = swap(list, l, i);
                    permute(list, l + 1, r, hamcircles);
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

    }
}