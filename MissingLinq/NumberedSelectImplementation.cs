using System;
using System.Collections;
using System.Collections.Generic;

namespace MissingLinq
{
	class NumberedSelectImplementation<Tin, Tout> : IEnumerable<Tout>
	{
		public NumberedSelectImplementation(IEnumerable<Tin> collection, long firstNumber, Func<long, Tin, Tout> projection )
		{
			_collection = collection;
			_n = firstNumber;
			_projection = projection;
		}

		private readonly IEnumerable<Tin> _collection;
		private long _n;
		private readonly Func<long, Tin, Tout> _projection;

		public IEnumerator<Tout> GetEnumerator()
		{
			foreach (Tin item in _collection)
			{
				yield return _projection(_n++, item);
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}