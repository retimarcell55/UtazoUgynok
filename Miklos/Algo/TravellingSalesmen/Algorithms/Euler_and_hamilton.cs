using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravellingSalesmen.Algorithms
{
    class Euler_and_hamilton
    {
        Euler_and_hamilton() { }    //nem kell az osztály...

        public List<Vertex> CalculateHamiltonCircuit(List<Edge> minSpanningTree, List<Edge> perfectMaching, Graph fullOriginalGraph, int startPointId)
        {
            foreach (Edge e in minSpanningTree)
            {
                e.Used = false;
            }
            foreach (Edge e in perfectMaching)
            {
                e.Used = false;
            }
            //eddig mindent kinulláztunk
            List<Edge> combinedEdgeList = new List<Edge>();
            combinedEdgeList.AddRange(minSpanningTree);
            combinedEdgeList.AddRange(perfectMaching);

            Graph g = new Graph();
            foreach (var item in fullOriginalGraph.Vertices)
            {
                g.addVertex(item);
            }
            foreach (var item in fullOriginalGraph.Edges)
            {
                g.addEdge(item);
            }
            //nem értem teljesen még a dolgokat....


            List<Edge> circuitEdges = new List<Edge>();                 //Az euler út éleinek sorrendje
            List<Vertex> circuitVertices = new List<Vertex>();          //Az Euler kör csúcsainak a sorrendje

            List<Vertex> currentPathVertices = new List<Vertex>();      //Egy pálya a csúcsokknak, addig megy egy úton amíg talál szabad élet
            List<Edge> currentPathEdges = new List<Edge>();             //Egy pálya az éleknek, addig megy egy úton amíg talál szabad élet


            //egy randol él hozzáadása (a csúcsokat is, mindkettővel együtt fogunk számolni)
            foreach (Edge item in g.Edges)
            {
                if (item.EndVertex.Id == startPointId)
                {
                    item.Used = true;
                    currentPathVertices.Add(item.EndVertex);      //a random él vége
                    currentPathVertices.Add(item.StartVertex);    //egy random él eleje

                    break;
                }
                else if (item.StartVertex.Id == startPointId)
                {
                    item.Used = true;
                    currentPathVertices.Add(item.StartVertex);    //egy random él eleje
                    currentPathVertices.Add(item.EndVertex);      //a random él vége
                    break;
                }
            }


            int usedEdgeCount = 1;  //mert egy már van
            while (usedEdgeCount != combinedEdgeList.Count)
            {
                if (currentPathVertices[0].Equals(currentPathVertices.Last<Vertex>()))  //ha az első és az utolsó csúcs megegyezik a pályában (itt BIZTOS VAN használatlan él)
                {
                    while (true)
                    {
                        bool haveUnusedEdge = false;
                        foreach (Edge e in g.getEdgesByVertex(currentPathVertices.Last<Vertex>().Id))
                        {
                            if (e.Used == false)
                            {
                                haveUnusedEdge = true;
                                break;
                            }
                        }
                        if (haveUnusedEdge)
                        {
                            break;      //kilép a while-ból, a currentPath utolsó tagjának van használatlan éle
                        }
                        else
                        {
                            circuitVertices.Add(currentPathVertices.Last<Vertex>());    //a körhöz adjuk a pálya utolsóját
                            currentPathVertices.Remove(currentPathVertices.Last<Vertex>());     //elveszük a pályából az utolsót
                            /*
                            //élek szintjén, a circuit Vertices utolsó és utolsó előtti pontja közötti élet vettük be !!!
                            if(circuitVertices.Count > 1)   //ha van már legalább két tagja
                            {
                                foreach (Edge e in g.getEdgesByVertex( circuitVertices[circuitVertices.Count - 1].Id) )
                                {
                                    if (e.StartVertex.Equals(circuitVertices[circuitVertices.Count - 2]))   //ha az eleje az utolsó előtti akkor jó sorrendben vagyunk
                                    {
                                        circuitEdges.Add(e);
                                        break;
                                    }
                                    else if (e.EndVertex.Equals(circuitVertices[circuitVertices.Count - 2]))    //ha a vége az utolsó előtti akkor cserélünk
                                    {
                                        Vertex temp = e.EndVertex;
                                        e.StartVertex = e.EndVertex;
                                        e.StartVertex = temp;

                                        circuitEdges.Add(e);
                                        break;
                                    }
                                }
                            }*/


                        }
                    }
                }
                else        //ha a pálya első és utolsó csúcsa nem egyezik meg
                {
                    foreach (Edge e in g.getEdgesByVertex(currentPathVertices.Last<Vertex>().Id))
                    {
                        if (e.StartVertex.Equals(currentPathVertices.Last<Vertex>()) && e.Used == false)    //ha egy használatlan élet találtunk
                        {
                            currentPathVertices.Add(e.EndVertex);
                            e.Used = true;
                            usedEdgeCount++;
                            break;                          //kilépünk a foreachból, csak egy új élet kértünk
                        }
                        else if (e.EndVertex.Equals(currentPathVertices.Last<Vertex>()) && e.Used == false)
                        {
                            currentPathVertices.Add(e.StartVertex);
                            e.Used = true;
                            usedEdgeCount++;
                            break;                           //kilépünk a foreachból, csak egy új élet kértünk
                        }
                    }
                }
            }

            while (currentPathVertices.Count != 0)   //a végén a pályát fordított sorrendben hozzáadjuk a körhöz
            {
                circuitVertices.Add(currentPathVertices.Last<Vertex>());        //a körhöz adjuk a pálya utolsóját
                currentPathVertices.Remove(currentPathVertices.Last<Vertex>());         //elveszük a pályából az utolsót
                /*
                //TODO élek: ez copy-zva van fentebbről....
                if (circuitVertices.Count > 1)   //ha van már legalább két tagja
                {
                    foreach (Edge e in g.getEdgesByVertex(circuitVertices[circuitVertices.Count - 1].Id))
                    {
                        if (e.StartVertex.Equals(circuitVertices[circuitVertices.Count - 2]))   //ha az eleje az utolsó előtti akkor jó sorrendben vagyunk
                        {
                            circuitEdges.Add(e);
                            break;
                        }
                        else if (e.EndVertex.Equals(circuitVertices[circuitVertices.Count - 2]))    //ha a vége az utolsó előtti akkor cserélünk
                        {
                            Vertex temp = e.EndVertex;
                            e.StartVertex = e.EndVertex;
                            e.StartVertex = temp;

                            circuitEdges.Add(e);
                            break;
                        }
                    }
                }*/


            }
            
            

            List<int> foundedIds = new List<int>();
            List<Vertex> hamiltonVertices = new List<Vertex>();
            foreach (var item in circuitVertices)
            {
                if(! foundedIds.Contains(item.Id))
                {
                    foundedIds.Add(item.Id);
                    hamiltonVertices.Add(item);
                }
            }
            //hamiltonVertices.Add(circuitVertices[0]);
            return hamiltonVertices;


            //ennek SZÁMÍT a sorrendje, mert azt bejárva megkapjukaz euler kört
            //és már rendezve is van, hogy az euler a start-nál indul és az endnél ér majd véget....
            /*
            //TODO hamilton
            //végigfutunk a csúcsokon, amelyik többször van benne azt betesszük egy listába(kivéve az elsőt, ő kétszer kell benne legyen...)
            List<Vertex> duplicates = new List<Vertex>();
            Dictionary<int, int> countVertexId = new Dictionary<int, int>();
            countVertexId[circuitVertices[0].Id]--;     //mert az első az elején is meg a végén is benne van, de mégsem duplikált
            foreach (Vertex v in circuitVertices)
            {
                countVertexId[v.Id]++;
                if(countVertexId[v.Id] > 1)
                {
                    duplicates.Add(v);
                }
            }   

            void TakeOutVertexFromEuler(Vertex beforeVer, Vertex ver, Vertex afterVer)
            {
                Edge replacementEdge = null;
                foreach (Edge e in g.Edges)
                {
                    if ((e.EndVertex.Equals(beforeVer) && e.StartVertex.Equals(afterVer)) || (e.StartVertex.Equals(beforeVer) && e.EndVertex.Equals(afterVer))) 
                    {
                        replacementEdge = e;
                    }
                }
                for (int i = 0; i < circuitEdges.Count - 1; i++)    //nem baj nekünk a -1, úgysem az utolsó és az utolsó utáni élet kell kitörölni
                {
                    if(circuitEdges[i].StartVertex.Equals(beforeVer) && circuitEdges[i].EndVertex.Equals(ver) )
                    {
                        //ha megtaláltuk az első élet amit törölni kell (a következőt is kell), ÉS a helyére szúrni az újat
                        circuitEdges.RemoveAt(i + 1);           //előbb kitöröljük a hátsót
                        circuitEdges.RemoveAt(i);               //mad kitöröljük az elsőt
                        circuitEdges.Insert(i, replacementEdge);//végül beszúrjuk a helyükre az új elemet
                    }
                }
                //a csúcs kiszedése a csúcs-sorozatból
                for(int i = 1; i < circuitVertices.Count - 1; i++)  //i = 1-től indul mert az elsőt békénhagyja, és csak Count-1 ig mert az utolsót is békén hagyja
                {
                    if(circuitVertices[i].Id == ver.Id)
                    {
                        circuitVertices.RemoveAt(i);
                    }
                }
            };*/


        }
    }
}
