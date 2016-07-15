using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UserStorage.Repository;
using UserStorage.Entity;
using System.IO;

namespace UserStorageTest
{
    [TestClass]
    public class XmlRepositoryTest
    {
        [TestMethod]
        public void SaveToXml_RepositoryWithThreeItems_FileIsExists_Test()
        {
            if (File.Exists("test.xml")) File.Delete("test.xml");
            XmlRepository rep = new XmlRepository("test.xml");
            var users = new List<User>() {
                new User() { FirstName = "name1" },
                new User() { FirstName = "name2" },
                new User() { FirstName = "name3" }
            };
            users.ForEach(e => rep.Add(e));
            rep.SaveToXml();
            Assert.IsTrue(File.Exists("test.xml"));
        }

        [TestMethod]
        public void LoadToXml_FileWithThreeItems_RepositoryWithThreeItems_Test()
        {
            XmlRepository rep = new XmlRepository("test.xml");
            Assert.AreEqual(3, rep.GetAll().Count());
        }
    }
}
