using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserStorageConfiguration;
using System.Configuration;

namespace ClientApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Configurator conf = new Configurator();
            var keeper = conf.Initialize();
            var keeperDomens  = conf.InitializeWithDomens();
            keeperDomens.Master.Add(new UserStorage.Entity.User() { FirstName = "a", LastName = "b" });
            var user = keeperDomens.Slaves.First().Search(e => e.FirstName == "a");
            var users = keeperDomens.Master.Search(e => true);
            try
            {
                keeperDomens.Slaves.First().Add(new UserStorage.Entity.User());
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.GetType());
            }
        }
    }
}
