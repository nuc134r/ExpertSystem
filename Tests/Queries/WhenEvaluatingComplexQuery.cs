using System.Linq;
using Logikek;
using NUnit.Framework;

namespace Tests.Queries
{
    [TestFixture]
    public class WhenEvaluatingComplexQuery : TestBase
    {
        [Test]
        public void CorrectSolutionsFromPlainFacts()
        {
            var code = MakeLines(new[]
            {
                "Likes(Tim, Tennis);",
                "Likes(Tim, Football);"
            });

            Parser.Run(code);
            var result = Parser.EvaluateQuery("Likes(Tim, X)?");

            var solutions = result.Results.First().Solutions.ToArray();

            Assert.AreEqual(true, result.WasSuccessful);

            Assert.AreEqual("Tennis", solutions[0]["X"]);
            Assert.AreEqual("Football", solutions[1]["X"]);
        }

        [Test]
        public void CorrectSolutionFromRule()
        {
            var code = MakeLines(new[]
            {
                "Likes(Tim, Tennis);",
                "Likes(Tim, Football);"
            });

            Parser.Run(code);
            var result = Parser.EvaluateQuery("Likes(Tim, X)?");

            var solutions = result.Results.First().Solutions.ToArray();

            Assert.AreEqual(true, result.WasSuccessful);

            Assert.AreEqual("Tennis", solutions[0]["X"]);
            Assert.AreEqual("Football", solutions[1]["X"]);
        }
    }
}