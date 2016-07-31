using System.Configuration;

namespace UserStorageConfiguration.Configuration.ServiceConfiguration
{
    public class ServiceConfig : ConfigurationSection
    {
        [ConfigurationProperty("ServiceCollection", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(Service),
            AddItemName = "Service")]
        public ServiceCollection Services
        {
            get
            {
                return (ServiceCollection)this["ServiceCollection"];
            }
        }

        public static ServiceConfig GetConfig()
        {
            return (ServiceConfig)ConfigurationManager.GetSection("Services") ?? new ServiceConfig();
        }
    }
}
