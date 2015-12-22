using System.Diagnostics;
using System.Linq;

namespace Core
{
    [DebuggerDisplay("{Description}")]
    public class GrammarToken
    {
        public readonly bool Optional;
        public readonly string[] Options;

        public readonly bool FixedWord;

        public readonly string Alphabet;
        public readonly string Description;

        public GrammarToken(string alphabet, string description = null, bool fixedWord = false)
        {
            description = description ?? $"'{alphabet}'";

            if (fixedWord) Options = new[] { alphabet };

            Alphabet = alphabet.ToLower() + alphabet.ToUpper();
            Description = description;
            FixedWord = fixedWord;
        }

        public GrammarToken(string[] options)
        {
            Optional = true;
            Options = options;

            foreach (var option in options)
            {
                Alphabet += option.ToLower() + option.ToUpper();
            }
        }

        public bool IsLegal(string token)
        {
            if (!FixedWord && !Optional) return true;

            if (FixedWord)
            {
                return token.ToLower() == Options[0].ToLower();
            }

            return Optional && Options.Any(option => option.ToLower() == token.ToLower());
        }
    }
}