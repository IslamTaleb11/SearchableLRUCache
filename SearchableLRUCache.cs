using System;
using System.Collections.Generic;


namespace SearchableLRUCache
{
    public class SearchableLRUCache<TKey, TValue> where TKey : IComparable<TKey>
    {
        private LRU<TKey, TValue> lru { get; set; }

        public SearchableLRUCache(int Capacity)
        {
            lru = new LRU<TKey, TValue>(Capacity);
        }

        public void Put(TKey Key, TValue Value)
        {
            lru.Put(Key, Value);
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
            return lru.DeleteKey(Key);
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
    }
}
