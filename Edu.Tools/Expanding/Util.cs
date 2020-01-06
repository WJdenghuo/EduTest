using System;
using System.Collections.Generic;
using System.Text;

namespace Edu.Tools.Expanding
{
    public static class Util
    {
        public static IEnumerable<T> Where<T>(this IEnumerable<T> sourse, Func<T, bool> predicate)
        {
            if (sourse == null || predicate == null)
            {
                throw new ArgumentNullException();
            }
            return WhereImp(sourse, predicate);
        }
        public static IEnumerable<T> WhereImp<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            foreach (var item in source)
            {
                if (predicate(item))
                {
                    yield return item;
                }
            }
        }
    }
}
