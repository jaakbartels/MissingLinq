using System.Linq;

namespace MissingLinq
{
	public static class ObjectExtensions
	{
		public static bool In<TElement, TListItem>(this TElement element, params TListItem[] list) where TElement : TListItem
		{
			return list.Contains(element);
		}

		public static bool NotIn<TElement, TListItem>(this TElement element, params TListItem[] list) where TElement : TListItem
		{
			return !element.In(list);
		}
	}
}
