using System;
using System.Collections.Generic;

namespace MissingLinq
{
	public class UniqueIndexImplementation<TListItem, TKey, TValue> : IUniqueIndex<TKey, TValue>
	{
		protected Dictionary<TKey, TValue> _dict;

		//using a list to store the value with null-key
		//this allows for easy testing on the presence of a null-key value
		protected List<TValue> _itemWithNullKey;

		public UniqueIndexImplementation(IEnumerable<TListItem> list, Func<TListItem, TKey> keyFunc, Func<TListItem, TValue> valueSelector)
		{
			_dict = new Dictionary<TKey, TValue>();
			_itemWithNullKey = new List<TValue>();
			list.ForEach(v => AddElement(keyFunc(v), valueSelector(v)));
		}

		protected void AddElement(TKey key, TValue v)
		{
			if (ReferenceEquals(key, null))
			{
				if (_itemWithNullKey.Count > 0) throw new ArgumentException("Duplicate Key");
				_itemWithNullKey.Add(v);
			}
			else
			{
				_dict.Add(key, v);
			}
		}

		public TValue Find(TKey key)
		{
			if (ReferenceEquals(key, null))
			{
				return _itemWithNullKey.FirstOrThrow(() => new KeyNotFoundException());
			}
			return _dict[key];
		}

		public TValue this[TKey key]
		{
			get { return Find(key); }
		}

		public bool Contains(TKey key)
		{
			return ReferenceEquals(key, null) ? _itemWithNullKey.Count > 0 : _dict.ContainsKey(key);
		}

		public bool TryGetValue(TKey key, out TValue item)
		{
			return _dict.TryGetValue(key, out item);
		}
	}
}