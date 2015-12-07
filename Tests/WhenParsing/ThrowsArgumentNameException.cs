using Core;
using Core.Exceptions;
using Core.Parsing;
using NUnit.Framework;

namespace Tests.WhenParsing
{
    [TestFixture]
    public class ThrowsArgumentNameException
    {
        [Test]
        public void OnEmptyArguments()
        {
            const string code = "Human() : Minded()";

            var parser = new Parser(code);

            Assert.Throws<ArgumentNameExpectedException>(() => { parser.Do(new RunContext()); });
        }

        [Test]
        public void OnMissedFirstArgument()
        {
            const string code = "Rule(, B) : Condition(A, B)";

            var parser = new Parser(code);

            Assert.Throws<ArgumentNameExpectedException>(() => { parser.Do(new RunContext()); });
        }

        [Test]
        public void OnMissedSecondArgument()
        {
            const string code = "Rule(A, ) : Condition(A, B)";

            var parser = new Parser(code);

            Assert.Throws<ArgumentNameExpectedException>(() => { parser.Do(new RunContext()); });
        }
    }
}