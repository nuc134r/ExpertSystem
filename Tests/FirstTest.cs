using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class FirstTest
    {
        [Test]
        public void IsWorking()
        {
            var a = 20;
            var b = 20;

            var c = a + b;

            Assert.AreEqual(c, 40);
        }
    }
}
