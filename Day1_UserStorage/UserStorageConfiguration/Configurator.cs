using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Reflection;
using UserStorageConfiguration.Configuration.FileConfiguration;
using UserStorageConfiguration.Configuration.ServiceConfiguration;
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
                    string filePath = GetFilePathFromConfig();
                    var masterConf = GetMasterServiceConfig();
                    var masterRep = GetMasterRepository(masterConf, filePath);
                    var master = new MasterService(masterRep);
                    var slaves = InitSlaves(master, masterRep);
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

        public ServiceKeeper InitializeWithDomens()
        {
            try
            {
                if (ValidateConfig())
                {
                    string filePath = GetFilePathFromConfig();
                    var masterConf = GetMasterServiceConfig();
                    var masterRep = GetMasterRepository(masterConf, filePath);
                    var master = InitMasterServiceInDomain(masterRep);
                    var slaves = InitSlaveServicesInDomains(master, masterRep);
                    return new ServiceKeeper(master, slaves);
                }
                else
                    throw new ConfigurationException();
            }
            catch (ConfigurationException ex)
            {
                throw ex;
            }
            //catch (Exception ex)
            //{
            //    throw new ConfigurationException();
            //}
        }

        private MasterService InitMasterServiceInDomain(IRepository<User> rep)
        {
            AppDomain domain = AppDomain.CreateDomain("MasterDomain");
            var loader = (DomainAssemblyLoader)domain.CreateInstanceAndUnwrap(Assembly.GetExecutingAssembly().FullName, typeof(DomainAssemblyLoader).FullName);
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"UserStorage.dll");
            var instance = loader.LoadFrom(path, typeof(MasterService), rep);
            return (MasterService)instance;
        }

        private IEnumerable<SlaveService> InitSlaveServicesInDomains(MasterService master, IRepository<User> rep)
        {
            int domainNumber = 1;
            var serviceConfig = ServiceConfig.GetConfig();
            List<SlaveService> result = new List<SlaveService>();
            foreach (var service in serviceConfig.Services)
            {
                var s = (Service)service;
                if (s.Type == "slave")
                {
                    int count = int.Parse(s.Count);
                    for (int i = 0; i < count; i++)
                    {
                        string domainName = "SlaveDomain" + domainNumber;
                        AppDomain domain = AppDomain.CreateDomain(domainName);
                        var loader = (DomainAssemblyLoader)domain.CreateInstanceAndUnwrap(Assembly.GetExecutingAssembly().FullName, typeof(DomainAssemblyLoader).FullName);
                        var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "UserStorage.dll");
                        var slave = (SlaveService)loader.LoadFrom(path, typeof(SlaveService), rep);
                        master.Added += slave.OnAdded;
                        master.Deleted += slave.OnDeleted;
                        result.Add(slave);
                        domainNumber++;
                    }
                }
            }
            return result;
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

        private IEnumerable<SlaveService> InitSlaves(MasterService master, IRepository<User> rep)
        {
            var serviceConfig = ServiceConfig.GetConfig();
            List<SlaveService> result = new List<SlaveService>();
            foreach (var service in serviceConfig.Services)
            {
                var s = (Service)service;
                if (s.Type == "slave")
                {
                    int count = int.Parse(s.Count);
                    for (int i = 0; i < count; i++)
                    {
                        var slave = new SlaveService((IRepository<User>)rep.Clone());
                        master.Added += slave.OnAdded;
                        master.Deleted += slave.OnDeleted;
                        result.Add(slave);
                    }
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

        private const string DefaultPath = "defaultFile.xml"; 
    }
}
