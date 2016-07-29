using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UserStorage.Interfacies;
using UserStorage.Entity;
using UserStorage.Extension;
using UserStorage;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Net.Sockets;
using System.IO;
using NLog;
using UserStorage.Serialization;

namespace UserStorage.Service
{
    [Serializable]
    public class SlaveService : MarshalByRefObject, IService<User>
    {
        private readonly IRepository<User> userRepository;
        private readonly bool isLogged = true;
        private readonly ServiceConnection connection;
        private ReaderWriterLockSlim slimLock = new ReaderWriterLockSlim();
        private Logger logger = LogManager.GetCurrentClassLogger();
        public Guid ServiceId { get; set; } = Guid.NewGuid();

        public SlaveService(IRepository<User> userRepository)
        {
            if (userRepository == null)
                throw new ArgumentNullException(nameof(userRepository));
            this.userRepository = (IRepository<User>)userRepository.Clone();
        }

        public SlaveService(IRepository<User> userRepository, ServiceConnection connection) : this(userRepository)
        {
            if (connection == null)
                throw new ArgumentNullException(nameof(connection));
            this.connection = connection;
            Listen();
        }

        public SlaveService(IRepository<User> userRepository, ServiceConnection connection, bool isLogged) : this(userRepository, connection)
        {
            this.isLogged = isLogged;
        }

        public int Add(User item)
        {
            if (isLogged)
                logger.Info("message");
            throw new FeatureNotAvailiableException();
        }

        public void Delete(User item)
        {
            if (isLogged)
                logger.Info("message");
            throw new FeatureNotAvailiableException();
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
                throw new ServiceException();
            }
        }

        protected void Listen()
        {
            Thread thread = new Thread(new ThreadStart(() =>
            {
                TcpListener listener = null;
                try
                {
                    listener = new TcpListener(connection.Address, connection.Port);
                    listener.Start();
                    while(true)
                    {                      
                        TcpClient client = listener.AcceptTcpClient();
                        NetworkStream stream = client.GetStream();
                        var message = DeserializeMessage(stream);
                        ProcessMessage(message);
                    }
                }
                finally
                {
                    listener?.Stop();
                }
            }));
            thread.IsBackground = true;
            thread.Start();
        }

        private ServiceMessage DeserializeMessage(Stream stream)
        {
            var serializer = new JsonSerializer();
            return serializer.DeserializeObject(stream);
            //var formatter = new BinaryFormatter();
            //return formatter.Deserialize(stream) as ServiceMessage;
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
                throw new ServiceException();
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
                throw new ServiceException();
            }
        }

        public void Save()
        {
            if (isLogged)
                logger.Info("message");
            throw new FeatureNotAvailiableException();
        }
    }
}
