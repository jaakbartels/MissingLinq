using System.Collections.Generic;
using System.Linq;

namespace MissingLinq
{
	public class Grouping<TKey, TValue> : List<TValue>, IGrouping<TKey, TValue>
	{
		public Grouping(TKey key)
		{
			Key = key;
		}

		public Grouping(TKey key, IEnumerable<TValue> values) : base(values)
		{
			Key = key;
		}

		public TKey Key { get; private set; }
	}
}