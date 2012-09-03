namespace Creelio.Framework.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Creelio.Framework.Extensions;

    public static class IComparableExtensions
    {
        public static bool IsLessThan<T>(this IComparable<T> comparable, T other)
        {
            Validate(comparable, other);
            return comparable.CompareTo(other) < 0;
        }

        public static bool IsGreaterThan<T>(this IComparable<T> comparable, T other)
        {
            Validate(comparable, other);
            return comparable.CompareTo(other) > 0;
        }

        public static bool ValueEquals<T>(this IComparable<T> comparable, T other)
        {
            Validate(comparable, other);
            return comparable.CompareTo(other) == 0;
        }

        public static bool IsLessThanOrEqualTo<T>(this IComparable<T> comparable, T other)
        {
            Validate(comparable, other);
            return IsLessThan(comparable, other) || ValueEquals(comparable, other);
        }

        public static bool IsGreaterThanOrEqualTo<T>(this IComparable<T> comparable, T other)
        {
            Validate(comparable, other);
            return IsGreaterThan(comparable, other) || ValueEquals(comparable, other);
        }

        public static int CompareToProperties<T>(this T comparable, T other, params string[] propertyNames)
        {
            comparable.ThrowIfNull(_ => new NullReferenceException("comparable"));
            other.ThrowIfNull(_ => new NullReferenceException("other"));

            var properties = GetProperties<T>(propertyNames);

            var comparisons = from prop in properties
                              let propType = prop.Property.PropertyType
                              let compareTo = propType.GetMethod("CompareTo", new Type[] { propType }) ?? propType.GetMethod("CompareTo")
                              let cv = prop.GetValue(comparable)
                              let ov = prop.GetValue(other)
                              select (int)compareTo.Invoke(cv, new object[] { ov });

            return comparisons.FirstOrDefault(c => c != (int)ComparableResult.EqualToOther);
        }

        private static IEnumerable<InnerPropertyReflector> GetProperties<T>(string[] propertyNames)
        {
            propertyNames
                .ThrowIf(
                    pns => pns == null || pns.Length < 1,
                    _ => new ArgumentNullException("propertyNames", "Property names list cannot be null or empty."))
                .ThrowIf(
                    pns => pns.Any(pn => string.IsNullOrWhiteSpace(pn)),
                    _ => new ArgumentException("One of the given property names is blank."));

            var type = typeof(T);
            var properties = from pn in propertyNames
                             select InnerPropertyReflector.Create(type, pn);

            return properties;
        }

        private static void Validate<T>(IComparable<T> comparable, object other)
        {
            comparable.ThrowIfNull(_ => new NullReferenceException("comparable"));
            other.ThrowIfNull(_ => new NullReferenceException("other"));
        }

        private class InnerPropertyReflector
        {
            private List<PropertyInfo> _properties = null;
                        
            private InnerPropertyReflector()
            {
            }

            public PropertyInfo Property
            {
                get
                {
                    return Properties.LastOrDefault();
                }
            }

            public List<PropertyInfo> Properties
            {
                get
                {
                    if (_properties == null)
                    {
                        _properties = new List<PropertyInfo>();
                    }

                    return _properties;
                }
            }

            public static InnerPropertyReflector Create(Type type, string compositePropertyName)
            {
                var propertyNames = compositePropertyName.Split(StringSplitOptions.RemoveEmptyEntries, '.');

                propertyNames.ThrowIf(p => p.Length < 1, _ => new ArgumentException(string.Format("Invalid property name: '{0}'.", compositePropertyName)));

                var innerType = type;
                var reflector = new InnerPropertyReflector();

                foreach (var propertyName in propertyNames)
                {
                    var property = innerType.GetProperty(propertyName);
                    property.ThrowIfNull(
                        _ => new ArgumentException(string.Format("{0} does not contain a property '{1}'.", type, compositePropertyName)));

                    reflector.Properties.Add(property);

                    innerType = property.PropertyType;
                }

                var interfaces = reflector.Property.PropertyType.GetInterfaces();
                interfaces.ThrowIf(
                    i => !interfaces.Any(intr => intr == typeof(IComparable) || intr == typeof(IComparable<>)),
                    _ => new ArgumentException(string.Format("Property '{1}' of type {0} does not implement IComparable or IComparable<T>.", type, compositePropertyName)));

                return reflector;
            }

            public object GetValue(object baseObject)
            {
                var value = baseObject;

                foreach (var property in Properties)
                {
                    value = property.GetValue(baseObject, null);
                    baseObject = value;
                }

                return value;
            }
        }
    }
}