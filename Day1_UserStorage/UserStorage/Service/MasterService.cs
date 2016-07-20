using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Sockets; 
using UserStorage.Interfacies;
using UserStorage.Entity;
using System.Runtime.Serialization.Formatters.Binary;
using NLog;

namespace UserStorage.Service
{
    [Serializable]
    public class MasterService : MarshalByRefObject, IService<User>
    {
        private Logger logger = LogManager.GetCurrentClassLogger();
        private readonly IRepository<User> userRepository;
        private readonly bool isLogged = false;
        private ReaderWriterLockSlim slimLock = new ReaderWriterLockSlim();
        private readonly IEnumerable<ServiceConnection> connections;
        //public event EventHandler<DataUpdatedEventArgs<User>> Added;
        //public event EventHandler<DataUpdatedEventArgs<User>> Deleted;

        public MasterService(IRepository<User> userRepository)
        {
            if (userRepository == null)
                throw new ArgumentNullException(nameof(userRepository));
            this.userRepository = (IRepository<User>)userRepository.Clone();
            //Added += (o, arg) => { };
            //Deleted += (o, arg) => { };
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
                throw new ServiceException();
            }
        }

        public void Delete(int id)
        {
            try
            {
                if (isLogged)
                    logger.Info("message");
                User user = null;
                try
                {
                    slimLock.EnterWriteLock();
                    user = userRepository.GetById(id);
                    userRepository.Delete(user);
                }
                finally
                {
                    slimLock.ExitWriteLock();
                }
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
                try
                {
                    slimLock.EnterReadLock();
                    return userRepository.SearchAll(searchCriterias);
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
                throw new ServiceException();
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

        private void SendMessage(ServiceMessage msg)
        {
            foreach(var cn in connections)
            {
                TcpClient clinet = null;
                Stream stream = null;
                try
                {
                    TcpClient client = new TcpClient(cn.Address.ToString(), cn.Port);
                    var data = SerializeMessage(msg);
                    stream = client.GetStream();
                    stream.Write(data, 0, data.Length);
                }
                finally
                {
                    stream?.Close();
                    clinet?.Close();
                }
            }
        }

        private byte[] SerializeMessage(ServiceMessage msg)
        {
            BinaryFormatter fm = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            fm.Serialize(ms, msg);
            return ms.GetBuffer();
        }

    }
}
