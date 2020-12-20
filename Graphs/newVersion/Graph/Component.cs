using System.Collections.Generic;

namespace Graph
{
    public class Component
    {
        public List<Vertex> vertexList;
        public bool isBipartite = true;

        public Component(List<Vertex> vertices) {
            vertexList = vertices;
            MakeVerticesUnvisited();

            AssignMarkers(vertexList[0].PredefineMarker(1));

            if (!isBipartite) {
                CleanMarkers();
            }
        }

        public List<Vertex> GetAllVerticesWithMarker(int marker) {
            return vertexList.FindAll(vertex => vertex.marker == marker);
        }

        public void AssignMarkers(Vertex startVertex)
        {
            startVertex.isVisited = true;
            int currentMarker = startVertex.marker;

            foreach (Vertex adjVertex in startVertex.adjVertexList)
            {
                if (adjVertex.marker == 0)
                {
                    adjVertex.marker = -currentMarker;
                    continue;
                }

                if (adjVertex.marker == currentMarker)
                {
                    isBipartite = false;
                }
            }

            if (isBipartite)
            {
                List<Vertex> nextVertexList = startVertex.adjVertexList.FindAll(vertex => !vertex.isVisited);
                
                if (nextVertexList.Count > 0)
                {
                    foreach (Vertex nextVertex in nextVertexList)
                    {
                        AssignMarkers(nextVertex);
                    }
                }
            }
        }

        public void CleanMarkers() {
            foreach (Vertex vertex in vertexList)
            {
                vertex.marker = 0;
            }
        }

        public void MakeVerticesUnvisited()
        {
            foreach (Vertex vertex in vertexList)
            {
                vertex.isVisited = false;
            }
        }
    }
}
