using System.Diagnostics;

namespace Core
{
    public class Parser : IParser
    {
        private RunContext _context;
        private string _code;
        private int position = 0;

        public ParseResult Do(string code, RunContext context)
        {
            var stopwatch = Stopwatch.StartNew();
            var optimizedCode = PreprocessCode(code);
            _context = context;
            _code = optimizedCode;

            // TODO Surprisingly parsing 

            stopwatch.Stop();
            return new ParseResult(code, optimizedCode, stopwatch.Elapsed.TotalMilliseconds);
        }

        private static string PreprocessCode(string code)
        {
            code = code.RemoveAllEntriesOf("\n");
            code = code.RemoveAllEntriesOf("\r");
            code = code.RemoveAllEntriesOf(" ");

            return code;
        }

        private void Parse()
        {
            
        }

        private void ParseRule()
        {

        }

        private void ParseRuleCondition()
        {

        }
    }
}