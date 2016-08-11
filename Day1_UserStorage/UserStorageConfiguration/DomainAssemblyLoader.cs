using System;
using System.Linq;
using System.Reflection;

namespace UserStorageConfiguration
{
    public class DomainAssemblyLoader : MarshalByRefObject
    {
        /// <summary>
        /// Create instance of object of defined type from defined assembly
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="type"></param>
        /// <param name="p"></param>
        /// <returns></returns>
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
