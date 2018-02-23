using System;
using System.Collections;
using System.Collections.Generic;

namespace MissingLinq
{
	public class JoinLeftImplementation<TLeft, TRight,TKey> : IEnumerable<Tuple<TLeft, TRight>>
	{
		private readonly IEnumerable<TLeft> _left;
		private readonly IEnumerable<TRight> _right;
		private readonly Func<TLeft, TKey> _leftKeyFunc;
		private readonly Func<TRight, TKey> _rightKeyFunc;

		public JoinLeftImplementation(IEnumerable<TLeft> left, IEnumerable<TRight> right, Func<TLeft, TKey> leftKeyFunc, Func<TRight, TKey> rightKeyFunc)
		{
			_left = left;
			_right = right;
			_leftKeyFunc = leftKeyFunc;
			_rightKeyFunc = rightKeyFunc;
		}

		public IEnumerator<Tuple<TLeft, TRight>> GetEnumerator()
		{
			var idx = _right.BuildIndex(_rightKeyFunc);
			var enumerator = _left.GetEnumerator();
			while (enumerator.MoveNext())
			{
				var leftValue = enumerator.Current;
				TKey key = _leftKeyFunc(leftValue);
				if (idx.Contains(key))
				{
					foreach (var rightValue in idx.Find(key))
						yield return Tuple.Create(leftValue, rightValue);
				}
				else
				{
					yield return Tuple.Create(leftValue, default(TRight));
				}
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}