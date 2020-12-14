using System;
using System.Collections.Generic;

namespace Dijkstra
{
    public class DijkstraPathFinder
    {
        class Vertex
        {
            public int number;
            public List<Vertex> adjacentVerticeList;
            public List<int> pathToAdjacentVerticesList;

            public Vertex(int vertexNumber)
            {
                number = vertexNumber;
                adjacentVerticeList = new List<Vertex>();
                pathToAdjacentVerticesList = new List<int>();
            }
        }

        class Graph
        {
            public Graph()
            {
            }
        }
    }
}