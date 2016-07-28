using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Activation;
using UserStorage.Service;

namespace WcfService.Configuration
{
    public class CustomServiceHostFactory : ServiceHostFactory
    {
        private readonly ServiceProxy proxy;

        public CustomServiceHostFactory(ServiceProxy proxy)
        {
            this.proxy = proxy;
        }

        protected override ServiceHost CreateServiceHost(Type serviceType,
            Uri[] baseAddresses)
        {
            return new CustomServiceHost(this.proxy, serviceType, baseAddresses);
        }

    }
}
