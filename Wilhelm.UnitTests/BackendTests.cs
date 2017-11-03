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
    }
}
