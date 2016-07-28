using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
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

        public Guid ServiceId
        {
            get
            {
                return Master.ServiceId;
            }
            set { }
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

        private volatile int curSlave = 0;

        public IEnumerable<User> Search(Func<User, bool> searchCriteria)
        {
            if (Slaves.Count < 0)
                return Master.Search(searchCriteria);
            else
            {
                int slave = curSlave;
                curSlave = (curSlave + 1) % Slaves.Count;
                return Slaves[slave].Search(searchCriteria);
            }
        }

    }
}
