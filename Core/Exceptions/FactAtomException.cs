namespace Core.Exceptions
{
    public class FactAtomException : ParsingException
    {
        public FactAtomException(string code, int position) : base(code, position)
        {
        }

        public override string Message => "Fact argument can not be atom";
    }
}