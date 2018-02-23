using System;
using System.Collections.Generic;
using System.Linq;

namespace MissingLinq
{
	public class IndexImplementation<TItem, TKey, TValue> : IIndex<TKey, TValue>
	{
		protected readonly Dictionary<TKey, List<TValue>> _dict;
		protected readonly List<TValue> _itemsWithNullKey;

		public IndexImplementation(IEnumerable<TItem> list, Func<TItem, TKey> keyFunc, Func<TItem, TValue> valueSelector)
		{
			var groupedList = list.GroupBy(keyFunc, valueSelector).ToList();
			_dict = groupedList.Where(g => !ReferenceEquals(g.Key, null)).ToDictionary(g => g.Key, g => g.ToList());
			_itemsWithNullKey =
			  new List<TValue>(groupedList.Where(g => ReferenceEquals(g.Key, null)).SelectMany(g => g.Select(v => v)));
		}

		public IEnumerable<TValue> Find(TKey key)
		{
			if (ReferenceEquals(key, null)) return _itemsWithNullKey;

			List<TValue> v;
			if (_dict.TryGetValue(key, out v))
				return v;
			return new TValue[0];
		}

		public IEnumerable<TValue> this[TKey key]
		{
			get { return Find(key); }
		}

		public bool Contains(TKey key)
		{
			return (ReferenceEquals(key, null) && _itemsWithNullKey.Count > 0) 
				|| (!ReferenceEquals(key, null) &&  _dict.ContainsKey(key));
		}
	}
}