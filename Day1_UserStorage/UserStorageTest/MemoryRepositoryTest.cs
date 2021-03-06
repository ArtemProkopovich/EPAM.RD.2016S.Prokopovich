﻿using System;
using System.Text;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UserStorage.Interfacies;
using UserStorage.Entity;
using UserStorage.Repository;

namespace UserStorageTest
{
    /// <summary>
    /// Summary description for RepositoryTest
    /// </summary>
    [TestClass]
    public class MemoryRepositoryTest
    {

        [TestMethod]
        public void Add_ValidationWithNotNullFirstNameAndLastName_UserWithCorrectParams_ReturnUserIdGreaterZero()
        {
            //arrange
            Func<User, bool> validateFunc = e => e.FirstName != null && e.LastName != null;
            User user = new User() { FirstName = "name", LastName = "name" };
            MemoryRepository rep = new MemoryRepository(null, validateFunc);
            //act
            var result = rep.Add(user);
            //assert
            Assert.IsTrue(result > 0);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidArgumentException))]
        public void Add_ValidationWithNotNullFirstNameAndLastName_UserWithUncorrectParams_ThrowInvalidArgumentException()
        {
            //arrange
            Func<User, bool> validateFunc = e => e.FirstName != null && e.LastName != null;
            User user = new User() { FirstName = null, LastName = "name" };
            MemoryRepository rep = new MemoryRepository(null, validateFunc);
            //act
            var result = rep.Add(user);
            //assert
            Assert.IsTrue(result > 0);
        }

        [TestMethod]
        public void Add_DefaultIdGenerateMethod_AddThreeElementsInStorage_ReturnIndexesFromOneToThree()
        {
            //arrange
            User[] users = new User[] {new User() { FirstName = null, LastName = "name1" }
            ,new User() { FirstName = null, LastName = "name2" }
            ,new User() { FirstName = null, LastName = "name3" }};
            MemoryRepository rep = new MemoryRepository();
            //act
            List<int> results = new List<int>();
            foreach(var u in users)
            {
                results.Add(rep.Add(u));
            }
            //assert
            int[] expected = new int[] { 1, 2, 3 };
            CollectionAssert.AreEqual(expected, results);
        }

        [TestMethod]
        [ExpectedException(typeof(EndedIdValuesException))]
        public void Add_CustomIdGenerateMethodForThreeIDs_AddFourElementsInStorage_ThrowEndedIdValuesException()
        {
            //arrange
            User[] users = new User[] {new User() { FirstName = null, LastName = "name1" }
            ,new User() { FirstName = null, LastName = "name2" }
            ,new User() { FirstName = null, LastName = "name3" }
            ,new User() { FirstName = null, LastName = "name4" }};
            MemoryRepository rep = new MemoryRepository(new int[] { 1, 2, 3 });
            //act
            List<int> results = new List<int>();
            foreach (var u in users)
            {
                results.Add(rep.Add(u));
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
            //act
            foreach (var u in users)
            {
                rep.Add(u);
            }
            var result = rep.SearchAll(e => e.LastName == "name");
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
            //act
            foreach (var u in users)
            {
                rep.Add(u);
            }
            var result = rep.SearchAll(e => e.LastName == "name3");
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
            //act
            var result = rep.Add(user);
            var repUser = rep.SearchAll(e => e.Id == result).FirstOrDefault();
            rep.Delete(repUser);
            var resultUser = rep.SearchAll(e => e.Id == result).FirstOrDefault();
            //assert
            Assert.IsNull(resultUser);    
        }
    }
}
