using System.Configuration;

namespace UserStorageConfiguration.Configuration.ServiceConfiguration
{
    [ConfigurationCollection( typeof( Service ) )]
    public class ServiceCollection : ConfigurationElementCollection
    {
        public Service this[int index]
        {
            get
            {
                return base.BaseGet(index) as Service;
            }
            set
            {
                if (base.BaseGet(index) != null)
                {
                    base.BaseRemoveAt(index);
                }
                this.BaseAdd(index, value);
            }
        }

        public new Service this[string responseString]
        {
            get { return (Service)BaseGet(responseString); }
            set
            {
                if (BaseGet(responseString) != null)
                {
                    BaseRemoveAt(BaseIndexOf(BaseGet(responseString)));
                }
                BaseAdd(value);
            }
        }


        protected override ConfigurationElement CreateNewElement()
        {
            return new Service();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((Service)element).Type;
        }
    }
}
