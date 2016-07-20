using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication
{
    class Program
    {


        static void Main(string[] args)
        {
            KeyValuePair<int, int> a = new KeyValuePair<int, int>(4, 31);
            KeyValuePair<int, int> b = new KeyValuePair<int, int>(5, 30);
            Console.WriteLine(a.GetHashCode());
            Console.WriteLine(b.GetHashCode());
            Console.Read();

            Action<int> ab = (int x) => { return; };
            Action<int> ab2 = (int x) => { return; };
            Action<int,int> ab3 = (int x,int y) => {return ; };
            Console.WriteLine(ab.GetHashCode());
            Console.WriteLine(ab2.GetHashCode());
            Console.WriteLine(ab3.GetHashCode());
        }

    }
}
