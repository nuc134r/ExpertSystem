using Core;
using Core.Exceptions;
using NUnit.Framework;

namespace Tests.WhenParsing
{
    [TestFixture]
    public class ThrowsMissingSemicolonException
    {
        [SetUp]
        public void Initialize()
        {
            parser = new Parser();
        }

        private Parser parser;

        [Test]
        public void OnMissedSemicolon()
        {
            const string code = "Human(X)";

            Assert.Throws<MissingSemicolonException>(() => { parser.Do(code, new RunContext()); });
        }
    }
}