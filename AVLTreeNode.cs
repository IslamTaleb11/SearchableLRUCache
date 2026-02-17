using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchableLRUCache
{
    internal class AVLTreeNode <T>
    {
        public AVLTreeNode<T> left { set; get; }
        public AVLTreeNode<T> right { set; get; }
        public T value { set; get; }
        public int height { set; get; }

        public AVLTreeNode(T Value)
        {
            this.value = Value;
            height = 1;
        }
    }
}
