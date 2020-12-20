using System;
using System.Collections.Generic;

namespace Graph
{
    public class Graph
    {
        public List<Vertex> vertexList = new List<Vertex>();
        public List<Edge> edgeList = new List<Edge>();
        public List<Component> componentList;

        public Graph(List<int[]> adjList, int n)
        {
            for (int i = 0; i < n; i++)
            {
                vertexList.Add(new Vertex(i));
            }

            foreach (int[] adjItem in adjList)
            {
                vertexList[adjItem[0]].adjVertexList.Add(vertexList[adjItem[1]]);
                vertexList[adjItem[1]].adjVertexList.Add(vertexList[adjItem[0]]);

                Edge newEdge = new Edge(vertexList[adjItem[0]], vertexList[adjItem[1]]);
                edgeList.Add(newEdge);
            }

            componentList = GetComponents();
        }

        public int edgeIndexByTwoVertices(Vertex v1, Vertex v2, List<Edge> edges)
        {
            Edge edge = new Edge(v1, v2);

            for (int i = 0; i < edges.Count; i++)
            {
                if (edges[i].IsSimilarTo(edge))
                {
                    return i;
                }
            }

            return -1;
        }

        public Vertex FirstUnvisitedVertex()
        {
            foreach (Vertex vertex in vertexList)
            {
                if (vertex.isVisited == false)
                {
                    return vertex;
                }
            }

            return null;
        }

        public bool HasAllEvenDegrees()
        {
            foreach (Vertex vertex in vertexList)
            {
                if (!vertex.IsEven())
                {
                    return false;
                }
            }

            return true;
        }

        public void FormComponentVertexListRecursively(Vertex vertex, List<Vertex> vertexList)
        {
            vertexList.Add(vertex);
            vertex.isVisited = true;

            foreach (Vertex adjVertex in vertex.adjVertexList)
            {
                if (!adjVertex.isVisited)
                {
                    FormComponentVertexListRecursively(adjVertex, vertexList);
                }
            }
        }

        public Component GetComponent(Vertex vertex)
        {
            List<Vertex> componentVertexList = new List<Vertex>();

            FormComponentVertexListRecursively(vertex, componentVertexList);

            return new Component(componentVertexList);
        }

        public List<Component> GetComponents()
        {
            List<Component> components = new List<Component>();
            Vertex firstUnvisitedVertex = FirstUnvisitedVertex();

            while (firstUnvisitedVertex != null)
            {
                components.Add(GetComponent(firstUnvisitedVertex));
                firstUnvisitedVertex = FirstUnvisitedVertex();
            }

            MakeVerticesUnvisited();

            return components;
        }

        public void MakeVerticesUnvisited()
        {
            foreach (Vertex vertex in vertexList)
            {
                vertex.isVisited = false;
            }
        }

        public bool IsEuler()
        {
            if (!HasAllEvenDegrees())
            {
                return false;
            }

            int filledComponents = 0;

            foreach (Component component in componentList)
            {
                if (component.vertexList.Count > 1)
                {
                    filledComponents += 1;
                }
            }

            return filledComponents < 2;
        }

        public List<Vertex> EulerCycle()
        {
            if (IsEuler())
            {
                int currentVertexIndex = 0;

                if (!HasAllEvenDegrees())
                {
                    while (vertexList[currentVertexIndex].IsEven())
                    {
                        currentVertexIndex++;
                    }
                }

                Vertex currentVertex = vertexList[currentVertexIndex];

                List<Edge> unvisitedEdges = new List<Edge>(edgeList);
                List<Vertex> eulerCycle = new List<Vertex>();

                Stack<Vertex> vertexStack = new Stack<Vertex>();
                vertexStack.Push(currentVertex);

                while (unvisitedEdges.Count > 0)
                {
                    if (currentVertex.IsDeadEnd(unvisitedEdges))
                    {
                        Console.WriteLine("is dead end");
                        while (vertexStack.Peek().IsDeadEnd(unvisitedEdges))
                        {
                            eulerCycle.Add(vertexStack.Pop());
                            currentVertex = vertexStack.Peek();
                        }
                    }
                    else
                    {
                        foreach (Vertex adjVertex in currentVertex.adjVertexList)
                        {
                            int edgeIndex = edgeIndexByTwoVertices(currentVertex, adjVertex, unvisitedEdges);

                            if (edgeIndex != -1)
                            {
                                Edge nextEdge = unvisitedEdges[edgeIndex];
                                currentVertex = adjVertex;
                                vertexStack.Push(currentVertex);
                                unvisitedEdges.Remove(nextEdge);

                                break;
                            }
                        }
                    }
                }

                while (vertexStack.Count > 0)
                {
                    eulerCycle.Add(vertexStack.Pop());
                }

                return eulerCycle;
            }

            return null;
        }

        public bool IsBipartite()
        {
            return componentList.TrueForAll(component => component.isBipartite);
        }

        public List<List<Vertex>> BipartiteParts()
        {
            if (IsBipartite())
            {
                List<Vertex> firstPart = new List<Vertex>();
                List<Vertex> secondPart = new List<Vertex>();

                foreach (Component component in componentList)
                {
                    firstPart.AddRange(component.GetAllVerticesWithMarker(1));
                    secondPart.AddRange(component.GetAllVerticesWithMarker(-1));
                }

                return new List<List<Vertex>>(new[] { firstPart, secondPart });
            }

            return null;
        }
    }
}
