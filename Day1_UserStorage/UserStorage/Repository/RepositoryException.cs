using System;
using System.Runtime.Serialization;

namespace UserStorage.Repository
{
    [Serializable]
    public class RepositoryException : Exception
    {
        public RepositoryException()
        {

        }

        public RepositoryException(string message) : base(message)
        {
        }

        public RepositoryException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public RepositoryException(SerializationInfo info, StreamingContext context)
        {

        }
    }
}
