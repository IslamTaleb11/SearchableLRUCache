using System;
using System.Collections.Generic;


namespace SearchableLRUCache
{
    internal class LRU<TKey, TValue> where TKey : IComparable<TKey>
    {
        private int Capacity { set; get; }
        private Dictionary<TKey, TValue> cache;
        private LinkedList<TKey> linkedList;
        private Dictionary<TKey, LinkedListNode<TKey>> nodesList;
        private object lockObject;
        private AVLTree<TKey> avlTree;

        public LRU(int capacity)
        {
            this.Capacity = capacity;
            cache = new Dictionary<TKey, TValue>();
            linkedList = new LinkedList<TKey>();
            nodesList = new Dictionary<TKey, LinkedListNode<TKey>>();

            lockObject = new Object();

            avlTree = new AVLTree<TKey>();
        }

        private bool IsCacheFull()
        {
            return cache.Count >= Capacity;
        }

        public TValue Get(TKey Key)
        {
            lock (lockObject)
            {
                if (cache.TryGetValue(Key, out TValue value))
                {

                    LinkedListNode<TKey> node = nodesList[Key];
                    linkedList.Remove(node);
                    linkedList.AddFirst(node);

                    return value;
                }
                return default(TValue);
            }
        }


        private void Insert(TKey Key, TValue Value)
        {
            lock (lockObject)
            {
                cache.Add(Key, Value);
                LinkedListNode<TKey> node = linkedList.AddFirst(Key);
                nodesList.Add(Key, node);
                avlTree.Insert(Key);
            }
        }


        private void Remove()
        {
            lock (lockObject)
            {
                LinkedListNode<TKey> leastRecentlyUsedNode = linkedList.Last;

                linkedList.Remove(leastRecentlyUsedNode);
                cache.Remove(leastRecentlyUsedNode.Value);
                nodesList.Remove(leastRecentlyUsedNode.Value);
                avlTree.DeleteNode(leastRecentlyUsedNode.Value);
            }
        }

        private void Override(TKey Key, TValue Value)
        {
            lock (lockObject)
            {
                cache[Key] = Value;
                LinkedListNode<TKey> node = nodesList[Key];
                linkedList.Remove(node);
                linkedList.AddFirst(node);
            }
        }

        public void Put(TKey Key, TValue Value)
        {
            if (cache.ContainsKey(Key))
            {
                Override(Key, Value);
            }
            else
            {
                if (!IsCacheFull())
                {
                    Insert(Key, Value);
                }
                else
                {
                    Remove();
                    Insert(Key, Value);
                }
            }
        }


        public bool ContainsKey(TKey Key)
        {
            return cache.ContainsKey(Key);
        }


        public TValue Peek(TKey Key)
        {
            if (cache.TryGetValue(Key, out TValue Value))
            {
                return Value;
            }

            return default(TValue);
        }


        public bool DeleteKey(TKey Key)
        {
            
             lock (lockObject)
             {
                    LinkedListNode<TKey> node = nodesList[Key];
                    linkedList.Remove(node);

                    cache.Remove(Key);
                    nodesList.Remove(Key);

                    return !ContainsKey(Key) ? true : false;
             }
            
            return false;
        }


        public Dictionary<TKey, TValue> GetAllValuesAsc()
        {
            lock (lockObject)
            {
                Dictionary<TKey, TValue> resultDictionary = new Dictionary<TKey, TValue>();
                LinkedList<TKey> tempLinkedList = new LinkedList<TKey>(linkedList);

                for (int i = 0; i < cache.Count; i++)
                {
                    TKey firstKey = (TKey)tempLinkedList.First.Value;
                    tempLinkedList.RemoveFirst();
                    resultDictionary.Add(firstKey, cache[firstKey]);
                }
                return resultDictionary;
            }
        }

        public List<TKey> SearchByPrefix(TKey Prefix, Dictionary<TKey, List<TKey>> cachedRecentQueries)
        {
            List<TKey> list = new List<TKey>(); 
            return avlTree.AutoComplete(Prefix, list, cachedRecentQueries);
        }

        public void printCache()
        {
            foreach (var item in linkedList)
            {
                Console.WriteLine(item);
            }
        }

        public void printSortedSearchableLRUCache()
        {
            avlTree.PrintTree();
        }

    }
}
