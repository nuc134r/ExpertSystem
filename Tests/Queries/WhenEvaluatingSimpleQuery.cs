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
    }
}