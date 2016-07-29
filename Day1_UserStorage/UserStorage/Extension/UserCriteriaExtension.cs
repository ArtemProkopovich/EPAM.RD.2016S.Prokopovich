using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using UserStorage.Entity;
using UserStorage.Interfacies;

namespace UserStorage.Extension
{
    public static class UserCriteriaExtension
    {
        public static Func<User, bool>CreateFunc(this ICriteria<User> criteria)
        {
            if (criteria == null)
            {
                return (e) => { return true; };
            }
            var user = criteria as UserCriteria;
            if (user == null)
                throw new ArgumentException(nameof(criteria));
            Func<User, bool> result = (e) =>
            {
                bool r = true;
                r &= user.Id == null ? r : user.Id == e.Id;
                r &= user.FirstName == null ? r : user.FirstName == e.FirstName;
                r &= user.LastName == null ? r : user.LastName == e.LastName;
                r &= user.PersonalId == null ? r : user.PersonalId == e.PersonalId;
                return r;
            };
            return result;
                
        }
    }
}
