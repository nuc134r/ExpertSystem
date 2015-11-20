using System;

namespace Core.Exceptions
{
    public class ParsingException : Exception
    {
        public ParsingException(string code, int position)
        {
            Code = code;
            Position = position;
        }

        public string Code { get; }
        public int Position { get; }
    }
}