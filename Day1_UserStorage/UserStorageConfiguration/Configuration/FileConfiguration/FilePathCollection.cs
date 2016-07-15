using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace UserStorageConfiguration.Configuration.FileConfiguration
{
    public class FilePathCollection : ConfigurationElementCollection
    {
        public FilePath this[int index]
        {
            get
            {
                return base.BaseGet(index) as FilePath;
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

        public new FilePath this[string responseString]
        {
            get { return (FilePath)BaseGet(responseString); }
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
            return new FilePath();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((FilePath)element).Path;
        }
    }
}
