using System.Collections;
using System.Collections.Generic;

namespace MissingLinq
{
	public abstract class RangeBase<T> : IEnumerable<T>
	{
		/// <summary>
		/// Creates a range from start to end (inclusive)
		/// </summary>
		public RangeBase(T start, T end)
		{
			Start = start;
			End = end;
		}

		public T Start { get; set; }
		public T End { get; set; }

		public IEnumerator<T> GetEnumerator()
		{
			for (var current = Start; Contains(current); current = Next(current))
				yield return current;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		protected abstract T Next(T current);
		public abstract bool Contains(T item);
	}
}