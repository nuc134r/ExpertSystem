using System.Linq;
using NUnit.Framework;
using Sprache;

namespace Tests.Grammar
{
    [TestFixture]
    public class General
    {
        [Test]
        public void AnIdentifierIsASequenceOfCharacters()
        {
            const string input = "   Human  ()";

            var id = Logikek.Grammar.Identifier.Parse(input);

            Assert.AreEqual("Human", id);
        }

        [Test]
        public void AnIdentifierCannotStartWithQuote()
        {
            const string input = "   \"Human  ()";

            Assert.Throws<ParseException>(() => Logikek.Grammar.Identifier.Parse(input));
        }

        [Test]
        public void ArgumentsAreBetweenBracketsAndDelimitedByCommas()
        {
            const string input = "(Minded, Alive)";

            var arguments = Logikek.Grammar.Arguments.Parse(input);

            Assert.AreEqual("Minded", arguments.ToArray()[0].Name);
            Assert.AreEqual("Alive", arguments.ToArray()[1].Name);
        }

        [Test]
        public void FactIsAnIdentifierWithArgumentsAndSemicolon()
        {
            const string input = "Human(Minded, Alive);";

            var fact = Logikek.Grammar.Fact.Parse(input);
            
            Assert.AreEqual("Human", fact.Name);
            Assert.AreEqual("Minded", fact.Arguments.ToArray()[0].Name);
            Assert.AreEqual("Alive", fact.Arguments.ToArray()[1].Name);
        }

        [Test]
        public void FactParserFailsParsingRule()
        {
            const string input = "Human(X) : Minded(X) And Alive(X)";

            Assert.Throws<ParseException>(() => Logikek.Grammar.Fact.Parse(input));
        }
    }
}