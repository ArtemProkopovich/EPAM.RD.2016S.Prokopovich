using System;
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
