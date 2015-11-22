namespace Core.Exceptions
{
    public class UnexpectedTokenException : ParsingException
    {
        public UnexpectedTokenException(string code, int position) : base(code, position)
        {
        }

        public override string Message => $"Unexpected token '{Code[Position]}'";
    }
}