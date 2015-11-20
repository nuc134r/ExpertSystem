using Core;
using Core.Exceptions;
using NUnit.Framework;

namespace Tests.WhenParsing
{
    [TestFixture]
    public class ThrowsUnexpectedLineEndException
    {
        [SetUp]
        public void Initialize()
        {
            parser = new Parser();
        }

        private Parser parser;

        [Test]
        public void OnNotClosedBracket()
        {
            const string code = "Friends(Alex, Alisa";

            Assert.Throws<UnexpectedLineEndException>(() => { parser.Do(code, new RunContext()); });
        }
    }
}