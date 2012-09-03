namespace Creelio.Framework.UnitTesting
{
    using System;
    using System.Collections.Generic;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    public static class DtoAssert
    {
        public static void DeserializedObjectIsEqualToOriginal<T>(T source, IEqualityComparer<T> comparer)
        {
            DeserializedObjectIsEqualToOriginal<T>(source, (x, y) => comparer.Equals(x, y));
        }

        public static void DeserializedObjectIsEqualToOriginal<T>(T source, Func<T, T, bool> equals)
        {
            var clone = Serializer.CloneBySerialization(source);
            var result = equals(source, clone);
            Assert.IsTrue(result, "The clone object does not equal the original object when using serialization/deserialization.");
        }

        public static void DataContractDeserializedObjectIsEqualToOriginal<T>(T source, IEqualityComparer<T> comparer)
        {
            DataContractDeserializedObjectIsEqualToOriginal<T>(source, (x, y) => comparer.Equals(x, y));
        }

        public static void DataContractDeserializedObjectIsEqualToOriginal<T>(T source, Func<T, T, bool> equals)
        {
            var clone = Serializer.CloneByDataContractSerialization(source);
            var result = equals(source, clone);
            Assert.IsTrue(result, "The clone object does not equal the original object when using data contract serialization/deserialization.");
        }
    }
}