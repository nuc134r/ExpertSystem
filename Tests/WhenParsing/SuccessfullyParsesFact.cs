using System.Linq;
using Core;
using NUnit.Framework;

namespace Tests.WhenParsing
{
    [TestFixture]
    public class SuccessfullyParsesFact
    {
        [SetUp]
        public void Initialize()
        {
            parser = new Parser();
        }

        private Parser parser;

        [Test]
        public void WithOneArgument()
        {
            const string code = "Human(Mark);";

            var context = new RunContext();
            parser.Do(code, context);

            var fact = context.Facts.First();

            Assert.AreEqual("Human", fact.Name);
            Assert.AreEqual("Mark", fact.Arguments.First().Name);
        }

        [Test]
        public void WithTwoArguments()
        {
            const string code = "Likes(Alex, Miranda);";

            var context = new RunContext();
            parser.Do(code, context);

            var fact = context.Facts.First();

            Assert.AreEqual("Likes", fact.Name);
            Assert.AreEqual("Alex", fact.Arguments.First().Name);
            Assert.AreEqual("Miranda", fact.Arguments.Last().Name);
        }
    }
}