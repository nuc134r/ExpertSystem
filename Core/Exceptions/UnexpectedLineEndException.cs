using System;

namespace Core.Exceptions
{
    public class UnexpectedLineEndException : Exception
    {
        public override string Message => "Unexpected line end";
    }
}