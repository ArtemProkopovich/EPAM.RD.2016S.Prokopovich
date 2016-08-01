using System;
using System.Runtime.Serialization;

namespace WcfService
{
    [DataContract]
    public class WcfServiceException
    {
        [DataMember]
        public Exception exception;
        [DataMember]
        public string message;

        public WcfServiceException() 
        {

        }
    }
}
