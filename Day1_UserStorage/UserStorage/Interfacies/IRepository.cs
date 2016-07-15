using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserStorage.Entity;

namespace UserStorage.Interfacies
{
    public interface IRepository<T> : ICloneable
    {
        bool IsValid(T model);
        int Add(T item);
        T GetById(int id);
        IEnumerable<T> GetAll();
        IEnumerable<T> SearchAll(Func<T, bool> criteria);
        IEnumerable<T> SearchAll(params Func<T, bool>[] criterias);
        T SearchFirst(Func<T, bool> criteria);
        void Delete(T user);
        void Save();
    }
}
