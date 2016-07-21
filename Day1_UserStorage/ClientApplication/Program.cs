using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UserStorageConfiguration;
using System.Configuration;
using UserStorage.Service;
using UserStorage.Entity;

namespace ClientApplication
{
    public class Program
    {
        static volatile Random random = new Random();

        public static void Main(string[] args)
        {
            Configurator conf = new Configurator();
            var keeper = conf.Initialize();
            
            CancellationTokenSource cts = new CancellationTokenSource();
            CancellationToken token = cts.Token;           
            var start = new ManualResetEventSlim(false);

            WaitCallback callMaster = (object param) =>
            {
                start.Wait();
                MasterService master = (MasterService)param;
                List<User> addedUsers = master.Search(e => true).ToList();
                while (true)
                {
                    if (token.IsCancellationRequested)
                        break;
                    int opType = random.Next(3);
                    switch (opType)
                    {
                        case 0:
                            var user = GenerateUser();
                            addedUsers.Add(user);
                            Console.WriteLine("User {0} will be add to master.", user);
                            master.Add(user);
                            break;
                        case 1:
                            if (addedUsers.Count > 0)
                            {
                                var dUser = addedUsers[random.Next(addedUsers.Count)];
                                addedUsers.RemoveAll(e => e.Equals(dUser));
                                Console.WriteLine("User {0} will be deleted from master.", dUser);
                                master.Delete(dUser);
                            }
                            break;                             
                        default:
                            var users = master.Search(e => true);
                            PrintUsers("Users in master now:", users);
                            break;
                    }
                    Thread.Sleep(1200);
                }
            };

            WaitCallback callSlave = (object param) =>
            {
                start.Wait();
                SlaveService slave = (SlaveService)param;
                while (true)
                {
                    if (token.IsCancellationRequested)
                        break;
                    var users = slave.Search(e => true);
                    PrintUsers("Users in slave " + slave.ServiceId.ToString().Substring(0, 6) + " now:", users);
                    Thread.Sleep(1000);
                }
            };

            ThreadPool.QueueUserWorkItem(callMaster, keeper.Master);
            foreach(var s in keeper.Slaves)
            {
                ThreadPool.QueueUserWorkItem(callSlave, s);
            }
            
            Console.WriteLine("Services will started.");
            Console.WriteLine("Press any key to stop");
            start.Set();
            Console.ReadLine();
            cts.Cancel();
            Console.WriteLine("Threads stoped");
            Console.WriteLine("Data from master will be saved");
            keeper.Master.Save();
            Console.ReadLine();          
        }


        private static void PrintUsers(string message, IEnumerable<User> users)
        {
            Console.WriteLine(message);
            int i = 1;
            foreach(var u in users)
            {
                Console.WriteLine("{0}. {1}", i, u);
            }
        }

        private static User GenerateUser()
        {
            User result = new User();
            result.FirstName = GenerateString(5);
            result.LastName = GenerateString(10);
            result.BirthDate = DateTime.FromBinary(random.Next());
            result.PersonalId = GenerateString(5);
            return result;
            
        }

        private static string GenerateString(int length)
        {
            var alphabet = "abcdefghijklmnopqrstuvwxyz0123456789";
            var result = new StringBuilder();
            for(int i = 0; i < length; i++)
            {
                result.Append(alphabet[random.Next(alphabet.Length)]);
            }
            return result.ToString();
        }
    }
}
