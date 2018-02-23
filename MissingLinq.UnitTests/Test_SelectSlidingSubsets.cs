using System.Linq;
using NUnit.Framework;

namespace MissingLinq.UnitTests
{
	[TestFixture]
	public class Test_SelectSlidingSubsets
	{
		[Test]
		public void ReturnsNothingIfSourceCollectionIsSmallerThanSubsetSize()
		{
			var data = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
			Assert.IsEmpty(data.SelectSlidingSubsets(10));
		}

		[Test]
		public void ReturnsCompleteSetIfSubsetSizeIsEqualToSourceCollectionSize()
		{
			var data = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
			CollectionAssert.AreEqual(data, data.SelectSlidingSubsets(9).First());
		}

		[Test]
		public void AllReturnedSubsetsHaveRequestedLength()
		{
			var data = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

			Assert.IsTrue(data.SelectSlidingSubsets(3).All(sub => sub.Count() == 3));
		}

		[Test]
		public void ReturnsCorrectSubsets()
		{
			var data = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

			CollectionAssert.AreEqual(
				new[]
				{
					new []{1,2,3}, 
					new []{2,3,4},
					new []{3,4,5},
					new []{4,5,6},
					new []{5,6,7},
					new []{6,7,8},
					new []{7,8,9}
				}, data.SelectSlidingSubsets(3));
		}

		[Test, ExpectedException]
		public void ThrowsWhenSubsetSizeSmallerThan1()
		{
			var data = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
			data.SelectSlidingSubsets(0);
		}
	}
}
