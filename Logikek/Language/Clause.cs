using System.Collections.Generic;

namespace Logikek.Language
{
    public class Clause
    {
        public Clause(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }
        public List<ClauseArgument> Arguments { get; set; }
    }
}