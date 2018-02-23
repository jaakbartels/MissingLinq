using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace MissingLinq.UnitTests
{
  [TestFixture]
  public class Test_WhereKey
  {
    [Test]
    public void WherekeyInReturnsOnlyItemsInRightList()
    {
      var leftList = new[] {1, 2, 3, 4, 5, 6, 7, 8, 9};
      var rightList = new[] {2, 3, 5, 10};

      var result = leftList
        .WhereKey(i => i)
        .In(rightList);

      CollectionAssert.AreEquivalent(new[] {2, 3, 5}, result.ToArray());
    }

    [Test]
    public void WherekeyWorksWithPrecalculatedLookup()
    {
      var leftList = new[] {1, 2, 3, 4, 5, 6, 7, 8, 9};
      var rightList = new[] {2, 3, 5, 10}.ToLookup(x => x);

      var result = leftList
        .WhereKey(i => i)
        .In(rightList);

      CollectionAssert.AreEquivalent(new[] {2, 3, 5}, result.ToArray());
    }

    [Test]
    public void WherekeyWorksWithPrecalculatedDictionary()
    {
      var leftList = new[] {1, 2, 3, 4, 5, 6, 7, 8, 9};

      var rightList = new[]
      {
          new{id=2, name="two"},
          new{id=3, name="three"},
          new{id=5, name="five"},
          new{id=10, name="ten"}
      }.ToDictionary(x => x.id, x => x.name);

      var result = leftList
        .WhereKey(i => i)
        .In(rightList);

      CollectionAssert.AreEquivalent(new[] {2, 3, 5}, result.ToArray());
    }    
      
    [Test]
    public void WherekeyWorksWithPrecalculatedHashSet()
    {
      var leftList = new[] {1, 2, 3, 4, 5, 6, 7, 8, 9};

      var rightList = new HashSet<int> {2,3,5,10};

      var result = leftList
        .WhereKey(i => i)
        .In(rightList);

      CollectionAssert.AreEquivalent(new[] {2, 3, 5}, result.ToArray());
    }

    [Test]
    public void WherekeyNotinReturnsOnlyItemsNotInRightList()
    {
      var leftList = new[] {1, 2, 3, 4, 5, 6, 7, 8, 9};
      var rightList = new[] {2, 3, 5, 10};

      var result = leftList
        .WhereKey(i => i)
        .NotIn(rightList);

      CollectionAssert.AreEquivalent(new[] {1, 4, 6, 7, 8, 9}, result.ToArray());
    }

    [Test]
    public void ReturnsValuesFromLeftList()
    {
      var leftList = new[]
                       {
                         new {ID = 1, Name = "JaSteLiX"},
                         new {ID = 2, Name = "ThiXa"}
                       };

      var result = leftList.WhereKey(v => v.Name).In(new[] {"JaSteLiX", "WiBar"});

      Assert.AreSame(leftList[0], result.First());
    }


    [Test]
    public void WherekeyInReturnsEmptyListWhenLeftAndRightHaveNoCommonElements()
    {
      var leftList = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
      var rightList = new[] { 10, 12};

      var result = leftList
        .WhereKey(i => i)
        .In(rightList);

      Assert.IsTrue(!result.Any());
    }


    [Test]
    public void WherekeyNotinReturnsEmptyListWhenLeftAndRightAreSame()
    {
      var list = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

      var result = list
        .WhereKey(i => i)
        .NotIn(list);

      Assert.IsTrue(!result.Any());
    }
  }


}
