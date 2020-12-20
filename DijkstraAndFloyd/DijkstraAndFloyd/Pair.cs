using System.Collections.Generic;

namespace DijkstraAndFloyd
{
    public struct Pair
    {
        public int vertexNumber;
        public int distanceToVertex;

        public Pair(int vNumber, int distToV)
        {
            vertexNumber = vNumber;
            distanceToVertex = distToV;
        } 
    }
}
