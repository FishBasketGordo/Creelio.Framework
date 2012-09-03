namespace Creelio.Framework.Extensions
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Creelio.Framework.Extensions;

    public static class IEnumerableExtensions
    {
        public static List<T> ToList<T>(this IEnumerable collection)
        {
            collection.ThrowIfNull(_ => new NullReferenceException("Cannot convert null collection to List<T>."));

            var list = new List<T>();

            foreach (T item in collection)
            {
                list.Add(item);
            }

            return list;
        }

        public static IEnumerable<T> TakeLast<T>(this IEnumerable<T> collection, int count)
        {
            collection.ThrowIfNull(() => new NullReferenceException("Collection is null."));

            if (count <= 0)
            {
                return Enumerable.Empty<T>();
            }

            var collectionCount = collection.Count();

            if (count >= collectionCount)
            {
                return collection;
            }

            return collection.Skip(collectionCount - count);
        }

        public static IEnumerable<Tuple<T1, T2>> CombineItems<T1, T2>(
            this IEnumerable<T1> collection1, 
            IEnumerable<T2> collection2)
        {
            return CombineItems<T1, T2, Tuple<T1, T2>>(
                collection1, 
                collection2, 
                (item1, item2) => Tuple.Create(item1, item2));
        }

        public static IEnumerable<Tuple<T1, T2, T3>> CombineItems<T1, T2, T3>(
            this IEnumerable<T1> collection1,
            IEnumerable<T2> collection2,
            IEnumerable<T3> collection3)
        {
            return CombineItems<T1, T2, T3, Tuple<T1, T2, T3>>(
                collection1,
                collection2,
                collection3,
                (item1, item2, item3) => Tuple.Create(item1, item2, item3));
        }

        public static IEnumerable<Tuple<T1, T2, T3, T4>> CombineItems<T1, T2, T3, T4>(
            this IEnumerable<T1> collection1,
            IEnumerable<T2> collection2,
            IEnumerable<T3> collection3,
            IEnumerable<T4> collection4)
        {
            return CombineItems<T1, T2, T3, T4, Tuple<T1, T2, T3, T4>>(
                collection1,
                collection2,
                collection3,
                collection4,
                (item1, item2, item3, item4) => Tuple.Create(item1, item2, item3, item4));
        }

        public static IEnumerable<Tuple<T1, T2, T3, T4, T5>> CombineItems<T1, T2, T3, T4, T5>(
            this IEnumerable<T1> collection1,
            IEnumerable<T2> collection2,
            IEnumerable<T3> collection3,
            IEnumerable<T4> collection4,
            IEnumerable<T5> collection5)
        {
            return CombineItems<T1, T2, T3, T4, T5, Tuple<T1, T2, T3, T4, T5>>(
                collection1,
                collection2,
                collection3,
                collection4,
                collection5,
                (item1, item2, item3, item4, item5) => Tuple.Create(item1, item2, item3, item4, item5));
        }

        public static IEnumerable<TResult> CombineItems<T1, T2, TResult>(
            this IEnumerable<T1> collection1,
            IEnumerable<T2> collection2,
            Func<T1, T2, TResult> selector)
        {
            using (var enumerator1 = collection1.GetEnumerator())
            using (var enumerator2 = collection2.GetEnumerator())
            {
                while (enumerator1.MoveNext() 
                    && enumerator2.MoveNext())
                {
                    yield return selector(
                        enumerator1.Current, 
                        enumerator2.Current);
                }
            }
        }

        public static IEnumerable<TResult> CombineItems<T1, T2, T3, TResult>(
            this IEnumerable<T1> collection1,
            IEnumerable<T2> collection2,
            IEnumerable<T3> collection3,
            Func<T1, T2, T3, TResult> selector)
        {
            using (var enumerator1 = collection1.GetEnumerator())
            using (var enumerator2 = collection2.GetEnumerator())
            using (var enumerator3 = collection3.GetEnumerator())
            {
                while (enumerator1.MoveNext() 
                    && enumerator2.MoveNext()
                    && enumerator3.MoveNext())
                {
                    yield return selector(
                        enumerator1.Current,
                        enumerator2.Current,
                        enumerator3.Current);
                }
            }            
        }

        public static IEnumerable<TResult> CombineItems<T1, T2, T3, T4, TResult>(
            this IEnumerable<T1> collection1,
            IEnumerable<T2> collection2,
            IEnumerable<T3> collection3,
            IEnumerable<T4> collection4,
            Func<T1, T2, T3, T4, TResult> selector)
        {
            using (var enumerator1 = collection1.GetEnumerator())
            using (var enumerator2 = collection2.GetEnumerator())
            using (var enumerator3 = collection3.GetEnumerator())
            using (var enumerator4 = collection4.GetEnumerator())
            {
                while (enumerator1.MoveNext()
                    && enumerator2.MoveNext()
                    && enumerator3.MoveNext()
                    && enumerator4.MoveNext())
                {
                    yield return selector(
                        enumerator1.Current,
                        enumerator2.Current,
                        enumerator3.Current,
                        enumerator4.Current);
                }
            }
        }

        public static IEnumerable<TResult> CombineItems<T1, T2, T3, T4, T5, TResult>(
            this IEnumerable<T1> collection1,
            IEnumerable<T2> collection2,
            IEnumerable<T3> collection3,
            IEnumerable<T4> collection4,
            IEnumerable<T5> collection5,
            Func<T1, T2, T3, T4, T5, TResult> selector)
        {
            using (var enumerator1 = collection1.GetEnumerator())
            using (var enumerator2 = collection2.GetEnumerator())
            using (var enumerator3 = collection3.GetEnumerator())
            using (var enumerator4 = collection4.GetEnumerator())
            using (var enumerator5 = collection5.GetEnumerator())
            {
                while (enumerator1.MoveNext()
                    && enumerator2.MoveNext()
                    && enumerator3.MoveNext()
                    && enumerator4.MoveNext()
                    && enumerator5.MoveNext())
                {
                    yield return selector(
                        enumerator1.Current,
                        enumerator2.Current,
                        enumerator3.Current,
                        enumerator4.Current,
                        enumerator5.Current);
                }
            }
        }

        public static string ToString<T>(this IEnumerable<T> collection, string separator)
        {
            return ToString<T>(collection, separator, delegate(T item) { return item.ToString(); });
        }

        public static string ToString<T>(this IEnumerable<T> collection, string separator, Func<T, string> toString)
        {
            collection.ThrowIfNull(_ => new NullReferenceException("The collection is null."));

            separator = separator ?? string.Empty;

            return collection.Select(item => toString(item))
                       .Aggregate((s1, s2) => string.Format("{0}{1}{2}", s1, separator, s2));
        }

        public static string ToCsv<T>(this IEnumerable<T> collection)
        {
            return ToString(collection, ",");
        }

        public static string ToCsv<T>(this IEnumerable<T> collection, Func<T, string> toString)
        {
            return ToString(collection, ",", toString);
        }

        public static string ToCsv<T>(this IEnumerable<T> collection, bool addSpaceAfterComma)
        {
            return ToString(collection, addSpaceAfterComma ? ", " : ",");
        }

        public static string ToCsv<T>(this IEnumerable<T> collection, bool addSpaceAfterComma, Func<T, string> toString)
        {
            return ToString(collection, addSpaceAfterComma ? ", " : ",", toString);
        }

        public static string ToCsv<T>(this IEnumerable<T> collection, string conjunction)
        {
            return ToCsv(collection, conjunction, item => item.ToString());
        }

        public static string ToCsv<T>(this IEnumerable<T> collection, string conjunction, Func<T, string> toString)
        {
            collection.ThrowIfNull(_ => new NullReferenceException("The collection is null."));

            conjunction = (conjunction ?? string.Empty).Length > 0 
                        ? string.Format("{0} ", conjunction)
                        : conjunction;            

            return string.Format(
                "{0}, {1}{2}",
                ToString(collection.Take(collection.Count() - 1), ", ", toString),
                conjunction,
                toString(collection.Last()));
        }

        public static string ToSingleQuotedCsv<T>(this IEnumerable<T> collection)
        {
            return ToCsv<T>(collection, item => item.ToSingleQuotedString());
        }

        public static string ToSingleQuotedCsv<T>(this IEnumerable<T> collection, Func<T, string> toString)
        {
            return ToCsv<T>(collection, item => item.ToSingleQuotedString(toString));
        }

        public static string ToSingleQuotedCsv<T>(this IEnumerable<T> collection, bool addSpaceAfterComma)
        {
            return ToCsv<T>(collection, addSpaceAfterComma, item => item.ToSingleQuotedString());
        }

        public static string ToSingleQuotedCsv<T>(this IEnumerable<T> collection, bool addSpaceAfterComma, Func<T, string> toString)
        {
            return ToCsv<T>(collection, addSpaceAfterComma, item => item.ToSingleQuotedString(toString));
        }

        public static string ToSingleQuotedCsv<T>(this IEnumerable<T> collection, string conjunction)
        {
            return ToCsv<T>(collection, conjunction, item => item.ToSingleQuotedString());
        }

        public static string ToSingleQuotedCsv<T>(this IEnumerable<T> collection, string conjunction, Func<T, string> toString)
        {
            return ToCsv<T>(collection, conjunction, item => item.ToSingleQuotedString(toString));
        }

        public static List<T> FromString<T>(this string s, string separator, Converter<string, T> fromString)
        {
            return FromString<T>(s, separator, fromString, StringSplitOptions.None);
        }

        public static List<T> FromString<T>(this string s, string separator, Converter<string, T> fromString, StringSplitOptions options)
        {
            separator = separator ?? string.Empty;

            string[] subStrings = s.Split(new string[] { separator }, options);

            List<T> collection = new List<T>();
            foreach (string subStr in subStrings)
            {
                collection.Add(fromString(subStr));
            }

            return collection;
        }

        public static List<string> FromCsv(this string str)
        {
            return FromString<string>(str, ",", item => item);
        }

        public static List<string> FromCsv(this string str, bool trim)
        {
            return FromCsv(str, trim, StringSplitOptions.None);
        }

        public static List<string> FromCsv(this string str, bool trim, StringSplitOptions options)
        {
            return FromString<string>(str, ",", item => item.Trim(), options);
        }

        public static List<string> FromCsv(this string str, bool trimStart, bool trimEnd)
        {
            return FromCsv(str, trimStart, trimEnd, StringSplitOptions.None);
        }

        public static List<string> FromCsv(this string str, bool trimStart, bool trimEnd, StringSplitOptions options)
        {
            if (trimStart && trimEnd)
            {
                return FromCsv(str, true, options);
            }

            Converter<string, string> fromString =
                item =>
                {
                    if (trimStart)
                    {
                        return item.TrimStart();
                    }
                    else if (trimEnd)
                    {
                        return item.TrimEnd();
                    }
                    else
                    {
                        return item;
                    }
                };

            return FromString<string>(str, ",", fromString, options);
        }

        public static List<T> FromCsv<T>(this string str, Converter<string, T> fromString)
        {
            return FromString<T>(str, ",", fromString);
        }

        public static List<T> FromCsv<T>(this string str, Converter<string, T> fromString, StringSplitOptions options)
        {
            return FromString<T>(str, ",", fromString, options);
        }        
    }
}