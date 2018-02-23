using NUnit.Framework;

namespace MissingLinq.UnitTests
{
	[TestFixture]
	public class Test_KeyedList
	{
		private KeyedList<int, int> _keyedList;
		private int _items;

		[SetUp]
		public void Setup()
		{
			_items = 0;
			_keyedList = new KeyedList<int, int>(i => i * i);
			_keyedList.Added += KeyedList_Added;
			_keyedList.Removed += KeyedList_Removed;
			_keyedList.Cleared += KeyedList_Cleared;
			_keyedList.AddRange(new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 });

		}

		[TearDown]
		public void TearDown()
		{
			_keyedList.Added -= KeyedList_Added;
			_keyedList.Removed -= KeyedList_Removed;
			_keyedList.Cleared -= KeyedList_Cleared;
		}

		private void KeyedList_Added(object sender, ObservableListEventArgs<int> eventArgs)
		{
			_items++;
		}

		private void KeyedList_Removed(object sender, ObservableListEventArgs<int> eventArgs)
		{
			_items--;
		}

		private void KeyedList_Cleared(object sender, ObservableListEventArgs<int> eventArgs)
		{
			_items=0;
		}

		[Test]
		public void CanRetrieveByKey()
		{
			Assert.AreEqual(9, _keyedList[81]);
		}

		[Test]
		public void ContainsValue()
		{
			Assert.IsTrue(_keyedList.ContainsValue(9));
		}

		[Test]
		public void ContainsKey()
		{
			Assert.IsTrue(_keyedList.ContainsKey(81));
		}

		[Test]
		public void ContainsValueNegative()
		{
			Assert.IsFalse(_keyedList.ContainsValue(19));
		}

		[Test]
		public void ContainsKeyNegative()
		{
			Assert.IsFalse(_keyedList.ContainsKey(181));
		}

		[Test]
		public void TryGetItem()
		{
			int result;
			Assert.IsTrue(_keyedList.TryGetItem(81, out result));
			Assert.AreEqual(9, result);
		}

		[Test]
		public void TryGetItemNegative()
		{
			int result;
			Assert.IsFalse(_keyedList.TryGetItem(181, out result));
		}

		[Test]
		public void CallingAddUpdatesIndex()
		{
			_keyedList.Add(12);
			Assert.IsTrue(_keyedList.ContainsKey(144));
		}

		[Test]
		public void CallingRemoveUpdatesIndex()
		{
			_keyedList.RemoveValue(9);
			Assert.IsFalse(_keyedList.ContainsKey(81));
		}

		[Test]
		public void AddedEventFiresOnAddRange()
		{
			Assert.AreEqual(_keyedList.Count, _items);
		}

		[Test]
		public void AddedEventFiresOnAdd()
		{
			_keyedList.Add(12);
			Assert.AreEqual(_keyedList.Count, _items);
		}

		[Test]
		public void RemoveEventFiresOnRemoveValue()
		{
			_keyedList.RemoveValue(3);
			Assert.AreEqual(_keyedList.Count, _items);
		}

		[Test]
		public void RemoveEventFiresOnRemoveByKey()
		{
			_keyedList.RemoveByKey(9);
			Assert.AreEqual(_keyedList.Count, _items);
		}

		[Test]
		public void RemoveEventFiresOnRemoveAll()
		{
			_keyedList.RemoveAll(v => (v & 1) == 0);
			Assert.AreEqual(_keyedList.Count, _items);
		}

		[Test]
		public void ClearEventFiresOnClear()
		{
			_keyedList.Clear();
			Assert.AreEqual(0, _items);
		}


	}
}
