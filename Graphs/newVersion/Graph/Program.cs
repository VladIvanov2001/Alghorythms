using System;
using System.Collections.Generic;
using System.Linq;

namespace Graph
{
    class Program
    {
        public static List<int[]> ComposeAdjacenciesList(int[] raveledArray)
        {
            List<int[]> adjList = new List<int[]>();

            for (int i = 0; i < raveledArray.Length / 2; i++)
            {
                adjList.Add(new []{raveledArray[2 * i], raveledArray[2 * i + 1]});
            }

            return adjList;
        }

        static void Main(string[] args)
        {
            // euler cycle
            
            int[] raveledList = {0, 1, 0, 2, 0, 3, 2, 1, 2, 3, 0, 4, 2, 4};
            int n = 5;
            
            List<int[]> adjacenciesList = ComposeAdjacenciesList(raveledList);
            Graph graph = new Graph(adjacenciesList, n);

            List<Vertex> cycle = graph.EulerCycle();

            if (cycle != null)
            {
                Console.WriteLine("euler cycle:");
                
                foreach (Vertex vertex in cycle)
                {
                    Console.WriteLine(vertex.number);
                }
            }
            else
            {
                Console.WriteLine("graph is not euler");
            }

            Console.WriteLine("=====");
            
            // bipartite graph

            raveledList = new[] {0, 1, 0, 3, 1, 2, 1, 4, 2, 3, 3, 4};
            n = 5;

            adjacenciesList = ComposeAdjacenciesList(raveledList);
            graph = new Graph(adjacenciesList, n);

            List<List<Vertex>> bipartiteParts = graph.BipartiteParts();

            if (bipartiteParts != null)
            {
                Console.WriteLine("first bipartite part:");

                foreach (Vertex vertex in bipartiteParts[0])
                {
                    Console.WriteLine(vertex.number);
                }
                
                Console.WriteLine("second bipartite part:");

                foreach (Vertex vertex in bipartiteParts[1])
                {
                    Console.WriteLine(vertex.number);
                }
            }
            else
            {
                Console.WriteLine("graph is not bipartite");
            }
        }
    }
}