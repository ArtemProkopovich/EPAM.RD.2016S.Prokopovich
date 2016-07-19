using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace UserStorage.Interfacies
{
    [Serializable]
    public class EndedIdValuesException : Exception
    {

        public EndedIdValuesException()
        {
        }

        public EndedIdValuesException(SerializationInfo info, StreamingContext context)
        {

        }
    }
}
