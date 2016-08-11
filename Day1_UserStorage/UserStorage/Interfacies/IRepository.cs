using System;
using System.Collections.Generic;
using UserStorage.Entity;

namespace UserStorage.Interfacies
{
    /// <summary>
    /// Interface of repository
    /// </summary>
    /// <typeparam name="T">Type of repository objects</typeparam>
    public interface IRepository<T> : ICloneable
    {
        bool IsValid(T model);
        int Add(T item);
        IEnumerable<T> SearchAll(Func<User, bool> searchCriteria);
        void Delete(T user);
        void Save();
    }
}
