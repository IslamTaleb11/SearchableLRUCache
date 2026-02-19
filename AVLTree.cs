using System;
using System.Collections.Generic;


namespace SearchableLRUCache
{
    internal class AVLTree<T> where T : IComparable<T>

    {
        private AVLTreeNode<T> root;

        public void Insert(T value)
        {
            root = Insert(value, root);
        }

        private AVLTreeNode<T> Insert(T value, AVLTreeNode<T> node)
        {
            if (node == null)
            {
                return new AVLTreeNode<T>(value);
            }

            if (value.CompareTo(node.value) > 0)
            {
                node.right = Insert(value, node.right);
            }

            if (value.CompareTo(node.value) < 0)
            {
                node.left = Insert(value, node.left);
            }

            UpdateHeight(node);

            return BalanceTree(node);
        }


        private int GetBalanceFactor(AVLTreeNode<T> node)
        {
            return (Height(node.left) - Height(node.right));
        }

        private AVLTreeNode<T> BalanceTree(AVLTreeNode<T> node)
        {
            int balanceFactor = GetBalanceFactor(node);

            if (balanceFactor < -1 && GetBalanceFactor(node.right) <= 0)
            {
                return LeftRotation(node);
            }

            if (balanceFactor > 1 && GetBalanceFactor(node.left) >= 0)
            {
                return RightRotation(node);
            }

            if (balanceFactor < -1 && GetBalanceFactor(node.right) > 0)
            {
                node.right = RightRotation(node.right);
                return LeftRotation(node);
            }

            if ((balanceFactor > 1 && GetBalanceFactor(node.left) < 0))
            {
                node.left = LeftRotation(node.left);
                return RightRotation(node);
            }

            return node;
        }

        private AVLTreeNode<T> LeftRotation(AVLTreeNode<T> originalRoot)
        {
            AVLTreeNode<T> newRoot = originalRoot.right;
            AVLTreeNode<T> originalLeftChild = newRoot.left;
            newRoot.left = originalRoot;
            originalRoot.right = originalLeftChild;

            UpdateHeight(originalRoot);
            UpdateHeight(newRoot);

            return newRoot;
        }


        private AVLTreeNode<T> RightRotation(AVLTreeNode<T> originalRoot)
        {
            AVLTreeNode<T> newRoot = originalRoot.left;
            AVLTreeNode<T> originalRightChild = newRoot.right;
            newRoot.right = originalRoot;
            originalRoot.left = originalRightChild;

            UpdateHeight(originalRoot);
            UpdateHeight(newRoot);

            return newRoot;
        }

        private void UpdateHeight(AVLTreeNode<T> node)
        {
            node.height = 1 + Math.Max(Height(node.left), Height(node.right));
        }


        private int Height(AVLTreeNode<T> node)
        {
            return node != null ? node.height : 0;
        }

        public void DeleteNode(T value)
        {
            root = DeleteNode(root, value);
        }

        private AVLTreeNode<T> DeleteNode(AVLTreeNode<T> node, T value)
        {
            if (node == null)
            {
                return null;
            }

            if (value.CompareTo(node.value) < 0)
            {
                node.left = DeleteNode(node.left, value);
            }
            else if (value.CompareTo(node.value) > 0)
            {
                node.right = DeleteNode(node.right, value);
            }
            else
            {
                if (node.left == null)
                {
                    return node.right;
                }
                if (node.right == null)
                {
                    return node.left;
                }

                AVLTreeNode<T> minValueNode = GetMinValueNode(node.right);

                node.value = minValueNode.value;
                node.right = DeleteNode(node.right, minValueNode.value);
            }

            UpdateHeight(node);

            return BalanceTree(node);
        }


        private AVLTreeNode<T> GetMinValueNode(AVLTreeNode<T> node)
        {
            AVLTreeNode<T> minNode = node;

            while (minNode.left != null)
            {
                minNode = minNode.left;
            }

            return minNode;
        }

        public List<T> AutoComplete(T prefix, List<T> list, Dictionary<T, List<T>> cachedRecentQueries)
        {
            AutoComplete(root, prefix, list);
            cachedRecentQueries.Add(prefix, list);
            return list;
        }

        private void AutoComplete(AVLTreeNode<T> node, T prefix, List<T> list)
        {
            if (node != null)
            {
                if (node.value.ToString().StartsWith(prefix.ToString()))
                {
                    list.Add(node.value);
                    AutoComplete(node.right, prefix, list);
                    AutoComplete(node.left, prefix, list);
                }
                else
                {
                    if (string.Compare(prefix.ToString(), node.value.ToString(), StringComparison.OrdinalIgnoreCase) < 0)
                    {
                        AutoComplete(node.left, prefix, list);
                    }
                    else
                    {
                        AutoComplete(node.right, prefix, list);
                    }
                }
            }
        }
 

        public void PrintTree()
        {
            PrintTree(root, "", true);
        }

        private void PrintTree(AVLTreeNode<T> node, string indent, bool last)
        {
            if (node != null)
            {
                Console.Write(indent);
                if (last)
                {
                    Console.Write("R----");
                    indent += "     ";
                }
                else
                {
                    Console.Write("L----");
                    indent += "|    ";
                }
                Console.WriteLine(node.value);
                PrintTree(node.left, indent, false);
                PrintTree(node.right, indent, true);
            }
        }
    }
}
