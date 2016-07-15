using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserStorage.Interfacies;
using UserStorage.Entity;
using NLog;

namespace UserStorage.Service
{
    public class MasterService : IService<User>
    {
        private Logger logger = LogManager.GetCurrentClassLogger();
        private readonly IRepository<User> userRepository;
        private readonly bool isLogged = false;
        public event EventHandler<DataUpdatedEventArgs<User>> Added;
        public event EventHandler<DataUpdatedEventArgs<User>> Deleted;

        public MasterService(IRepository<User> userRepository)
        {
            this.userRepository = userRepository;
            Added += (o, arg) => { };
            Deleted += (o, arg) => { };
        }

        public MasterService(IRepository<User> userRepository, bool isLogged) : this(userRepository)
        {
            this.isLogged = isLogged;
        }

        public int Add(User item)
        {
            try
            {
                if (isLogged)
                    logger.Info("message");
                int result = userRepository.Add(item);
                OnAdded(new DataUpdatedEventArgs<User>() { data = userRepository.GetById(result) });
                return result;
            }
            catch (Exception ex)
            {
                throw new ServiceException();
            }
        }

        public void Delete(int id)
        {
            try
            {
                if (isLogged)
                    logger.Info("message");
                var user = userRepository.GetById(id);
                userRepository.Delete(user);
                OnDeleted(new DataUpdatedEventArgs<User>() { data = user });
            }
            catch (Exception ex)
            {
                throw new ServiceException();
            }
        }

        public IEnumerable<User> Search(params Func<User, bool>[] searchCriterias)
        {
            try
            {
                if (isLogged)
                    logger.Info("message");
                return userRepository.SearchAll(searchCriterias);
            }
            catch (Exception ex)
            {
                throw new ServiceException();
            }
        }

        public void Save()
        {
            try
            {
                if (isLogged)
                    logger.Info("message");
                userRepository.Save();
            }
            catch (Exception ex)
            {
                throw new ServiceException();
            }
        }

        protected void OnAdded(DataUpdatedEventArgs<User> arg)
        {
            Added?.Invoke(this, arg);
        }

        protected void OnDeleted(DataUpdatedEventArgs<User> arg)
        {
            Deleted?.Invoke(this, arg);
        }

    }
}
