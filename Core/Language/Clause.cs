using System.Collections.Generic;

namespace Core.Language
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