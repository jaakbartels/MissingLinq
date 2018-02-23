using System.Collections.Generic;
using System.Linq;

namespace MissingLinq
{
	public static class Query
	{
		public static IEnumerable<T> Empty<T>()
		{
			return new T[0].AsEnumerable();
		}

		public static IEnumerable<T> From<T>(params T[] elements)
		{
			return elements.AsEnumerable();
		}
	}
}
