using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Diagnostics;
using System.Reflection;
using UserStorageConfiguration.Configuration.FileConfiguration;
using UserStorageConfiguration.Configuration.ServiceConfiguration;
using UserStorageConfiguration.Configuration.NetworkConfiguration;
using UserStorage.Service;
using UserStorage.Repository;
using UserStorage.Interfacies;
using UserStorage.Entity;

namespace UserStorageConfiguration
{
    public class Configurator
    {
        private List<string> storageTypes = new List<string>() { "memory", "xml" };
        private List<string> Types = new List<string>() { "master", "slave" };
        private BooleanSwitch sw = new BooleanSwitch("logSwitch", "description", "1");

        public ServiceKeeper Initialize()
        {
            try
            {
                if (ValidateConfig())
                {
                    var masterRep = GetMasterRepository(GetMasterServiceConfig(), GetFilePathFromConfig());
                    var master = InitMasterService(masterRep);
                    var slaves = InitSlaveServices(master, masterRep);
                    return new ServiceKeeper(master, slaves);
                }
                else
                    throw new ConfigurationException();
            }
            catch (ConfigurationException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new ConfigurationException();
            }
        }

        private MasterService InitMasterService(IRepository<User> repository)
        {
            return CreateService<MasterService>("MasterDomain", repository);
        }

        private IEnumerable<SlaveService> InitSlaveServices(MasterService master, IRepository<User> repository)
        {
            int slaveCount = GetSlavesCount();
            var serviceConfig = ServiceConfig.GetConfig();
            List<SlaveService> result = new List<SlaveService>();
            for (int i = 0; i < slaveCount; i++)
            {
                string domainName = "SlaveDomain" + i + 1;
                var slave = CreateService<SlaveService>(domainName, repository);
                //master.Added += slave.OnAdded;
                //master.Deleted += slave.OnDeleted;
                result.Add(slave);
            }
            return result;
        }

        private T CreateService<T>(string domainName, IRepository<User> repository)
        {
            AppDomain domain = AppDomain.CreateDomain(domainName);
            var loader = (DomainAssemblyLoader)domain.CreateInstanceAndUnwrap(Assembly.GetExecutingAssembly().FullName, typeof(DomainAssemblyLoader).FullName);
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "UserStorage.dll");
            return (T)loader.LoadFrom(path, typeof(T), repository);
        }

        private bool ValidateConfig()
        {
            var serviceConfig = ServiceConfig.GetConfig();
            int masterCount = 0;
            bool masterFound = false;
            foreach(var service in serviceConfig.Services)
            {
                if (service is Service)
                {
                    var s = (Service)service;
                    if (!storageTypes.Any(e => e == s.Storage.ToLower()))
                        throw new ConfigurationException();
                    if (!Types.Any(e => e == s.Type.ToLower()))
                        throw new ConfigurationException();
                    if (s.Type.ToLower() == "slave" && s.Storage.ToLower() != "memory")
                        throw new ConfigurationException();
                    int count = 0;
                    if (s.Type.ToLower() == "master")
                    {
                        if (masterFound)
                            throw new ConfigurationException();
                        else
                            masterFound = true;
                    }
                    if (int.TryParse(s.Count, out count))
                    {
                        if (count < 0)
                            throw new ConfigurationException();
                        if (s.Type.ToLower() == "master")
                            masterCount += count;
                    }
                    else
                        throw new ConfigurationException();
                }
                else
                {
                    throw new ConfigurationException();
                }
            }
            if (masterCount != 1 || !masterFound)
                throw new ConfigurationException();
            return true;
        }

        private int GetSlavesCount()
        {
            int result = 0;
            var serviceConfig = ServiceConfig.GetConfig();
            foreach (var service in serviceConfig.Services)
            {
                var s = (Service)service;
                if (s.Type == "slave")
                {
                    int count = int.Parse(s.Count);
                    result += count;
                }
            }
            return result;
        }
            
        private Service GetMasterServiceConfig()
        {
            var serviceConfig = ServiceConfig.GetConfig();
            Service master;
            foreach(var service in serviceConfig.Services)
            {
                if (((Service)service).Type == "master")
                {
                    master = (Service)service;
                    return master;
                }
            }
            return null;
        }

        private IRepository<User> GetMasterRepository(Service master, string filePath)
        {
            IRepository<User> result;
            if (master.Storage.ToLower() == "xml")
            {
                result = new XmlRepository(filePath);
            }
            else
            {
                result = new MemoryRepository();
            }
            return result;
        }

        private string GetFilePathFromConfig()
        {
            var pathConfig = FilePathConfig.GetConfig();
            foreach(var path in pathConfig.FilePaths)
            {
                return (path as FilePath)?.Path ?? DefaultPath;
            }
            return DefaultPath;
        }

        private ServiceConnection GetConnectionFromConfig()
        {
            var connConfig = ConnectionConfig.GetConfig();
            foreach (var conn in connConfig.Connections)
            {
                var connection = conn as Connection;
                return new ServiceConnection() { Address = IPAddress.Parse(connection.Address), Port = int.Parse(connection.Port) };
            }
            return DefaultConnection;
        }

        private readonly ServiceConnection DefaultConnection = new ServiceConnection() { Address = IPAddress.Parse("127.0.0.1"), Port = 10000 };
        private const string DefaultPath = "defaultFile.xml"; 
    }
}
