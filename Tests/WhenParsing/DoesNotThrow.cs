using Core;
using NUnit.Framework;

namespace Tests.WhenParsing
{
    [TestFixture]
    public class DoesNotThrow
    {
        [Test]
        public void DoesNotThrowOnEmptyLine()
        {
            const string code = "";

            var parser = new Parser(code);
 
            Assert.DoesNotThrow(() => { parser.Do(new RunContext()); });
        }
    }
}