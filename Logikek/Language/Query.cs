using System.Collections.Generic;
using System.Linq;

namespace Logikek.Language
{
    public class Query : Clause
    {
        public Query(string name, IEnumerable<ClauseArgument> arguments) : base(name, arguments)
        {
        }

        public bool HasAtoms
        {
            get { return Arguments.Any(arg => arg.IsAtom); }
        }
    }
}