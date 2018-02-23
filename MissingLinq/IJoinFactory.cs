using System;
using System.Collections.Generic;

namespace MissingLinq
{
	interface IJoinFactory
	{
		IEnumerable<Tuple<TLeft, TRight>> CreateJoin<TLeft, TRight, TKey>(
			IEnumerable<TLeft> leftCollection,
			IEnumerable<TRight> rightCollection, 
			Func<TLeft, TKey> leftKeyFunc, 
			Func<TRight, TKey> rightKeyFunc);
	}
}