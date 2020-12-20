using System.Collections.Generic;

namespace DijkstraAndFloyd
{
    public abstract class Graph
    {
        public static int INFINITY = 1000000;

        public int n;
        public int startVertexIndex;
        public int[][] matrix;

        protected Graph(int[][] pathMatrix, int startVI) {
            matrix = pathMatrix;
            n = matrix.Length;
            startVertexIndex = startVI;
        }

        public abstract int DistanceTo(int destinationVertexIndex);

        public abstract List<int> PathTo(int destinationVertexIndex);
    }
}
