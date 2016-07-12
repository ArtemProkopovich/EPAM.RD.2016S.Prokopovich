using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserStorage.Entity;

namespace UserStorage.Interfacies
{
    public interface IRepository<T>
    {
        bool IsValid(T model);
        int Add(T item);
        T GetById(int id);
        IEnumerable<T> GetAll();
        IEnumerable<int> SearchAll(Func<T, bool> criteria);
        int SearchFirst(Func<T, bool> criteria);
        void Delete(User user);
    }
}
