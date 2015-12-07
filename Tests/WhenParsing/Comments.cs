using System.Linq;
using Core;
using Core.Exceptions;
using Core.Parsing;
using NUnit.Framework;

namespace Tests.WhenParsing
{
    [TestFixture]
    public class Comments
    {
        [Test]
        public void ThrowsOnUnclosedComment()
        {
            const string code = "/* comment ";

            var parser = new Parser(code);

            Assert.Throws<UnexpectedLineEndException>((() => { parser.Do(new RunContext()); }));
        }

        [Test]
        public void Ignores()
        {
            const string code = "/* comment */";

            var parser = new Parser(code);

            Assert.DoesNotThrow((() => { parser.Do(new RunContext()); }));
        }

        [Test]
        public void IgnoresAndParsesFact()
        {
            const string code = @"
            /* Here is a comment */
            /* Check out some code below */
            
            Human(Mark);

            /* Hope you enjoyed that */";

            var parser = new Parser(code);
            var context = new RunContext();

            parser.Do(context);

            var fact = context.Facts.First();

            Assert.AreEqual("Human", fact.Name);
            Assert.AreEqual("Mark", fact.Arguments.First().Name);
        }
    }
}