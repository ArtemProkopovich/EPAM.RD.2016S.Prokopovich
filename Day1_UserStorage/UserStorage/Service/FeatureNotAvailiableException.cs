using System;
using System.Runtime.Serialization;
namespace UserStorage.Service
{
    [Serializable]
    public class FeatureNotAvailiableException : ServiceException
    {
        public FeatureNotAvailiableException() : base()
        {
            
        }

        public FeatureNotAvailiableException(string message) : base(message)
        {
        }

        public FeatureNotAvailiableException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public FeatureNotAvailiableException(SerializationInfo info, StreamingContext context)
        {

        }
    }
}
