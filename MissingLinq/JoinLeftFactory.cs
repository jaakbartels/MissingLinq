using System;
using System.Collections.Generic;

namespace MissingLinq
{
	public class JoinLeftFactory : IJoinFactory
	{
		public IEnumerable<Tuple<TLeft, TRight>> CreateJoin<TLeft, TRight, TKey>(IEnumerable<TLeft> leftCollection, IEnumerable<TRight> rightCollection, Func<TLeft, TKey> leftKeyFunc,
			Func<TRight, TKey> rightKeyFunc)
		{
			return new JoinLeftImplementation<TLeft, TRight, TKey>(leftCollection, rightCollection, leftKeyFunc, rightKeyFunc);
		}
	}
}