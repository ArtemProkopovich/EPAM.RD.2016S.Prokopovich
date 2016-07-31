using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using UserStorage.Entity;

namespace ClientApplication
{
    public class Program
    {
        static volatile Random random = new Random();

        public static void Main(string[] args)
        {
            Mutex mutex;
            while (!Mutex.TryOpenExisting("name", out mutex))
                Thread.Sleep(100);
            mutex.WaitOne();

            var service = new ServiceReference.UserServiceClient();
            var cts = new CancellationTokenSource();
            var token = cts.Token;
            var start = new ManualResetEventSlim(false);

            WaitCallback callService = (object state) =>
            {
                start.Wait();
                var addedUsers = service.Search(null).ToList();
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
                            Console.WriteLine("User {0} will be add.", user);
                            service.Add(user);
                            break;
                        case 1:
                            if (addedUsers.Count > 0)
                            {
                                var dUser = addedUsers[random.Next(addedUsers.Count)];
                                addedUsers.RemoveAll(e => e.Equals(dUser));
                                Console.WriteLine("User {0} will be deleted.", dUser);
                                service.Delete(dUser);
                            }
                            break;
                        default:
                            var users = service.Search(null);
                            PrintUsers("Users in rep now:", users);
                            break;
                    }
                    Thread.Sleep(random.Next(500, 1500));
                }
            };

            for (int i = 0; i < 1; i++)
            {
                ThreadPool.QueueUserWorkItem(callService);
            }

            Console.WriteLine("Threads will started.");
            Console.WriteLine("Press any key to stop");
            start.Set();
            Console.ReadLine();
            cts.Cancel();
            Console.WriteLine("Threads stoped");
            Console.ReadLine();
        }


        private static void PrintUsers(string message, IEnumerable<User> users)
        {
            Console.WriteLine(message);
            int i = 1;
            foreach(var u in users)
            {
                Console.WriteLine("{0}. {1}", i, u);
                i++;
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
