using System.Linq;
using Core;
using NUnit.Framework;

namespace Tests.WhenParsing
{
    [TestFixture]
    public class ParsesFacts
    {
        [Test]
        public void WithOneArgument()
        {
            const string code = "Human(Mark);";

            var parser = new Parser(code);
            var context = new RunContext();

            parser.Do(context);

            var fact = context.Facts.First();

            Assert.AreEqual("Human", fact.Name);
            Assert.AreEqual("Mark", fact.Arguments.First().Name);
        }

        [Test]
        public void WithTwoArguments()
        {
            const string code = "Likes(Alex, Miranda);";

            var parser = new Parser(code);
            var context = new RunContext();
            parser.Do(context);

            var fact = context.Facts.First();

            Assert.AreEqual("Likes", fact.Name);
            Assert.AreEqual("Alex", fact.Arguments.First().Name);
            Assert.AreEqual("Miranda", fact.Arguments.Last().Name);
        }

        [Test]
        public void TwoFacts()
        {
            const string code = "Likes(Alex, Miranda);" +
                                "Hates(Miranda, Alex);";

            var parser = new Parser(code);
            var context = new RunContext();
            parser.Do(context);

            var alexLikesMiranda = context.Facts.First();
            var mirandaHatesAlex = context.Facts.Last();

            Assert.AreEqual("Likes", alexLikesMiranda.Name);
            Assert.AreEqual("Hates", mirandaHatesAlex.Name);

            Assert.AreEqual("Alex", alexLikesMiranda.Arguments.First().Name);
            Assert.AreEqual("Miranda", alexLikesMiranda.Arguments.Last().Name);

            Assert.AreEqual("Miranda", mirandaHatesAlex.Arguments.First().Name);
            Assert.AreEqual("Alex", mirandaHatesAlex.Arguments.Last().Name);
        }
    }
}