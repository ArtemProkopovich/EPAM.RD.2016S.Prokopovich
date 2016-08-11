using System;
using UserStorage.Entity;
namespace UserStorage.Service
{
    [Serializable]
    public enum Operation { Add=1, Delete=2 };

    /// <summary>
    /// Message that passed between master and slaves
    /// </summary>
    [Serializable]
    public class ServiceMessage
    {
        public Operation Operation { get; set; }
        public User user { get; set; }
    }
}
