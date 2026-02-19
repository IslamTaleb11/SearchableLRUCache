using System;
using System.Collections.Generic;


namespace SearchableLRUCache
{
    public class SearchableLRUCache<TKey, TValue> where TKey : IComparable<TKey>
    {
        private LRU<TKey, TValue> lru { get; set; }
        private Dictionary<TKey, List<TKey>> cachedRecentQueries;

        public SearchableLRUCache(int Capacity)
        {
            lru = new LRU<TKey, TValue>(Capacity);
            cachedRecentQueries = new Dictionary<TKey, List<TKey>>();
        }

        public void Put(TKey Key, TValue Value)
        {
            lru.Put(Key, Value);
            ClearCachedRecentQueries();
        }
        public TValue Get(TKey Key)
        {
            return lru.Get(Key);
        }

        public bool ContainsKey(TKey Key)
        {
            return lru.ContainsKey(Key);
        }

        public TValue Peek(TKey Key)
        {
            return lru.Peek(Key);
        }

        public bool DeleteKey(TKey Key)
        {
            if (lru.ContainsKey(Key))
            {
                ClearCachedRecentQueries();
                return lru.DeleteKey(Key);
            }
            return false;
        }

        public Dictionary<TKey, TValue> GetAllValuesAsc()
        {
            return lru.GetAllValuesAsc();
        }

        public void printCache()
        {
            lru.printCache();
        }

        public void printSortedSearchableLRUCache()
        {
            lru.printSortedSearchableLRUCache();
        }

        public List<TKey> SearchByPrefix(TKey Prefix)
        {
            if (cachedRecentQueries.ContainsKey(Prefix))
            {
                return cachedRecentQueries[Prefix];
            }
            return lru.SearchByPrefix(Prefix, cachedRecentQueries);
        }

        private void ClearCachedRecentQueries()
        {
            cachedRecentQueries.Clear();
        }
    }
}
