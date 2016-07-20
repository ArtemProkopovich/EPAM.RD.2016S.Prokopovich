﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace UserStorageConfiguration.Configuration.NetworkConfiguration
{
    public class ConnectionConfig : ConfigurationSection
    {
        [ConfigurationProperty("ConnectionCollection", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(Connection),
            AddItemName = "Connection")]
        public ConnectionCollection Connections
        {
            get
            {
                return (ConnectionCollection)this["ConnectionCollection"];
            }
        }

        public static ConnectionConfig GetConfig()
        {
            return (ConnectionConfig)ConfigurationManager.GetSection("Connections") ?? new ConnectionConfig();
        }
    }
}
