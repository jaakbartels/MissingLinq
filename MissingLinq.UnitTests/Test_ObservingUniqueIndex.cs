using System.Linq;
using NUnit.Framework;

namespace MissingLinq.UnitTests
{
  [TestFixture]
  public class Test_ObservingUniqueIndex
  {
    private ObservableList<TestObject> _items;
    private IUniqueIndex<string, TestObject> _itemsByName;
    private IUniqueIndex<int, TestObject> _itemsById;

    [SetUp]
    public void Setup()
    {
      _items = new ObservableList<TestObject>(new []
        {
          new TestObject(1, "one"),
          new TestObject(2, "two")
        } );
      _itemsByName = _items.BuildUniqueIndex(v => v.Name);
      _itemsById = _items.BuildUniqueIndex(v => v.Id);
    }

    [Test]
    public void CanRetrieveValues()
    {
      var v = _itemsById.Find(1);

      Assert.AreEqual("one", v.Name);
    }

    [Test]
    public void can_add_values()
    {
      _items.Add(new TestObject(3, "three"));

      var v = _itemsById.Find(3);

      Assert.AreEqual("three", v.Name);
    }

    [Test]
    public void KeyCanBeNull()
    {
      _items.Add(new TestObject(3, null));

      var v = _itemsByName.Find(null);

      Assert.AreEqual(3, v.Id);
    }

    [Test]
    public void CanRemoveValues()
    {
      var v = _itemsById.Find(2);

      _items.Remove(v);

      Assert.IsFalse(_itemsById.Contains(2));
    }

    [Test]
    public void CanRemoveAllValues()
    {
      var allItems = _items.ToArray();
      allItems.ForEach(item => _items.Remove(item));

      Assert.IsFalse(_itemsById.Contains(1));
      Assert.IsFalse(_itemsById.Contains(2));
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
  }
}