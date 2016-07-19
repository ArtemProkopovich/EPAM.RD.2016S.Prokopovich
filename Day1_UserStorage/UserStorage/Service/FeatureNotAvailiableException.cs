using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
namespace UserStorage.Service
{
    [Serializable]
    public class FeatureNotAvailiableException : ServiceException
    {
        public FeatureNotAvailiableException() : base()
        {
            
        }

        public FeatureNotAvailiableException(SerializationInfo info, StreamingContext context)
        {

        }
    }
}
