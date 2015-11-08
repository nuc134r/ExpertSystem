using Core;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class WhenParsing
    {
        [SetUp]
        public void Initialize()
        {
            parser = new Parser();
        }

        private Parser parser;

        [Test]
        public void WhitespaceIsDeleted()
        {
            const string code = "  Fluffy(X); \n\r  Skinny(X);  ";

            var result = parser.Do(code);

            Assert.AreEqual("Fluffy(X);Skinny(X);", result.OptimizedCode);
        }
    }
}