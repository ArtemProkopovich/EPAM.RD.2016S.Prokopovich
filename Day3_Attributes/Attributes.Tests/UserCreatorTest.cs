using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Attributes.Creator;
using System.ComponentModel.DataAnnotations;

namespace Attributes.Tests
{
    [TestClass]
    public class UserCreatorTest
    {
        [TestMethod]
        public void CreateUserFromClass__ReturnEqualUsers_Test()
        {
            var users = new User[] {
                new User(1) { FirstName = "Alexander", LastName = "Alexandrov" },
                new User(2) { FirstName = "Semen", LastName = "Semenov" },
                new User(3) { FirstName = "Petr", LastName = "Petrov" } };
            UserCreator cr = new UserCreator();
            var result = cr.CreateUserFromClass();
            CollectionAssert.AreEqual(result.ToArray(), users);
        }

        [TestMethod]
        public void CreateAdvancedUserFromAssembly_ReturnEqualUsers_Test()
        {
            var users = new AdvancedUser[] {
                new AdvancedUser(4, 2329423) { FirstName = "Pavel", LastName = "Pavlov" },
                new AdvancedUser(1,3443454) { FirstName = "Semen", LastName = "Semenov" },
            };
            UserCreator cr = new UserCreator();
            var result = cr.CreateAdvancedUserFromAssembly();
            CollectionAssert.AreEqual(result.ToArray(), users);
        }
    
        [TestMethod]
        public void ValidateUser_ReturnTrue_Test()
        {
            var user = new User(1) { FirstName = "name", LastName = "lname" };
            UserCreator cr = new UserCreator();
            List<ValidationResult> results = null;
            var result = cr.ValidateUser(user, out results);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ValidateUser_IdGreaterThanThousand_ReturnFalse_Test()
        {
            var user = new User(1001) { FirstName = "name", LastName = "lname" };
            UserCreator cr = new UserCreator();
            List<ValidationResult> results = null;
            var result = cr.ValidateUser(user, out results);
            Assert.IsFalse(result);
            Assert.IsNotNull(results);
        }

        [TestMethod]
        public void ValidateUser_FirstNameLengthGreaterThan20_ReturnFalse_Test()
        {
            var user = new User(1) { FirstName = "nameaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", LastName = "lname" };
            UserCreator cr = new UserCreator();
            List<ValidationResult> results = null;
            var result = cr.ValidateUser(user, out results);
            Assert.IsFalse(result);
            Assert.IsNotNull(results);
        }

    }
}
