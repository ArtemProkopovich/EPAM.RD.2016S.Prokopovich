using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UserStorage.Entity;
using UserStorage.Extension;

namespace UserStorageTest
{
    [TestClass]
    public class UserCriteriaExtensionTest
    {
        [TestMethod]
        public void CreateFunc_ReturnCorrectFunction()
        {
            ///arrange
            User user = new User() { Id = 1, FirstName = "name" };
            UserCriteria criteria = new UserCriteria() { Id = 1, FirstName = "name" };
            Func<User, bool> function = (u) => { return u.Id == 1 && u.FirstName == "name"; };
            ///act
            var resultFunc = criteria.CreateFunc();
            ///assert
            Assert.AreEqual(resultFunc(user), function(user));
        }
    }
}
