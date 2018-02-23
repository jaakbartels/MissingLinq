using System;

namespace MissingLinq
{
	public interface IPartialJoinWithRightList<TLeft, TRight>
	{
		IPartialJoinWithLeftKey<TLeft, TRight, TKey> On<TKey>(Func<TLeft, TKey> leftKeyFunc);
	}
}