using System.Collections.Generic;

namespace Core
{
    public class ParseResult
    {
        public readonly double ElapsedTime;
        public readonly string OptimizedCode;
        public readonly string OriginalCode;

        public ParseResult(string originalCode, string optimizedCode, double elapsedTime)
        {
            OriginalCode = originalCode;
            OptimizedCode = optimizedCode;
            ElapsedTime = elapsedTime;
        }
    }
}