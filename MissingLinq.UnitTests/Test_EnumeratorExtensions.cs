using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace MissingLinq.UnitTests
{
	[TestFixture]
	public class Test_EnumeratorExtensions
	{
		[Test]
		public void IsEmptyReturnsFalse()
		{
			Assert.IsFalse(new[] {1, 2, 3}.IsEmpty());
		}

		[Test]
		public void IsEmptyReturnsTrue()
		{
			Assert.IsTrue(new int[0].IsEmpty());
		}

		[Test]
		public void IsNullOrEmptyReturnsFalse()
		{
			Assert.IsFalse(new[] { 1, 2, 3 }.IsNullOrEmpty());
		}

		[Test]
		public void IsNullOrEmptyReturnsTrueIfNull()
		{
			IEnumerable<int> list = null;
			Assert.IsTrue(list.IsNullOrEmpty());
		}

		[Test]
		public void IsNullOrEmptyReturnsTrueIfEmpty()
		{
			Assert.IsTrue(new int[0].IsNullOrEmpty());
		}

		[Test]
		public void IsNotEmptyReturnsFalse()
		{
			Assert.IsFalse(new int[0].IsNotEmpty());
		}

		[Test]
		public void IsNotEmptyReturnsTrue()
		{
			Assert.IsTrue(new[] {1, 2, 3}.IsNotEmpty());
		}

		[Test]
		public void FirstOrThrowReturnsFirstElement()
		{
			Assert.AreEqual(1, new[] { 1, 2, 3, 4 }.FirstOrThrow(() => new EmptyListException()));
		}

		[Test, ExpectedException(typeof(EmptyListException))]
		public void FirstOrThrowThrowsWhenListIsEmpty()
		{
			Assert.AreEqual(1, new int[0].FirstOrThrow(() => new EmptyListException()));
		}

		[Test]
		public void SingleOrThrowReturnsSingleElement()
		{
			Assert.AreEqual(1, new[] { 1 }.SingleOrThrow(() => new EmptyListException(), () => new MoreThanOneElementException()));
		}

		[Test, ExpectedException(typeof(EmptyListException))]
		public void SingleOrThrowThrowsWhenListIsEmpty()
		{
			new int[0].SingleOrThrow(() => new EmptyListException(), () => new MoreThanOneElementException());
		}

		[Test, ExpectedException(typeof(MoreThanOneElementException))]
		public void SingleOrThrowThrowsWhenListIsTooLong()
		{
			new[] { 1, 2, 3, 4 }.SingleOrThrow(() => new EmptyListException(), () => new MoreThanOneElementException());
		}

		private class EmptyListException : Exception
		{ }
		private class MoreThanOneElementException : Exception
		{ }

		[Test]
		public void AppendAddsItemToEndOfSequence()
		{
			CollectionAssert.AreEqual(new[] { 1, 2, 3, 4, 5 }, new[] { 1, 2, 3, 4 }.Append(5));
		}

		[Test]
		public void AppendToEmptySequenceReturnsOneItem()
		{
			CollectionAssert.AreEqual(new[] { 5 }, new int[0].Append(5));
		}

		[Test]
		public void PrependAddsItemToStartOfSequence()
		{
			CollectionAssert.AreEqual(new[] { 5, 1, 2, 3, 4 }, new[] { 1, 2, 3, 4 }.Prepend(5));
		}

		[Test]
		public void PrependToEmptySequenceReturnsOneItem()
		{
			CollectionAssert.AreEqual(new[] { 5 }, new int[0].Prepend(5));
		}

		[Test]
		public void ZipReturnsExpectedTuples()
		{
			var left = new[] { "a", "b", "c" };
			var right = new[] { 1, 2, 3 };

			var expectedResult = new[] { Tuple.Create("a", 1), Tuple.Create("b", 2), Tuple.Create("c", 3) };

			CollectionAssert.AreEqual(expectedResult, left.Zip(right));
		}

		[Test]
		public void ZipStopsEnumeratingIfLeftIsShorter()
		{
			var left = new[] { "a", "b" };
			var right = new[] { 1, 2, 3 };

			var expectedResult = new[] { Tuple.Create("a", 1), Tuple.Create("b", 2) };

			CollectionAssert.AreEqual(expectedResult, left.Zip(right));
		}

		[Test]
		public void ZipStopsEnumeratingIfRightIsShorter()
		{
			var left = new[] { "a", "b", "c" };
			var right = new[] { 1, 2};

			var expectedResult = new[] { Tuple.Create("a", 1), Tuple.Create("b", 2) };

			CollectionAssert.AreEqual(expectedResult, left.Zip(right));
		}

		[Test]
		public void ToDisplayStringReturnsExpectedFormattedString()
		{
			var list = new[] { 1, 2, 3, 4 };

			CollectionAssert.AreEqual("[1,2,3,4]", list.ToDisplayString());
		}

		[Test]
		public void MaxByReturnsExpectedValue()
		{
			var versions = new List<SoftwareVersion>
			{
				new SoftwareVersion(1, 2, 3, 4),
				new SoftwareVersion(1, 2, 3, 40),
				new SoftwareVersion(1, 2, 3, 400),
				new SoftwareVersion(1, 2, 3, 5)
			};

			Assert.AreEqual(new SoftwareVersion(1, 2, 3, 400), versions.MaxBy(v => v.InternalNumber));
		}

		[Test]
		public void MaxByReturnsFirstOccurrenceWhenThereAreMultipleObjectsWithSameMaxValue()
		{
			var v1 = new SoftwareVersion(1, 2, 3, 400);
			var v2 = new SoftwareVersion(1, 2, 3, 400);
				
			var versions = new List<SoftwareVersion>
			{
				new SoftwareVersion(1, 2, 3, 4),
				v1,
				new SoftwareVersion(1, 2, 3, 40),
				v2,
				new SoftwareVersion(1, 2, 3, 5)
			};

			Assert.AreEqual(v1, versions.MaxBy(v => v.InternalNumber));
		}

		[Test, ExpectedException(typeof(InvalidOperationException))]
		public void MaxByThrowsWhenListIsEmpty()
		{
			var versions = new List<SoftwareVersion>();
			var max = versions.MaxBy(v => v.MajorNumber);
		}

		[Test]
		public void MinByReturnsExpectedValue()
		{
			var versions = new List<SoftwareVersion>
			{
				new SoftwareVersion(1, 2, 3, 4),
				new SoftwareVersion(1, 2, 3, 40),
				new SoftwareVersion(1, 2, 3, 2),
				new SoftwareVersion(1, 2, 3, 400),
				new SoftwareVersion(1, 2, 3, 5)
			};

			Assert.AreEqual(new SoftwareVersion(1, 2, 3, 2), versions.MinBy(v => v.InternalNumber));
		}

		[Test]
		public void MinByReturnsFirstOccurrenceWhenThereAreMultipleObjectsWithSameMinValue()
		{
			var v1 = new SoftwareVersion(1, 2, 3, 2);
			var v2 = new SoftwareVersion(1, 2, 3, 2);
			var versions = new List<SoftwareVersion>
			{
				new SoftwareVersion(1, 2, 3, 4),
				v1,
				new SoftwareVersion(1, 2, 3, 40),
				v2,
				new SoftwareVersion(1, 2, 3, 400),
				new SoftwareVersion(1, 2, 3, 5)
			};

			Assert.AreEqual(v1, versions.MinBy(v => v.InternalNumber));
		}

		[Test, ExpectedException(typeof(InvalidOperationException))]
		public void MinByThrowsWhenListIsEmpty()
		{
			var versions = new List<SoftwareVersion>();
			var max = versions.MinBy(v => v.MajorNumber);
		}

		[Test]
		public void NoneReturnsTrue()
		{
			var data = new[] { 1, 2, 3, 4, 5 };

			Assert.IsTrue(data.None(x => x > 5));
		}

		[Test]
		public void NoneReturnsFalse()
		{
			var data = new[] { 1, 2, 3, 4, 5 };

			Assert.IsFalse(data.None(x => x >= 5));
		}

		[Test]
		public void NoneReturnsTrueIfListIsEmpty()
		{
			var data = new int[0];

			Assert.IsTrue(data.None(x => x >= 5));
		}

	    [Test]
	    public void FlattenReturnsAllItems()
	    {
            //Arrange
	        var data = new List<Item>
	        {
	            new Item("a")
                {
                    Children =
                    {
                        new Item("b")
                        {
                            Children =
                            {
                                new Item("c"),
                                new Item("d")
                                {
                                    Children =
                                    {
                                        new Item("e")
                                    }
                                }
                            }
                        },
                        new Item("f") {Children= {}},
                    }
                },
                new Item("g")
            }; 

            //Act
	        var result = data.Flatten(item => item.Children).Select(item => item.Code);

            //Assert
	        result.Should().BeEquivalentTo("a", "b", "c", "d", "e", "f", "g");

	    }

	    [Test]
	    public void FlattenDoesNotThrowWhenChildrenIsNull()
	    {
            //Arrange
	        var data = new List<Item>
	        {
	            new Item("a") { Children = null }
            }; 

            //Act
	        var result = data.Flatten(item => item.Children).Select(item => item.Code);

            //Assert
	        //Nothing, should not throw;
	    }

        [Test]
	    public void FlattenReturnsEmptyListWhenSourceIsEmpty()
	    {
            //Arrange
	        var data = new List<Item>(); 

            //Act
	        var result = data.Flatten(item => item.Children);

            //Assert
	        result.Should().BeEmpty();
	    }

        [Test]
	    public void FlattenReturnsEmptyListWhenSourceIsNull()
	    {
            //Arrange
	        List<Item> data = null; 

            //Act
	        var result = data.Flatten(item => item.Children);

            //Assert
	        result.Should().BeEmpty();
	    }

	    [Test]
	    public void AllowModificationDoesNotThrowWhenSourceIsModified()
	    {
	        var source = new List<int> {0, 1, 2, 3, 4, 5, 6, 7, 8, 9};

	        source
                .AllowModification()
                .Where(n => n % 2 == 0)
                .ForEach(n => source.Remove(n));
	    }

	    class Item
	    {
	        public Item(string code)
	        {
	            Code = code;
                Children = new List<Item>();
	        }

	        public readonly string Code;
	        public List<Item> Children { get; set; }
	    }

	    class SoftwareVersion
	    {
	        public SoftwareVersion(int majorNumber, int minorNumber, int bugfixNumber, int internalNumber)
	        {
	            MajorNumber = majorNumber;
	            MinorNumber = minorNumber;
	            InternalNumber = internalNumber;
	            BugfixNumber = bugfixNumber;
	        }

	        public int MajorNumber;
	        public int MinorNumber;
	        public int BugfixNumber;   
	        public int InternalNumber;

	        protected bool Equals(SoftwareVersion other)
	        {
	            return MajorNumber == other.MajorNumber && MinorNumber == other.MinorNumber && BugfixNumber == other.BugfixNumber && InternalNumber == other.InternalNumber;
	        }

	        public override bool Equals(object obj)
	        {
	            if (ReferenceEquals(null, obj)) return false;
	            if (ReferenceEquals(this, obj)) return true;
	            if (obj.GetType() != this.GetType()) return false;
	            return Equals((SoftwareVersion) obj);
	        }

	        public override int GetHashCode()
	        {
	            unchecked
	            {
	                var hashCode = MajorNumber;
	                hashCode = (hashCode*397) ^ MinorNumber;
	                hashCode = (hashCode*397) ^ BugfixNumber;
	                hashCode = (hashCode*397) ^ InternalNumber;
	                return hashCode;
	            }
	        }
	    }
	}
}