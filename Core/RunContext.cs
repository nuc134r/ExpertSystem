using System.Collections.Generic;

namespace Core
{
    public class RunContext
    {
        public RunContext()
        {
            Rules = new List<Rule>();
            //Facts = new List<Fact>();
        }

        public List<Rule> Rules;
        //TODO public List<Fact> Facts;
    }
}