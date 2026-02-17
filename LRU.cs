using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchableLRUCache
{
    public class LRU<TKey, TValue>
    {
        private int Capacity { set; get; }
        private Dictionary<TKey, TValue> cache;
        private LinkedList<TKey> linkedList;
        private Dictionary<TKey, LinkedListNode<TKey>> nodesList;
        private object lockObject;

        public LRU(int capacity)
        {
            this.Capacity = capacity;
            cache = new Dictionary<TKey, TValue>();
            linkedList = new LinkedList<TKey>();
            nodesList = new Dictionary<TKey, LinkedListNode<TKey>>();

            lockObject = new Object();
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
            if (ContainsKey(Key))
            {
                lock (lockObject)
                {
                    LinkedListNode<TKey> node = nodesList[Key];
                    linkedList.Remove(node);

                    cache.Remove(Key);
                    nodesList.Remove(Key);

                    return !ContainsKey(Key) ? true : false;
                }
            }
            return false;
        }


        public Dictionary<TKey, TValue> GetAllValuesAsc()
        {
            lock (lockObject)
            {
                Dictionary<TKey, TValue> resultDictionary = new Dictionary<TKey, TValue>();
                LinkedList<TKey> tempLinkedList = new LinkedList<TKey>(linkedList);

                for (int i = 0; i < Capacity; i++)
                {
                    TKey firstKey = (TKey)tempLinkedList.First.Value;
                    tempLinkedList.RemoveFirst();
                    resultDictionary.Add(firstKey, cache[firstKey]);
                }
                return resultDictionary;
            }
        }

        public void printCache()
        {
            foreach (var item in linkedList)
            {
                Console.WriteLine(item);
            }
        }

    }
}
