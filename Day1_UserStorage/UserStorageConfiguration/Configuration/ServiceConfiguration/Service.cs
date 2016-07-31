using System.Configuration;

namespace UserStorageConfiguration.Configuration.ServiceConfiguration
{
    public class Service : ConfigurationElement
    {
        [ConfigurationProperty("type", IsRequired = true)]
        public string Type
        {
            get
            {
                return this["type"] as string;
            }
        }
        [ConfigurationProperty("storage", IsRequired = true)]
        public string Storage
        {
            get
            {
                return this["storage"] as string;
            }
        }
        [ConfigurationProperty("count", IsRequired = true)]
        public string Count
        {
            get
            {
                return this["count"] as string;
            }
        }
    }
}
