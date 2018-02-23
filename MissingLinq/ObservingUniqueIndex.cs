using System;

namespace MissingLinq
{
	public class ObservingUniqueIndex<TListItem, TKey, TValue> : UniqueIndexImplementation<TListItem, TKey, TValue>
	{
		public ObservingUniqueIndex(ObservableList<TListItem> list, Func<TListItem, TKey> keyFunc, Func<TListItem, TValue> valueSelector)
			: base(list, keyFunc, valueSelector)
		{
			_keyFunc = keyFunc;
			_valueSelector = valueSelector;
			list.Added += (sender, args) => AddElement(_keyFunc(args.Element), _valueSelector(args.Element));
			list.Removed += (sender, args) => RemoveElement(args.Element);
			list.Cleared += (sender, args) => Clear();
		}

		private readonly Func<TListItem, TKey> _keyFunc;
		private readonly Func<TListItem, TValue> _valueSelector;

		private void RemoveElement(TListItem element)
		{
			var key = _keyFunc(element);
			if (ReferenceEquals(key, null))
			{
				_itemWithNullKey.Clear();
			}
			else
			{
				_dict.Remove(key);
			}
		}

		private void Clear()
		{
			_dict.Clear();
			_itemWithNullKey.Clear();
		}
	}
}