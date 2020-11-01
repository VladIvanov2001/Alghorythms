using System;
using System.Collections.Generic;

namespace Graphs
{
    class Vertex
    {
        public int number;
        public List<Vertex> vertexList;
        public bool isVisited;

        public Vertex(int vertexNumber)
        {
            number = vertexNumber;
            vertexList = new List<Vertex>();
            isVisited = false;
        }

        public bool VertexIsAdjacentTo(Vertex vertex)
        {
            foreach (Vertex v in vertexList)
            {
                if (vertex.number == v.number)
                {
                    return true;
                }
            }

            return false;
        }
    }

    class Edge
    {
        public Vertex startVertex;
        public Vertex endVertex;

        public Edge(Vertex vertex1, Vertex vertex2)
        {
            startVertex = vertex1;
            endVertex = vertex2;
        }

        public bool EdgeIsSimilarTo(Edge edge)
        {
            return startVertex.number == edge.startVertex.number && endVertex.number == edge.endVertex.number ||
                   startVertex.number == edge.endVertex.number && endVertex.number == edge.startVertex.number;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }
}