﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ClientApplication.ServiceReference {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ServiceReference.IUserService")]
    public interface IUserService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IUserService/Add", ReplyAction="http://tempuri.org/IUserService/AddResponse")]
        int Add(UserStorage.Entity.User item);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IUserService/Add", ReplyAction="http://tempuri.org/IUserService/AddResponse")]
        System.Threading.Tasks.Task<int> AddAsync(UserStorage.Entity.User item);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IUserService/Search", ReplyAction="http://tempuri.org/IUserService/SearchResponse")]
        UserStorage.Entity.User[] Search();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IUserService/Search", ReplyAction="http://tempuri.org/IUserService/SearchResponse")]
        System.Threading.Tasks.Task<UserStorage.Entity.User[]> SearchAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IUserService/Delete", ReplyAction="http://tempuri.org/IUserService/DeleteResponse")]
        void Delete(UserStorage.Entity.User item);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IUserService/Delete", ReplyAction="http://tempuri.org/IUserService/DeleteResponse")]
        System.Threading.Tasks.Task DeleteAsync(UserStorage.Entity.User item);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IUserServiceChannel : ClientApplication.ServiceReference.IUserService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class UserServiceClient : System.ServiceModel.ClientBase<ClientApplication.ServiceReference.IUserService>, ClientApplication.ServiceReference.IUserService {
        
        public UserServiceClient() {
        }
        
        public UserServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public UserServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public UserServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public UserServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public int Add(UserStorage.Entity.User item) {
            return base.Channel.Add(item);
        }
        
        public System.Threading.Tasks.Task<int> AddAsync(UserStorage.Entity.User item) {
            return base.Channel.AddAsync(item);
        }
        
        public UserStorage.Entity.User[] Search() {
            return base.Channel.Search();
        }
        
        public System.Threading.Tasks.Task<UserStorage.Entity.User[]> SearchAsync() {
            return base.Channel.SearchAsync();
        }
        
        public void Delete(UserStorage.Entity.User item) {
            base.Channel.Delete(item);
        }
        
        public System.Threading.Tasks.Task DeleteAsync(UserStorage.Entity.User item) {
            return base.Channel.DeleteAsync(item);
        }
    }
}
