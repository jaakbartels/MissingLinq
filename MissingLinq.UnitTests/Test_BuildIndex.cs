using System;
using System.Linq;
using NUnit.Framework;

namespace MissingLinq.UnitTests
{
  [TestFixture]
  public class Test_BuildIndex
  {
    [Test]
    public void RetrievesSingleValue()
    {
      var values = new[]
                     {
                       Tuple.Create(5, "vijf"),
                       Tuple.Create(6, "zes"),
                       Tuple.Create(7, "zeven"),
                       Tuple.Create(8, "acht"),
                       Tuple.Create(1, "een"),
                       Tuple.Create(3, "drie"),
                       Tuple.Create(4, "vier")
                     };

      var idx = values.BuildIndex(v => v.Item1);

      Assert.AreEqual("vier", idx[4].First().Item2);
      Assert.AreEqual(1, idx.Find(4).Count());
    }

    [Test]
    public void HandlesDuplicateValues()
    {
      var values = new[]
                     {
                       Tuple.Create(1, "een"),
                       Tuple.Create(2, "twee"),
                       Tuple.Create(4, "vier"),
                       Tuple.Create(4, "four"),
                       Tuple.Create(5, "vijf"),
                       Tuple.Create(6, "zes"),
                       Tuple.Create(7, "zeven"),
                       Tuple.Create(8, "acht")
                     };

      var idx = values.BuildIndex(v => v.Item1);

      CollectionAssert.AreEquivalent(new[] {"vier", "four"},idx[4].Select(v => v.Item2).ToArray());
      Assert.AreEqual(2, idx.Find(4).Count());
    }

	[Test]
	public void ReturnsEmptyListWhenKeyNotFound()
	{
		var values = new[]
                     {
                       Tuple.Create(1, "een"),
                       Tuple.Create(2, "twee"),
                       Tuple.Create(4, "vier"),
                       Tuple.Create(4, "four")
                     };

		var idx = values.BuildIndex(v => v.Item1);

		Assert.AreEqual(0, idx.Find(8).Count());
	}

	[Test]
	public void CanBuildIndexWithElementSelector()
	{
		var values = new[]
                     {
                       Tuple.Create(1, "een") ,
                       Tuple.Create(2, "twee"),
                       Tuple.Create(4, "vier"),
                       Tuple.Create(4, "four")
                     };

		var idx = values.BuildIndex(v => v.Item1, v => v.Item2);

		CollectionAssert.AreEquivalent(new []{"vier", "four"}, idx.Find(4));
	}

	[Test]
    public void KeyCanBeNull()
    {
      var values = new[]
                     {
                       Tuple.Create(1, "een"),
                       Tuple.Create(2, "twee"),
                       Tuple.Create(4, (string)null),
                       Tuple.Create(4, "four")
                     };

      var idx = values.BuildIndex(v => v.Item2);

      Assert.AreEqual(1, idx.Find(null).Count());
    }
  }
}
