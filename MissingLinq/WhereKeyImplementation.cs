using System;
using System.Collections.Generic;
using System.Linq;

namespace MissingLinq
{
    class WhereKeyImplementation<TKey, TValue> : IWhereKeyWithKey<TKey, TValue>
    {
        private readonly IEnumerable<TValue> _leftList;
        private readonly Func<TValue, TKey> _keyFunc;

        public WhereKeyImplementation(IEnumerable<TValue> leftList, Func<TValue, TKey> keyFunc)
        {
            _leftList = leftList;
            _keyFunc = keyFunc;
        }

        public IEnumerable<TValue> In(IEnumerable<TKey> rightList)
        {
            return FilterLeftList(new PlainListLookupStrategy(rightList), shouldBeInRightList: true);
        }

        public IEnumerable<TValue> NotIn(IEnumerable<TKey> rightList)
        {
            return FilterLeftList(new PlainListLookupStrategy(rightList), shouldBeInRightList: false);
        }

        public IEnumerable<TValue> In(HashSet<TKey> rightList)
        {
            return FilterLeftList(new PreCalculatedHashSetStrategy(rightList), shouldBeInRightList: true);
        }

        public IEnumerable<TValue> NotIn(HashSet<TKey> rightList)
        {
            return FilterLeftList(new PreCalculatedHashSetStrategy(rightList), shouldBeInRightList: false);
        }

        public IEnumerable<TValue> In<TIgnore>(ILookup<TKey, TIgnore> lookup)
        {
            return FilterLeftList(new PreCalculatedLookupStrategy<TIgnore>(lookup), shouldBeInRightList: true);
        }

        public IEnumerable<TValue> NotIn<TIgnore>(ILookup<TKey, TIgnore> lookup)
        {
            return FilterLeftList(new PreCalculatedLookupStrategy<TIgnore>(lookup), shouldBeInRightList: false);
        }

        public IEnumerable<TValue> In<TIgnore>(Dictionary<TKey, TIgnore> dictionary)
        {
            return FilterLeftList(new PreCalculatedDictionaryStrategy<TIgnore>(dictionary), shouldBeInRightList: true);
        }

        public IEnumerable<TValue> NotIn<TIgnore>(Dictionary<TKey, TIgnore> dictionary)
        {
            return FilterLeftList(new PreCalculatedDictionaryStrategy<TIgnore>(dictionary), shouldBeInRightList: false);
        }

        private IEnumerable<TValue> FilterLeftList(ILookupStrategy lookupStrategy, bool shouldBeInRightList)
        {
            return _leftList.Where(v => lookupStrategy.Contains(_keyFunc(v)) ^ !shouldBeInRightList);
        }

        interface ILookupStrategy
        {
            bool Contains(TKey key);
        }

        private class PlainListLookupStrategy : PreCalculatedHashSetStrategy
        {
            public PlainListLookupStrategy(IEnumerable<TKey> items) : base(new HashSet<TKey>(items))
            {}
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