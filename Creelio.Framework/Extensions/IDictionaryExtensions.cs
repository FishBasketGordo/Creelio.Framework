namespace Creelio.Framework.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class IDictionaryExtensions
    {
        public static IDictionary<K, V> AddOrSet<K, V>(this IDictionary<K, V> dictionary, K key, V value)
        {
            Validate(dictionary);

            if (dictionary.ContainsKey(key))
            {
                dictionary[key] = value;
            }
            else
            {
                dictionary.Add(key, value);
            }

            return dictionary;
        }

        public static V GetOrAddDefault<K, V>(this IDictionary<K, V> dictionary, K key)
        {
            return dictionary.GetOrAddDefault(key, () => default(V));
        }

        public static V GetOrAddDefault<K, V>(this IDictionary<K, V> dictionary, K key, V defaultValue)
        {
            return dictionary.GetOrAddDefault(key, () => defaultValue);
        }

        public static V GetOrAddDefault<K, V>(this IDictionary<K, V> dictionary, K key, Func<V> getDefault)
        {
            Validate(dictionary, getDefault);

            if (!dictionary.ContainsKey(key))
            {
                dictionary.Add(key, getDefault());
            }

            return dictionary[key];
        }

        public static V GetOrReturnDefault<K, V>(this IDictionary<K, V> dictionary, K key)
        {
            return dictionary.GetOrReturnDefault(key, () => default(V));
        }

        public static V GetOrReturnDefault<K, V>(this IDictionary<K, V> dictionary, K key, V defaultValue)
        {
            return dictionary.GetOrReturnDefault(key, () => defaultValue);
        }

        public static V GetOrReturnDefault<K, V>(this IDictionary<K, V> dictionary, K key, Func<V> getDefault)
        {
            Validate(dictionary, getDefault);

            if (dictionary.ContainsKey(key))
            {
                return dictionary[key];
            }
            else
            {
                return getDefault();
            }
        }

        public static bool ContainsKeyValue<K, V>(this IDictionary<K, V> dictionary, K key, V value)
        {
            Validate(dictionary);

            if (!dictionary.ContainsKey(key))
            {
                return false;
            }

            var dictionaryValue = dictionary[key];

            if (dictionaryValue == null)
            {
                return value == null;
            }
            else
            {
                return dictionaryValue.Equals(value);
            }
        }

        public static bool IsNullOrEmpty<K, V>(this IDictionary<K, V> dictionary)
        {
            return dictionary == null || !dictionary.Any();
        }

        private static void Validate<K, V>(IDictionary<K, V> dictionary)
        {
            if (dictionary == null)
            {
                throw new NullReferenceException("The dictionary object passed to the extension method was null.");
            }
        }

        private static void Validate<K, V>(IDictionary<K, V> dictionary, Func<V> getDefault)
        {
            Validate(dictionary);

            if (getDefault == null)
            {
                throw new ArgumentNullException("getDefault");
            }
        }
    }
}