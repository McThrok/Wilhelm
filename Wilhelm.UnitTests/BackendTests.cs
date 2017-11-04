using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wilhelm.Backend;

namespace Wilhelm.UnitTests
{
    class BackendTests
    {
        [Test]
        public void Add_AddPositive_Returns_5()
        {
            Class1 c = new Class1();
            int expectedResult = 5;
            int result = c.Add(2, 3);
            Assert.AreEqual(expectedResult, result);
        }

        [TestCase(5,1,4)]
        [TestCase(7,3,4)]
        public void Add_AddPositive_ReturnsNumber(int expected, int a, int b)
        {
            Class1 c = new Class1();
            int result = c.Add(a,b);
            Assert.AreEqual(expected, result);
        }
    }
}
