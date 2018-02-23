using System.Collections.Generic;

namespace MissingLinq
{
  public interface IIndex<TKey, TValue>
  {
    IEnumerable<TValue> Find(TKey key);
    IEnumerable<TValue> this[TKey key] { get; }
    bool Contains(TKey key);
  }
}