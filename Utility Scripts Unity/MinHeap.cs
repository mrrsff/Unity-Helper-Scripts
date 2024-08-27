using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinHeap<T>
{
    private struct HeapElement
    {
        public T item;
        public int priority;
    }

    private HeapElement[] elements;
    private Dictionary<T, int> positions;

    public int Count { get; private set; }

    public MinHeap(int capacity)
    {
        elements = new HeapElement[capacity];
        positions = new Dictionary<T, int>();
        Count = 0;
    }

    public void Insert(T item, int priority)
    {
        elements[Count].item = item;
        elements[Count].priority = priority;
        positions[item] = Count;
        BubbleUp(Count);
        Count++;
    }

    public T ExtractMin()
    {
        T result = elements[0].item;
        positions.Remove(result);
        Count--;
        Swap(0, Count);
        BubbleDown(0);
        return result;
    }

    public bool Contains(T item)
    {
        return positions.ContainsKey(item);
    }

    public void UpdatePriority(T item, int priority)
    {
        if (!positions.TryGetValue(item, out int index))
            return;

        int oldPriority = elements[index].priority;
        elements[index].priority = priority;

        if (priority < oldPriority)
            BubbleUp(index);
        else
            BubbleDown(index);
    }

    private void BubbleUp(int index)
    {
        while (index > 0)
        {
            int parentIndex = (index - 1) / 2;
            if (elements[index].priority < elements[parentIndex].priority)
            {
                Swap(index, parentIndex);
                index = parentIndex;
            }
            else
            {
                break;
            }
        }
    }

    private void BubbleDown(int index)
    {
        int smallest = index;

        while (true)
        {
            int leftChild = 2 * index + 1;
            int rightChild = 2 * index + 2;

            if (leftChild < Count && elements[leftChild].priority < elements[smallest].priority)
                smallest = leftChild;
            if (rightChild < Count && elements[rightChild].priority < elements[smallest].priority)
                smallest = rightChild;

            if (smallest != index)
            {
                Swap(index, smallest);
                index = smallest;
            }
            else
            {
                break;
            }
        }
    }

    private void Swap(int i, int j)
    {
        HeapElement temp = elements[i];
        elements[i] = elements[j];
        elements[j] = temp;

        positions[elements[i].item] = i;
        positions[elements[j].item] = j;
    }
}
