using System;
using System.Collections.Generic;
using System.Linq;

namespace MissingLinq
{
    class WhereKeyImplementation<TKey, TValue> : IWhereKeyWithKey<TKey, TValue>
    {
        private readonly IEnumerable<TValue> _leftList;
        private readonly Func<TValue, TKey> _keyFunc;

        private ILookupStrategy _lookupStrategy;

        public WhereKeyImplementation(IEnumerable<TValue> leftList, Func<TValue, TKey> keyFunc)
        {
            _leftList = leftList;
            _keyFunc = keyFunc;
        }

        public IEnumerable<TValue> In(IEnumerable<TKey> rightList)
        {
            _lookupStrategy = new PlainListLookupStrategy(rightList);
            return FilterLeftList(shouldBeInRightList: true);
        }

        public IEnumerable<TValue> NotIn(IEnumerable<TKey> rightList)
        {
            _lookupStrategy = new PlainListLookupStrategy(rightList);
            return FilterLeftList(shouldBeInRightList: false);
        }

        public IEnumerable<TValue> In(HashSet<TKey> rightList)
        {
            _lookupStrategy = new PreCalculatedHashSetStrategy(rightList);
            return FilterLeftList(shouldBeInRightList: true);
        }

        public IEnumerable<TValue> NotIn(HashSet<TKey> rightList)
        {
            _lookupStrategy = new PreCalculatedHashSetStrategy(rightList);
            return FilterLeftList(shouldBeInRightList: false);
        }

        public IEnumerable<TValue> In<TIgnore>(ILookup<TKey, TIgnore> lookup)
        {
            _lookupStrategy = new PreCalculatedLookupStrategy<TIgnore>(lookup);
            return FilterLeftList(shouldBeInRightList: true);
        }

        public IEnumerable<TValue> NotIn<TIgnore>(ILookup<TKey, TIgnore> lookup)
        {
            _lookupStrategy = new PreCalculatedLookupStrategy<TIgnore>(lookup);
            return FilterLeftList(shouldBeInRightList: false);
        }

        public IEnumerable<TValue> In<TIgnore>(Dictionary<TKey, TIgnore> dictionary)
        {
            _lookupStrategy = new PreCalculatedDictionaryStrategy<TIgnore>(dictionary);
            return FilterLeftList(shouldBeInRightList: true);
        }

        public IEnumerable<TValue> NotIn<TIgnore>(Dictionary<TKey, TIgnore> dictionary)
        {
            _lookupStrategy = new PreCalculatedDictionaryStrategy<TIgnore>(dictionary);
            return FilterLeftList(shouldBeInRightList: false);
        }

        private IEnumerable<TValue> FilterLeftList(bool shouldBeInRightList)
        {
            return _leftList.Where(v => _lookupStrategy.Contains(_keyFunc(v)) ^ !shouldBeInRightList);
        }

        interface ILookupStrategy
        {
            bool Contains(TKey key);
        }

        private class PlainListLookupStrategy : ILookupStrategy
        {
            public PlainListLookupStrategy(IEnumerable<TKey> items)
            {
                _lookup = items.ToLookup(x => x);
            }

            private readonly ILookup<TKey, TKey> _lookup;
            public bool Contains(TKey key)
            {
                return _lookup.Contains(key);
            }
        }

        private class PreCalculatedLookupStrategy<TIgnore> : ILookupStrategy
        {
            public PreCalculatedLookupStrategy(ILookup<TKey, TIgnore> items)
            {
                _lookup = items;
            }

            private readonly ILookup<TKey, TIgnore> _lookup;
            public bool Contains(TKey key)
            {
                return _lookup.Contains(key);
            }
        }

        private class PreCalculatedDictionaryStrategy<TIgnore> : ILookupStrategy
        {
            public PreCalculatedDictionaryStrategy(Dictionary<TKey, TIgnore> dictionary)
            {
                _dictionary = dictionary;
            }

            private readonly Dictionary<TKey, TIgnore> _dictionary;
            public bool Contains(TKey key)
            {
                return _dictionary.ContainsKey(key);
            }
        }

        private class PreCalculatedHashSetStrategy : ILookupStrategy
        {
            public PreCalculatedHashSetStrategy(HashSet<TKey> hashSet)
            {
                _hashSet = hashSet;
            }

            private readonly HashSet<TKey> _hashSet;

            public bool Contains(TKey key)
            {
                return _hashSet.Contains(key);
            }
        }
    }
}