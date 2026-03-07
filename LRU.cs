using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace SearchableLRUCache
{
    internal class LRU<TKey, TValue> where TKey : IComparable<TKey>
    {
        private class CacheItem
        {
            public TValue value;
            public DateTime expiration;
        }
        private int Capacity { set; get; }
        private Dictionary<TKey, CacheItem> cache;
        private LinkedList<TKey> linkedList;
        private Dictionary<TKey, LinkedListNode<TKey>> nodesList;
        private object lockObject;
        private AVLTree<TKey> avlTree;
        private CacheItem cacheItem;
        private Timer cleanupTimer;
        public LRU(int capacity)
        {
            this.Capacity = capacity;
            cache = new Dictionary<TKey, CacheItem>();
            linkedList = new LinkedList<TKey>();
            nodesList = new Dictionary<TKey, LinkedListNode<TKey>>();

            lockObject = new Object();

            avlTree = new AVLTree<TKey>();
           

        }

        public int CountCache()
        {
            return cache.Count;
        }

        private bool IsCacheFull()
        {
            return cache.Count >= Capacity;
        }

        public TValue Get(TKey Key)
        {
            lock (lockObject)
            {
                if (cache.TryGetValue(Key, out CacheItem item))
                {

                    LinkedListNode<TKey> node = nodesList[Key];
                    linkedList.Remove(node);
                    linkedList.AddFirst(node);

                    return item.value;
                }
                return default(TValue);
            }
        }


        private void Insert(TKey Key, TValue Value, DateTime? expiration = null)
        {
            lock (lockObject)
            {
                CacheItem cacheItem = new CacheItem();
                cacheItem.value = Value;
                cacheItem.expiration = expiration ?? DateTime.MaxValue;
                cache.Add(Key, cacheItem);
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
                cache[Key].value = Value;
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

        public void Put(TKey Key, TValue Value, DateTime Expiration)
        {
            if (cache.ContainsKey(Key))
            {
                Override(Key, Value);
            }
            else
            {
                if (!IsCacheFull())
                {
                    Insert(Key, Value, Expiration);
                }
                else
                {
                    Remove();
                    Insert(Key, Value, Expiration);
                }
            }
        }


        public void StartCleanupTimer()
        {
            cleanupTimer = new Timer(_ =>
            {
                lock (lockObject)
                {
                    var keysToDelete = cache.Where(kv => 
                    kv.Value.expiration != DateTime.MaxValue && kv.Value.expiration < DateTime.UtcNow)
                    .Select(kv => kv.Key)
                    .ToList();

                    foreach (var item in keysToDelete)
                    {
                        DeleteKey(item);
                        avlTree.DeleteNode(item);
                    }
                }
            }, null, TimeSpan.Zero, TimeSpan.FromSeconds(7)); // run every 7 seconds
        }


        public bool ContainsKey(TKey Key)
        {
            return cache.ContainsKey(Key);
        }


        public TValue Peek(TKey Key)
        {
            if (cache.TryGetValue(Key, out CacheItem item))
            {
                return item.value;
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
                    resultDictionary.Add(firstKey, cache[firstKey].value);
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
