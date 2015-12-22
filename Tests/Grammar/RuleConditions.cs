using System.Linq;
using NUnit.Framework;
using Sprache;

namespace Tests.Grammar
{
    [TestFixture]
    public class RuleConditions
    {
        [Test]
        public void RuleCondition()
        {
            const string input = "Likes(X)";

            var condition = Logikek.Grammar.RuleCondition.Parse(input);

            Assert.AreEqual(false, condition.IsNegated);
            Assert.AreEqual("Likes", condition.Name);
            Assert.AreEqual("X", condition.Arguments.ToArray()[0].Name);
        }

        [Test]
        public void NegatedRuleCondition()
        {
            const string input = "NOT Likes(X)";

            var condition = Logikek.Grammar.RuleCondition.Parse(input);

            Assert.AreEqual(true, condition.IsNegated);
            Assert.AreEqual("Likes", condition.Name);
            Assert.AreEqual("X", condition.Arguments.ToArray()[0].Name);
        }
    }
}