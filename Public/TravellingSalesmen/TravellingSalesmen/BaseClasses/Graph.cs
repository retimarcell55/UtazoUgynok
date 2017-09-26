﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravellingSalesmen
{
    [Serializable]
    public class Graph
    {
        private int vertexCount;
        private int edgeCount;
        private double[,] adjacencyMatrix;
        private List<Vertex> vertices;
        private List<Edge> edges;

        public int VertexCount { get => vertexCount; set => vertexCount = value; }
        public int EdgeCount { get => edgeCount; set => edgeCount = value; }
        public double[,] AdjacencyMatrix { get => adjacencyMatrix; }
        public List<Vertex> Vertices { get => vertices; set => vertices = value; }
        public List<Edge> Edges { get => edges; set => edges = value; }

        public Graph(List<Vertex> vertices)
        {
            this.vertices = vertices;
            adjacencyMatrix = new double[vertices.Count, vertices.Count];
            vertexCount = vertices.Count;
            edgeCount = vertices.Count * vertices.Count;
            edges = new List<Edge>();
            for (int i = 0; i < vertices.Count; i++)
            {
                for (int j = 0; j < vertices.Count; j++)
                {
                    double distance = 0;
                    if(i != j)
                    {
                        distance = Math.Sqrt(Math.Pow(vertices[j].Position.X - vertices[i].Position.X, 2) + Math.Pow(vertices[j].Position.Y - vertices[i].Position.Y, 2));
                        distance = Math.Round(distance, 2);
                        if (j >= i)
                        {
                            edges.Add(new Edge(vertices[i], vertices[j], false, distance));
                        }
                    }
                    adjacencyMatrix[i, j] = distance;
                }
            }
        }

        public void AddEdge(Edge e)
        {
            Edges.Add(e);
            EdgeCount++;
        }

        public void RemoveEdge(Edge e)
        {
            Edges.Remove(e);
            EdgeCount = Edges.Count;
        }

        public void AddVertex(Vertex v)
        {
            Vertices.Add(v);
            VertexCount++;
        }

        public void RemoveVertex(Vertex v)
        {
            Vertices.Remove(v);
            VertexCount = Vertices.Count;
        }
    }
}