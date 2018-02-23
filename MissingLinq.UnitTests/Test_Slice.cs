using System.Linq;
using NUnit.Framework;

namespace MissingLinq.UnitTests
{
  [TestFixture]
  public class Test_Slice
  {
    [Test]
    public void YieldsSpecifiedSlicesize()
    {
      var list = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 };

      var result = list.Slice(6);

      Assert.AreEqual(6, result.First().Count());
    }

    [Test]
    public void YieldsExpectedNumberOfSlices()
    {
      var list = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 };

      var result = list.Slice(6);

      Assert.AreEqual(2, result.Count());
    }

    [Test]
    public void YieldsAllElements()
    {
      var list = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 };

      var result = list.Slice(3);

      CollectionAssert.AreEquivalent(list, result.SelectMany(s => s.Select(i => i)).ToArray());
    }

    [Test]
    public void SmallerSliceIsReturnedLast()
    {
      var list = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 };

      var result = list.Slice(3);

      Assert.AreEqual(1, result.Last().Count());
    }

    
  }
}
