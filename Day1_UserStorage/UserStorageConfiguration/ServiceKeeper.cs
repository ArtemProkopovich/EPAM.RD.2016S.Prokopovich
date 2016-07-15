using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserStorage.Interfacies;
using UserStorage.Service;

namespace UserStorageConfiguration
{
    public class ServiceKeeper
    {
        public MasterService Master { get; private set; }
        public IEnumerable<SlaveService> Slaves { get; private set; }
        public ServiceKeeper(MasterService master, IEnumerable<SlaveService> slaves)
        {
            this.Master = master;
            this.Slaves = slaves;
        }
    }
}
