using System.Linq;
using NUnit.Framework;

namespace MissingLinq.UnitTests
{
  [TestFixture]
  public class Test_ObservingIndex
  {
    private ObservableList<TestObject> _items;
    private IIndex<string, TestObject> _itemsByName;
    private IIndex<int, TestObject> _itemsById;

    [SetUp]
    public void Setup()
    {
      _items = new ObservableList<TestObject>(new []
        {
          new TestObject(1, "one"),
          new TestObject(2, "two")
        } );
      _itemsByName = _items.BuildIndex(v => v.Name);
      _itemsById = _items.BuildIndex(v => v.Id);
    }

    [Test]
    public void AddsNewElementToIndices()
    {
      _items.Add(new TestObject(3, "three"));

      Assert.AreEqual("three", _itemsById[3].First().Name);
      Assert.AreEqual(3, _itemsByName["three"].First().Id);
    }

    [Test]
    public void CanAddElementWithSameKey()
    {
      _items.Add(new TestObject(1, "een"));

      Assert.AreEqual(2, _itemsById[1].Count());
    }

    [Test]
    public void FindReturnsEmptyListAfterRemovalOfLastObjectWithKey()
    {
      _items.Remove(new TestObject(1, "one"));

      Assert.IsTrue(_itemsById.Find(1).IsEmpty());
    }

    [Test]
    public void OtherObjectsWithSameKeyAreNotRemovedFromIndex()
    {
      _items.Add(new TestObject(1, "een"));
      _items.Remove(new TestObject(1, "one"));

      Assert.AreEqual(1, _itemsById.Find(1).Count());
      Assert.AreEqual("een", _itemsById.Find(1).First().Name);
    }

    [Test]
	public void IndexIsEmptyAfterClear()
	{
		_items.Clear();

		Assert.IsFalse(_itemsById.Contains(1));
		Assert.IsFalse(_itemsById.Contains(2));
		Assert.IsFalse(_itemsByName.Contains("one"));
		Assert.IsFalse(_itemsByName.Contains("two"));
	}

	[Test]
    public void KeyCanBeNull()
    {
      _items.Add(new TestObject(99, null));

      Assert.IsTrue(_itemsByName.Find(null).Count() == 1);
    }
  }
}
