using System.Collections.Generic;

namespace DijkstraAndFloyd
{
    public class GraphFloyd : Graph
    {
        public int[][] pathLengthMatrix;
        public int[][] pathMatrix;

        public GraphFloyd(int[][] matrix, int startVI) : base(matrix, startVI) {
            InitializePathLengthMatrix();
            initializePathMatrix();

            for (int floydIter = 0; floydIter < n; floydIter++) {
                for (int i = 0; i < n; i++) {
                    for (int j = 0; j < n; j++) {
                        if (i != floydIter && j != floydIter) {
                            int potentiallyBetterDistance = pathLengthMatrix[i][floydIter] + pathLengthMatrix[floydIter][j];

                            if (potentiallyBetterDistance < pathLengthMatrix[i][j]) {
                                pathLengthMatrix[i][j] = potentiallyBetterDistance;
                                pathMatrix[i][j] = pathMatrix[i][floydIter];
                            }
                        }
                    }
                }
            }            
        }

        public override List<int> PathTo(int destinationVertexIndex) {
            List<int> path = new List<int> {startVertexIndex };

            int pathIndex = pathMatrix[startVertexIndex][destinationVertexIndex];

            while (pathIndex != destinationVertexIndex) {
                path.Add(pathIndex);
                pathIndex = pathMatrix[pathIndex][destinationVertexIndex];
            }
            path.Add(pathIndex);

            return path;
        }

        public override int DistanceTo(int destinationVertexIndex) {
            return pathLengthMatrix[startVertexIndex][destinationVertexIndex];
        }

        private void InitializePathLengthMatrix() {
            List<int[]> plList = new List<int[]> { };

            for (int i = 0; i < n; i++) {
                plList.Add(new int[n]);

                for (int j = 0; j < n; j++) {
                    plList[i][j] = matrix[i][j];
                }
            }

            pathLengthMatrix = plList.ToArray();
        }

        private void initializePathMatrix() {
            List<int[]> pList = new List<int[]> { };

            for (int i = 0; i < n; i++)
            {
                pList.Add(new int[n]);

                for (int j = 0; j < n; j++)
                {
                    pList[i][j] = j;
                }
            }

            pathMatrix = pList.ToArray();
        }
    }
}
