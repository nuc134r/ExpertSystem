using System.Linq;
using Logikek;
using NUnit.Framework;

namespace Tests.Queries
{
    [TestFixture]
    public class WhenEvaluatingSimpleQuery : TestBase
    {
        [Test]
        public void TrueIfFactExits()
        {
            var code = MakeLines(new[]
            {
                "Cat(Tim);"
            });

            Parser.Run(code);
            var result = Parser.EvaluateQuery("Cat(Tim)?");

            Assert.AreEqual(true, result.Results.First().Result);
        }

        [Test]
        public void FalseIfFactDoesNotExist()
        {
            var code = MakeLines(new[]
            {
                "Cat(Tim);"
            });

            Parser.Run(code);
            var result = Parser.EvaluateQuery("Cat(Max)?");

            Assert.AreEqual(false, result.Results.First().Result);
        }

        [Test]
        public void FalseIfFactHasWrongArgumentOrder()
        {
            var code = MakeLines(new[]
            {
                "Likes(Tim, Angela);"
            });

            Parser.Run(code);
            var result = Parser.EvaluateQuery("Likes(Angela, Tim)?");

            Assert.AreEqual(false, result.Results.First().Result);
        }

        [Test]
        public void TrueIfSuccessRule()
        {
            var code = MakeLines(new[]
            {
                "Cat(X) : Fluffy(X);",
                "Fluffy(Tom);"
            });

            Parser.Run(code);
            var result = Parser.EvaluateQuery("Cat(Tom)?");

            Assert.AreEqual(true, result.Results.First().Result);
        }

        [Test]
        public void TrueIfSuccessRuleWithTwoConditions()
        {
            var code = MakeLines(new[]
            {
                "Cat(X) : Fluffy(X) AND Animal(X);",
                "Fluffy(Tom);",
                "Animal(Tom);"
            });

            Parser.Run(code);
            var result = Parser.EvaluateQuery("Cat(Tom)?");

            Assert.AreEqual(true, result.Results.First().Result);
        }

        [Test]
        public void TrueIfSuccessRuleWithTwoArguments()
        {
            var code = MakeLines(new[]
            {
                "Friends(A, B) : Likes(A, B) AND Likes(B, A);",
                "Likes(Tim, Angela);",
                "Likes(Angela, Tim);"
            });

            Parser.Run(code);
            var result = Parser.EvaluateQuery("Friends(Tim, Angela)?");

            Assert.AreEqual(true, result.Results.First().Result);
        }

        [Test]
        public void TrueIfFoundOutInductive()
        {
            var code = MakeLines(new[]
            {
                "Friends(A, B) : Likes(A, B) AND Likes(B, A);",
                "Friends(Tim, Angela);"
            });

            Parser.Run(code);
            var result = Parser.EvaluateQuery("Likes(Tim, Angela)?");

            Assert.AreEqual(true, result.Results.First().Result);
        }

        [Test]
        public void FalseIfNotSure()
        {
            var code = MakeLines(new[]
            {
                "Friends(A, B) : Likes(A, B) AND Likes(B, A) OR Pals(A, B);",
                "Friends(Tim, Angela);"
            });

            Parser.Run(code);
            var result = Parser.EvaluateQuery("Likes(Tim, Angela)?");

            Assert.AreEqual(false, result.Results.First().Result);
        }
    }
}