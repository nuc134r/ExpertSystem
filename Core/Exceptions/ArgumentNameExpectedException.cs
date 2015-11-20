namespace Core.Exceptions
{
    public class ArgumentNameExpectedException : ParsingException
    {
        public ArgumentNameExpectedException(string code, int position) : base(code, position)
        {
        }

        public override string Message => "Argument name expected";
    }
}