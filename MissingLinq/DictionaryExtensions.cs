using System;
using System.Collections.Generic;

namespace MissingLinq
{
	public static class DictionaryExtensions
	{
		public static Dictionary<TKey, TValue> AddRange<TKey, TValue>(this Dictionary<TKey, TValue> toDictionary, Dictionary<TKey, TValue> fromDictionary, bool overwriteOriginals = true)
		{
			if (toDictionary == null) throw new ArgumentNullException(nameof(toDictionary));
			if (fromDictionary == null) throw new ArgumentNullException(nameof(fromDictionary));

			foreach (var pair in fromDictionary)
			{
				if (!overwriteOriginals && fromDictionary.ContainsKey(pair.Key))
				{
					continue;
				}

				toDictionary[pair.Key] = pair.Value;
			}
			return toDictionary;
		}

		public static bool ContainsKeyWithValue<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue requestedValue, IEqualityComparer<TValue> equalityComparer = null)
		{
			var keyValue = default(TValue);
			return dictionary.TryGetValue(key, out keyValue) && (equalityComparer?.Equals(requestedValue, keyValue) ?? Equals(requestedValue, keyValue));
		}
	}
}
