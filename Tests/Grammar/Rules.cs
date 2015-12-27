using System.Linq;
using NUnit.Framework;
using Logikek.Language;
using Sprache;

namespace Tests.Grammar
{
    [TestFixture]
    public class Rules
    {

        [Test]
        public void ParsingRule()
        {
            const string input = "Friends(X, Y) : Likes(X, Y) AND NOT Enemies(X, Y);";

            var rule = Logikek.Grammar.Rule.Parse(input);
            var firstCondition = rule.Conditions.ElementAt(0);
            var otherCondition = rule.Conditions.ElementAt(1);

            Assert.AreEqual("Friends", rule.Name);
            Assert.AreEqual("X", rule.Arguments.ToArray()[0].Name);
            Assert.AreEqual("Y", rule.Arguments.ToArray()[1].Name);
            Assert.AreEqual("Likes", firstCondition.Condition.Name);
            Assert.AreEqual(false, firstCondition.Condition.IsNegated);
            Assert.AreEqual("Enemies", otherCondition.Condition.Name);
            Assert.AreEqual(true, otherCondition.Condition.IsNegated);
        }

        [Test]
        public void ParsingRuleWithThreeConditions()
        {
            const string input = "Friends(X, Y) : Likes(X, Y) AND NOT Enemies(X, Y) OR Pals(X, Y);";

            var rule = Logikek.Grammar.Rule.Parse(input);
            var firstCondition = rule.Conditions.ElementAt(0);
            var secondCondition = rule.Conditions.ElementAt(1);
            var thirdCondition = rule.Conditions.ElementAt(2);

            Assert.AreEqual("Friends", rule.Name);
            Assert.AreEqual("X", rule.Arguments.ToArray()[0].Name);
            Assert.AreEqual("Y", rule.Arguments.ToArray()[1].Name);
            Assert.AreEqual("Likes", firstCondition.Condition.Name);
            Assert.AreEqual(false, firstCondition.Condition.IsNegated);
            Assert.AreEqual("Enemies", secondCondition.Condition.Name);
            Assert.AreEqual(true, secondCondition.Condition.IsNegated);
            Assert.AreEqual("Pals", thirdCondition.Condition.Name);
            Assert.AreEqual(false, thirdCondition.Condition.IsNegated);
            Assert.AreEqual(ConditionOperator.Or, thirdCondition.Operator);
        }

        [Test]
        public void ThrowsOnRuleMissingSemicolon()
        {
            const string input = "Friends(X, Y) : Likes(X, Y) AND NOT Enemies(X, Y)";

            Assert.Throws<ParseException>(() => { Logikek.Grammar.Rule.Parse(input); });
        }
    }
}