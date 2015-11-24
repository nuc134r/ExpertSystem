using Core;
using Core.Exceptions;
using NUnit.Framework;

namespace Tests.WhenParsing
{
    [TestFixture]
    public class ThrowsArgumentNameException
    {
        [SetUp]
        public void Initialize()
        {
            parser = new Parser();
        }

        private Parser parser;

        [Test]
        public void OnEmptyArguments()
        {
            const string code = "Human() : Minded()";

            Assert.Throws<ArgumentNameExpectedException>(() => { parser.Do(code, new RunContext()); });
        }

        [Test]
        public void OnMissedFirstArgument()
        {
            const string code = "Rule(, B) : Condition(A, B)";

            Assert.Throws<ArgumentNameExpectedException>(() => { parser.Do(code, new RunContext()); });
        }

        [Test]
        public void OnMissedSecondArgument()
        {
            const string code = "Rule(A, ) : Condition(A, B)";

            Assert.Throws<ArgumentNameExpectedException>(() => { parser.Do(code, new RunContext()); });
        }
    }
}