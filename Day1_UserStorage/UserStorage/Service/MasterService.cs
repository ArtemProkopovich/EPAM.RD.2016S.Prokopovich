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
    [Serializable]
    public class MasterService : MarshalByRefObject, IService<User>
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly IRepository<User> userRepository;
        private readonly bool isLogged = true;
        private readonly ReaderWriterLockSlim slimLock = new ReaderWriterLockSlim();
        private readonly IEnumerable<ServiceConnection> connections;

        public MasterService(IRepository<User> userRepository)
        {
            if (userRepository == null)
                throw new ArgumentNullException(nameof(userRepository));
            this.userRepository = (IRepository<User>)userRepository.Clone();
        }

        public MasterService(IRepository<User> userRepository, IEnumerable<ServiceConnection> connections) : this(userRepository)
        {
            if (connections == null)
                throw new ArgumentNullException(nameof(connections));
            this.connections = connections;
        }

        public MasterService(IRepository<User> userRepository, IEnumerable<ServiceConnection> connections, bool isLogged) : this(userRepository, connections)
        {
            this.isLogged = isLogged;
        }

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
                OnAdded(new DataUpdatedEventArgs<User>() { data = userRepository.GetById(result) });
                return result;
            }
            catch (Exception ex)
            {
                throw new ServiceException("Error in the service", ex);
            }
        }

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

        public void Save()
        {
            try
            {
                if (isLogged)
                    logger.Info("message");
                try
                {
                    slimLock.EnterWriteLock();
                    userRepository.Save();
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

        private byte[] SerializeMessage(ServiceMessage msg)
        {
            MemoryStream ms = new MemoryStream();
            var serializer = new JsonSerializer();
            serializer.SerializeObject(msg, ms);
            return ms.GetBuffer();
        }

    }
}
