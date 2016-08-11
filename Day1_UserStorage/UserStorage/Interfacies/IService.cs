using System.Collections.Generic;


namespace UserStorage.Interfacies
{
    /// <summary>
    /// Interface of service
    /// </summary>
    /// <typeparam name="T">Type of objects provided by service</typeparam>
    public interface IService<T>
    {
        int Add(T item);
        IEnumerable<T> Search(ICriteria<T> searchCriteria);
        void Delete(T item);
        void Save();
    }
}
