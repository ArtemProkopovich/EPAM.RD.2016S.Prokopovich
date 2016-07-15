using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UserStorageConfiguration;

namespace UserStorageTest
{
    [TestClass]
    public class ConfiguratorTest
    {
        [TestMethod]
        public void Initialize_InConfigFileOneMasterFiveSlaves_ReturnServiceKeeperWithMasterAndFiveSlaves_Test()
        {
            Configurator conf = new Configurator();
            ServiceKeeper srv = conf.Initialize();
            Assert.IsNotNull(srv.Master);
            Assert.AreEqual(srv.Slaves.Count(), 5);
        }

        [TestMethod]
        [ExpectedException(typeof(ConfigurationException))]
        public void Initialize_InConfigFileNonMaster_ThrowConfigurationException_Test()
        {
            Configurator conf = new Configurator();
            ServiceKeeper srv = conf.Initialize();
        }

        [TestMethod]
        [ExpectedException(typeof(ConfigurationException))]
        public void Initialize_InConfigFileMasterCountEqualTwo_ThrowConfigurationException_Test()
        {
            Configurator conf = new Configurator();
            ServiceKeeper srv = conf.Initialize();
        }
    }
}
