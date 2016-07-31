using System.Configuration;

namespace UserStorageConfiguration.Configuration.NetworkConfiguration
{
    public class Connection : ConfigurationElement
    {
        [ConfigurationProperty("address", IsRequired = true)]
        public string Address
        {
            get
            {
                return this["address"] as string;
            }
        }
        [ConfigurationProperty("port", IsRequired = true)]
        public string Port
        {
            get
            {
                return this["port"] as string;
            }
        }
    }
}
