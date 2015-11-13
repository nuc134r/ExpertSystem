using Core;
using Core.Exceptions;
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
        public void EverythingIsOk()
        {
            const string code = "Human(X) : Minded(X)";

            var result = parser.Do(code, new RunContext());

            Assert.Pass($"{result.ElapsedTime} ms");
        }

        [Test]
        public void ThrowsUnexpectedTokenException()
        {
            const string code = "Hum;an(X) : Minded(X)";

            Assert.Throws<UnexpectedTokenException>(() => { parser.Do(code, new RunContext()); });
        }

        [Test]
        public void WhitespaceIsDeleted()
        {
            const string code = "  Human(X) : \n\r Minded(X)   \n\r ;  ";

            var result = parser.Do(code, new RunContext());

            Assert.AreEqual("Human(X):Minded(X);", result.OptimizedCode);
        }
    }
}