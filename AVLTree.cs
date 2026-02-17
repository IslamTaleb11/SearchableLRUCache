using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchableLRUCache
{
    public class AVLTree<T> where T : IComparable<T>

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
                return LeftBalance(node);
            }

            if (balanceFactor > 1 && GetBalanceFactor(node.left) >= 0)
            {
                return RightBalance(node);
            }
        }

        private AVLTreeNode<T> LeftBalance(AVLTreeNode<T> originalRoot)
        {
            AVLTreeNode<T> newRoot = originalRoot.right;
            AVLTreeNode<T> originalLeftChild = newRoot.left;
            newRoot.left = originalRoot;
            originalRoot.right = originalLeftChild;

            UpdateHeight(originalRoot);
            UpdateHeight(newRoot);

            return newRoot;
        }


        private AVLTreeNode<T> RightBalance(AVLTreeNode<T> originalRoot)
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
    }
}
