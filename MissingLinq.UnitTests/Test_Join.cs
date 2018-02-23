using System.Linq;
using NUnit.Framework;

namespace MissingLinq.UnitTests
{
	[TestFixture]
	public class Test_Join
	{
		[Test]
		public void CanUseAlternativeSyntax()
		{
			var inner = new[] { 
				new { value = 1, name = "one" }
				, new { value = 6, name = "six" }
				, new { value = 11, name = "eleven" } 
			};
			
			var outer = new[] { 1, 7, 6};

			var result = outer
				.Join(inner)
				.On(o => o).Equals(i => i.value)
				.Select(t => t.Item2.name);

			CollectionAssert.AreEquivalent(new []{"one", "six"}, result);
		}
	}
}