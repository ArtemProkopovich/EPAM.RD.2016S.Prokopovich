using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserStorage.Serialization
{
    [AttributeUsage(AttributeTargets.Class)]
    public class JsonObjectAttribute : Attribute
    {
    }
}
