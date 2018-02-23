using System;
using System.Collections.Generic;

namespace MissingLinq
{
	struct PartialJoinWithRightList<TLeft, TRight> : IPartialJoinWithRightList<TLeft, TRight>
	{
		public PartialJoinWithRightList(IEnumerable<TLeft> leftCollection, IEnumerable<TRight> rightCollection, IJoinFactory factory)
		{
			_leftCollection = leftCollection;
			_rightCollection = rightCollection;
			_factory = factory;
		}

		private readonly IEnumerable<TLeft> _leftCollection;
		private readonly IEnumerable<TRight> _rightCollection;
		private readonly IJoinFactory _factory;

		public IPartialJoinWithLeftKey<TLeft, TRight, TKey> On<TKey>(Func<TLeft, TKey> leftKeyFunc)
		{
			return new PartialJoinWithLeftKey<TLeft, TRight, TKey>(_leftCollection, _rightCollection, leftKeyFunc, _factory);
		}
	}
}