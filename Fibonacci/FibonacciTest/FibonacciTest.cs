using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Fibbonachi;

namespace FibonacciTest
{
    [TestClass]
    public class FibonacciTest
    {
        [TestMethod]
        public void FibonacciEnumerator_TestMethod()
        {
            var f = new FibonacciEnumerable();
            CollectionAssert.AreEqual(array, f.Take(10).ToArray());
        }

        private int[] array = new int[] { 1, 1, 2, 3, 5, 8, 13, 21, 34, 55 };

        private class FibonacciEnumerable : IEnumerable<int>
        {
            public IEnumerator<int> GetEnumerator()
            {
                return new FibonacciEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
    }
}
