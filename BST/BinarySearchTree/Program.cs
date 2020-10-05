using System;
using System.Collections.Generic;
using System.Linq;

namespace BinarySearchTree
{
    public class Node
    {
        public int data;
        public Node left, right, parent;
        public int countLeftNodes = 0, countRightNodes = 0;

        public Node(int item)
        {
            data = item;
            left = right = null;
            parent = null;
        }
    }

    public class Tree
    {
        public Node node;
        public Node root;

        public Tree(int[] arr)
        {
            root = new Node(arr[0]);
            for (int i = 1; i < arr.Length; i++)
            {
                InsertMethod(arr[i]);
            }
        }

        public void InsertMethod(int node)
        {
            Node currentNode = root;
            while (true)
            {
                if (node <= currentNode.data) //влево
                {
                    if (currentNode.left != null) //если слева есть дочерний узел
                    {
                        currentNode.countLeftNodes++;
                        currentNode = currentNode.left;
                    }
                    else
                    {
                        Node newNode = new Node(node); //если слева нет дочернего узла
                        currentNode.countLeftNodes++;
                        currentNode.left = newNode;
                        newNode.parent = currentNode;
                        break;
                    }
                }
                else
                {
                    if (currentNode.right != null) //если справа есть дочерний узел
                    {
                        currentNode.countRightNodes++;
                        currentNode = currentNode.right;
                    }
                    else
                    {
                        Node newNode = new Node(node); //если справа нет дочернего узла
                        currentNode.countRightNodes++;
                        currentNode.right = newNode;
                        newNode.parent = currentNode;
                        break;
                    }
                }
            }
        }

        public void PreOrder(List<int> arr, Node node)
        {
            if (node.left != null)
            {
                PreOrder(arr, node.left);
            }

            arr.Add(node.data);
            if (node.right != null)
            {
                PreOrder(arr, node.right);
            }
        }

        public void PostOrder(List<int> arr, Node node)
        {
            if (node.right != null)
            {
                PostOrder(arr, node.right);
            }

            arr.Add(node.data);

            if (node.left != null)
            {
                PostOrder(arr, node.left);
            }
        }

        public Node kMin(int k)
        {
            Node node = root;
            while (node.countLeftNodes + 1 != k)
            {
                if (node.countLeftNodes + 1 < k)
                {
                    node = node.right;
                    k -= node.countLeftNodes;
                }
                else
                {
                    node = node.left;
                }
            }

            return node;
        }

        public int maxDepth(Node node)
        {
            if (node != null)
            {
                /* compute the depth of each subtree */
                int lDepth = maxDepth(node.left);
                int rDepth = maxDepth(node.right);

                /* use the larger one */
                if (lDepth > rDepth)
                {
                    return (lDepth + 1);
                }
                else
                {
                    return (rDepth + 1);
                }
            }
            else
                return 0;
        }

        public void TurnNodesLeft(Node centreNode)
        {
            Node rightNode = centreNode.right;
            Node parent = centreNode.parent;
            if (rightNode != null)
            {
                Node leftChildRightNodes = rightNode.left;
                int countChildrenLeftNodes = centreNode.countLeftNodes;
                int countLeftNodes = rightNode.countLeftNodes;
                if (parent != null)
                {
                    if (centreNode.data > parent.data)
                    {
                        parent.right = rightNode;
                    }
                    else
                    {
                        parent.left = rightNode;
                    }
                }

                centreNode.parent = rightNode;
                centreNode.right = leftChildRightNodes;
                centreNode.countLeftNodes = countLeftNodes;
                rightNode.parent = parent;
                rightNode.left = centreNode;
                rightNode.countLeftNodes = countChildrenLeftNodes + countLeftNodes + 1;

                if (leftChildRightNodes != null)
                {
                    leftChildRightNodes.parent = centreNode;
                }

                if (parent == null)
                {
                    root = rightNode;
                }
            }
            else
            {
                if (parent != null)
                {
                    Node leftNode = centreNode.left;
                    int countLeftNode = centreNode.countLeftNodes;
                    Node parentParent = parent.parent;
                    if (parentParent != null)
                    {
                        if (parent.data > parentParent.data)
                        {
                            parentParent.right = centreNode;
                        }
                        else
                        {
                            parentParent.left = centreNode;
                        }
                    }

                    parent.parent = centreNode;
                    parent.right = leftNode;
                    parent.countRightNodes = countLeftNode;
                    centreNode.parent = parentParent;
                    centreNode.left = parent;
                    centreNode.countLeftNodes = countLeftNode + parent.countLeftNodes + 1;
                    leftNode.parent = parent;
                    if (parentParent == null)
                    {
                        root = centreNode;
                    }
                }
            }
        }

        public void TurnNodesRight(Node centreNode)
        {
            Node leftNode = centreNode.left;
            Node parent = centreNode.parent;
            if (leftNode != null)
            {
                Node rightChildLeftNodes = leftNode.right;
                int countChildrenRightNodes = centreNode.countRightNodes;
                int countRightNodes = leftNode.countRightNodes;
                if (parent != null)
                {
                    if (centreNode.data > parent.data)
                    {
                        parent.right = leftNode;
                    }
                    else
                    {
                        parent.left = leftNode;
                    }
                }

                centreNode.parent = leftNode;
                centreNode.left = rightChildLeftNodes;
                centreNode.countLeftNodes = countRightNodes;
                leftNode.parent = parent;
                leftNode.right = centreNode;
                leftNode.countRightNodes = countChildrenRightNodes + countRightNodes + 1;

                if (rightChildLeftNodes != null)
                {
                    rightChildLeftNodes.parent = centreNode;
                }

                if (parent == null)
                {
                    root = leftNode;
                }
            }
            else
            {
                if (parent != null)
                {
                    Node rightNode = centreNode.right;
                    int countRightNode = centreNode.countRightNodes;
                    Node parentParent = parent.parent;
                    if (parentParent != null)
                    {
                        if (parent.data > parentParent.data)
                        {
                            parentParent.right = centreNode;
                        }
                        else
                        {
                            parentParent.left = centreNode;
                        }
                    }

                    parent.parent = centreNode;
                    parent.left = rightNode;
                    parent.countLeftNodes = countRightNode;
                    centreNode.parent = parentParent;
                    centreNode.right = parent;
                    centreNode.countRightNodes = countRightNode + parent.countRightNodes + 1;
                    rightNode.parent = parent;
                    if (parentParent == null)
                    {
                        root = centreNode;
                    }
                }
            }
        }

        public void BalanceTree(Node node)
        {
            double middleK = (node.countLeftNodes + node.countRightNodes + 1) / 2.0;
            middleK = Math.Ceiling(middleK);
            Node middleNode = kMin((int) middleK);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            int[] arr = new[] {6, 4, 2, 30,1,9};
            Tree tree = new Tree(arr);
            List<int> arrForPreOrder = new List<int>();
            List<int> arrForPostOrder = new List<int>();
            
            Console.WriteLine(tree.root.data+ " Корень дерева ");
            Console.WriteLine(tree.root.left.data+ " Левое поддерево");
            Console.WriteLine(tree.root.right.data+ " Правое поддерево");
            
            tree.PreOrder(arrForPreOrder, tree.root);
            tree.PostOrder(arrForPostOrder, tree.root);
            Console.WriteLine("Обход по возрастанию элементов");
            foreach (int elem in arrForPreOrder)
            {
                Console.Write(elem + " ");
            }
            Console.WriteLine("");
            
            Console.WriteLine("Обход по убыванию элементов");
            foreach (int elem in arrForPostOrder)
            {
                Console.Write(elem + " ");
            }
            Console.WriteLine("");

            
            tree.TurnNodesRight(tree.root);
            Console.WriteLine(tree.root.data);
            Console.WriteLine(tree.root.left.data);
            Console.WriteLine(tree.maxDepth(tree.root));
        }
    }
}