using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserStorage.Interfacies;
using UserStorage.Entity;
using UserStorage;
using NLog;

namespace UserStorage.Service
{
    public class SlaveService : IService<User>
    {
        private readonly IRepository<User> userRepository;
        private readonly bool isLogged = false;
        private Logger logger = LogManager.GetCurrentClassLogger();

        public SlaveService(IRepository<User> userRepository)
        {
            this.userRepository = userRepository;
        }

        public SlaveService(IRepository<User> userRepository, bool isLogged) : this(userRepository)
        {
            this.isLogged = isLogged;
        }

        public int Add(User item)
        {
            if (isLogged)
                logger.Info("message");
            throw new FeatureNotAvailiableException();
        }

        public void Delete(int id)
        {
            if (isLogged)
                logger.Info("message");
            throw new FeatureNotAvailiableException();
        }

        public IEnumerable<User> Search(params Func<User, bool>[] searchCriterias)
        {
            if (isLogged)
                logger.Info("message");
            return userRepository.SearchAll(searchCriterias);
        }

        public void OnAdded(object sender, DataUpdatedEventArgs<User> args)
        {
            if (isLogged)
                logger.Info("message");
            userRepository.Add(args.data);
        }
        public void OnDeleted(object sender, DataUpdatedEventArgs<User> args)
        {
            if (isLogged)
                logger.Info("message");
            userRepository.Delete(args.data);
        }

        public void Save()
        {
            if (isLogged)
                logger.Info("message");
            throw new FeatureNotAvailiableException();
        }
    }
}
