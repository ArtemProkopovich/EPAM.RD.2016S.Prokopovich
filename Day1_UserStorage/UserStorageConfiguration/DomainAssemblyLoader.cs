using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UserStorage.Interfacies;
using UserStorage.Entity;

namespace UserStorageConfiguration
{
    public class DomainAssemblyLoader : MarshalByRefObject
    {
        public object LoadFrom(string fileName, Type type, IRepository<User> rep)
        {
            var assembly = Assembly.LoadFrom(fileName);
            var types = assembly.GetTypes();
            var instanceType = types.FirstOrDefault(e => e.Name == type.Name);
            var instance = Activator.CreateInstance(instanceType, new object[] { rep });
            return instance;
        }
    }
}
