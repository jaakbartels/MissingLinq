using System;
using System.Linq;
using NUnit.Framework;

namespace MissingLinq.UnitTests
{
	[TestFixture]
	public class Test_JoinLeft
	{
		[Test]
		public void PairsElementsThatExistInBothLists()
		{
			var left = new[] { 3, 6 };
			var right = new[] { 1, 2, 3, 4, 5, 6, 7, 8 };

			var result = left.JoinLeft(right, i => i, i => i).ToArray();

			Assert.IsTrue(result.Contains(Tuple.Create(3, 3)));
			Assert.IsTrue(result.Contains(Tuple.Create(6, 6)));
			Assert.AreEqual(2, result.Count());
		}

		[Test]
		public void ReturnsDuplicatePairsWhenLeftContainsDuplicates()
		{
			var left = new[] { 3, 6, 6 };
			var right = new[] { 1, 2, 3, 4, 5, 6, 7, 8 };

			var result = left.JoinLeft(right, i => i, i => i).ToArray();

			Assert.IsTrue(result.Contains(Tuple.Create(3, 3)));
			Assert.IsTrue(result.Count(t => t.Equals(Tuple.Create(6, 6))) == 2);
			Assert.AreEqual(3, result.Count());
		}
		[Test]

		public void ReturnsDuplicatePairsWhenRightContainsDuplicates()
		{
			var left = new[] { 3, 6 };
			var right = new[] { 1, 2, 3, 4, 5, 6, 3, 8, 3 };

			var result = left.JoinLeft(right, i => i, i => i).ToArray();

			Assert.IsTrue(result.Contains(Tuple.Create(6, 6)));
			Assert.IsTrue(result.Count(t => t.Equals(Tuple.Create(3, 3))) == 3);
			Assert.AreEqual(4, result.Count());
		}

		[Test]
		public void ReturnsDefaultWhenRightlistDoesNotContainElement()
		{
			var left = new[] { 3, 6, 11 };
			var right = new[] { 1, 2, 3, 4, 5, 6, 3, 8, 3 };

			var result = left.JoinLeft(right, i => i, i => i).ToArray();

			Assert.IsTrue(result.Contains(Tuple.Create(11, 0)));
		}

		[Test]
		public void CanUseSimplifiedApiIfLeftAndRightAreSameType()
		{
			var left = new[] { new { i = 3, s = "three" }, new { i = 6, s = "six" }, new { i = 11, s = "eleven" } };
			var right = new[] { new { i = 1, s = "one" } };

			var result = left.JoinLeft(right, i => i).ToArray();

			Assert.AreEqual(3, result.Count());
		}

		[Test]
		public void CanUseAlternativeSyntax()
		{
			var left = new[] { new { i = 3, s = "three" }, new { i = 6, s = "six" }, new { i = 11, s = "eleven" } };
			var right = new[] { new { i = 1, s = "one" } };

			var result = left
				.JoinLeft(right)
				.On(l => l.i)
				.Equals(r => r.i);

			Assert.AreEqual(3, result.Count());
		}
	}
}
