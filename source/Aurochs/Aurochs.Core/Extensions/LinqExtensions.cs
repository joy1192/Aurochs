using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Aurochs.Core.Extensions
{
    public static class LinqExtensions
    {
        private class RambdaComparer<TSource> : IEqualityComparer<TSource>
        {
            private Func<TSource, TSource, bool> Predicate { get; }

            private Func<TSource, int> HashGenerator { get; }

            public RambdaComparer(Func<TSource,TSource,bool> predicate, Func<TSource, int> hashGenerator)
            {
                Predicate = predicate;
                HashGenerator = hashGenerator;
            }

            public bool Equals(TSource x, TSource y)
            {
                return Predicate(x, y);
            }

            public int GetHashCode(TSource obj)
            {
                return HashGenerator(obj);
            }
        }


        public static IEnumerable<TSource> Distinct<TSource>(this IEnumerable<TSource> source, Func<TSource, TSource, bool> predicate, Func<TSource, int> hashGenerator)
        {
            // TODO: local method使って遅延評価時のNullReferenceチェックすること
            var comparer = new RambdaComparer<TSource>(predicate, hashGenerator);

            return source.Distinct(comparer);
        }
    }
}
