using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace MissingLinq.UnitTests
{
	[TestFixture]
	public class Test_DictionaryExtensions
	{
		public class Item : IEquatable<Item>
		{
			public string Name { get; }

			public Item(string name)
			{
				Name = name;
			}

			public bool Equals(Item other)
			{
				if (ReferenceEquals(null, other)) return false;
				if (ReferenceEquals(this, other)) return true;
				return string.Equals(Name, other.Name);
			}

			public override bool Equals(object obj)
			{
				if (ReferenceEquals(null, obj)) return false;
				if (ReferenceEquals(this, obj)) return true;
				if (obj.GetType() != this.GetType()) return false;
				return Equals((Item)obj);
			}

			public override int GetHashCode()
			{
				return (Name != null ? Name.GetHashCode() : 0);
			}

			public static bool operator ==(Item left, Item right)
			{
				return Equals(left, right);
			}

			public static bool operator !=(Item left, Item right)
			{
				return !Equals(left, right);
			}
		}

		public class ItemEq : IEqualityComparer<Item>
		{
			public bool Equals(Item x, Item y)
			{
				if (ReferenceEquals(x, y)) return true;
				return string.Equals(x?.Name, y.Name);
			}

			public int GetHashCode(Item obj)
			{
				return obj?.GetHashCode() ?? 0;
			}
		}

		[Test]
		public void ContainsKeyWithValueReturnsTrue_String()
		{
			var dictionary = new Dictionary<int, string> { { 0, "zero"}, {1, "one"}, {2, "two"}, {3, "three"} };
			Assert.IsTrue(dictionary.ContainsKeyWithValue(1, "one"));
		}

		[Test]
		public void ContainsKeyWithValueReturnsFalse_String()
		{
			var dictionary = new Dictionary<int, string> { { 0, "zero" }, { 1, "one" }, { 2, "two" }, { 3, "three" } };
			Assert.IsFalse(dictionary.ContainsKeyWithValue(1, "two"));
		}

		[Test]
		public void ContainsKeyWithValueReturnsFalse_String_Null()
		{
			var dictionary = new Dictionary<int, string> { { 0, "zero" }, { 1, "one" }, { 2, "two" }, { 3, "three" } };
			Assert.IsFalse(dictionary.ContainsKeyWithValue(1, null));
		}

		[Test]
		public void ContainsKeyWithValueReturnsTrue_Item()
		{
			var dictionary = new Dictionary<int, Item> { { 0, new Item("zero") }, { 1, new Item("one") }, { 2, new Item("two") }, { 3, new Item("three") } };
			Assert.IsTrue(dictionary.ContainsKeyWithValue(1, new Item("one")));
		}

		[Test]
		public void ContainsKeyWithValueReturnsTrue_Item_EqualityComparere()
		{
			var dictionary = new Dictionary<int, Item> { { 0, new Item("zero") }, { 1, new Item("one") }, { 2, new Item("two") }, { 3, new Item("three") } };
			Assert.IsTrue(dictionary.ContainsKeyWithValue(1, new Item("one"), new ItemEq()));
		}

		[Test]
		public void ContainsKeyWithValueReturnsFalse_Item()
		{
			var dictionary = new Dictionary<int, Item> { { 0, new Item("zero") }, { 1, new Item("one") }, { 2, new Item("two") }, { 3, new Item("three") } };
			Assert.IsFalse(dictionary.ContainsKeyWithValue(1, new Item("two")));
		}

		[Test]
		public void ContainsKeyWithValueReturnsFalse_Item_EqualityComparere()
		{
			var dictionary = new Dictionary<int, Item> { { 0, new Item("zero") }, { 1, new Item("one") }, { 2, new Item("two") }, { 3, new Item("three") } };
			Assert.IsFalse(dictionary.ContainsKeyWithValue(1, new Item("two"), new ItemEq()));
		}

		[Test]
		public void ContainsKeyWithValueReturnsFalse_Item_Null()
		{
			var dictionary = new Dictionary<int, Item> { { 0, new Item("zero") }, { 1, new Item("one") }, { 2, new Item("two") }, { 3, new Item("three") } };
			Assert.IsFalse(dictionary.ContainsKeyWithValue(1, null));
		}

		[Test]
		public void ContainsKeyWithValueReturnsTrue_Item_Null()
		{
			var dictionary = new Dictionary<int, Item> { { 0, null }, { 1, new Item("one") }, { 2, new Item("two") }, { 3, new Item("three") } };
			Assert.IsTrue(dictionary.ContainsKeyWithValue(0, null));
		}
	}
}
