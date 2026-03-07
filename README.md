# 📄 SearchableLRUCache

A high-performance, in-memory **searchable cache** implemented in C#, combining **LRU (Least Recently Used) eviction** with a **self-balancing AVL tree** for fast **prefix-based search**. Ideal for autocomplete, smart search, and any application requiring both quick lookup and sorted results.

---

## Features & Time Complexity

### 1️⃣ LRU Cache

- Stores key-value pairs with a **maximum capacity**.
- Automatically **evicts the least recently used items** when full.
- **Time Complexity:**
  - `Put` (Insert/Update): **O(1)**
  - `Get` (Access): **O(1)**
  - `Delete`: **O(1)**
- **Implementation:** Dictionary + LinkedList hybrid.

### 2️⃣ AVL Tree Integration

- Maintains keys in **sorted order** for fast prefix search.
- **Time Complexity:**
  - `Insert`: **O(log n)**
  - `Delete`: **O(log n)**
  - `Search`: **O(log n)** (exact match)

### 3️⃣ Search by Prefix

- Quickly find all keys starting with a given string.
- **Time Complexity:**
  - Locate starting node: **O(log n)**
  - Traverse matching nodes: **O(m)**, where `m` = number of matching keys
  - Overall: **O(log n + m)**
- Case-insensitive search. Ideal for autocomplete.

### 4️⃣ Cached Recent Queries

- Stores results of previous prefix searches to avoid redundant AVL traversal.
- **Time Complexity:**
  - Cache hit: **O(1)**
  - Cache miss: **O(log n + m)** (same as prefix search)
- Automatically cleared when new entries are added or deleted.

### 5️⃣ Thread-Safe Operations

- All cache operations (`Put`, `Get`, `Delete`) are **thread-safe**.
- Ensures safe access in multi-threaded applications.
- **Time Complexity:** Minimal overhead; logical operation complexity unchanged.

### 6️⃣ Smart Autocomplete Demo

- Examples include domains like:
  `google.com`, `github.com`, `gitlab.com`, `gmail.com`, `stackexchange.com`, etc.
- Prefix search returns instant suggestions.
- **Time Complexity:** **O(log n + m)** per search.

### 7️⃣ Get All Values in Ascending Order

- Traverse cache in sorted order using AVL tree.
- **Time Complexity:** **O(n)**, where `n` = number of items in cache.

### 8️⃣ Delete Keys

- Remove a key from both LRU and AVL tree.
- **Time Complexity:** **O(log n)** (AVL deletion) + **O(1)** (Dictionary + LinkedList removal)
### 9️⃣ Expiration / Time-To-Live (TTL)

- Each key-value pair can have an optional expiration time.

- Automatically removed from cache once the expiration is reached.

- Thread-safe background cleanup runs periodically to remove expired items.

**Time Complexity:**

- Background cleanup: O(n) per cleanup cycle (n = number of cached items)

- Ideal for session caches, temporary data storage, or time-sensitive information.
```csharp
// Insert with expiration
cache.Put("meta.com", 15, DateTime.UtcNow.AddMinutes(3));

// Access before expiration
var value = cache.Get("meta.com"); // returns 15

// After 3 minutes, the item is automatically removed
```
---

## Installation

1. Clone the repository:
```bash
git clone https://github.com/IslamTaleb11/SearchableLRUCache.git
```

2. Open in Visual Studio or any C# IDE.
3. Build the solution.
4. Run the console application to see autocomplete in action.

---

## Usage
```csharp
var cache = new SearchableLRUCache<string, int>(20);

// Insert items
cache.Put("google.com", 1);
cache.Put("github.com", 2);
cache.Put("gitlab.com", 3);

// Search by prefix
var results = cache.SearchByPrefix("git");
foreach (var item in results)
{
    Console.WriteLine(item); // github.com, gitlab.com
}

// Access cache
var value = cache.Get("github.com");

// Delete key
cache.DeleteKey("gitlab.com");

// Get all items in ascending order
var allItems = cache.GetAllValuesAsc();
```

---

## Use Cases

- Autocomplete engines (search boxes, IDEs, web apps)
- Smart caching systems
- Fast lookup of large datasets with prefix-based queries
- Clinic or inventory management systems

---

## Future Enhancements

- Add frequency-based ranking to sort search results by popularity.
- Implement fuzzy search for typo tolerance.
- Limit number of search results returned per query.
- Make prefix cache LRU-based to avoid unbounded growth.
- Expose as a NuGet package for easy integration.

---

## Author

**Islam Taleb** – Full-stack developer specializing in C# and backend.
