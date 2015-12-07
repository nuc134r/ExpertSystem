using Core;
using Core.Exceptions;
using Core.Parsing;
using NUnit.Framework;

namespace Tests.WhenParsing
{
    [TestFixture]
    public class ThrowsFactAtomException
    {
        [Test]
        public void OnAtomAsOnlyArgument()
        {
            const string code = "Human(X);";

            var parser = new Parser(code);

            Assert.Throws<FactAtomException>(() => { parser.Do(new RunContext()); });
        }

        [Test]
        public void OnAtomAsFirstArgument()
        {
            const string code = "Likes(X, Mark);";

            var parser = new Parser(code);

            Assert.Throws<FactAtomException>(() => { parser.Do(new RunContext()); });
        }

        [Test]
        public void OnAtomAsSecondArgument()
        {
            const string code = "Likes(Maria, X, Mark);";

            var parser = new Parser(code);

            Assert.Throws<FactAtomException>(() => { parser.Do(new RunContext()); });
        }

        [Test]
        public void OnAtomAsLastArgument()
        {
            const string code = "Likes(Maria, X);";

            var parser = new Parser(code);

            Assert.Throws<FactAtomException>(() => { parser.Do(new RunContext()); });
        }
    }
}