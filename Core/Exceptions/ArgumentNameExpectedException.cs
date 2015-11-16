using System;

namespace Core.Exceptions
{
    public class ArgumentNameExpectedException : Exception
    {
        public override string Message => "Argument name expected";
    }
}