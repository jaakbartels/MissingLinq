using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace MissingLinq
{
	public class SlidingWindowImplementation<T> : IEnumerable<IEnumerable<T>>
	{
		private readonly IEnumerable<T> _list;
		private readonly int _subsetSize;

		public SlidingWindowImplementation(IEnumerable<T> list, int subsetSize)
		{
		    if (subsetSize <= 0)
		    {
		        throw new Exception("Sliding subset size should be greater than 0.");
		    }
			_list = list;
			_subsetSize = subsetSize;
		}

		public IEnumerator<IEnumerable<T>> GetEnumerator()
		{
			var subset = new List<T>(_subsetSize);

			foreach (T item in _list)
			{
				subset.Add(item);
				if (subset.Count == _subsetSize)
				{
					yield return subset;
					subset.RemoveAt(0);
				}
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}