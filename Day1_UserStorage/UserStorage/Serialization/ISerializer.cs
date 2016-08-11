using System.IO;


namespace UserStorage.Serialization
{
    /// <summary>
    /// Interface of serializer
    /// </summary>
    /// <typeparam name="T">Type of object to serialize</typeparam>
    public interface ISerializer<T>
    {
        void SerializeObject(T obj, Stream stream);
        T DeserializeObject(Stream stream);
    }
}
