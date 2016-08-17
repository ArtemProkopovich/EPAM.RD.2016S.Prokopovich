using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UserStorage.Service;
using UserStorage.Repository;

namespace UserStorageTest
{
    [TestClass]
    public class SlaveServiceTest
    {
        [TestMethod]
        [ExpectedException(typeof(FeatureNotAvailiableException))]
        public void Add_ThrowException()
        {
            ///arrange
            SlaveService srv = new SlaveService(new MemoryRepository());
            ///act
            srv.Add(null);
            ///assert
        }

        [TestMethod]
        [ExpectedException(typeof(FeatureNotAvailiableException))]
        public void Delete_ThrowException()
        {
            ///arrange
            SlaveService srv = new SlaveService(new MemoryRepository());
            ///act
            srv.Delete(null);
            ///assert
        }

        [TestMethod]
        [ExpectedException(typeof(FeatureNotAvailiableException))]
        public void Save_ThrowException()
        {
            ///arrange
            SlaveService srv = new SlaveService(new MemoryRepository());
            ///act
            srv.Save();
            ///assert
        }
    }
}
