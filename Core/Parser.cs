using System;

namespace Core
{
    public class Parser : IParser
    {
        public ParseResult Do(string code)
        {
            var optimizedCode = PreprocessCode(code);

            var result = new ParseResult(code, optimizedCode);

            // ...

            return result;
        }

        private string PreprocessCode(string code)
        {
            code = code.RemoveAllEntriesOf("\n");
            code = code.RemoveAllEntriesOf("\r");
            code = code.RemoveAllEntriesOf(" ");

            return code;
        }
    }
}