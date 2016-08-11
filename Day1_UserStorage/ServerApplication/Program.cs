using System;
using WcfService;
using System.ServiceModel;
using System.ServiceModel.Description;
using UserStorage.Service;
using UserStorageConfiguration;
using System.Threading;

namespace ServerApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            bool createdNew = false;
            Mutex mutex = new Mutex(true, "name", out createdNew);
            Uri baseAddress = new Uri("http://localhost:8733/Design_Time_Addresses/WcfService/UserService/");
            try
            {
                ////Initialize service and start server
                ServiceProxy proxy = new Configurator().Initialize();
                var service = new UserService(proxy);
                service.Added += OnAdded;
                service.Deleted += OnDeleted;
                service.Searched += OnSearched;
                using (ServiceHost host = new ServiceHost(service, baseAddress))
                {
                    ServiceMetadataBehavior smb = new ServiceMetadataBehavior();
                    smb.HttpGetEnabled = true;
                    smb.MetadataExporter.PolicyVersion = PolicyVersion.Policy15;
                    host.Description.Behaviors.Add(smb);
                    host.Open();
                    ////When server succesfully started release mutex.
                    mutex.ReleaseMutex();

                    Console.WriteLine("The service is ready at {0}", baseAddress);
                    Console.WriteLine("Press <Enter> to stop the service.");
                    Console.ReadLine();

                    host.Close();
                }
                ////Save master state when server closed.
                proxy.Save();
                Console.WriteLine("Master state was saved");
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Server failed with {ex.Message}");
            }
            
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }

        /// <summary>
        /// Event on user adding
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        protected static void OnAdded(object sender, ServiceEventArgs args)
        {
            Console.WriteLine($"User {args.User} was added.");
        }
        /// <summary>
        /// Event on user deleting
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        protected static void OnDeleted(object sender, ServiceEventArgs args)
        {
            Console.WriteLine($"User {args.User} was deleted.");
        }
        /// <summary>
        /// Event on searching
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        protected static void OnSearched(object sender, ServiceSearchEventArgs args)
        {
            Console.WriteLine($"Search by criteria {args.Criteria} returned {args.Count} results.");
        }
    }
}
