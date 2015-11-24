namespace Core.Exceptions
{
    public class MissingSemicolonException : ParsingException
    {
        public MissingSemicolonException(string code, int position) : base(code, position)
        {
        }

        public override string Message => "Missing ';', ':' or '?'";
    }
}