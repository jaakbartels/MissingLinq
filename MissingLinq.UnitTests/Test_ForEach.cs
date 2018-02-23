using System.Linq;
using NUnit.Framework;

namespace MissingLinq.UnitTests
{
  [TestFixture]
  public class Test_ForEach
  {
    [Test]
    public void RunsExpectedNumberOfTimes()
    {
      var listA = new[] { 1, 2, 3, 4 };
      var counter = 0;

      listA.ForEach(a => counter++);

      Assert.AreEqual(listA.Count(), counter);
    }

    [Test]
    public void EnumeratesAllElements()
    {
      var listA = new[] { 1, 2, 3, 4 };
      var sum = 0;

      listA.ForEach(a => sum+=a);

      Assert.AreEqual(1+2+3+4, sum);
    }
  }
}
