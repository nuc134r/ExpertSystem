using System.Collections.Generic;

namespace Logikek.Language
{
    public class Query : Clause
    {
        public Query(string name, IEnumerable<ClauseArgument> arguments) : base(name, arguments)
        {
        }
    }
}