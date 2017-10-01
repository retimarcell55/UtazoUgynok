using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravellingSalesmen
{
    //https://hu.wikipedia.org/wiki/Prim-algoritmus
    class MinimumSpanningTree
    {
        private int vertexCount;
        private double maxEdgeWeight;
        private List<Vertex> treeVertices;      //Vertices of the tree
        private List<Vertex> nonTreeVertices;   //vertices of non tree vertices
        private List<Edge> treeEdges;           //all edges
        private List<Edge> intermediateEdges;   //between the treeVerticles and nonTreeVerticles
        private Graph originalGraph;

        public MinimumSpanningTree(Graph g)
        {
            originalGraph = g;
            treeVertices = new List<Vertex>();
            treeVertices.Add(originalGraph.Vertices[0]);        //add a random vertex
            nonTreeVertices = new List<Vertex>();
            nonTreeVertices = originalGraph.Vertices;
            nonTreeVertices.Remove(originalGraph.Vertices[0]);
            treeEdges = new List<Edge>();
            treeEdges = originalGraph.Edges;
            intermediateEdges = new List<Edge>();
            maxEdgeWeight = 0;
            foreach (Edge e in treeEdges)   //calc max weight
            {
                if (e.Weight > maxEdgeWeight)
                    maxEdgeWeight = e.Weight;
            }

        }

        public Graph CalculateMinimumSpanningTree()
        {
            while(nonTreeVertices.Count > 0)
            {
                RecalcIntermediateEdges();
                Edge min = SearchMinimumWeightIntermediateEdge();
                foreach (Vertex v in treeVertices)
                {
                    if(min.StartVertex.Equals(v))
                    {
                        MoveVertex(min.EndVertex);
                        break;
                    }
                    if(min.EndVertex.Equals(v))
                    {
                        MoveVertex(min.StartVertex);
                        break;
                    }
                }
            }

            Graph minimumSpanningTree = new Graph();
            minimumSpanningTree.Edges = treeEdges;
            minimumSpanningTree.Vertices = treeVertices;
            return minimumSpanningTree;// TODO
        }

        private void MoveVertex(Vertex v)
        {
            if(nonTreeVertices.Count > 0)
            {
                nonTreeVertices.Remove(v);
                treeVertices.Add(v);
            }
        }

        private void RecalcIntermediateEdges()
        {
            intermediateEdges.Clear();

            foreach (Vertex treeV in treeVertices)
            {
                foreach (Vertex v in nonTreeVertices)
                {
                    foreach (Edge e in originalGraph.Edges)
                    {
                        if( (e.StartVertex.Equals(treeV) && e.EndVertex.Equals(v)) || (e.StartVertex.Equals(v) && e.EndVertex.Equals(treeV)) )
                        {
                            intermediateEdges.Add(e);
                        }
                    }
                }
            }
        }

        private Edge SearchMinimumWeightIntermediateEdge()
        {
            double minimumWeight = maxEdgeWeight + 1;   //all edge is lower !
            Edge temp = null;
            foreach (Edge e in intermediateEdges)
            {
                if (e.Weight < minimumWeight)
                {
                    minimumWeight = e.Weight;
                    temp = e;
                }
            }

            return temp;
        }



    }
}
