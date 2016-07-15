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
        }
    }
}
