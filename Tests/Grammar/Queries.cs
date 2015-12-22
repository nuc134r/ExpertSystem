using System.Linq;
using NUnit.Framework;
using Sprache;

namespace Tests.Grammar
{
    [TestFixture]
    public class Queries
    {

        [Test]
        public void ParsingQuery()
        {
            const string input = "Friends(Alex, Jennifer)?";

            var query = Logikek.Grammar.Query.Parse(input);

            Assert.AreEqual("Friends", query.Name);
            Assert.AreEqual("Alex", query.Arguments.ToArray()[0].Name);
            Assert.AreEqual("Jennifer", query.Arguments.ToArray()[1].Name);
        }

        [Test]
        public void ThrowsOnQueryMissingQuestionMark()
        {
            const string input = "Friends(Alex, Jennifer)";

            var ex = Assert.Throws<ParseException>(() => { Logikek.Grammar.Query.Parse(input); });
        }
    }
}