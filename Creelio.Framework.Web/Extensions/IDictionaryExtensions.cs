namespace Creelio.Framework.Web.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Creelio.Framework.Extensions;

    public static class IDictionaryExtensions
    {
        public static string ToQueryString<K, V>(this IDictionary<K, V> dictionary)
        {
            return ToQueryString<K, V>(dictionary, k => k.ToString(), v => v.ToString());
        }

        public static string ToQueryString<K, V>(this IDictionary<K, V> dictionary, bool includeQuestionMark)
        {
            return ToQueryString<K, V>(dictionary, includeQuestionMark, k => k.ToString(), v => v.ToString());
        }

        public static string ToQueryString<K, V>(
            this IDictionary<K, V> dictionary,
            Func<K, string> keyToString,
            Func<V, string> valueToString)
        {
            return ToQueryString<K, V>(dictionary, true, keyToString, valueToString);
        }

        public static string ToQueryString<K, V>(
            this IDictionary<K, V> dictionary,
            bool includeQuestionMark,
            Func<K, string> keyToString,
            Func<V, string> valueToString)
        {
            Validate(dictionary);

            var sb = new StringBuilder();

            if (includeQuestionMark)
            {
                sb.Append("?");
            }

            var kvpStrings = from kvp in dictionary
                             select string.Format(
                                 "{0}={1}",
                                 /*HttpUtility.UrlEncode(*/keyToString(kvp.Key)/*)*/,
                                 /*HttpUtility.UrlEncode(*/valueToString(kvp.Value)/*)*/);

            sb.Append(kvpStrings.Aggregate((s1, s2) => string.Format("{0}&{1}", s1, s2)));

            return sb.ToString();
        }

        public static IDictionary<string, string> FillFromQueryString(this IDictionary<string, string> dictionary, string queryString)
        {
            return FillFromQueryString<string, string>(dictionary, queryString, k => k, v => v);
        }

        public static IDictionary<K, V> FillFromQueryString<K, V>(
            this IDictionary<K, V> dictionary,
            string queryString,
            Converter<string, K> keyConverter,
            Converter<string, V> valueConverter)
        {
            Validate(dictionary);

            queryString = queryString.TrimOrEmpty();

            if (queryString.Length > 2)
            {
                if (queryString.StartsWith("?"))
                {
                    queryString = queryString.Substring(1);
                }

                var kvps = queryString.Split('&');
                foreach (var kvp in kvps)
                {
                    var kvpParts = kvp.Split('=');
                    if (kvpParts.Length == 2)
                    {
                        var key = keyConverter(kvpParts[0]);
                        var value = valueConverter(kvpParts[1]);

                        dictionary.AddOrSet(key, value);
                    }
                }
            }

            return dictionary;
        }

        private static void Validate<K, V>(IDictionary<K, V> dictionary)
        {
            if (dictionary == null)
            {
                throw new NullReferenceException("The dictionary object passed to the extension method was null.");
            }
        }
    }
}
