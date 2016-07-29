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
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace ServerApplication
{
    public class Program
    {
        public class AsyncTcpServer
        {
            private string address;
            private int port;
            public AsyncTcpServer(string address, int port)
            {
                this.address = address;
                this.port = port;
            }

            public async void Start()
            {
                TcpListener listener = new TcpListener(IPAddress.Parse(address), port);
                listener.Start();

                while (true)
                {
                    try
                    {
                        var tcpClient = await listener.AcceptTcpClientAsync();
                        HandleConnectionAsync(tcpClient);
                    }
                    catch (Exception exp)
                    {

                    }
                }
            }

            private async void HandleConnectionAsync(TcpClient tcpClient)
            {
                try
                {
                    using (var networkStream = tcpClient.GetStream())
                    using (var reader = new StreamReader(networkStream))
                    using (var writer = new StreamWriter(networkStream))
                    {
                        writer.AutoFlush = true;
                        while (true)
                        {
                            var dataFromServer = await reader.ReadLineAsync();
                            if (string.IsNullOrEmpty(dataFromServer))
                            {
                                break;
                            }
                            await writer.WriteLineAsync("FromServer-" + dataFromServer);
                        }
                    }
                }
                catch (Exception exp)
                {
                }
                finally
                {
                    tcpClient.Close();
                }

            }
        }

        public static void Main(string[] args)
        {
            bool createdNew = false;
            Mutex mutex = new Mutex(true, "name", out createdNew);
            Uri baseAddress = new Uri("http://localhost:8733/Design_Time_Addresses/WcfService/UserService/");
            ServiceProxy proxy = new Configurator().Initialize();
            using (ServiceHost host = new CustomServiceHost(proxy, typeof(UserService), baseAddress))
            {
                ServiceMetadataBehavior smb = new ServiceMetadataBehavior();
                smb.HttpGetEnabled = true;
                smb.MetadataExporter.PolicyVersion = PolicyVersion.Policy15;
                host.Description.Behaviors.Add(smb);
                host.Open();

                mutex.ReleaseMutex();

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
