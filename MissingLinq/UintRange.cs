namespace MissingLinq
{
	public class UintRange : RangeBase<uint>
	{
		public UintRange(uint start, uint end)
			: base(start, end)
		{ }

		/// <summary>
		/// Initializes a range from 0 to numberOfElements-1
		/// </summary>
		public UintRange(uint numberOfElements)
			: base(0, numberOfElements - 1)
		{ }

		protected override uint Next(uint current)
		{
			return current + 1;
		}

		public override bool Contains(uint item)
		{
			return Start <= item && item <= End;
		}
	}
}