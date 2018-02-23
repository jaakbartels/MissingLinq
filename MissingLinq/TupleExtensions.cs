using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MissingLinq
{
    public static class TupleExtensions
    {
        public static bool Contains<T>(this Tuple<T, T> t, T element)
        {
            return Equals(t.Item1, element) || Equals(t.Item2, element);
        }

        public static bool Contains<T>(this Tuple<T, T, T> t, T element)
        {
            return Equals(t.Item1, element) || Equals(t.Item2, element) || Equals(t.Item3, element);
        }

        /// <summary>
        /// Where with built-in destructurization of Tuple. Allows use of Where((a, b) => ...) instead of Where(t => t.Item1 ...)
        /// </summary>
        public static IEnumerable<Tuple<T1, T2>> Where<T1, T2>(this IEnumerable<Tuple<T1, T2>> source, Func<T1, T2, bool> predicate)
        {
            return source.Where(t => predicate(t.Item1, t.Item2));
        }

        /// <summary>
        /// Select with built-in destructurization of Tuple. Allows use of Select((a, b) => ...) instead of Select(t => t.Item1 ...)
        /// </summary>
        public static IEnumerable<T3> Select<T1, T2, T3>(this IEnumerable<Tuple<T1, T2>> source, Func<T1, T2, T3> projection)
        {
            return source.Select(t => projection(t.Item1, t.Item2));
        }
    }
}
