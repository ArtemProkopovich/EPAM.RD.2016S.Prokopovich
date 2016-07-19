using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace UserStorage.Service
{
    [Serializable]
    public class ServiceException : Exception
    {
        public ServiceException() : base()
        {

        }

        public ServiceException(SerializationInfo info, StreamingContext context)
        {

        }
    }
}
