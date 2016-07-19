using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserStorage.Repository
{
    [Serializable]
    public class RepositoryException : Exception
    {
        public RepositoryException()
        {

        }

        public RepositoryException(SerializationInfo info, StreamingContext context)
        {

        }
    }
}
