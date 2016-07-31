using System.Configuration;

namespace UserStorageConfiguration.Configuration.FileConfiguration
{
    public class FilePathConfig: ConfigurationSection
    {
        public static FilePathConfig GetConfig()
        {
            return (FilePathConfig)ConfigurationManager.GetSection("FilePaths") ?? new FilePathConfig();
        }

        [ConfigurationProperty("FilePathCollection")]
        [ConfigurationCollection(typeof(FilePath), AddItemName = "FilePath")]
        public FilePathCollection FilePaths
        {
            get
            {
                object o = this["FilePathCollection"];
                return o as FilePathCollection;
            }
        }
    }
}
