using System;
using System.Collections;
using System.Collections.Generic;

namespace MissingLinq
{
	public class ZipImplementation<TLeft, TRight> : IEnumerable<Tuple<TLeft, TRight>>
	{
		public ZipImplementation(IEnumerable<TLeft> leftSequence, IEnumerable<TRight> rightSequence)
		{
			_leftSequence = leftSequence;
			_rightSequence = rightSequence;
		}

		private IEnumerable<TLeft> _leftSequence;
		private IEnumerable<TRight> _rightSequence;
 
		public IEnumerator<Tuple<TLeft, TRight>> GetEnumerator()
		{
			var leftEnum = _leftSequence.GetEnumerator();
			var rightEnum = _rightSequence.GetEnumerator();

			while (leftEnum.MoveNext() && rightEnum.MoveNext())
			{
				yield return Tuple.Create(leftEnum.Current, rightEnum.Current);
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}