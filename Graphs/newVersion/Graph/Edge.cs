using System.Collections.Generic;

namespace Graph
{
    public class Edge
    {
        public Vertex startVertex;
        public Vertex endVertex;

        public Edge(Vertex vertex1, Vertex vertex2)
        {
            startVertex = vertex1;
            endVertex = vertex2;
        }

        public bool IsSimilarTo(Edge edge)
        {
            return startVertex.number == edge.startVertex.number && endVertex.number == edge.endVertex.number ||
                   startVertex.number == edge.endVertex.number && endVertex.number == edge.startVertex.number;
        }

        public bool IsIn(List<Edge> edgeList)
        {
            foreach (Edge edge in edgeList)
            {
                if (IsSimilarTo(edge))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
