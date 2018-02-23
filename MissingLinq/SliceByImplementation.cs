using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MissingLinq
{
	public partial class SliceByImplementation<TKey, TValue> : IEnumerable<IGrouping<TKey, TValue>>
	{
		public SliceByImplementation(IEnumerable<TValue> elements, Func<TValue, TKey> keyFunc)
		{
			_elements = elements;
			_keyFunc = keyFunc;
		}

		private readonly IEnumerable<TValue> _elements;
		private readonly Func<TValue, TKey> _keyFunc;

		public IEnumerator<IGrouping<TKey, TValue>> GetEnumerator()
		{
			if (_elements.Any())
			{
				var value = _elements.First();
				var oldKey = _keyFunc(value);
				var group = new Grouping<TKey, TValue>(oldKey);
				group.Add(value);
				foreach (var v in _elements.Skip(1))
				{
					var key = _keyFunc(v);
					if (!Equals(key, oldKey))
					{
						yield return group;
						group = new Grouping<TKey, TValue>(key);
						oldKey = key;
					}
					group.Add(v);
				}
				yield return group;
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}