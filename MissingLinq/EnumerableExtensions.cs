using System;
using System.Collections.Generic;
using System.Linq;

namespace MissingLinq
{
	public static class EnumerableExtensions
	{
		/// <summary>
		/// Executes an action on each element of the list.
		/// This is a "fluent interface" (dotted syntax) version of foreach
		/// </summary>
		public static void ForEach<TValue>(this IEnumerable<TValue> list, Action<TValue> action)
		{
            list.ForEach((n, item) => action(item));
		}

		/// <summary>
		/// Executes an action on each element of the list. Action gets an item from the list together with the index of that item
		/// </summary>
		public static void ForEach<TValue>(this IEnumerable<TValue> list, Action<int, TValue> action)
		{
		    int counter = 0;
			foreach (var item in list)
			{
				action(counter, item);
			    counter++;
			}
		}

		/// <summary>
		/// Returns true if list contains no elements.
		/// </summary>
		public static bool IsEmpty<T>(this IEnumerable<T> list)
		{
			return !list.Any();
		}

		/// <summary>
		/// Returns true if the list is null or contains no elements.
		/// </summary>
		public static bool IsNullOrEmpty<T>(this IEnumerable<T> list)
		{
			return list == null || !list.Any();
		}

		/// <summary>
		/// Returns true if list contains no elements for which the predicate is true.
		/// </summary>
		public static bool None<T>(this IEnumerable<T> list, Predicate<T> predicate)
		{
			return !list.Any(t => predicate(t));
		}

		/// <summary>
		/// Returns true if list contains no elements for which the predicate is true.
		/// </summary>
		public static bool None<T>(this IEnumerable<T> list)
		{
			return !list.Any();
		}

		/// <summary>
		/// Returns true if list contains at least one element.
		/// </summary>
		public static bool IsNotEmpty<T>(this IEnumerable<T> list)
		{
			return list.Any();
		}

		/// <summary>
		/// Returns true if the list contains at least one element for which the predicate is true
		/// </summary>
		public static bool Contains<T>(this IEnumerable<T> list, Func<T, bool> predicate)
		{
			return list.Any(predicate);
		}

		/// <summary>
		/// Evaluates the IEnumerable (performing any pending deferred actions).
		/// Does nothing if the IEnumerable is a List or an Array 
		/// </summary>
		public static IEnumerable<T> Evaluate<T>(this IEnumerable<T> elements)
		{
			if (elements is List<T>) return elements;
			if (elements is T[]) return elements;
			return elements.ToList();
		}

		/// <summary>
		/// Returns pairs of values that appear next to each other in the original list
		/// </summary>
		public static IEnumerable<Tuple<T, T>> SelectAdjacentPairs<T>(this IEnumerable<T> list)
		{
			var enumerator = list.GetEnumerator();
			if (enumerator.MoveNext())
			{
				T prev = enumerator.Current;
				while (enumerator.MoveNext())
				{
					yield return Tuple.Create(prev, enumerator.Current);
					prev = enumerator.Current;
				}
			}
		}

	    /// <summary>
	    /// Returns all possible combinations of 2 values from the list 
	    /// (order is not important, i.e. {A,B} = {B,A} and only one of them is returned)
	    /// </summary>
	    public static IEnumerable<Tuple<T, T>> SelectAllCombinations<T>(this IEnumerable<T> elements)
	    {
	        List<T> list = elements.ToList();
	        for (int a = 0; a < list.Count - 1; a++)
	        {
	            for (int b = a + 1; b < list.Count; b++)
	            {
	                yield return Tuple.Create(list[a], list[b]);
	            }
	        }
	    }

		/// <summary>
		/// Returns all possible combinations of 2 lists 
		/// SelectAllCombinations({1,2}, {A,B}) will return {1A, 1B, 2A, 2B}.
		/// </summary>
		public static IEnumerable<Tuple<T1, T2>> SelectAllCombinations<T1, T2>(this IEnumerable<T1> list1, IEnumerable<T2> list2)
		{
			return list1.SelectMany(x => list2, Tuple.Create);
		}

		/// <summary>
		/// Divides the list in chunks of (at most) sliceSize elements. The last chunk can contain fewer elements
		/// </summary>
		public static IEnumerable<IEnumerable<TValue>> Slice<TValue>(this IEnumerable<TValue> list, long sliceSize)
		{
			var enumerator = list.GetEnumerator();
			var cnt = 0;
			var collector = new List<TValue>();
			while (enumerator.MoveNext())
			{
				collector.Add(enumerator.Current);
				cnt++;
				if (cnt >= sliceSize)
				{
					yield return collector;
					collector = new List<TValue>();
					cnt = 0;
				}
			}
	        if (collector.Count() != 0)
	        {
	            yield return collector;
	        }
		}

		/// <summary>
		/// Divides the list in chunks of elements that have the same key value. Unlike grouping, non-adjacent elements are not
		/// regrouped. i.e. the list aaaabbbaacccc wil result in a list of four groups: {aaaa}{bbb}{aa} and {cccc}. (GroupBy would
		/// merge the second group of a's with the first group)
		/// </summary>
		public static IEnumerable<IGrouping<TKey, TValue>> SliceBy<TKey, TValue>(this IEnumerable<TValue> list,
			Func<TValue, TKey> keyfunc)
		{
			return new SliceByImplementation<TKey, TValue>(list, keyfunc);
		}

		/// <summary>
		/// Builds an Index to speed up subsequent Find() actions. Groups elements with the same key.
		/// </summary>
		public static IIndex<TKey, TValue> BuildIndex<TKey, TValue>(this IEnumerable<TValue> list, Func<TValue, TKey> keyFunc)
		{
			return new IndexImplementation<TValue, TKey, TValue>(list, keyFunc, v => v);
		}

		/// <summary>
		/// Builds an Index to speed up subsequent Find() actions. Groups elements with the same key.
		/// </summary>
		public static IIndex<TKey, TValue> BuildIndex<TItem, TKey, TValue>(this IEnumerable<TItem> list, Func<TItem, TKey> keyFunc, Func<TItem, TValue> elementSelector)
		{
			return new IndexImplementation<TItem, TKey, TValue>(list, keyFunc, elementSelector);
		}

		/// <summary>
		/// Builds an Index to speed up subsequent Find() actions. Throws if the same key is encountered twice.
		/// </summary>
		public static IUniqueIndex<TKey, TValue> BuildUniqueIndex<TKey, TValue>(this IEnumerable<TValue> list,
																				Func<TValue, TKey> keyFunc)
		{
			return BuildUniqueIndex(list, keyFunc, item => item);
		}

		/// <summary>
		/// Builds an Index to speed up subsequent Find() actions. Throws if the same key is encountered twice.
		/// </summary>
		public static IUniqueIndex<TKey, TValue> BuildUniqueIndex<TItem, TKey, TValue>(this IEnumerable<TItem> list,
																				Func<TItem, TKey> keyFunc, Func<TItem, TValue> valueSelector)
		{
			return new UniqueIndexImplementation<TItem, TKey, TValue>(list, keyFunc, valueSelector);
		}

		/// <summary>
		/// Returns the elements of a list whose key appears (use with .In) or does 
		/// not appear (use with .NotIn) in another list.
		/// WhereKey(...) should allways be followed by either .In(...) or .NotIn(...) 
		/// </summary>
		public static IWhereKeyWithKey<TKey, TValue> WhereKey<TKey, TValue>(this IEnumerable<TValue> leftList, Func<TValue, TKey> keyFunc)
		{
			return new WhereKeyImplementation<TKey, TValue>(leftList, keyFunc);
		}

		/// <summary>
		/// Performs a left outer join of two lists and returns a list of paired values of the left and right lists.
		/// The "B" field of the returned pair can be null (if TRight is a reference type) or the default value (if TRight is 
		/// a value type), when no value with a specific key is found in the joined list
		/// </summary>
		public static IEnumerable<Tuple<TLeft, TRight>> JoinLeft<TKey, TLeft, TRight>(this IEnumerable<TLeft> left,
																				IEnumerable<TRight> right,
																				Func<TLeft, TKey> leftKeyFunc,
																				Func<TRight, TKey> rightKeyFunc)
		{
			return new JoinLeftImplementation<TLeft, TRight, TKey>(left, right, leftKeyFunc, rightKeyFunc);
		}

		/// <summary>
		/// Performs a left outer join of two lists and returns a list of paired values of the left and right lists.
		/// The "B" field of the returned pair can be null (if TValue is a reference type) or the default value (if TValue is 
		/// a value type), when no value with a specific key is found in the joined list
		/// </summary>
		public static IEnumerable<Tuple<TValue, TValue>> JoinLeft<TValue, TKey>(this IEnumerable<TValue> left,
																				IEnumerable<TValue> right,
																				Func<TValue, TKey> keyFunc)
		{
			return new JoinLeftImplementation<TValue, TValue, TKey>(left, right, keyFunc, keyFunc);
		}

		/// <summary>
		/// Performs a left outer join of two lists and returns a list of paired values of the left and right lists.
		/// The "B" field of the returned pair can be null (if TRight is a reference type) or the default value (if TRight is 
		/// a value type), when no value with a specific key is found in the joined list
		/// </summary>
		public static IPartialJoinWithRightList<TLeft, TRight> JoinLeft<TLeft, TRight>(this IEnumerable<TLeft> left, IEnumerable<TRight> right)
		{
			return new PartialJoinWithRightList<TLeft, TRight>(left, right, new JoinLeftFactory());
		}

		/// <summary>
		/// Performs an inner join of two lists and returns a list of paired values of the left and right lists.
		/// </summary>
		public static IPartialJoinWithRightList<TLeft, TRight> Join<TLeft, TRight>(this IEnumerable<TLeft> left, IEnumerable<TRight> right)
		{
			return new PartialJoinWithRightList<TLeft, TRight>(left, right, new InnerJoinFactory());
		}

		/// <summary>
		/// Returns all elements whose key value occurs more than once 
		/// </summary>
		public static IEnumerable<TValue> SelectDuplicates<TKey, TValue>(this IEnumerable<TValue> list, Func<TValue, TKey> keyFunc)
		{
			return list.GroupBy(keyFunc).Where(g => g.CountIsGreaterThan(1)).SelectMany(g => g.Select(v => v));
		}

		/// <summary>
		/// Checks if list contains more than count elements.
		/// Does not enumerate the whole list, as opposed to using IEnumerable.Count()
		/// </summary>
		public static bool CountIsGreaterThan<T>(this IEnumerable<T> list, int count)
		{
			return list.Take(count + 1).Count() == count + 1;
		}

		/// <summary>
		/// Checks if list contains fewer than count elements.
		/// Does not enumerate the whole list, as opposed to using IEnumerable.Count()
		/// </summary>
		public static bool CountIsLessThan<T>(this IEnumerable<T> list, int count)
		{
			return list.Take(count).Count() < count;
		}


		/// <summary>
		/// Returns the first element in the list or executes the exceptionCreator if the list is empty.
		/// Used to throw a more meaningful exception instead of the standard "empty list does not have a first element..."
		/// </summary>
		public static T FirstOrThrow<T>(this IEnumerable<T> list, Func<Exception> exceptionCreator)
		{
			var enumerator = list.GetEnumerator();
			if (enumerator.MoveNext()) return enumerator.Current;
			throw exceptionCreator();
		}

		/// <summary>
		/// Returns the single element in the list
		/// Throws exception created by the exceptionCreator if either the list is empty or contains more than one element.
		/// Used to throw a more meaningful exception instead of the standard "empty list does not have a first element..."
		/// </summary>
		public static T SingleOrThrow<T>(this IEnumerable<T> list, Func<Exception> exceptionCreator)
		{
			return SingleOrThrow(list, exceptionCreator, exceptionCreator);
		}

		/// <summary>
		/// Returns the single element in the list
		/// Throws exception created by emptyListExceptionCreator if the list is empty.
		/// Throws exception created by moreThanOneElementExceptionCreator if the list contains more than one element.
		/// Used to throw a more meaningful exception instead of the standard "empty list does not have a first element..."
		/// </summary>
		public static T SingleOrThrow<T>(this IEnumerable<T> list, Func<Exception> emptyListExceptionCreator, Func<Exception> moreThanOneElementExceptionCreator)
		{
			var enumerator = list.GetEnumerator();

			if (enumerator.MoveNext())
			{
				//first element found
				T candidate = enumerator.Current;

				//now assert there are no other elements left (remember, you asked 'Single')
				if (enumerator.MoveNext())
				{
					//second element found -> Exception !
					throw moreThanOneElementExceptionCreator();
				}

				return candidate;
			}

			//no element found
			throw emptyListExceptionCreator();
		}

		/// <summary>
		/// Adds an item to the beginning of the list
		/// </summary>
		public static IEnumerable<T> Prepend<T>(this IEnumerable<T> sequence, T item)
		{
			return new[] { item }.Concat(sequence);
		}

		/// <summary>
		/// Adds an item to the end of the list
		/// </summary>
		public static IEnumerable<T> Append<T>(this IEnumerable<T> sequence, T item)
		{
			return sequence.Concat(new[] { item });
		}

		/// <summary>
		/// Find the item with the highest whatever-you-want-to-find.
		/// If two keys are equal, the first object with that key will be returned.
		/// </summary>
		public static TItem MaxBy<TItem, TValue>(this IEnumerable<TItem> sequence, Func<TItem, TValue> keyfunc)
			where TValue : IComparable<TValue>
		{
			return FindExtremeBy(sequence, keyfunc, (l, r) => (l.CompareTo(r) > 0));
		}

		/// <summary>
		/// Find the item with the lowest whatever-you-want-to-find.
		/// If two keys are equal, the first object with that key will be returned.
		/// </summary>
		public static TItem MinBy<TItem, TValue>(this IEnumerable<TItem> sequence, Func<TItem, TValue> keyfunc)
			where TValue : IComparable<TValue>
		{
			return FindExtremeBy(sequence, keyfunc, (l, r) => (l.CompareTo(r) < 0));
		}

		private static TItem FindExtremeBy<TItem, TValue>(IEnumerable<TItem> sequence, Func<TItem, TValue> keyfunc, Func<TValue, TValue, bool> myComparer)
			where TValue : IComparable<TValue>
		{
			if (sequence == null || sequence.IsEmpty())
			{
				throw new InvalidOperationException("Cannot find extreme in empty list.");
			}
			var array = sequence.ToArray();
			TItem maxT = array.ElementAt(0);
			TValue maxValue = keyfunc(maxT);
			foreach (TItem v in array.Skip(1))
			{
				TValue k = keyfunc(v);
				if (myComparer(k, maxValue))
				{
					maxValue = k;
					maxT = v;
				}
			}
			return maxT;
		}

		/// <summary>
		/// returns pairs of elements that have the same position in left and right lists 
		/// (element 0 of left is paired with element 0 of right a.s.o.)
		/// </summary>
		public static IEnumerable<Tuple<TLeft, TRight>> Zip<TLeft, TRight>(this IEnumerable<TLeft> leftSequence,
																			IEnumerable<TRight> rightSequence)
		{
			return new ZipImplementation<TLeft, TRight>(leftSequence, rightSequence);
		}

		/// <summary>
		/// Returns true if the two list contain the same elements (not necessarily in the same order)
		/// (e.g. {1,2} is equivalent to {2,1}), but {1,1,2} is NOT equivalent to {1,2,2}) 
		/// </summary>
		public static bool SequenceEquivalent<T>(this IEnumerable<T> left, IEnumerable<T> right)
		{
			return SequenceEquivalent(left, right, EqualityComparer<T>.Default);
		}

		public static bool SequenceEquivalent<T>(this IEnumerable<T> left, IEnumerable<T> right, IEqualityComparer<T> comparer)
		{
			if (ReferenceEquals(left, right)) return true;
			if (ReferenceEquals(left, null)) return ReferenceEquals(right, null);
			if (ReferenceEquals(right, null)) return false;

			var leftList = left as IList<T> ?? left.ToList();
			var rightList = right as IList<T> ?? right.ToList();

			if (leftList.Count != rightList.Count) return false;

			ILookup<T, T> leftLookup = leftList.ToLookup(l => l, comparer);
			return rightList
				.GroupBy(r => r)
				.All(g => leftLookup.Contains(g.Key) && leftLookup[g.Key].Count() == g.Count());
		}

		public static string ToDisplayString<T>(this IEnumerable<T> sequence)
		{
			return string.Format("[{0}]", string.Join(",", sequence.Select(x => (x == null) ? "null" : x.ToString()).ToArray()));
		}

		/// <summary>
		/// Splits the collection of T in subcollections based on the presence of a separator element, much like string.Split does for strings.
		/// Does not return empty collections (i.e. two adjacent separators are treated as one)
		/// The separator element itself is not part of the returned subcollection(s)
		/// </summary>
		public static IEnumerable<IEnumerable<T>> Split<T>(this IEnumerable<T> collection, T separator, IEqualityComparer<T> comparer = null)
		{
			return new SplitImplementation<T>(collection, separator, comparer ?? EqualityComparer<T>.Default);
		}

		/// <summary>
		/// Overload of Sum that allows to calculate the sum of a collection of TimeSpans
		/// </summary>
		public static TimeSpan Sum<T>(this IEnumerable<T> collection, Func<T, TimeSpan> selector)
		{
			return TimeSpan.FromTicks(collection.Sum(x => selector(x).Ticks));
		}

		public static TValue MaxOrDefault<TData, TValue>(this IEnumerable<TData> list, Func<TData, TValue> getValueFunc)
		{
			return list.Any() ? list.Max(getValueFunc) : default(TValue);
		}

		public static TValue MinOrDefault<TData, TValue>(this IEnumerable<TData> list, Func<TData, TValue> getValueFunc)
		{
			return list.Any() ? list.Min(getValueFunc) : default(TValue);
		}

		public static IEnumerable<IEnumerable<T>> SelectSlidingSubsets<T>(this IEnumerable<T> list, int subsetSize)
		{
			return new SlidingWindowImplementation<T>(list, subsetSize);
		}

        ///Flattens a tree-structured list. Returns parents and children, and their children, and ... in one flat list
        public static IEnumerable<T> Flatten<T>(this IEnumerable<T> structuredList, Func<T, IEnumerable<T>> getChildren)
        {
            if (structuredList == null) return Query.Empty<T>();

            return structuredList.SelectMany(item =>
            {
                var children = getChildren(item);
                if (children == null)
                {
                    return new[] {item};
                }
                return children.Flatten(getChildren).Prepend(item);
            });
        }

	    public static List<T> AllowModification<T>(this IEnumerable<T> source)
	    {
	        var copy = new List<T>(source);
	        return copy;
	    }
    }
}
