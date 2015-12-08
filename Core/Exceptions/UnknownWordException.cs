namespace Core.Exceptions
{
    internal class UnknownWordException : ParsingException
    {
        private readonly string word;

        public UnknownWordException(string code, int position, string word) : base(code, position)
        {
            this.word = word;
        }

        public override string Message => $"Unknown word \"{word}\"";
    }
}