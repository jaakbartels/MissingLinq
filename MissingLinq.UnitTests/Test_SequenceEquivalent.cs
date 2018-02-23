using System;
using NUnit.Framework;

namespace MissingLinq.UnitTests
{
	[TestFixture]
	public class Test_SequenceEquivalent
	{
		[Test]
		public void ReturnsTrue()
		{
			Assert.IsTrue(new[] { 1, 2 }.SequenceEquivalent(new[] { 1, 2 }));
			Assert.IsTrue(new[] { 1, 2 }.SequenceEquivalent(new[] { 2, 1 }));
			Assert.IsTrue(new[] { 1, 2, 3, 4, 5 }.SequenceEquivalent(new[] { 1, 2, 5, 4, 3 }));
			Assert.IsTrue(new[] { 1, 2, 2, 2 }.SequenceEquivalent(new[] { 2, 1, 2, 2 }));
		}

		[Test]
		public void ReturnsFalse()
		{
			Assert.IsFalse(new[] { 1, 2 }.SequenceEquivalent(null));
			Assert.IsFalse(new byte[0].SequenceEquivalent(null));
			Assert.IsFalse(new[] { 1, 2 }.SequenceEquivalent(new[] { 1, 3 }));
			Assert.IsFalse(new[] { 1, 1, 2 }.SequenceEquivalent(new[] { 2, 1, 2 }));
			Assert.IsFalse(new[] { 1, 2, 3, 4, 5 }.SequenceEquivalent(new[] { 1, 2, 5, 4 }));
		}

		[Test]
		public void UsesComparer()
		{
			var comparer = StringComparer.CurrentCultureIgnoreCase;
			Assert.IsTrue(new[] { "a", "B" }.SequenceEquivalent(new[] { "b", "A" }, comparer));
		}
	}
}
