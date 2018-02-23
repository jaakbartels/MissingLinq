using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace MissingLinq.UnitTests
{
	[TestFixture]
	public class Test_Query
	{
		[Test]
		public void Empty_returns_empty_enumerable_of_requested_type()
		{
			IEnumerable<int> sut = Query.Empty<int>();

			Assert.AreEqual(0, sut.Count());
			Assert.IsInstanceOf<IEnumerable<int>>(sut);
		}

		[Test]
		public void From_creates_enumerable_with_given_elements()
		{
			var sut = Query.From(0, 1, 2, 3);

			Assert.AreEqual(4, sut.Count());
			CollectionAssert.AreEqual(new int[]{0,1,2,3}, sut.ToArray());
		}
	}
}
