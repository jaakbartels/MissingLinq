using System;
using System.Linq;
using NUnit.Framework;

namespace MissingLinq.UnitTests
{
	[TestFixture]
	public class Test_SelectAllCombinations
	{
		[Test]
		public void ReturnsEmptySetIfOriginalListIsEmpty()
		{
			var list = Query.Empty<int>();
			var result = list.SelectAllCombinations();
			Assert.IsTrue(result.IsEmpty());
		}

		[Test]
		public void ReturnsEmptySetIfOriginalListContainsOneElement()
		{
			var array = new[] { 1 };
			var result = array.SelectAllCombinations();
			Assert.IsTrue(result.IsEmpty());
		}

		[Test]
		public void ReturnsOnePairIfArgumentContainsTwoElements()
		{
			var array = new[] { 1, 2 };
			var result = array.SelectAllCombinations().ToList();

			Assert.AreEqual(1, result.Count);
			Tuple<int, int> first = result.First();
			Assert.IsTrue(first.Contains(1) && first.Contains(2));
		}

		[Test]
		public void ReturnsExpectedPairs()
		{
			var array = new[] { 1, 2, 3, 4 };

			var result = array.SelectAllCombinations().ToList();

			Assert.AreEqual(6, result.Count);
			Assert.IsTrue(result.Any(p => p.Contains(1) && p.Contains(2)));
			Assert.IsTrue(result.Any(p => p.Contains(1) && p.Contains(3)));
			Assert.IsTrue(result.Any(p => p.Contains(1) && p.Contains(4)));
			Assert.IsTrue(result.Any(p => p.Contains(2) && p.Contains(3)));
			Assert.IsTrue(result.Any(p => p.Contains(2) && p.Contains(4)));
			Assert.IsTrue(result.Any(p => p.Contains(3) && p.Contains(4)));
		}


		[Test]
		public void ReturnsEmptySetIfOneOfOriginalListsIsEmpty()
		{
			var list1 = Query.Empty<int>().ToList();
			var list2 = new[] {1, 2, 3, 4};

			var result = list1.SelectAllCombinations(list2);
			Assert.IsTrue(result.IsEmpty());

			result = list2.SelectAllCombinations(list1);
			Assert.IsTrue(result.IsEmpty());
		}

		[Test]
		public void ReturnsExpectedPairs2()
		{
			var list1 = new[] { "1", "2" };
			var list2 = new[] { "A", "B" };

			var result = list1.SelectAllCombinations(list2).ToList();

			Assert.AreEqual(4, result.Count);
			Assert.IsTrue(result.Any(p => p.Contains("1") && p.Contains("A")));
			Assert.IsTrue(result.Any(p => p.Contains("1") && p.Contains("B")));
			Assert.IsTrue(result.Any(p => p.Contains("2") && p.Contains("A")));
			Assert.IsTrue(result.Any(p => p.Contains("2") && p.Contains("B")));
		}
	}

}
