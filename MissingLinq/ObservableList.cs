using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MissingLinq
{
	/// <summary>
	/// A list which fires Added and Deleted events when elements are added to or deleted from the list
	/// </summary>
	public class ObservableList<T> : IList<T>
	{
		protected List<T> InnerList;

		public ObservableList()
		{
			InnerList = new List<T>();
		}

		public ObservableList(IEnumerable<T> elements)
		{
			InnerList = elements.ToList();
		}

		public event EventHandler<ObservableListEventArgs<T>> Added;
		public event EventHandler<ObservableListEventArgs<T>> Removed;
		public event EventHandler<ObservableListEventArgs<T>> Cleared;

		private void OnAdded(T element)
		{
			var handler = Added;
			handler?.Invoke(this, new ObservableListEventArgs<T>(element));
		}

		private void OnRemoved(T element)
		{
			var handler = Removed;
			handler?.Invoke(this, new ObservableListEventArgs<T>(element));
		}

		private void OnCleared()
		{
			var handler = Cleared;
			handler?.Invoke(this, new ObservableListEventArgs<T>(default(T)));
		}

		public void Add(T element)
		{
			InnerList.Add(element);
			OnAdded(element);
		}

		public void AddRange(IEnumerable<T> elements)
		{
			var collection = elements as T[] ?? elements.ToArray();
			InnerList.AddRange(collection);
			collection.ForEach(OnAdded);
		}

		public bool Remove(T element)
		{
			var didRemove = InnerList.Remove(element);
			if (didRemove) OnRemoved(element);
			return didRemove;
		}

		/// <summary>
		/// Removes all elements from the list and fires the Cleared event
		/// </summary>
		public void Clear()
		{
			InnerList.Clear();
			OnCleared();
		}

		public int Count => InnerList.Count;

		public IEnumerator<T> GetEnumerator()
		{
			return InnerList.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		/// <summary>
		/// Builds an Index that will speed up subsequent lookups in the list. The index is updated automatically when
		/// items or added to or deleted from the list.
		/// </summary>
		public IIndex<TKey, T> BuildIndex<TKey>(Func<T, TKey> keyFunc)
		{
			return new ObservingIndex<TKey, T>(this, keyFunc);
		}

		/// <summary>
		/// Builds a UniqueIndex that will speed up subsequent lookups in the list. The index is updated automatically when
		/// items or added to or deleted from the list. Adding duplicate keys will throw.
		/// </summary>
		public IUniqueIndex<TKey, T> BuildUniqueIndex<TKey>(Func<T, TKey> keyFunc)
		{
			return new ObservingUniqueIndex<T, TKey, T>(this, keyFunc, i => i);
		}

		public bool Contains(T item)
		{
			return InnerList.Contains(item);
		}

		public T this[int index]
		{
			get { return InnerList[index]; }
			set { InnerList[index] = value; }
		}

		public int IndexOf(T element)
		{
			return InnerList.IndexOf(element);
		}

		public void CopyTo(T[] array, int arrayIndex)
		{
			InnerList.CopyTo(array, arrayIndex);
		}

		public bool IsReadOnly => false;

		public void Insert(int index, T item)
		{
			InnerList.Insert(index, item);
			OnAdded(item);
		}

		public void RemoveAt(int index)
		{
			var itemToRemove = InnerList[index];
			InnerList.RemoveAt(index);
			OnRemoved(itemToRemove);
		}

		public List<T> ToList()
		{
			return InnerList;
		}
	}
}
