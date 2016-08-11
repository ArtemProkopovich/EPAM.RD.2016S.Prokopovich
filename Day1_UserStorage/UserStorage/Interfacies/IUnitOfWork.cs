using UserStorage.Entity;

namespace UserStorage.Interfacies
{
    /// <summary>
    /// Interface of unit of work
    /// </summary>
    public interface IUnitOfWork
    {
        IRepository<User> userRepository { get; set; }
    }
}
