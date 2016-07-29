using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserStorage.Entity;
namespace UserStorage.Service
{
    [Serializable]
    public enum Operation { Add=1, Delete=2 };
    [Serializable]
    public class ServiceMessage
    {
        public Operation Operation { get; set; }
        public User user { get; set; }
    }
}
