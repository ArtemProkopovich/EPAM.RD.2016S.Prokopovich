using UserStorage.Entity;

namespace UserStorage.Interfacies
{
    public interface IUnitOfWork
    {
        IRepository<User> userRepository { get; set; }
    }
}
