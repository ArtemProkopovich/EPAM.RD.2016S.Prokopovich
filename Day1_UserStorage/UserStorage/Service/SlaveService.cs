using System;
using System.Collections.Generic;
using System.Threading;
using UserStorage.Interfacies;
using UserStorage.Entity;
using UserStorage.Extension;
using System.Net.Sockets;
using System.IO;
using System.Threading.Tasks;
using NLog;
using UserStorage.Net;
using UserStorage.Serialization;

namespace UserStorage.Service
{
    [Serializable]
    public class SlaveService : MarshalByRefObject, IService<User>
    {
        private readonly IRepository<User> userRepository;
        private readonly bool isLogged = true;
        private readonly ServiceConnection connection;
        private readonly ReaderWriterLockSlim slimLock = new ReaderWriterLockSlim();
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        public SlaveService(IRepository<User> userRepository)
        {
            if (userRepository == null)
                throw new ArgumentNullException(nameof(userRepository));
            this.userRepository = (IRepository<User>)userRepository.Clone();
            Listen();
        }

        public SlaveService(IRepository<User> userRepository, ServiceConnection connection) : this(userRepository)
        {
            if (connection == null)
                throw new ArgumentNullException(nameof(connection));
            this.connection = connection;
        }

        public SlaveService(IRepository<User> userRepository, ServiceConnection connection, bool isLogged) : this(userRepository, connection)
        {
            this.isLogged = isLogged;
        }

        public int Add(User item)
        {
            if (isLogged)
                logger.Info("message");
            throw new FeatureNotAvailiableException("Add method not availible for slave service.");
        }

        public void Delete(User item)
        {
            if (isLogged)
                logger.Info("message");
            throw new FeatureNotAvailiableException("Delete method not availible for slave service.");
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

        protected void Listen()
        {
            ThreadPool.QueueUserWorkItem(async (e) =>
            {
                try
                {
                    TcpListener listener = new TcpListener(connection.Address, connection.Port);
                    listener.Start();
                    while (true)
                    {
                        TcpClient tcpClient = null;
                        try
                        {
                            tcpClient = await listener.AcceptTcpClientAsync();
                            NetworkStream stream = tcpClient.GetStream();
                            var message = await ReadMessage(stream);
                            ProcessMessage(message);
                        }
                        finally
                        {
                            tcpClient?.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new ServiceException("Exception in tcp connection", ex);
                }
            });
        }

        private Task<ServiceMessage> ReadMessage(Stream stream)
        {
            var serializer = new JsonSerializer();
            return Task.FromResult(serializer.DeserializeObject(stream));
        }

        private void ProcessMessage(ServiceMessage message)
        {
            switch (message.Operation)
            {
                case Operation.Add:
                    OnAdded(null, new DataUpdatedEventArgs<User>() { data = message.user });
                    break;
                case Operation.Delete:
                    OnDeleted(null, new DataUpdatedEventArgs<User>() { data = message.user });
                    break;
            }
        }

        protected void OnAdded(object sender, DataUpdatedEventArgs<User> args)
        {
            try
            {
                if (isLogged)
                    logger.Info("message");
                try
                {
                    slimLock.EnterWriteLock();
                    userRepository.Add(args.data);
                }
                finally
                {
                    slimLock.ExitWriteLock();
                }
            }
            catch(Exception ex)
            {
                throw new ServiceException("Error in the service", ex);
            }
        }
        protected void OnDeleted(object sender, DataUpdatedEventArgs<User> args)
        {
            try
            {
                if (isLogged)
                    logger.Info("message");
                try
                {
                    slimLock.EnterWriteLock();
                    userRepository.Delete(args.data);
                }
                finally
                {
                    slimLock.ExitWriteLock();
                }
            }
            catch(Exception ex)
            {
                throw new ServiceException("Error in the service", ex);
            }
        }

        public void Save()
        {
            if (isLogged)
                logger.Info("message");
            throw new FeatureNotAvailiableException("Save method not availible for slave service.");
        }
    }
}
