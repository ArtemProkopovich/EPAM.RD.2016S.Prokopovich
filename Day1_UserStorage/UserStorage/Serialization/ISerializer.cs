using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace UserStorage.Serialization
{
    public interface ISerializer<T>
    {
        void SerializeObject(T obj, Stream stream);
        T DeserializeObject(Stream stream);
    }
}
