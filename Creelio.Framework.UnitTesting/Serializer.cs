namespace Creelio.Framework.UnitTesting
{
    using System;
    using System.IO;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Formatters.Binary;

    public class Serializer
    {
        public static Stream Serialize(object source)
        {
            var formatter = new BinaryFormatter();
            return Serialize(source, formatter);
        }

        public static Stream Serialize(object source, IFormatter formatter)
        {
            var stream = new MemoryStream();
            formatter.Serialize(stream, source);
            return stream;
        }

        public static Stream DataContractSerialize(object source)
        {
            var serializer = new DataContractSerializer(source.GetType());
            var stream = new MemoryStream();
            serializer.WriteObject(stream, source);
            return stream;
        }

        public static T Deserialize<T>(Stream stream)
        {
            var formatter = new BinaryFormatter();
            return Deserialize<T>(stream, formatter);
        }

        public static T Deserialize<T>(Stream stream, IFormatter formatter)
        {
            stream.Position = 0;
            return (T)formatter.Deserialize(stream);
        }

        public static T DataContractDeserialize<T>(Stream stream, Type type)
        {
            var serializer = new DataContractSerializer(type);
            stream.Position = 0;
            return (T)serializer.ReadObject(stream);
        }

        public static T CloneBySerialization<T>(T source)
        {
            return Deserialize<T>(Serialize(source));
        }

        public static T CloneBySerialization<T>(T source, IFormatter formatter)
        {
            return Deserialize<T>(Serialize(source, formatter), formatter);
        }

        public static T CloneByDataContractSerialization<T>(T source)
        {
            return DataContractDeserialize<T>(DataContractSerialize(source), typeof(T));
        }
    }
}
