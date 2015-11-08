using System;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

using Core;

namespace Tests
{
    [TestFixture]
    public class WhenParsing
    {
        private Parser parser;

        [SetUp]
        public void Initialize()
        {
            parser = new Parser();
        }

        [Test]
        public void WhitespaceIsDeleted()
        {
            const string code = "  Fluffy(X); \n\r  Skinny(X);  ";

            var result = parser.Do(code);

            Assert.AreEqual("Fluffy(X);Skinny(X);", result.OptimizedCode);
        }
    }
}
