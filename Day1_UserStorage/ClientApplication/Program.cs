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
        private static readonly int threadsCount = 5;
        private static volatile Random random = new Random();      

        public static void Main(string[] args)
        {
            ////Wait while server started and release mutex;
            Mutex mutex;
            while (!Mutex.TryOpenExisting("name", out mutex))
                Thread.Sleep(100);
            mutex.WaitOne();

            var service = new ServiceReference.UserServiceClient();
            var cts = new CancellationTokenSource();
            var token = cts.Token;
            var start = new ManualResetEventSlim(false);
            ////Callback for thread that wile be adding and deleting users from storage; 
            WaitCallback callService = (object state) =>
            {
                start.Wait();
                var users = service.Search(null).ToList();
                while (true)
                {
                    
                    if (token.IsCancellationRequested)
                        break;
                    try
                    {
                        int opType = random.Next(2);
                        switch (opType)
                        {
                            case 0:
                                var user = GenerateUser();
                                Console.WriteLine("User {0} will be add.", user);
                                int result = service.Add(user);
                                user.Id = result;
                                users.Add(user);
                                break;
                            case 1:
                                if (users.Count > 0)
                                {
                                    var dUser = users.ElementAt(random.Next(users.Count));
                                    users.RemoveAll(e => e.Equals(dUser));
                                    Console.WriteLine("User {0} will be deleted.", dUser);
                                    service.Delete(dUser);
                                }
                                break;
                        }
                        Thread.Sleep(random.Next(500, 1500));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error when accessing the service {ex.Message}");
                        break;
                    }
                }
            };

            ////Callback for thread that will be take all users from storage. 
            WaitCallback searchingService=(object state) =>
            {
                start.Wait();
                while (true)
                {
                    if (token.IsCancellationRequested)
                        break;
                    try
                    {
                        var users = service.Search(null).ToList();
                        PrintUsers("Users in rep now:", users);
                        Thread.Sleep(random.Next(1000, 1500));
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine($"Error when accessing the service {ex.Message}");
                        break;
                    }
                }
            };

            ////Start threads
            for (int i = 0; i < threadsCount; i++)
            {
                if (i == 0)
                    ThreadPool.QueueUserWorkItem(callService);
                else
                    ThreadPool.QueueUserWorkItem(searchingService);
            }

            Console.WriteLine("Threads will started.");
            Console.WriteLine("Press any key to stop");
            start.Set();
            Console.ReadLine();
            cts.Cancel();
            Console.WriteLine("Threads stoped");
            Console.ReadLine();
        }

        /// <summary>
        /// Outputon the console list of users with previous message;
        /// </summary>
        /// <param name="message">Message that will be output before list</param>
        /// <param name="users">List of users</param>
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

        /// <summary>
        /// Generate user with random values of properties
        /// </summary>
        /// <returns></returns>
        private static User GenerateUser()
        {
            User result = new User();
            result.FirstName = GenerateString(5);
            result.LastName = GenerateString(10);
            result.BirthDate = DateTime.FromBinary(random.Next());
            result.PersonalId = GenerateString(5);
            return result;
            
        }

        /// <summary>
        /// Generate string with given length and text and numeric symbols
        /// </summary>
        /// <param name="length">Length of string</param>
        /// <returns></returns>
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
