using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Graph
{
    class Vertex
    {
        public int number;
        public List<Vertex> adjVertexList;
        public bool isVisited;

        public Vertex(int vertexNumber)
        {
            number = vertexNumber;
            adjVertexList = new List<Vertex>();
            isVisited = false;
        }

        public bool IsEven()
        {
            return adjVertexList.Count % 2 == 0;
        }

        public bool IsAdjacentTo(Vertex vertex)
        {
            foreach (Vertex v in adjVertexList)
            {
                if (vertex.number == v.number)
                {
                    return true;
                }
            }

            return false;
        }

        public bool IsIn(List<Vertex> vertexList)
        {
            foreach (Vertex vertex in vertexList)
            {
                if (number == vertex.number)
                {
                    return true;
                }
            }

            return false;
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
    }

    class Edge
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

    class Graph
    {
        public List<Vertex> vertexList;
        public List<Edge> edgeList;

        public Graph(List<int[]> adjList, int n)
        {
            vertexList = new List<Vertex>();
            edgeList = new List<Edge>();
            
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

        public void MakeVerticesUnvisited()
        {
            for (int i = 0; i < vertexList.Count; i++)
            {
                vertexList[i].isVisited = false;
            }
        }

        public void FormComponentRecursively(Vertex vertex, List<Vertex> component)
        {
            component.Add(vertex);
            vertex.isVisited = true;

            foreach (Vertex adjVertex in vertex.adjVertexList)
            {
                if (!adjVertex.IsIn(component))
                {
                    FormComponentRecursively(adjVertex, component);
                }
            }
        }

        public List<Vertex> Component(Vertex vertex)
        {
            List<Vertex> component = new List<Vertex>();
            
            FormComponentRecursively(vertex, component);

            return component;
        }

        public List<List<Vertex>> Components()
        {
            List<List<Vertex>> components = new List<List<Vertex>>();
            Vertex firstUnvisitedVertex = FirstUnvisitedVertex();
            
            while (firstUnvisitedVertex != null)
            {
                components.Add(Component(firstUnvisitedVertex));
                firstUnvisitedVertex = FirstUnvisitedVertex();
            }

            MakeVerticesUnvisited();

            return components;
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

        public bool IsEuler()
        {
            if (!HasAllEvenDegrees())
            {
                return false;
            }

            List<List<Vertex>> components = Components();
            int filledComponents = 0;
            
            foreach (List<Vertex> component in components)
            {
                if (component.Count > 1)
                {
                    filledComponents += 1;
                }
            }

            if (filledComponents > 1)
            {
                return false;
            }

            return true;
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
                List<Vertex> eulerPath = new List<Vertex>();
                
                Stack<Vertex> vertexStack = new Stack<Vertex>();
                vertexStack.Push(currentVertex);

                while (unvisitedEdges.Count > 0)
                {
                    if (currentVertex.IsDeadEnd(unvisitedEdges))
                    {
                        Console.WriteLine("is dead end");
                        while (vertexStack.Peek().IsDeadEnd(unvisitedEdges))
                        {
                            eulerPath.Add(vertexStack.Pop());
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
                    eulerPath.Add(vertexStack.Pop());
                }

                return eulerPath;
            }

            return null;
        }

        public void FormBipartiteGraphPartsRecursively(Vertex currentVertex, List<Vertex> firstPart,
            List<Vertex> secondPart, bool firstPartFlag = true)
        {
            if (firstPartFlag)
            {
                firstPart.Add(currentVertex);
            }
            else
            {
                secondPart.Add(currentVertex);
            }

            currentVertex.isVisited = true;

            foreach (Vertex adjVertex in currentVertex.adjVertexList)
            {
                if (!adjVertex.isVisited)
                {
                    FormBipartiteGraphPartsRecursively(adjVertex, firstPart, secondPart, !firstPartFlag);
                }
            }
        }
        
        public bool ComponentIsBipartite(List<Vertex> component)
        {
            List<Vertex> firstPart = new List<Vertex>();
            List<Vertex> secondPart = new List<Vertex>();
            
            FormBipartiteGraphPartsRecursively(component[0], firstPart, secondPart);
            
            MakeVerticesUnvisited();

            for (int i = 1; i < firstPart.Count; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    if (firstPart[i].IsAdjacentTo(firstPart[j]))
                    {
                        return false;
                    }
                }
            }
            
            for (int i = 1; i < secondPart.Count; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    if (secondPart[i].IsAdjacentTo(secondPart[j]))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public bool IsBipartite()
        {
            foreach (List<Vertex> component in Components())
            {
                if (!ComponentIsBipartite(component))
                {
                    return false;
                }
            }

            return true;
        }

        public List<List<Vertex>> BipartiteParts()
        {
            if (IsBipartite())
            {
                List<Vertex> firstPart = new List<Vertex>();
                List<Vertex> secondPart = new List<Vertex>();

                foreach (List<Vertex> component in Components())
                {
                    List<Vertex> componentFirstPart = new List<Vertex>();
                    List<Vertex> componentSecondPart = new List<Vertex>();
                    
                    FormBipartiteGraphPartsRecursively(component[0], componentFirstPart, componentSecondPart);
                    
                    MakeVerticesUnvisited();

                    foreach (Vertex vertex in componentFirstPart)
                    {
                        firstPart.Add(vertex);
                    }
                    
                    foreach (Vertex vertex in componentSecondPart)
                    {
                        secondPart.Add(vertex);
                    }
                }
                
                return new List<List<Vertex>>(new []{firstPart, secondPart});
            }

            return null;
        }
    }

    class Program
    {
        public static List<int[]> ComposeAdjacenciesList(int[] raveledArray)
        {
            List<int[]> adjList = new List<int[]>();

            for (int i = 0; i < raveledArray.Length / 2; i++)
            {
                adjList.Add(new []{raveledArray[2 * i], raveledArray[2 * i + 1]});
            }

            return adjList;
        }

        static void Main(string[] args)
        {
            // euler cycle
            
            int[] raveledList = new[] {0, 1, 0, 2, 0, 3, 2, 1, 2, 3, 0, 4, 2, 4};
            int n = 5;
            
            List<int[]> adjacenciesList = ComposeAdjacenciesList(raveledList);

            // foreach (int[] item in adjacenciesList)
            // {
            //     Console.WriteLine($"{item[0]}, {item[1]}");
            // }
            
            Graph graph = new Graph(adjacenciesList, n);

            List<Vertex> cycle = graph.EulerCycle();

            if (cycle != null)
            {
                Console.WriteLine("euler cycle:");
                
                foreach (Vertex vertex in cycle)
                {
                    Console.WriteLine(vertex.number);
                }
            }
            else
            {
                Console.WriteLine("graph is not euler");
            }

            Console.WriteLine("=====");
            
            // bipartite graph

            raveledList = new[] {0, 1, 0, 3, 1, 2, 1, 4, 2, 1, 2, 3, 3, 2, 3, 4, 4, 1, 4, 3};
            n = 5;

            adjacenciesList = ComposeAdjacenciesList(raveledList);
            graph = new Graph(adjacenciesList, n);

            List<List<Vertex>> bipartiteParts = graph.BipartiteParts();

            if (bipartiteParts != null)
            {
                Console.WriteLine("first bipartite part:");

                foreach (Vertex vertex in bipartiteParts[0])
                {
                    Console.WriteLine(vertex.number);
                }
                
                Console.WriteLine("second bipartite part:");

                foreach (Vertex vertex in bipartiteParts[1])
                {
                    Console.WriteLine(vertex.number);
                }
            }
            else
            {
                Console.WriteLine("graph is not bipartite");
            }
        }
    }
}