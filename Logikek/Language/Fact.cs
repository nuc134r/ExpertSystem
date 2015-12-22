using System.Collections.Generic;

namespace Logikek.Language
{
    public class Fact : Clause
    {
        public Fact(string name, IEnumerable<ClauseArgument> arguments) : base(name, arguments)
        {
        }
    }
}