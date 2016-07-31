using System;
using System.Linq;
using System.Reflection;

namespace UserStorageConfiguration
{
    public class DomainAssemblyLoader : MarshalByRefObject
    {
        public object LoadFrom(string fileName, Type type, params object[] p)
        {
            var assembly = Assembly.LoadFrom(fileName);
            var types = assembly.GetTypes();
            var instanceType = types.FirstOrDefault(e => e.Name == type.Name);
            var instance = Activator.CreateInstance(instanceType, p);
            return instance;
        }
    }
}
