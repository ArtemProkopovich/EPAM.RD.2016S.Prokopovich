using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserStorage.Interfacies;
using UserStorage.Entity;

namespace UserStorage.Interfacies
{
    public interface IUnitOfWork
    {
        IRepository<User> userRepository { get; set; }
    }
}
