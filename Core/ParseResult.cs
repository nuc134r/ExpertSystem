using System.Collections.Generic;

namespace Core
{
    public class ParseResult
    {
        public readonly string OptimizedCode;
        public readonly string OriginalCode;

        public List<Rule> Rules;
        //TODO public List<Fact> Facts;

        public ParseResult(string originalCode, string optimizedCode)
        {
            OriginalCode = originalCode;
            OptimizedCode = optimizedCode;
        }
    }
}