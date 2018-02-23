using System;
using System.Linq;
using NUnit.Framework;

namespace MissingLinq.UnitTests
{
  [TestFixture]
  public class Test_SelectAdjacentPairs
  {
    [Test]
    public void ReturnsEmptyListIfSourceIsEmpty()
    {
      var list = new int[0];
      var result = list.SelectAdjacentPairs();
      Assert.IsTrue(result.IsEmpty());
    }

    [Test]
    public void ReturnsEmptyListIfSourceHasOnlyOneElement()
    {
      var list = new[] { 1 };
      var result = list.SelectAdjacentPairs();
      Assert.IsTrue(result.IsEmpty());
    }

    [Test]
    public void ReturnsExpectedListOfTuples()
    {
      var list = new []{1,2,3,4};
      var result = list.SelectAdjacentPairs();
      CollectionAssert.AreEqual(new[] { Tuple.Create(1, 2), Tuple.Create(2, 3), Tuple.Create(3, 4)}, result.ToArray());
    }
  }
}
