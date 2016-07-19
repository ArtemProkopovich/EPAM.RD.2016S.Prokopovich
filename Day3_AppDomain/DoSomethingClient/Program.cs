using System;
using System.IO;
using System.Reflection;
using MyInterfaces;

namespace DoSomethingClient
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var input = new Input
            {
                Users = new User[]
                {
                    new User
                    {
                        Id = 1,
                        Name = "Vasily",
                        Age = 23
                    },
                    new User
                    {
                        Id = 2,
                        Name = "Semen",
                        Age = 35
                    },
                    new User
                    {
                        Id = 3,
                        Name = "Pawel",
                        Age = 22
                    }
                }
            };

            Method1(input);
            Method2(input);
            Console.ReadLine();
        }

        private static void Method1(Input input)
        {
            // TODO: Create a domain with name MyDomain.
            AppDomain domain = AppDomain.CreateDomain("MyDomain");
            var loader = (DomainAssemblyLoader)domain.CreateInstanceAndUnwrap(Assembly.GetExecutingAssembly().FullName, typeof(DomainAssemblyLoader).FullName);
            try
            {
                var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"MyDomain\MyLibrary.dll");
                Result result = loader.LoadFile(path, input); // TODO: Use loader here.

                Console.WriteLine("Method1: {0}", result.Value);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: {0}", e.Message);
            }
            // TODO: Unload domain
            AppDomain.Unload(domain);
        }

        private static void Method2(Input input)
        {
            var appDomainSetup = new AppDomainSetup
            {
                ApplicationBase = AppDomain.CurrentDomain.BaseDirectory,
                PrivateBinPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MyDomain")
            };
            var security = new System.Security.Policy.Evidence();
            // TODO: Create a domain with name MyDomain and setup from appDomainSetup.
            AppDomain domain = AppDomain.CreateDomain("MyDomain", security, appDomainSetup);
            var loader = (DomainAssemblyLoader)domain.CreateInstanceAndUnwrap(Assembly.GetExecutingAssembly().FullName, typeof(DomainAssemblyLoader).FullName);
            try
            {
                var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"MyDomain\MyLibrary.dll");
                Result result = loader.LoadFrom(path, input); // TODO: Use loader here.

                Console.WriteLine("Method2: {0}", result.Value);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: {0}", e.Message);
            }

            // TODO: Unload domain
            AppDomain.Unload(domain);
        }
    }
}
