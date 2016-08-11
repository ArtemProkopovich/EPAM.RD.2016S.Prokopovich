using System;
using System.Collections.Generic;
using System.Linq;
using UserStorage.Interfacies;
using UserStorage.Entity;

namespace UserStorage.Service
{
    /// <summary>
    /// Proxy for services
    /// </summary>
    public class ServiceProxy : IService<User>
    {
        private IService<User> Master { get; set; }
        private List<IService<User>> Slaves { get; set; }
        /// <summary>
        /// Create service proxy with master and any slaves
        /// </summary>
        /// <param name="master"></param>
        /// <param name="slaves"></param>
        public ServiceProxy(IService<User> master, IEnumerable<IService<User>> slaves)
        {
            this.Master = master;
            this.Slaves = slaves.ToList();
        }

        /// <summary>
        /// Add item in service
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public int Add(User item)
        {
            return Master.Add(item);
        }

        /// <summary>
        /// Delete item form service
        /// </summary>
        /// <param name="item"></param>
        public void Delete(User item)
        {
            Master.Delete(item);
        }

        /// <summary>
        /// Save state of service
        /// </summary>
        public void Save()
        {
            Master.Save();
        }

        private volatile int currentSlave = 0;

        /// <summary>
        /// Search user in service
        /// </summary>
        /// <param name="searchCriteria"></param>
        /// <returns></returns>
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
