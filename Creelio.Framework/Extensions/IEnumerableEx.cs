using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Creelio.Framework.Extensions
{
    public static class IEnumerableEx
    {
        public static List<T> ToList<T>(this IEnumerable collection)
        {
            collection.ThrowIfNotExists(new NullReferenceException("Cannot convert null collection to List<T>."));

            var list = new List<T>();

            foreach (T item in collection)
            {
                list.Add(item);
            }

            return list;
        }
    }
}
