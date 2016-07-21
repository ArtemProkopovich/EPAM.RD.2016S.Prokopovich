using System;
using System.Collections.Generic;
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
        IEnumerable<T> Search(params Func<T, bool>[] searchCriterias);
        void Delete(T item);
        void Save();
    }
}
