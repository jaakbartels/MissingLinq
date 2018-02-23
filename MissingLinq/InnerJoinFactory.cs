using System;
using System.Collections.Generic;
using System.Linq;

namespace MissingLinq
{
	public class InnerJoinFactory : IJoinFactory
	{
		public IEnumerable<Tuple<TLeft, TRight>> CreateJoin<TLeft, TRight, TKey>(IEnumerable<TLeft> leftCollection, IEnumerable<TRight> rightCollection, Func<TLeft, TKey> leftKeyFunc,
			Func<TRight, TKey> rightKeyFunc)
		{
			return leftCollection.Join<TLeft, TRight, TKey, Tuple<TLeft, TRight>>(rightCollection, leftKeyFunc, rightKeyFunc, Tuple.Create);
		}
	}
}