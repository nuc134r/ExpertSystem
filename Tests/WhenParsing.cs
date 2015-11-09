using Core;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class WhenParsing
    {
        [SetUp]
        public void Initialize()
        {
            parser = new Parser();
        }

        private Parser parser;

        [Test]
        public void WhitespaceIsDeleted()
        {
            const string code = "  Fluffy(X); \n\r  Skinny(X);  ";

            var result = parser.Do(code, new RunContext());

            Assert.AreEqual("Fluffy(X);Skinny(X);", result.OptimizedCode);
        }

        [Test]
        [Ignore]
        public void StopwatchWorks()
        {
            string code = "\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r"
                        + "Monster(X); \n\r\n\r\n\r\n\r\n\r\n\r"
                        + "\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r"
                        + "\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r"
                        + "\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r";

            for (int i = 0; i < 20; i++)
            {
                code += code;
            }

            var result = parser.Do(code, new RunContext());

            Assert.Pass($"Deleted some spaces in {result.ElapsedTime} ms");
        }
    }
}