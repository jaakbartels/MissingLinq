namespace MissingLinq
{
	public interface IUniqueIndex<TKey, TValue>
	{
		/// <summary>
		/// Finds the element with the requested key. Throws if index does not contain an element with that key.
		/// </summary>
		/// <param name="key">Key of element to retrieve</param>
		/// <returns>Found element</returns>
		TValue Find(TKey key);

		/// <summary>
		/// Finds the element with the requested key. Throws if index does not contain an element with that key.
		/// </summary>
		/// <param name="key">Key of element to retrieve</param>
		/// <returns>Found element</returns>
		TValue this[TKey key] { get; }

		bool Contains(TKey key);
		bool TryGetValue(TKey key, out TValue item);
	}
}