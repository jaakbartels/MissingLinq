using System.Collections.Generic;
using System.Linq;

namespace MissingLinq
{
    public interface IWhereKeyWithKey<TKey, out TValue>
    {
        IEnumerable<TValue> In(IEnumerable<TKey> rightList);
        IEnumerable<TValue> NotIn(IEnumerable<TKey> rightList);
        IEnumerable<TValue> In<TIgnore>(ILookup<TKey, TIgnore> lookup);
        IEnumerable<TValue> NotIn<TIgnore>(ILookup<TKey, TIgnore> lookup);
        IEnumerable<TValue> In<TIgnore>(Dictionary<TKey, TIgnore> dictionary);
        IEnumerable<TValue> NotIn<TIgnore>(Dictionary<TKey, TIgnore> dictionary);
        IEnumerable<TValue> In(HashSet<TKey> rightList);
        IEnumerable<TValue> NotIn(HashSet<TKey> rightList);
    }
}