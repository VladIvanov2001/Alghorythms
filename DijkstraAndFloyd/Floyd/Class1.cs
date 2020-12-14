using System;
using System.Collections.Generic;

namespace Floyd
{
    public class FloydPathFinder
    {
        private int n;
        private List<List<int>> pathLengthMatrix;
        private List<List<int>> pathMatrix;
        
        public FloydPathFinder(List<List<int>> matrix)
        {
            n = matrix.Count;

            TransformPathLengthMatrixForFireTrucks(matrix);
            FormPathAndPathLengthMatrices(pathLengthMatrix, pathMatrix);
        }

        public void TransformPathLengthMatrixForFireTrucks(List<List<int>> pathLengthMatrix)
        {
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    int minPossiblePathLength = Math.Min(pathLengthMatrix[i][j], pathLengthMatrix[j][i]);

                    pathLengthMatrix[i][j] = minPossiblePathLength;
                    pathLengthMatrix[j][i] = minPossiblePathLength;
                }
            }
        }

        public void FormPathAndPathLengthMatrices(List<List<int>> pathLengthMatrix, List<List<int>> pathMatrix)
        {
        }
    }
}