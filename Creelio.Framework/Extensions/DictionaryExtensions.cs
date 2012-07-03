namespace Creelio.Framework.Core.Extensions.DictionaryExtensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class DictionaryExtensions
    {
        public static void AddOrSet<K, V>(this Dictionary<K, V> dictionary, K key, V value)
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
        }

        public static V GetOrAddDefault<K, V>(this Dictionary<K, V> dictionary, K key)
            where V : new()
        {
            return dictionary.GetOrAddDefault(key, () => new V());
        }

        public static V GetOrAddDefault<K, V>(this Dictionary<K, V> dictionary, K key, V defaultValue)
        {
            return dictionary.GetOrAddDefault(key, () => defaultValue);
        }

        public static V GetOrAddDefault<K, V>(this Dictionary<K, V> dictionary, K key, Func<V> getDefault)
        {
            Validate(dictionary, getDefault);

            if (!dictionary.ContainsKey(key))
            {
                dictionary.Add(key, getDefault());
            }

            return dictionary[key];
        }

        public static V GetOrReturnDefault<K, V>(this Dictionary<K, V> dictionary, K key)
            where V : new()
        {
            return dictionary.GetOrReturnDefault(key, () => new V());
        }

        public static V GetOrReturnDefault<K, V>(this Dictionary<K, V> dictionary, K key, V defaultValue)
        {
            return dictionary.GetOrReturnDefault(key, () => defaultValue);
        }

        public static V GetOrReturnDefault<K, V>(this Dictionary<K, V> dictionary, K key, Func<V> getDefault)
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

        public static V GetOrReturnNull<K, V>(this Dictionary<K, V> dictionary, K key)
            where V : class
        {
            return GetOrReturnDefault(dictionary, key, () => null);
        }

        public static bool ContainsKeyValue<K, V>(this Dictionary<K, V> dictionary, K key, V value)
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

        public static bool IsNullOrEmpty<K, V>(this Dictionary<K, V> dictionary)
        {
            return dictionary == null || !dictionary.Any();
        }

        private static void Validate<K, V>(Dictionary<K, V> dictionary)
        {
            if (dictionary == null)
            {
                throw new NullReferenceException("The dictionary object passed to the extension method was null.");
            }
        }

        private static void Validate<K, V>(Dictionary<K, V> dictionary, Func<V> getDefault)
        {
            Validate(dictionary);

            if (getDefault == null)
            {
                throw new ArgumentNullException("getDefault");
            }
        }
    }
}