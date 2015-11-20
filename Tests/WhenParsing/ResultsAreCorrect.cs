using System.Linq;
using Core;
using Core.Exceptions;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class ResultsAreCorrect
    {
        [SetUp]
        public void Initialize()
        {
            parser = new Parser();
        }

        private Parser parser;

        [Test]
        public void DoesNotThrowOnEmptyLine()
        {
            const string code = "";

            Assert.DoesNotThrow(() => { parser.Do(code, new RunContext()); });
        }
        
        //[Test]
        //public void SuccessfullyParsesFact()
        //{
        //    const string code = "Human(Alex, X);";

        //    var context = new RunContext();
        //    parser.Do(code, context);

        //    var fact = context.Facts.First();

        //    Assert.AreEqual("Human", fact.Name);
        //    Assert.AreEqual("Alex", fact.Arguments);
        //    Assert.AreEqual("Human", fact.Name);
        //}

        
    }
}