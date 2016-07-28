using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WcfService;
using WcfService.Configuration;
using System.ServiceModel;
using System.ServiceModel.Description;
using UserStorage.Service;
using UserStorageConfiguration;

namespace ServerApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Uri baseAddress = new Uri("http://localhost:8733/Design_Time_Addresses/WcfService/UserService/");
            ServiceProxy proxy = new Configurator().Initialize();
            using (ServiceHost host = new CustomServiceHost(proxy, typeof(UserService), baseAddress))
            {
                ServiceMetadataBehavior smb = new ServiceMetadataBehavior();
                smb.HttpGetEnabled = true;
                smb.MetadataExporter.PolicyVersion = PolicyVersion.Policy15;
                host.Description.Behaviors.Add(smb);
                host.Open();

                Console.WriteLine("The service is ready at {0}", baseAddress);
                Console.WriteLine("Press <Enter> to stop the service.");
                Console.ReadLine();

                host.Close();
            }
            proxy.Save();
            Console.WriteLine("Master state was saved");
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}
