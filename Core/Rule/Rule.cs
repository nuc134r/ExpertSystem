using System.Collections.Generic;

namespace Core
{
    public class Rule
    {
        public Rule(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }
        public List<RuleArgument> Arguments { get; set; }
        public RuleCondition Condition { get; set; }
    }
}