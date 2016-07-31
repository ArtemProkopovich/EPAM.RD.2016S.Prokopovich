using System.Collections.Generic;


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
