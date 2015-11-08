using System.Collections.Generic;

namespace Core
{
    public class ParseResult
    {
        public readonly string OriginalCode;
        public readonly string OptimizedCode;

        public ParseResult(string originalCode, string optimizedCode)
        {
            OriginalCode = originalCode;
            OptimizedCode = optimizedCode;
        }

        public List<Rule> Rules;
        //TODO public List<Fact> Facts;
    }
}