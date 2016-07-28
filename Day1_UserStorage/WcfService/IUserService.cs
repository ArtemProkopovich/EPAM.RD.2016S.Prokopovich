using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.ServiceModel;
using UserStorage.Interfacies;
using UserStorage.Entity;
using System.Text;

namespace WcfService
{

    [ServiceContract]
    public interface IUserService
    {
        [OperationContract]
        int Add(User item);

        [OperationContract]
        IEnumerable<User> Search();

        [OperationContract]
        void Delete(User item);
    }
}
