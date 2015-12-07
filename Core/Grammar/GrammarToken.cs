using System.Diagnostics;

namespace Core
{
    [DebuggerDisplay("{Description}")]
    public class GrammarToken
    {
        public readonly bool Optional;
        public readonly string[] Options;

        public readonly bool Constant;

        public readonly string Alphabet;
        public readonly string Description;

        public GrammarToken(string alphabet, string description = null, bool constant = false)
        {
            description = description ?? $"'{alphabet}'";

            Alphabet = alphabet;
            Description = description;
            Constant = constant;
        }

        public GrammarToken(string[] options)
        {
            Optional = true;
            Options = options;
        }
    }
}