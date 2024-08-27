using System.Collections.Generic;
using System;
using System.Runtime.Serialization;
using System.Collections;
using Newtonsoft.Json;

[Serializable]
public class HashTable<TKey, TValue> : ISerializable, IEnumerable<KeyValuePair<TKey, TValue>>, ICollection<KeyValuePair<TKey, TValue>>
{
    [JsonProperty("buckets")]
    public LinkedList<KeyValuePair<TKey, TValue>>[] buckets;

    int capacity;

    bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly => false;

    int ICollection<KeyValuePair<TKey, TValue>>.Count => capacity;

    public HashTable()
    {
        this.capacity = 16;
        buckets = new LinkedList<KeyValuePair<TKey, TValue>>[capacity];
    }

    public HashTable(int capacity = 16)
    {
        try
        {
            this.capacity = 16;
            buckets = new LinkedList<KeyValuePair<TKey, TValue>>[capacity];
        }
        catch (Exception ex)
        {
            Console.WriteLine(capacity + ": " + ex.Message);
            throw;
        }
    }

    private int GetBucketIndex(TKey key)
    {
        int hash = key.GetHashCode();
        int index = hash % buckets.Length;
        return index;
    }

    public void Add(TKey key, TValue value)
    {
        int index = GetBucketIndex(key);

        if (buckets[index] == null)
        {
            buckets[index] = new LinkedList<KeyValuePair<TKey, TValue>>();
        }
        buckets[index].AddLast(new KeyValuePair<TKey, TValue>(key, value));
    }

    public bool TryGetValue(TKey key, out TValue value)
    {
        int index = GetBucketIndex(key);
        var bucket = buckets[index];

        if (bucket != null)
        {
            foreach (var pair in bucket)
            {
                if (pair.Key.Equals(key))
                {
                    value = pair.Value;
                    return true;
                }
            }
        }

        value = default;
        return false;
    }

    public bool Remove(TKey key)
    {
        int index = GetBucketIndex(key);
        var bucket = buckets[index];

        if (bucket != null)
        {
            var node = bucket.First;
            while (node != null)
            {
                if (node.Value.Key.Equals(key))
                {
                    bucket.Remove(node);
                    return true;
                }
                node = node.Next;
            }
        }

        return false;
    }
    public int Size()
    {
        int size = 0;
        foreach (var bucket in buckets)
        {
            if (bucket != null)
            {
                size += bucket.Count;
            }
        }
        return size;
    }
    protected HashTable(SerializationInfo info, StreamingContext context)
    {
        buckets = (LinkedList<KeyValuePair<TKey, TValue>>[])info.GetValue("buckets", typeof(LinkedList<KeyValuePair<TKey, TValue>>[]));
    }

    // Serialization method
    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        info.AddValue("buckets", buckets);
    }

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
    {
        foreach (var bucket in buckets)
        {
            if (bucket != null)
            {
                foreach (var pair in bucket)
                {
                    yield return pair;
                }
            }
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
    {
        Add(item.Key, item.Value);
    }

    void ICollection<KeyValuePair<TKey, TValue>>.Clear()
    {
        foreach (var bucket in buckets)
        {
            if (bucket != null)
            {
                bucket.Clear();
            }
        }
    }

    bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
    {
        foreach(var pair in this)
        {
            if (pair.Key.Equals(item.Key) && pair.Value.Equals(item.Value))
            {
                return true;
            }
        }
        return false;
    }

    void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
    {
        if (array == null)
        {
            throw new ArgumentNullException(nameof(array), "Array cannot be null.");
        }

        if (arrayIndex < 0 || arrayIndex >= array.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(arrayIndex), "Array index is out of range.");
        }

        if (array.Length - arrayIndex < Size())
        {
            throw new ArgumentException("The destination array does not have enough space to copy the elements.", nameof(array));
        }

        foreach (var pair in this)
        {
            array[arrayIndex++] = pair;
        }
    }

    bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
    {
        try
        {
            Remove(item.Key);
            return true;
        }
        catch (Exception e)
        {
            UnityEngine.Debug.Log(e.Message);
            return false;
        }
    }
}