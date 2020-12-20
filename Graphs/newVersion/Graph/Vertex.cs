using System.Collections.Generic;

namespace Graph
{
    public class Vertex
    {
        public int number;
        public List<Vertex> adjVertexList = new List<Vertex>();
        public bool isVisited = false;
        public int marker = 0;

        public Vertex(int vertexNumber)
        {
            number = vertexNumber;
        }

        public bool IsEven()
        {
            return adjVertexList.Count % 2 == 0;
        }

        public bool IsDeadEnd(List<Edge> unvisitedEdgesList)
        {
            foreach (Vertex adjVertex in adjVertexList)
            {
                if (new Edge(this, adjVertex).IsIn(unvisitedEdgesList))
                {
                    return false;
                }
            }

            return true;
        }

        public Vertex PredefineMarker(int marker)
        {
            this.marker = marker;

            return this;
        }
    }
}
