using Core;
using Core.Exceptions;
using NUnit.Framework;

namespace Tests.WhenParsing
{
    [TestFixture]
    public class ThrowsFactAtomException
    {
        [SetUp]
        public void Initialize()
        {
            parser = new Parser();
        }

        private Parser parser;

        [Test]
        public void OnAtomAsFirstArgument()
        {
            const string code = "Likes(X, Mark);";

            Assert.Throws<FactAtomException>(() => { parser.Do(code, new RunContext()); });
        }

        [Test]
        public void OnAtomAsSecondArgument()
        {
            const string code = "Likes(Maria, X, Mark);";

            Assert.Throws<FactAtomException>(() => { parser.Do(code, new RunContext()); });
        }

        [Test]
        public void OnAtomAsLastArgument()
        {
            const string code = "Likes(Maria, X);";

            Assert.Throws<FactAtomException>(() => { parser.Do(code, new RunContext()); });
        }
    }
}