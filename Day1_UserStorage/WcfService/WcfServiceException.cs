using System;
using System.Runtime.Serialization;

namespace WcfService
{
    /// <summary>
    /// Exception that occurs in wcf service
    /// </summary>
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
