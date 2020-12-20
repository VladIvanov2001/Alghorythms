using System.Collections.Generic;

namespace DijkstraAndFloyd
{
    public class GraphDijkstra : Graph
    {
        public List<List<Pair>> adjacencyLists = new List<List<Pair>>();
        public List<int> distancesToOtherVertices = new List<int>();
        public List<int> predecessors = new List<int>();
        public List<bool> visitMarkers = new List<bool>();

        public GraphDijkstra(int[][] matrix, int startVI) : base(matrix, startVI) {
            adjacencyLists = MatrixToAdjacencyLists(matrix);

            for (int i = 0; i < n; i++) {
                distancesToOtherVertices.Add(INFINITY);
                predecessors.Add(-1);
                visitMarkers.Add(false);
            }

            distancesToOtherVertices[startVertexIndex] = 0;

            for (int i = 0; i < n; i++) {
                int indexOfNearestVertex = -1;

                for (int j = 0; j < n; j++) {
                    if (!visitMarkers[j] && (indexOfNearestVertex == -1 || distancesToOtherVertices[j] < distancesToOtherVertices[indexOfNearestVertex])) {
                        indexOfNearestVertex = j;
                    }
                }

                if (distancesToOtherVertices[indexOfNearestVertex] == INFINITY) {
                    break;
                }

                visitMarkers[indexOfNearestVertex] = true;

                foreach (Pair pair in adjacencyLists[indexOfNearestVertex]) {
                    int adjVertexIndex = pair.vertexNumber;
                    int distToAdjVerex = pair.distanceToVertex;

                    int potentiallyBetterDistance = distancesToOtherVertices[indexOfNearestVertex] + distToAdjVerex;

                    if (potentiallyBetterDistance < distancesToOtherVertices[adjVertexIndex]) {
                        distancesToOtherVertices[adjVertexIndex] = potentiallyBetterDistance;
                        predecessors[adjVertexIndex] = indexOfNearestVertex;
                    }
                }
            }
        }

        public override List<int> PathTo(int destinationVertexIndex) {
            List<int> path = new List<int>();
            int pathIndex = destinationVertexIndex;

            while (pathIndex != startVertexIndex) {
                path.Add(pathIndex);
                pathIndex = predecessors[pathIndex];
            }

            path.Add(pathIndex);
            path.Reverse();

            return path;
        }

        public override int DistanceTo(int destinationVertexIndex) {
            return distancesToOtherVertices[destinationVertexIndex];
        }

        private List<List<Pair>> MatrixToAdjacencyLists(int[][] matrix)
        {
            int n = matrix.Length;

            for (int i = 0; i < n; i++)
            {
                adjacencyLists.Add(new List<Pair> { });
            }

            for (int i = 0; i < n - 1; i++)
            {
                for (int j = i + 1; j < n; j++)
                {
                    if (matrix[i][j] < GraphDijkstra.INFINITY)
                    {
                        adjacencyLists[i].Add(new Pair(j, matrix[i][j]));
                        adjacencyLists[j].Add(new Pair(i, matrix[i][j]));
                    }
                }
            }

            return adjacencyLists;
        }
    }
}
