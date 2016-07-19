using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace UserStorage.Interfacies
{
    [Serializable]
    public class InvalidArgumentException: ArgumentException
    {
        public InvalidArgumentException(string message) : base(message)
        {

        }

        public InvalidArgumentException(string message, string paramName) : base(message, paramName)
        {

        }

        public InvalidArgumentException(SerializationInfo info, StreamingContext context)
        {

        }
    }
}
