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
            adjVertexList = new List<Vertex>(); //список смежностей
            isVisited = false;
        }

        public bool IsEven()
        {
            return adjVertexList.Count % 2 == 0;
        }

        public bool IsIn(List<Vertex> vertexList) //есть ли вершина в списке вершин
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

        public bool IsAdjacentTo(Vertex vertex) //проверка вершины в списке смежности
        {
            return vertex.IsIn(adjVertexList);
        }

        public bool IsDeadEnd(List<Edge> unvisitedEdgesList) //тупик
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

        public Graph(List<int[]> edges, int amountOfVertices)
        {
            vertexList = new List<Vertex>();
            edgeList = new List<Edge>();

            for (int i = 0; i < amountOfVertices; i++)
            {
                vertexList.Add(new Vertex(i));
            }

            foreach (int[] adjItem in edges)
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

        public void MakeVerticesUnvisited()//для обнуления после проверки на двудольность
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
            List<List<Vertex>> components = new List<List<Vertex>>();//каждый массив - компонента
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
            int filledComponents = 0;//количество непустых компонент

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
                Vertex currentVertex = vertexList[0];

                List<Edge> unvisitedEdges = new List<Edge>(edgeList);
                List<Vertex> eulerCycle = new List<Vertex>();

                Stack<Vertex> vertexStack = new Stack<Vertex>();
                vertexStack.Push(currentVertex);

                while (unvisitedEdges.Count > 0)//пока не посетим все ребра
                {
                    if (currentVertex.IsDeadEnd(unvisitedEdges))//для тупика
                    {
                        while (vertexStack.Peek().IsDeadEnd(unvisitedEdges))//смотрит на последний элемент
                        {
                            eulerCycle.Add(vertexStack.Pop());
                            currentVertex = vertexStack.Peek();
                        }
                    }
                    else
                    {
                        foreach (Vertex adjVertex in currentVertex.adjVertexList)
                        {
                            int edgeIndex = edgeIndexByTwoVertices(currentVertex, adjVertex, unvisitedEdges);//индекс ребра в массиве непосещенных ребер
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

        public void FormBipartiteGraphPartsRecursively(Vertex currentVertex, List<Vertex> firstPart,
            List<Vertex> secondPart, bool typeOfPart = true)
        {
            if (typeOfPart)
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
                    FormBipartiteGraphPartsRecursively(adjVertex, firstPart, secondPart, !typeOfPart);
                }
            }
        }

        public bool ComponentIsBipartite(Vertex componentStartVertex, List<Vertex> firstPart = null,
            List<Vertex> secondPart = null, bool typeOfPart = true)
        {
            firstPart ??= new List<Vertex>();
            secondPart ??= new List<Vertex>();

            componentStartVertex.isVisited = true;

            if (typeOfPart)
            {
                if (firstPart.Any(v => v.IsAdjacentTo(componentStartVertex)))
                {
                    return false;
                }

                firstPart.Add(componentStartVertex);
            }
            else
            {
                if (secondPart.Any(v => v.IsAdjacentTo(componentStartVertex)))
                {
                    return false;
                }

                secondPart.Add(componentStartVertex);
            }

            List<Vertex> allPossibleNextVertices = componentStartVertex.adjVertexList.Where(v => !v.isVisited).ToList();

            if (allPossibleNextVertices.Count > 0)
            {
                return allPossibleNextVertices.All(v =>
                    ComponentIsBipartite(v, firstPart, secondPart, !typeOfPart));
            }

            return true;
        }

        public bool IsBipartite()
        {
            foreach (List<Vertex> component in Components())
            {
                if (!ComponentIsBipartite(component[0]))
                {
                    foreach (Vertex vertex in component)//очистка(isVisited)
                    {
                        vertex.isVisited = false;
                    }
                    return false;
                }

                foreach (Vertex vertex in component)
                {
                    vertex.isVisited = false;
                }
            }

            return true;
        }

        public List<List<Vertex>> BipartiteParts()//найти разбиение на доли.
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

                return new List<List<Vertex>>(new[] {firstPart, secondPart});
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
                adjList.Add(new[] {raveledArray[2 * i], raveledArray[2 * i + 1]});
            }

            return adjList;
        }

        static void Main(string[] args)
        {
            // euler cycle

            int[] raveledList = {0, 1, 0, 2, 0, 3, 2, 1, 2, 3, 0, 4, 2, 4};
            int n = 5;

            List<int[]> adjacenciesList = ComposeAdjacenciesList(raveledList);

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

            raveledList = new[] {0, 1, 0, 3, 1, 2, 1, 4, 2, 3, 3, 4};
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