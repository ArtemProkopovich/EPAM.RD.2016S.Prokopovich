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
        Guid ServiceId { get; set; }
        int Add(T item);
        IEnumerable<T> Search(Func<T, bool> searchCriteria);
        void Delete(T item);
        void Save();
    }
}
