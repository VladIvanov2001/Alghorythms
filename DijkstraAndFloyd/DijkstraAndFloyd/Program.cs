using System.Collections.Generic;
using System.Linq;
using System;

namespace DijkstraAndFloyd
{
    class Program
    {
        public static void PreprocessMatrix(int[][] matrix) {
            int n = matrix.Length;

            for (int i = 0; i < n - 1; i++)
            {
                for (int j = i + 1; j < n; j++)
                {
                    matrix[i][j] = matrix[j][i] = Math.Min(matrix[i][j], matrix[j][i]);
                }
            }
        }

        public static int bestVertexToPlaceFireStation(int[][] matrix) {
            int n = matrix.Length;

            int bestVertexIndex = -1;
            int minBiggestDistance = Graph.INFINITY;

            for (int i = 0; i < n; i++) {
                GraphDijkstra graphWithIStartVertexIndex = new GraphDijkstra(matrix, i);
                // or
                // GraphFloyd graphWithIStartVertexIndex = new GraphFloyd(matrix, i);

                List<int> allDistances = new List<int>();

                for (int j = 0; j < n; j++) {
                    if (j != i) {
                        allDistances.Add(graphWithIStartVertexIndex.DistanceTo(j));
                    }
                }

                int biggestDistance = allDistances.Max();
                Console.WriteLine($"{i}, {biggestDistance}");

                if (biggestDistance < minBiggestDistance) {
                    bestVertexIndex = i;
                    minBiggestDistance = biggestDistance;
                }
            }

            return bestVertexIndex;
        }

        static void Main(string[] args)
        {
            int infinity = Graph.INFINITY;

            int[][] matrix = new int[][] {
                new int[] {infinity, 2, infinity, 6, infinity, 7, infinity },
                new int[] { infinity, infinity, 4, 3, infinity, 2, infinity },
                new int[] {infinity, infinity, infinity, infinity, 3, infinity, 5 },
                new int[] {infinity, infinity, infinity, infinity, 2, 1, infinity },
                new int[] {infinity, infinity, infinity, infinity, infinity, infinity, 2 },
                new int[] {7, infinity, 1, infinity, 2, infinity, 6 },
                new int[] {infinity, infinity, infinity, infinity, infinity, infinity, infinity},
            };

            PreprocessMatrix(matrix);

            // Dijkstra

            GraphDijkstra graph = new GraphDijkstra(matrix, 0);
            int destVertexIndex = 6;

            Console.WriteLine($"Path to {destVertexIndex}");

            foreach (int pathPoint in graph.PathTo(destVertexIndex)) {
                Console.Write($"{pathPoint} ");
            }

            Console.WriteLine($"\nDistance: {graph.DistanceTo(destVertexIndex)}");
            
            // Floyd

            GraphFloyd graphF = new GraphFloyd(matrix, 0);
            Console.WriteLine($"Path to {destVertexIndex}");

            foreach (int pathPoint in graphF.PathTo(destVertexIndex))
            {
                Console.Write($"{pathPoint} ");
            }

            Console.WriteLine($"\nDistance: {graphF.DistanceTo(destVertexIndex)}");

            //most optimal fire station location

            Console.WriteLine($"{bestVertexToPlaceFireStation(matrix)} is the best vertex to place a fire station at\n\n");
        }
    }
}
