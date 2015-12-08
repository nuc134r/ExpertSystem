using System;

namespace Core.Exceptions
{
    public class ParsingException : Exception
    {
        private string code;
        private int position;

        public ParsingException(string code, int line, int position)
        {
            Line = line;
            Code = code;
            Position = position;
        }

        public ParsingException(string code, int position)
        {
            this.code = code;
            this.position = position;
        }

        public int Line { get; set; }
        public string Code { get; }
        public int Position { get; }
    }
}