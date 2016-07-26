using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserStorage.Interfacies;
using UserStorage.Entity;

namespace UserStorage.Service
{
    public class ServiceProxy : IService<User>
    {
        private IService<User> Master { get; set; }
        private IEnumerable<IService<User>> Slaves { get; set; }
        public ServiceProxy(IService<User> master, IEnumerable<IService<User>> slaves)
        {
            this.Master = master;
            this.Slaves = slaves;
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

        public IEnumerable<User> Search(params Func<User, bool>[] searchCriterias)
        {
            throw new NotImplementedException();
        }
    }
}
