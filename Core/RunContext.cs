using System.Collections.Generic;
using Core.Language;

namespace Core
{
    public class RunContext
    {
        public RunContext()
        {
            Rules = new List<Rule>();
            Facts = new List<Fact>();
            Queries = new List<Query>();
        }

        public List<Rule> Rules;
        public List<Fact> Facts;
        public List<Query> Queries;
    }
}