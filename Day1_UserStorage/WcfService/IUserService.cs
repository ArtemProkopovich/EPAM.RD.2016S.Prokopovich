using System.Collections.Generic;
using System.ServiceModel;
using UserStorage.Entity;
using UserStorage.Service;

namespace WcfService
{

    [ServiceContract]
    public interface IUserService
    {        
        [OperationContract]
        [FaultContract(typeof(WcfServiceException))]
        int Add(User item);
       
        [OperationContract]
        [FaultContract(typeof(WcfServiceException))]
        IEnumerable<User> Search(UserCriteria criteria);
        
        [OperationContract]
        [FaultContract(typeof(WcfServiceException))]
        void Delete(User item);
    }
}
