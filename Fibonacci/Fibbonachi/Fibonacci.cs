using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fibbonachi
{
    public class Fibonacci
    {
        public IEnumerable<int> GetFibonacciSequence()
        {
            int prev = 0;
            int current = 1;
            while (true)
            {
                yield return current;
                if (int.MaxValue - current < prev)
                    break;
                int temp = current + prev;
                prev = current;
                current = temp;
            }
        }
    }
}
