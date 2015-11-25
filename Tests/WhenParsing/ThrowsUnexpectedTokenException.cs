using Core;
using Core.Exceptions;
using NUnit.Framework;

namespace Tests.WhenParsing
{
    [TestFixture]
    public class ThrowsUnexpectedTokenException
    {
        [Test]
        public void OnAccidentialSemicolon()
        {
            const string code = "Hum;an(X) : Minded(X)";
            var parser = new Parser(code);

            var ex = Assert.Throws<UnexpectedTokenException>(() => { parser.Do(new RunContext()); });

            Assert.AreEqual("Unexpected token ';'", ex.Message);
        }

        [Test]
        public void OnEmailSymbolInArgument()
        {
            const string code = "Human(@X) : Minded(X)";
            var parser = new Parser(code);

            var ex = Assert.Throws<UnexpectedTokenException>(() => { parser.Do(new RunContext()); });

            Assert.AreEqual("Unexpected token '@'", ex.Message);
        }
    }
}