using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using UserStorage.Entity;
using UserStorage.Service;

namespace WcfService
{
    public class ServiceEventArgs : EventArgs
    {
        public User User { get; set; }
    }

    public class ServiceSearchEventArgs : EventArgs
    {
        public UserCriteria Criteria { get; set; }
        public int Count { get; set; }
    }

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class UserService : IUserService
    {
        public event EventHandler<ServiceEventArgs> Added = (sender, args) => { };
        public event EventHandler<ServiceEventArgs> Deleted = (sender, args) => { };
        public event EventHandler<ServiceSearchEventArgs> Searched = (sender, args) => { };

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
            try
            {
                int result = proxy.Add(item);
                item.Id = result;
                OnAdded(new ServiceEventArgs() { User = item });
                return result;
            }
            catch(Exception ex)
            {
                throw new FaultException<WcfServiceException>(new WcfServiceException() { exception = ex }, new FaultReason("wcf exception"));
            }
        }

        public void Delete(User item)
        {
            try
            {
                proxy.Delete(item);
                OnDeleted(new ServiceEventArgs() { User = item });
            }
            catch (Exception ex)
            {
                throw new FaultException<WcfServiceException>(new WcfServiceException() { exception = ex }, new FaultReason("wcf exception"));
            }
        }

        public IEnumerable<User> Search(UserCriteria criteria)
        {
            try
            {
                var result = proxy.Search(criteria).ToList();
                OnSearched(new ServiceSearchEventArgs() { Criteria = criteria, Count = result.Count });
                return result;
            }
            catch (Exception ex)
            {
                throw new FaultException<WcfServiceException>(new WcfServiceException() { exception = ex }, new FaultReason("wcf exception"));
            }
        }

        protected void OnAdded(ServiceEventArgs args)
        {
            Added?.Invoke(this, args);
        }

        protected void OnDeleted(ServiceEventArgs args)
        {
            Deleted?.Invoke(this, args);
        }

        protected void OnSearched(ServiceSearchEventArgs args)
        {
            Searched?.Invoke(this, args);
        }
    }
}
