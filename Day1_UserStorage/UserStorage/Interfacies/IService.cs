using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserStorage.Entity;


namespace UserStorage.Interfacies
{
    public interface IService<T>
    {
        int Add(T item);
        IEnumerable<T> Search(ICriteria<T> searchCriteria);
        void Delete(T item);
        void Save();
    }
}
