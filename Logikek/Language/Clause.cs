using System.Collections.Generic;
using System.Linq;

namespace Logikek.Language
{
    public class Clause
    {
        public Clause(string name, IEnumerable<ClauseArgument> arguments)
        {
            Name = name;
            Arguments = arguments.ToList();
        }

        public string Name { get; private set; }
        public List<ClauseArgument> Arguments { get; private set; }
    }
}