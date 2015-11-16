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
        [Ignore]
        public void WhitespaceIsDeleted()
        {
            const string code = "  Human(X) : \n\r Minded(X);   \n\r ;  ";

            var result = parser.Do(code, new RunContext());

            Assert.AreEqual("Human(X):Minded(X);", result.OptimizedCode);
        }

        [Test]
        public void ThrowsUnexpectedTokenException1()
        {
            const string code = "Hum;an(X) : Minded(X)";

            var ex = Assert.Throws<UnexpectedTokenException>(() => { parser.Do(code, new RunContext()); });

            Assert.AreEqual("Unexpected token ';' at 3", ex.Message);
        }

        [Test]
        public void ThrowsUnexpectedTokenException2()
        {
            const string code = "Human(@X) : Minded(X)";

            var ex = Assert.Throws<UnexpectedTokenException>(() => { parser.Do(code, new RunContext()); });

            Assert.AreEqual("Unexpected token '@' at 6", ex.Message);
        }

        [Test]
        public void ThrowsUnexpectedLineEndException1()
        {
            const string code = "";

            Assert.Throws<UnexpectedLineEndException>(() => { parser.Do(code, new RunContext()); });
        }

        [Test]
        public void ThrowsUnexpectedLineEndException2()
        {
            const string code = "Human(X) : Minded(X)";

            Assert.Throws<UnexpectedLineEndException>(() => { parser.Do(code, new RunContext()); });
        }

        [Test]
        public void ThrowsArgumentNameExpectedException1()
        {
            const string code = "Human() : Minded()";

            Assert.Throws<ArgumentNameExpectedException>(() => { parser.Do(code, new RunContext()); });
        }

        [Test]
        public void ThrowsArgumentNameExpectedException2()
        {
            const string code = "Rule(, B) : Condition(A, B)";

            Assert.Throws<ArgumentNameExpectedException>(() => { parser.Do(code, new RunContext()); });
        }
    }
}