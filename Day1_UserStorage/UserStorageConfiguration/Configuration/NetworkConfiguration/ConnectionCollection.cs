using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace UserStorageConfiguration.Configuration.NetworkConfiguration
{
    [ConfigurationCollection(typeof(Connection))]
    public class ConnectionCollection : ConfigurationElementCollection
    {
        public Connection this[int index]
        {
            get
            {
                return base.BaseGet(index) as Connection;
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

        public new Connection this[string responseString]
        {
            get { return (Connection)BaseGet(responseString); }
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
            return new Connection();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((Connection)element).Port;
        }
    }
}
