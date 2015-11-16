using System;

namespace Core.Exceptions
{
    public class UnexpectedTokenException : Exception
    {
        public UnexpectedTokenException(string code, int position)
        {
            Code = code;
            Position = position;
        }

        public string Code { get; }
        public int Position { get; }

        public override string Message => $"Unexpected token '{Code[Position]}' at {Position}";
    }
}