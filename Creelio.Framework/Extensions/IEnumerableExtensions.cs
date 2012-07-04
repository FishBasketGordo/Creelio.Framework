namespace Creelio.Framework.Core.Extensions
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Creelio.Framework.Core.Extensions.MaybeMonad;

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