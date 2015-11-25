using Core;
using Core.Exceptions;
using NUnit.Framework;

namespace Tests.WhenParsing
{
    [TestFixture]
    public class ThrowsUnexpectedLineEndException
    {
        [Test]
        public void OnNotClosedBracket()
        {
            const string code = "Friends(Alex, Alisa";

            var parser = new Parser(code);

            Assert.Throws<UnexpectedLineEndException>(() => { parser.Do(new RunContext()); });
        }
    }
}