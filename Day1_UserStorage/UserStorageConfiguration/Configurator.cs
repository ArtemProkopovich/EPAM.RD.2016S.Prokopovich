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

        public ServiceProxy Initialize()
        {
            try
            {
                if (ValidateConfig())
                {
                    var masterRep = GetMasterRepository(GetMasterServiceConfig(), GetFilePathFromConfig());
                    var connections = GetConnectionsFromConfig();
                    var slaves = InitSlaveServices(masterRep, connections);
                    var master = InitMasterService(masterRep, connections.Take(slaves.Count()).ToList());                   
                    return new ServiceProxy(master, slaves);
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

        private MasterService InitMasterService(IRepository<User> repository, IEnumerable<ServiceConnection> connections)
        {
            return CreateService<MasterService>("MasterDomain", repository, connections);
        }

        private IEnumerable<SlaveService> InitSlaveServices(IRepository<User> repository, IEnumerable<ServiceConnection> connections)
        {
            int slaveCount = GetSlavesCount();
            var serviceConfig = ServiceConfig.GetConfig();
            var cns = connections.ToList();
            List<SlaveService> result = new List<SlaveService>();
            for (int i = 0; i < slaveCount; i++)
            {
                string domainName = "SlaveDomain" + i + 1;
                var slave = CreateService<SlaveService>(domainName, repository, cns[i]);
                result.Add(slave);
            }
            return result;
        }

        private T CreateService<T>(string domainName, params object[] p)
        {
            AppDomain domain = AppDomain.CreateDomain(domainName);
            var loader = (DomainAssemblyLoader)domain.CreateInstanceAndUnwrap(Assembly.GetExecutingAssembly().FullName, typeof(DomainAssemblyLoader).FullName);
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "UserStorage.dll");
            return (T)loader.LoadFrom(path, typeof(T), p);
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
            if (GetSlavesCount() > GetConnectionsFromConfig().Count())
                throw new ConfigurationException();
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

        private IEnumerable<ServiceConnection> GetConnectionsFromConfig()
        {
            var connConfig = ConnectionConfig.GetConfig();
            var result = new List<ServiceConnection>();
            foreach (var conn in connConfig.Connections)
            {
                var connection = conn as Connection;
                result.Add( new ServiceConnection() { Address = IPAddress.Parse(connection.Address), Port = int.Parse(connection.Port) });
            }
            return result;
        }

        private const string DefaultPath = "defaultFile.xml"; 
    }
}
