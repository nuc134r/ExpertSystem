using System;

namespace Core.Exceptions
{
    public class UnexpectedTokenException : Exception
    {
        private readonly string code;
        private readonly int position;

        public UnexpectedTokenException(string code, int position)
        {
            this.code = code;
            this.position = position;
        }

        public override string Message => $"Unexpected token '{code[position]}' at {position}";
    }
}