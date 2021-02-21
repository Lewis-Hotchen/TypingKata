using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace KataSpeedProfilerModule.Interfaces {
    /// <summary>
    /// This class is a LinkedList that can be used in a WPF MVVM scenario. Composition was used instead of inheritance,
    /// because inheriting from LinkedList does not allow overriding its methods.
    /// This implementation is from: https://stackoverflow.com/a/6998882 [Accessed 19/02/2021]
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IObservableLinkedList<T> : INotifyCollectionChanged, IEnumerable<T> {
        int Count { get; }
        LinkedListNode<T> First { get; }
        LinkedListNode<T> Last { get; }
        LinkedListNode<T> AddAfter(LinkedListNode<T> prevNode, T value);
        void AddAfter(LinkedListNode<T> node, LinkedListNode<T> newNode);
        LinkedListNode<T> AddBefore(LinkedListNode<T> node, T value);
        void AddBefore(LinkedListNode<T> node, LinkedListNode<T> newNode);
        LinkedListNode<T> AddFirst(T value);
        void AddFirst(LinkedListNode<T> node);
        LinkedListNode<T> AddLast(T value);
        void AddLast(LinkedListNode<T> node);
        void Clear();
        bool Contains(T value);
        void CopyTo(T[] array, int index);
        bool LinkedListEquals(object obj);
        LinkedListNode<T> Find(T value);
        LinkedListNode<T> FindLast(T value);
        Type GetLinkedListType();
        bool Remove(T value);
        void Remove(LinkedListNode<T> node);
        void RemoveFirst();
        void RemoveLast();
        void OnNotifyCollectionChanged();
    }
}