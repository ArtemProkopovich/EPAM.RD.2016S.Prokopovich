using System;
using System.Collections.Generic;
using System.Linq;
using UserStorage.Interfacies;
using UserStorage.Entity;

namespace UserStorage.Service
{
    public class ServiceProxy : IService<User>
    {
        private IService<User> Master { get; set; }
        private List<IService<User>> Slaves { get; set; }
        public ServiceProxy(IService<User> master, IEnumerable<IService<User>> slaves)
        {
            this.Master = master;
            this.Slaves = slaves.ToList();
        }

        public int Add(User item)
        {
            return Master.Add(item);
        }

        public void Delete(User item)
        {
            Master.Delete(item);
        }

        public void Save()
        {
            Master.Save();
        }

        private volatile int currentSlave = 0;

        public IEnumerable<User> Search(ICriteria<User> searchCriteria)
        {
            if (Slaves.Count < 0)
                return Master.Search(searchCriteria);
            else
            {
                int slave = currentSlave;
                currentSlave = (currentSlave + 1) % Slaves.Count;
                return Slaves[slave].Search(searchCriteria);
            }
        }

    }
}
