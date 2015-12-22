using System.Collections.Generic;
using System.Linq;

namespace Logikek.Language
{
    public class ClauseArgument
    {
        public ClauseArgument(string name)
        {
            Name = name;
        }

        public static IEnumerable<ClauseArgument> FromStrings(IEnumerable<string> names)
        {
            return names.Select(name => new ClauseArgument(name));
        }

        public string Name { get; }
        public bool IsAtom => (Name.Length == 1);
    }
}