﻿using System;
using UserStorage.Entity;
using UserStorage.Interfacies;

namespace UserStorage.Extension
{
    public static class UserCriteriaExtension
    {
        /// <summary>
        /// Create delegate from criteria
        /// </summary>
        /// <param name="criteria">Criteria to transform</param>
        /// <returns></returns>
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
