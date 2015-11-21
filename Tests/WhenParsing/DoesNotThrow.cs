using Core;
using NUnit.Framework;

namespace Tests.WhenParsing
{
    [TestFixture]
    public class DoesNotThrow
    {
        [SetUp]
        public void Initialize()
        {
            parser = new Parser();
        }

        private Parser parser;

        [Test]
        public void DoesNotThrowOnEmptyLine()
        {
            const string code = "";

            Assert.DoesNotThrow(() => { parser.Do(code, new RunContext()); });
        }
    }
}