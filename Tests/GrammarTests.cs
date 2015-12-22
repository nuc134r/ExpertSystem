using NUnit.Framework;
using Sprache;
using Logikek;

namespace Tests
{
    [TestFixture]
    public class GrammarTests
    {
        [Test]
        public void AnIdentifierIsASequenceOfCharacters()
        {
            const string input = "   Human  ()";
            var id = Grammar.Identifier.Parse(input);
            Assert.AreEqual("Human", id);
        }

        [Test]
        public void ArgumentsAreBetweenBracketsAndDelimitedByCommas()
        {
            const string input = "(Minded, Alive)";
            var arg = Grammar.Arguments.Parse(input);
            Assert.AreEqual(new[] { "Minded", "Alive" }, arg);
        }

        [Test]
        public void AnIdentifierCannotStartWithQuote()
        {
            const string input = "   \"Human  ()";
            Assert.Throws<ParseException>(() => Grammar.Identifier.Parse(input));
        }
    }
}