using System;
using System.Linq;

namespace MissingLinq
{
	/// <summary>
	/// Represents a list of items with auto-calculated keys. Items in the list can be retrieved/removed either by specifying the item itself (as in a list) or by specifying the key (as in a Dictionary)
	/// </summary>
	public class KeyedList<TKey, TValue> : ObservableList<TValue>
	{
		public KeyedList(Func<TValue, TKey> keyFunc)
		{
			_keyFunc = keyFunc;
			_index = BuildUniqueIndex(keyFunc);
		}

		private readonly Func<TValue, TKey> _keyFunc;
		private readonly IUniqueIndex<TKey, TValue> _index; 

		public TValue this[TKey key] => _index[key];

		public bool RemoveValue(TValue item)
		{
			return Remove(item);
		}

		public bool RemoveByKey(TKey key)
		{
			var canRemove = _index.Contains(key);
			if (canRemove)
			{
				Remove(_index[key]);
			}
			return canRemove;
		}

		public bool ContainsKey(TKey key)
		{
			return _index.Contains(key);
		}

		/// <summary>
		/// Returns true if the list contains value. Same as Contains(value), this method is added for readability
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public bool ContainsValue(TValue value)
		{
			return Contains(value);
		}

		public int RemoveAll(Predicate<TValue> predicate)
		{
			var valuesToRemove = InnerList.Where(v => predicate(v)).ToList();
			valuesToRemove.ForEach(v => Remove(v));
			return valuesToRemove.Count;
		}

		public int RemoveAll()
		{
			var count = InnerList.Count;
			Clear();
			return count;
		}

		public bool TryGetItem(TKey key, out TValue item)
		{
			return _index.TryGetValue(key, out item);
		}
	}
}