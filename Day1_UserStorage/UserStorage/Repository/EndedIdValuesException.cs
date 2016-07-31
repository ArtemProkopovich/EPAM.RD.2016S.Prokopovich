using System;
using System.Runtime.Serialization;
using UserStorage.Repository;

namespace UserStorage.Interfacies
{
    [Serializable]
    public class EndedIdValuesException : RepositoryException
    {

        public EndedIdValuesException()
        {
        }

        public EndedIdValuesException(string message) : base(message)
        {
        }

        public EndedIdValuesException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public EndedIdValuesException(SerializationInfo info, StreamingContext context)
        {

        }
    }
}
