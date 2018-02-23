using System;
using NUnit.Framework;

namespace MissingLinq.UnitTests
{
  [TestFixture]
  public class Test_UniqueIndex
  {
    [Test]
    public void CanRetrieveValues()
    {
      var list = new[] { new TestObject(1, "one"), new TestObject(2, "two"), new TestObject(3, "three") };

      var listByName = list.BuildUniqueIndex(v => v.Name);

      Assert.AreEqual(2, listByName.Find("two").Id);
    }

    [Test]
    public void ContainsReturnsTrue()
    {
      var list = new[] { new TestObject(1, "one"), new TestObject(2, "two"), new TestObject(3, "three") };

      var listByName = list.BuildUniqueIndex(v => v.Name);

      Assert.IsTrue(listByName.Contains("two"));
    }

    [Test]
    public void ContainsReturnsFalse()
    {
      var list = new[] { new TestObject(1, "one"), new TestObject(2, "two"), new TestObject(3, "three") };

      var listByName = list.BuildUniqueIndex(v => v.Name);

      Assert.IsFalse(listByName.Contains("four"));
    }

    [Test]
    public void KeyCanBeNull()
    {
      var list = new[] { new TestObject(1, "one"), new TestObject(2, null), new TestObject(3, "three") };

      var listByName = list.BuildUniqueIndex(v => v.Name);

      Assert.IsTrue(listByName.Contains(null));
    }

    [Test, ExpectedException(typeof(ArgumentException))]
    public void ThrowsIfListContainsDuplicateKey()
    {
      var list = new[] { new TestObject(1, "one"), new TestObject(2, "two"), new TestObject(3, "two") };

      list.BuildUniqueIndex(v => v.Name);
    }

    [Test]
    [ExpectedException(typeof(ArgumentException))]
    public void ThrowsIfListContainsDuplicateNullkey()
    {
      var list = new[] { new TestObject(1, "one"), new TestObject(2, null), new TestObject(3, null) };

      list.BuildUniqueIndex(v => v.Name);
    }
  }
}