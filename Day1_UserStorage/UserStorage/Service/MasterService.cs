using System;
using System.Collections.Generic;
using System.Threading;
using System.IO;
using System.Net.Sockets; 
using UserStorage.Interfacies;
using UserStorage.Entity;
using UserStorage.Extension;
using UserStorage.Serialization;
using NLog;
using UserStorage.Net;

namespace UserStorage.Service
{
    /// <summary>
    /// Implementation of IService of users that provided all operations
    /// </summary>
    [Serializable]
    public class MasterService : MarshalByRefObject, IService<User>
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly IRepository<User> userRepository;
        private readonly bool isLogged = true;
        private readonly ReaderWriterLockSlim slimLock = new ReaderWriterLockSlim();
        private readonly IEnumerable<ServiceConnection> connections = new List<ServiceConnection>();

        /// <summary>
        /// Create repository with specified repository
        /// </summary>
        /// <param name="userRepository">Init repository for service</param>
        public MasterService(IRepository<User> userRepository)
        {
            if (userRepository == null)
                throw new ArgumentNullException(nameof(userRepository));
            this.userRepository = (IRepository<User>)userRepository.Clone();
        }

        /// <summary>
        /// Create repository with specified repository and connections
        /// </summary>
        /// <param name="userRepository">Init repository for service</param>
        /// <param name="connections">Addresses which the master sends a messages</param>
        public MasterService(IRepository<User> userRepository, IEnumerable<ServiceConnection> connections) : this(userRepository)
        {
            if (connections == null)
                throw new ArgumentNullException(nameof(connections));
            this.connections = connections;
        }

        /// <summary>
        /// Create repository with specified repository and connections
        /// </summary>
        /// <param name="userRepository">Init repository for service</param>
        /// <param name="connections">Addresses which the master sends a messages</param>
        /// <param name="isLogged">It indicates whether the service log is</param>
        public MasterService(IRepository<User> userRepository, IEnumerable<ServiceConnection> connections, bool isLogged) : this(userRepository, connections)
        {
            this.isLogged = isLogged;
        }

        /// <summary>
        /// Add item in service
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public int Add(User item)
        {
            try
            {
                if (isLogged)
                    logger.Info("message");
                int result = 0;
                try
                {
                    slimLock.EnterWriteLock();
                    result = userRepository.Add(item);
                }
                finally
                {
                    slimLock.ExitWriteLock();
                }
                item.Id = result;
                OnAdded(new DataUpdatedEventArgs<User>() { data = item });
                return result;
            }
            catch (Exception ex)
            {
                throw new ServiceException("Error in the service", ex);
            }
        }

        /// <summary>
        /// Delete item from service
        /// </summary>
        /// <param name="item"></param>
        public void Delete(User item)
        {
            try
            {
                if (isLogged)
                    logger.Info("message");
                try
                {
                    slimLock.EnterWriteLock();
                    userRepository.Delete(item);
                }
                finally
                {
                    slimLock.ExitWriteLock();
                }
                OnDeleted(new DataUpdatedEventArgs<User>() { data = item });
            }
            catch (Exception ex)
            {
                throw new ServiceException("Error in the service", ex);
            }
        }

        /// <summary>
        /// Search items in repository
        /// </summary>
        /// <param name="searchCriteria"></param>
        /// <returns></returns>
        public IEnumerable<User> Search(ICriteria<User> searchCriteria)
        {
            try
            {
                if (isLogged)
                    logger.Info("message");
                try
                {
                    slimLock.EnterReadLock();
                    return userRepository.SearchAll(searchCriteria.CreateFunc());
                }
                finally
                {
                    slimLock.ExitReadLock();
                }
            }
            catch (Exception ex)
            {
                throw new ServiceException("Error in the service", ex);
            }
        }

        /// <summary>
        /// Save state of service
        /// </summary>
        public void Save()
        {
            try
            {
                if (isLogged)
                    logger.Info("message");
                try
                {
                    slimLock.EnterWriteLock();
                    var tempRep = userRepository as IStatefulRepository<User>;
                    tempRep?.Save();
                }
                finally
                {
                    slimLock.ExitWriteLock();
                }
            }
            catch (Exception ex)
            {
                throw new ServiceException("Error in the service", ex);
            }
        }

        protected void OnAdded(DataUpdatedEventArgs<User> arg)
        {
            SendMessage(new ServiceMessage() { Operation = Operation.Add, user = arg.data });
        }

        protected void OnDeleted(DataUpdatedEventArgs<User> arg)
        {
            SendMessage(new ServiceMessage() { Operation = Operation.Delete, user = arg.data });
        }

        /// <summary>
        /// Send message to clients of service
        /// </summary>
        /// <param name="msg"></param>
        private async void SendMessage(ServiceMessage msg)
        {
            foreach(var cn in connections)
            {
                try
                {
                    var client = new AsyncTcpClient(cn.Address, cn.Port);
                    var data = SerializeMessage(msg);
                    await client.SendMessage(data);
                }
                catch (Exception ex)
                {
                    throw new ServiceException("Error in the service", ex);
                }
            }
        }

        /// <summary>
        /// Serialize message to json
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        private byte[] SerializeMessage(ServiceMessage msg)
        {
            MemoryStream ms = new MemoryStream();
            var serializer = new JsonSerializer();
            serializer.SerializeObject(msg, ms);
            return ms.GetBuffer();
        }

    }
}
