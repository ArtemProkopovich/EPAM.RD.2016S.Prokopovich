using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using UserStorage.Entity;
using UserStorage.Service;

namespace WcfService
{

    public class UserService : IUserService
    {
        private readonly ServiceProxy proxy;

        public UserService()
        {

        }

        public UserService(ServiceProxy proxy)
        {
            this.proxy = proxy;
        }

        public int Add(User item)
        {
            return proxy.Add(item);
        }

        public void Delete(User item)
        {
            proxy.Delete(item);
        }

        public IEnumerable<User> Search(UserCriteria criteria)
        {
            return proxy.Search(criteria);
        }
    }
}
