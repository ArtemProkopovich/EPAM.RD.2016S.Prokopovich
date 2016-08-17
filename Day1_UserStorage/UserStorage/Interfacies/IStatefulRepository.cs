namespace UserStorage.Interfacies
{
    /// <summary>
    /// Interface of repository that state can be saved
    /// </summary>
    /// <typeparam name="T">Type of repository objects</typeparam>
    public interface IStatefulRepository<T>:IRepository<T>
    {
        void Save();
    }
}
