using System.Linq;
using Logikek.Language;
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
        public void Comment()
        {
            const string input = "/* Sample text */";

            var comment = Logikek.Grammar.Comment.Parse(input);

            Assert.AreEqual(" Sample text ", comment);
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

        [Test]
        public void EqualArguments()
        {
            var fact = new Fact("Friends", new[] {new ClauseArgument("Alex"), new ClauseArgument("Jane")});

            var query1 = new Fact("Friends", new[] {new ClauseArgument("Alex"), new ClauseArgument("Jane")});
            var query2 = new Fact("Friends", new[] {new ClauseArgument("Jane"), new ClauseArgument("Alex")});

            var result1 = query1.Arguments.SequenceEqual(fact.Arguments);
            var result2 = query2.Arguments.SequenceEqual(fact.Arguments);

            Assert.AreEqual(true, result1);
            Assert.AreEqual(false, result2);
        }
    }
}