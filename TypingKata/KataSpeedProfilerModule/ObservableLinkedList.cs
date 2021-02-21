using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using KataSpeedProfilerModule.Interfaces;

namespace KataSpeedProfilerModule {
    /// <summary>
    /// This class is a LinkedList that can be used in a WPF MVVM scenario. Composition was used instead of inheritance,
    /// because inheriting from LinkedList does not allow overriding its methods.
    /// This implementation is from: https://stackoverflow.com/a/6998882 [Accessed 19/02/2021]
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ObservableLinkedList<T> : IObservableLinkedList<T> {
        private readonly LinkedList<T> _linkedList;

        public int Count => _linkedList.Count;

        public LinkedListNode<T> First => _linkedList.First;

        public LinkedListNode<T> Last => _linkedList.Last;

        public ObservableLinkedList() {
            _linkedList = new LinkedList<T>();
        }

        public ObservableLinkedList(IEnumerable<T> collection) {
            _linkedList = new LinkedList<T>(collection);
        }

        public LinkedListNode<T> AddAfter(LinkedListNode<T> prevNode, T value) {
            var ret = _linkedList.AddAfter(prevNode, value);
            OnNotifyCollectionChanged();
            return ret;
        }

        public void AddAfter(LinkedListNode<T> node, LinkedListNode<T> newNode) {
            _linkedList.AddAfter(node, newNode);
            OnNotifyCollectionChanged();
        }

        public LinkedListNode<T> AddBefore(LinkedListNode<T> node, T value) {
            var ret = _linkedList.AddBefore(node, value);
            OnNotifyCollectionChanged();
            return ret;
        }

        public void AddBefore(LinkedListNode<T> node, LinkedListNode<T> newNode) {
            _linkedList.AddBefore(node, newNode);
            OnNotifyCollectionChanged();
        }

        public LinkedListNode<T> AddFirst(T value) {
            var ret = _linkedList.AddFirst(value);
            OnNotifyCollectionChanged();
            return ret;
        }

        public void AddFirst(LinkedListNode<T> node) {
            _linkedList.AddFirst(node);
            OnNotifyCollectionChanged();
        }

        public LinkedListNode<T> AddLast(T value) {
            var ret = _linkedList.AddLast(value);
            OnNotifyCollectionChanged();
            return ret;
        }

        public void AddLast(LinkedListNode<T> node) {
            _linkedList.AddLast(node);
            OnNotifyCollectionChanged();
        }

        public void Clear() {
            _linkedList.Clear();
            OnNotifyCollectionChanged();
        }

        public bool Contains(T value) {
            return _linkedList.Contains(value);
        }

        public void CopyTo(T[] array, int index) {
            _linkedList.CopyTo(array, index);
        }

        public bool LinkedListEquals(object obj) {
            return _linkedList.Equals(obj);
        }

        public LinkedListNode<T> Find(T value) {
            return _linkedList.Find(value);
        }

        public LinkedListNode<T> FindLast(T value) {
            return _linkedList.FindLast(value);
        }

        public Type GetLinkedListType() {
            return _linkedList.GetType();
        }

        public bool Remove(T value) {
            var ret = _linkedList.Remove(value);
            OnNotifyCollectionChanged();
            return ret;
        }

        public void Remove(LinkedListNode<T> node) {
            _linkedList.Remove(node);
            OnNotifyCollectionChanged();
        }

        public void RemoveFirst() {
            _linkedList.RemoveFirst();
            OnNotifyCollectionChanged();
        }

        public void RemoveLast() {
            _linkedList.RemoveLast();
            OnNotifyCollectionChanged();
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;
        public void OnNotifyCollectionChanged() {
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator() {
            return (_linkedList as IEnumerable<T>).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return (_linkedList as IEnumerable).GetEnumerator();
        }
    }
}