using System;
using System.Collections.Generic;

namespace MissingLinq
{
	public class ObservingIndex<TKey, TValue> : IndexImplementation<TValue, TKey, TValue>
	{
		private readonly Func<TValue, TKey> _keyFunc;

		public ObservingIndex(ObservableList<TValue> values, Func<TValue, TKey> keyFunc)
			: base(values, keyFunc, v=> v)
		{
			_keyFunc = keyFunc;
			values.Added += (sender, args) => AddElement(args.Element);
			values.Removed += (sender, args) => RemoveElement(args.Element);
			values.Cleared += (sender, args) => Clear();
		}

		private void RemoveElement(TValue element)
		{
			List<TValue> grp;
			var key = _keyFunc(element);
			if (ReferenceEquals(key, null))
			{
				_itemsWithNullKey.Remove(element);
			}
			else if (_dict.TryGetValue(key, out grp))
			{
				grp.Remove(element);
				if (grp.Count == 0) _dict.Remove(key);
			}
		}

		private void AddElement(TValue element)
		{
			List<TValue> grp;
			var key = _keyFunc(element);
			if (ReferenceEquals(key, null))
			{
				_itemsWithNullKey.Add(element);
			}
			else
			{
				if (!_dict.TryGetValue(key, out grp))
				{
					grp = new List<TValue>();
					_dict.Add(key, grp);
				}
				grp.Add(element);
			}
		}

		private void Clear()
		{
			_itemsWithNullKey.Clear();
			_dict.Clear();
		}
	}
}