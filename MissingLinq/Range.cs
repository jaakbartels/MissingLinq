namespace MissingLinq
{
	public class Range : RangeBase<int>
	{
		/// <summary>
		/// Initializes a range from 0 to numberOfElements-1
		/// </summary>
		public Range(int numberOfElements)
			: this(0, numberOfElements - 1)
		{ }

		public Range(int start, int end) : base(start, end)
		{}

		protected override int Next(int current)
		{
			return current + 1;
		}

		public override bool Contains(int item)
		{
			return Start <= item && item <= End;
		}
	}
}
