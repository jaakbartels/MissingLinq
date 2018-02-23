using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace MissingLinq.UnitTests
{
	[TestFixture]
	class Test_Split
	{
		[Test]
		public void ReturnsSubcollections()
		{
			var sut = new[] { 1, 2, 3, 0, 4, 5, 6, 7, 0, 8, 9 };

			var result = sut.Split(0).ToList();

			Assert.AreEqual(3, result.Count);
			CollectionAssert.AreEqual(new[] { 1, 2, 3 }, result[0]);
			CollectionAssert.AreEqual(new[] { 4, 5, 6, 7 }, result[1]);
			CollectionAssert.AreEqual(new[] { 8, 9 }, result[2]);
		}

		[Test]
		public void TreatsDoubleSeperatorsAsOne()
		{
			var sut = new[] { 1, 2, 3, 0, 0, 4 };

			var result = sut.Split(0).ToList();

			Assert.AreEqual(2, result.Count);
			CollectionAssert.AreEqual(new[] { 1, 2, 3 }, result[0]);
			CollectionAssert.AreEqual(new[] { 4 }, result[1]);
		}

		[Test]
		public void ReturnsEmptyListIfSourceIsEmpty()
		{
			var sut = Query.Empty<int>();

			var result = sut.Split(0).ToList();

			Assert.IsTrue(result.IsEmpty());
		}

		[Test]
		public void SuppressesEmptySubcollectionAtStart()
		{
			var sut = new[] { 0, 4 };

			var result = sut.Split(0).ToList();

			Assert.AreEqual(1, result.Count);
			CollectionAssert.AreEqual(new[] { 4 }, result[0]);
		}

		[Test]
		public void SuppressesEmptySubcollectionAtEnd()
		{
			var sut = new[] { 1, 0 };

			var result = sut.Split(0).ToList();

			Assert.AreEqual(1, result.Count);
			CollectionAssert.AreEqual(new[] { 1 }, result[0]);
		}

		[Test]
		public void UsesCustomComparer()
		{
			var sut = new[] { 2, 3, -1, 4, 5, 6, 7, 1, 8, 9 };

			var result = sut.Split(1, new CustomComparer()).ToList(); //should split on 1 and on -1

			Assert.AreEqual(3, result.Count);
			CollectionAssert.AreEqual(new[] { 2, 3 }, result[0]);
			CollectionAssert.AreEqual(new[] { 4, 5, 6, 7 }, result[1]);
			CollectionAssert.AreEqual(new[] { 8, 9 }, result[2]);
		}

		private class CustomComparer : IEqualityComparer<int>
		{
			public bool Equals(int x, int y)
			{
				return Math.Abs(x).Equals(Math.Abs(y));
			}

			public int GetHashCode(int x)
			{
				return Math.Abs(x);
			}
		}
	}
}
