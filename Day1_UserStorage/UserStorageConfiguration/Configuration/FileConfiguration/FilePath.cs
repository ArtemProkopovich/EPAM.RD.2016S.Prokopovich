﻿using System.Configuration;

namespace UserStorageConfiguration.Configuration.FileConfiguration
{
    public class FilePath : ConfigurationElement
    {
        [ConfigurationProperty("path", IsRequired = true)]
        public string Path
        {
            get
            {
                return this["path"] as string;
            }
        }
        [ConfigurationProperty("extension", IsRequired = false)]
        public string Extension
        {
            get
            {
                return this["extension"] as string;
            }
        }
    }
}
