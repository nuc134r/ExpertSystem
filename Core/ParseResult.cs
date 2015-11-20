namespace Core
{
    public class ParseResult
    {
        public readonly double ElapsedTime;
        public readonly string OriginalCode;

        public ParseResult(string originalCode, double elapsedTime)
        {
            OriginalCode = originalCode;
            ElapsedTime = elapsedTime;
        }
    }
}