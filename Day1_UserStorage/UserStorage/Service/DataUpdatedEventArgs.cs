using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserStorage.Service
{
    public class DataUpdatedEventArgs<T> : EventArgs
    {
        public T data;
    }
}
