using System.IO;


namespace UserStorage.Serialization
{
    public interface ISerializer<T>
    {
        void SerializeObject(T obj, Stream stream);
        T DeserializeObject(Stream stream);
    }
}
