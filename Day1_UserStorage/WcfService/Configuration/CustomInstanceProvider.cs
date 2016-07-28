using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Description;
using System.ServiceModel;
using System.ServiceModel.Channels;
using UserStorage.Service;

namespace WcfService.Configuration
{
    public class CustomInstanceProvider : IInstanceProvider, IContractBehavior
    {
        private readonly ServiceProxy proxy;

        public CustomInstanceProvider(ServiceProxy proxy)
        {
            if (proxy == null)
            {
                throw new ArgumentNullException(nameof(proxy));
            }

            this.proxy = proxy;
        }

        public void ReleaseInstance(InstanceContext instanceContext, object instance)
        {
            var disposable = instance as IDisposable;
            if (disposable != null)
            {
                disposable.Dispose();
            }
        }

        public void ApplyDispatchBehavior(ContractDescription contractDescription, ServiceEndpoint endpoint, DispatchRuntime dispatchRuntime)
        {
            dispatchRuntime.InstanceProvider = this;
        }

        public object GetInstance(InstanceContext instanceContext)
        {
            return new UserService(proxy);
        }

        public object GetInstance(InstanceContext instanceContext, Message message)
        {
            return this.GetInstance(instanceContext);
        }

        #region Unimplemented methods
        public void AddBindingParameters(ContractDescription contractDescription, ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyClientBehavior(ContractDescription contractDescription, ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
        }

        public void Validate(ContractDescription contractDescription, ServiceEndpoint endpoint)
        {
        }
        #endregion
    }
}
