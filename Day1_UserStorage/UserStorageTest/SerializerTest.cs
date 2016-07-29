using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UserStorage.Entity;
using UserStorage.Serialization;
using System.IO;
using UserStorage.Service;

namespace UserStorageTest
{
    [TestClass]
    public class SerializerTest
    {
        [TestMethod]
        public void SerializeTest()
        {
            User u = new User()
            {
                Id = 1,
                FirstName = "dfg",
                LastName = "fgfdg",
                Gender = Gender.Male,
                BirthDate = DateTime.Now,
                PersonalId = "dfg234",
                Visas = new Visa[]
            {
                new Visa() {StartTime = DateTime.Now, EndTime = DateTime.Now, Country = "dqweqweqwe" },
                new Visa() {StartTime = DateTime.Now, EndTime = DateTime.Now, Country = "zxcxczxc" }
            }
            };
            JsonSerializer js = new JsonSerializer();
            js.SerializeObject(new ServiceMessage() { Operation = Operation.Add, user = u }, new MemoryStream());
        }

        [TestMethod]
        public void DeserializeTest()
        {
            User u = new User()
            {
                Id = 1,
                FirstName = "dfg",
                LastName = "fgfdg",
                Gender = Gender.Male,
                BirthDate = DateTime.Now,
                PersonalId = "dfg234"
            };
            JsonSerializer js = new JsonSerializer();
            MemoryStream ms = new MemoryStream();
            js.SerializeObject(new ServiceMessage() { Operation = Operation.Add, user = u }, ms);
            ServiceMessage result = js.DeserializeObject(ms);
            Assert.AreEqual(u, result.user);
        }
    }
}
