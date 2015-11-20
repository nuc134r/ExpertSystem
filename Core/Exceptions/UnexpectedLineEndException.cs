namespace Core.Exceptions
{
    public class UnexpectedLineEndException : ParsingException
    {
        public UnexpectedLineEndException(string code, int position) : base(code, position)
        {
        }

        public override string Message => "Unexpected line end";
    }
}