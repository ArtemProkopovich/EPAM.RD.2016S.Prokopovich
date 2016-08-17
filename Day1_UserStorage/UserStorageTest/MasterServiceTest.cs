using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UserStorage.Entity;
using UserStorage.Service;
using UserStorage.Repository;
using System.Linq;
using System.Collections.Generic;
using System.IO;

namespace UserStorageTest
{
    [TestClass]
    public class MasterServiceTest
    {
        [TestMethod]
        public void Add_ValidationWithNotNullFirstNameAndLastName_UserWithCorrectParams_ReturnUserIdGreaterZero()
        {
            //arrange
            Func<User, bool> validateFunc = e => e.FirstName != null && e.LastName != null;
            User user = new User() { FirstName = "name", LastName = "name" };
            MemoryRepository rep = new MemoryRepository(null, validateFunc);
            MasterService srv = new MasterService(rep);
            //act
            var result = srv.Add(user);
            //assert
            Assert.IsTrue(result > 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ServiceException))]
        public void Add_ValidationWithNotNullFirstNameAndLastName_UserWithUncorrectParams_ThrowInvalidArgumentException()
        {
            //arrange
            Func<User, bool> validateFunc = e => e.FirstName != null && e.LastName != null;
            User user = new User() { FirstName = null, LastName = "name" };
            MemoryRepository rep = new MemoryRepository(null, validateFunc);
            MasterService srv = new MasterService(rep);
            //act
            var result = srv.Add(user);
            //assert
        }

        [TestMethod]
        public void Add_DefaultIdGenerateMethod_AddThreeElementsInStorage_ReturnIndexesFromOneToThree()
        {
            //arrange
            User[] users = new User[] {new User() { FirstName = null, LastName = "name1" }
            ,new User() { FirstName = null, LastName = "name2" }
            ,new User() { FirstName = null, LastName = "name3" }};
            MemoryRepository rep = new MemoryRepository();
            MasterService srv = new MasterService(rep);
            //act
            List<int> results = new List<int>();
            foreach (var u in users)
            {
                results.Add(srv.Add(u));
            }
            //assert
            int[] expected = new int[] { 1, 2, 3 };
            CollectionAssert.AreEqual(expected, results);
        }

        [TestMethod]
        [ExpectedException(typeof(ServiceException))]
        public void Add_CustomIdGenerateMethodForThreeIDs_AddFourElementsInStorage_ThrowEndedIdValuesException()
        {
            //arrange
            User[] users = new User[] {new User() { FirstName = null, LastName = "name1" }
            ,new User() { FirstName = null, LastName = "name2" }
            ,new User() { FirstName = null, LastName = "name3" }
            ,new User() { FirstName = null, LastName = "name4" }};
            MemoryRepository rep = new MemoryRepository(new int[] { 1, 2, 3 });
            MasterService srv = new MasterService(rep);
            //act
            List<int> results = new List<int>();
            foreach (var u in users)
            {
                results.Add(srv.Add(u));
            }
            //assert
        }

        [TestMethod]
        public void SearchAll_AddTwoElementsWithLastNameEqualName_SearchThisElements_ReturnIEnumerableWithTwoElements()
        {
            //arrange
            List<User> users = new List<User> {new User() {Id=1, FirstName = null, LastName = "name1" }
            ,new User() {Id=2, FirstName = null, LastName = "name" }
            ,new User() {Id=3, FirstName = null, LastName = "name" }};
            MemoryRepository rep = new MemoryRepository();
            MasterService srv = new MasterService(rep);
            //act
            foreach (var u in users)
            {
                srv.Add(u);
            }
            var result = srv.Search(new UserCriteria() { LastName = "name" });
            //assert
            users.RemoveAt(0);
            CollectionAssert.AreEqual(result.ToArray(), users.ToArray());
        }

        [TestMethod]
        public void SearchAll_AddTwoElements_SearchElementsNotExistsInStorage_ReturnIenumerableWithZeroElements()
        {
            //arrange
            List<User> users = new List<User> {new User() {Id=1, FirstName = null, LastName = "name1" }
            ,new User() {Id=2, FirstName = null, LastName = "name" }
            ,new User() {Id=3, FirstName = null, LastName = "name" }};
            MemoryRepository rep = new MemoryRepository();
            MasterService srv = new MasterService(rep);
            //act
            foreach (var u in users)
            {
                srv.Add(u);
            }
            var result = srv.Search(new UserCriteria() { LastName = "name3" });
            //assert
            users.RemoveAt(0);
            Assert.AreEqual(result.Count(), 0);
        }


        [TestMethod]
        public void Delete_AddElementInStorage_DeleteElementFromStorage_GetElementById_ReturnNull()
        {
            //arrange
            User user = new User() { FirstName = "name", LastName = "name" };
            MemoryRepository rep = new MemoryRepository();
            MasterService srv = new MasterService(rep);
            //act
            var result = srv.Add(user);
            var srvUser = srv.Search(new UserCriteria() { Id = result }).FirstOrDefault();
            srv.Delete(srvUser);
            var resultUser = srv.Search(new UserCriteria() { Id = result }).FirstOrDefault();
            //assert
            Assert.IsNull(resultUser);
        }

        [TestMethod]
        public void Save_AssertIfFileExists()
        {
            ///arrange
            if (File.Exists("test.xml")) File.Delete("test.xml");
            XmlRepository rep = new XmlRepository("test.xml");
            MasterService srv = new MasterService(rep);
            var users = new List<User>() {
                new User() { FirstName = "name1" },
                new User() { FirstName = "name2" },
                new User() { FirstName = "name3" }
            };
            ///act
            users.ForEach(e => srv.Add(e));
            srv.Save();
            ///assert
            Assert.IsTrue(File.Exists("test.xml"));
        }
    }
}
