using System;
using System.Runtime.Serialization;

namespace UserStorage.Interfacies
{
    [Serializable]
    public class InvalidArgumentException: ArgumentException
    {
        public InvalidArgumentException()
        {
        }

        public InvalidArgumentException(string message) : base(message)
        {

        }

        public InvalidArgumentException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public InvalidArgumentException(string message, string paramName) : base(message, paramName)
        {

        }

        public InvalidArgumentException(SerializationInfo info, StreamingContext context)
        {

        }

        public InvalidArgumentException(string message, string paramName, Exception innerException) : base(message, paramName, innerException)
        {
        }
    }
}
