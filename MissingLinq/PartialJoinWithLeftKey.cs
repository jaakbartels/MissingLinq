using System;
using System.Collections.Generic;

namespace MissingLinq
{
	struct PartialJoinWithLeftKey<TLeft, TRight, TKey> : IPartialJoinWithLeftKey<TLeft, TRight, TKey>
	{
		public PartialJoinWithLeftKey(IEnumerable<TLeft> leftCollection, IEnumerable<TRight> rightCollection, Func<TLeft, TKey> leftKeyFunc, IJoinFactory factory)
		{
			_leftCollection = leftCollection;
			_rightCollection = rightCollection;
			_leftKeyFunc = leftKeyFunc;
			_factory = factory;
		}

		private readonly IEnumerable<TLeft> _leftCollection;
		private readonly IEnumerable<TRight> _rightCollection;
		private readonly Func<TLeft, TKey> _leftKeyFunc;
		private readonly IJoinFactory _factory;

		public IEnumerable<Tuple<TLeft, TRight>> Equals(Func<TRight, TKey> rightKeyFunc)
		{
			return _factory.CreateJoin(_leftCollection, _rightCollection, _leftKeyFunc, rightKeyFunc);
		}
	}
}