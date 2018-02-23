using System;
using System.Collections.Generic;

namespace MissingLinq
{
	public interface IPartialJoinWithLeftKey<TLeft, TRight, TKey>
	{
		IEnumerable<Tuple<TLeft, TRight>> Equals(Func<TRight, TKey> rightKeyFunc);
	}
}