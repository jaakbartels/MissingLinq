using System;
using System.Linq;
using NUnit.Framework;

namespace MissingLinq.UnitTests
{
    [TestFixture]
    public class Test_TupleExtensions
    {
        [Test]
        public void WhereWithTuple()
        {
            var sut = new[] {Tuple.Create("test", 1)};

            var result = sut.Where((a, b) => a.StartsWith("t"));

            Assert.AreEqual(sut.First(), result.First());
        }

        [Test]
        public void SelectWithTuple()
        {
            var sut = new[] {Tuple.Create("test", 1)};

            var result = sut.Select((a, b) => a + b);

            Assert.AreEqual("test1", result.First());
        }
    }
}
