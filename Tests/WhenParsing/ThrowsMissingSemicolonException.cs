using Core;
using Core.Exceptions;
using NUnit.Framework;

namespace Tests.WhenParsing
{
    [TestFixture]
    public class ThrowsMissingSemicolonException
    {
        [Test]
        public void OnMissedSemicolon()
        {
            const string code = "Human(X)";

            var parser = new Parser(code);

            Assert.Throws<MissingSemicolonException>(() => { parser.Do(new RunContext()); });
        }
    }
}