using Core;
using Core.Exceptions;
using NUnit.Framework;

namespace Tests.WhenParsing
{
    [TestFixture]
    public class ThrowsUnexpectedTokenException
    {
        [SetUp]
        public void Initialize()
        {
            parser = new Parser();
        }

        private Parser parser;

        [Test]
        public void OnAccidentialSemicolon()
        {
            const string code = "Hum;an(X) : Minded(X)";

            var ex = Assert.Throws<UnexpectedTokenException>(() => { parser.Do(code, new RunContext()); });

            Assert.AreEqual("Unexpected token ';'", ex.Message);
        }

        [Test]
        public void OnEmailSymbolInArgument()
        {
            const string code = "Human(@X) : Minded(X)";

            var ex = Assert.Throws<UnexpectedTokenException>(() => { parser.Do(code, new RunContext()); });

            Assert.AreEqual("Unexpected token '@'", ex.Message);
        }
    }
}