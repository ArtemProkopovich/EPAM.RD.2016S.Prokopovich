using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace UserStorage.Service
{
    [Serializable]
    public class ServiceConnection
    {
        public IPAddress Address { get; set; } 
        public int Port { get; set; }
    }
}
