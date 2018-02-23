using System.Collections;
using System.Collections.Generic;

namespace MissingLinq
{
	public class SplitImplementation<T> : IEnumerable<IEnumerable<T>>
	{
		public SplitImplementation(IEnumerable<T> collection, T separator, IEqualityComparer<T> comparer)
		{
			_collection = collection;
			_separator = separator;
			_comparer = comparer;
		}

		private readonly T _separator;
		private readonly IEqualityComparer<T> _comparer;
		private readonly IEnumerable<T> _collection;

		public IEnumerator<IEnumerable<T>> GetEnumerator()
		{
			var accumulated = new List<T>();
			foreach (var item in _collection)
			{
				if (_comparer.Equals(item, _separator))
				{
					if (accumulated.Count > 0)
					{
						yield return accumulated;
						accumulated = new List<T>();
					}
				}
				else
				{
					accumulated.Add(item);
				}
			}
			if (accumulated.Count > 0)
			{
				yield return accumulated;
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}