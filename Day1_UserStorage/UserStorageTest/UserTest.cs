using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UserStorage.Entity;

namespace UserStorageTest
{
    [TestClass]
    public class UserTest
    {
        [TestMethod]
        public void Equals_WithReferenceEqualObjects_ReturnTrue()
        {
            ///arrange
            User user1 = new User() { Id = 3, FirstName = "name" };
            var user2 = user1;
            ///act
            var result = user1.Equals(user2);
            ///assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Equals_WithEqualFirstNameAndLastNameParams_ReturnTrue()
        {
            ///arrange
            User user1 = new User() { Id = 3, FirstName = "name", LastName = "lname" };
            User user2 = new User() { Id = 1, FirstName = "name", LastName = "lname" };
            ///act
            var result = user1.Equals(user2);
            ///assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Equals_WithNotEqualFirstNameAndLastNameParams_ReturnFalse()
        {
            ///arrange
            User user1 = new User() { Id = 3, FirstName = "name", LastName = "lname" };
            User user2 = new User() { Id = 1, FirstName = "name", LastName = "name" };
            ///act
            var result = user1.Equals(user2);
            ///assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void GetHashCode_WithEquaObjects_ReturnEqualValues()
        {
            ///arrange
            User user1 = new User() { Id = 3, FirstName = "name", LastName = "lname" };
            User user2 = new User() { Id = 1, FirstName = "name", LastName = "lname" };
            ///act
            int result1 = user1.GetHashCode();
            int result2 = user2.GetHashCode();
            ///assert
            Assert.AreEqual(result1, result2);
        }

        [TestMethod]
        public void GetHashCode_WithNotEquaObjects_ReturnNotEqualValues()
        {
            ///arrange
            User user1 = new User() { Id = 3, FirstName = "name", LastName = "lname" };
            User user2 = new User() { Id = 1, FirstName = "name", LastName = "name" };
            ///act
            int result1 = user1.GetHashCode();
            int result2 = user2.GetHashCode();
            ///assert
            Assert.AreNotEqual(result1, result2);
        }

        [TestMethod] 
        public void Clone_WithUserObject_ReturnEqualCloneObject()
        {
            ///arrange
            User user = new User() { Id = 3, FirstName = "name", LastName = "lname" };
            ///act
            var result = user.Clone();
            ///assert
            Assert.AreEqual(result, user);
        }

        [TestMethod]
        public void ToString_ReturnCorrectString()
        {
            ///arrange
            User user = new User() { Id = 3, FirstName = "name", LastName = "lname" };
            var expected = "User ID: 3 FirstName: name LastName: lname.";
            ///act
            var result = user.ToString();
            Assert.AreEqual(expected, result);
        }
    }
}
