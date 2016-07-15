using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fibbonachi
{
    public class FibonacciEnumerator : IEnumerator<int>
    {
        int prev = 0;
        int current = 1;
        public int Current
        {
            get
            {
                int temp = current + prev;
                prev = current;
                int currentTemp = current;
                current = temp;
                return currentTemp;
            }
        }

        object IEnumerator.Current
        {
            get
            {               
                return Current;
            }
        }

        public void Dispose()
        {
            
        }

        public bool MoveNext()
        {
            if (int.MaxValue - current < prev)
                return false;
            return true;
        }

        public void Reset()
        {
            prev = 0;
            current = 1;
        }
    }
}
