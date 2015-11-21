using System.Collections.Generic;

namespace Core.Language
{
    public class RuleCondition
    {
        public bool IsNegated { get; set; }
    }

    public class SimpleCondition : RuleCondition
    {
        public SimpleCondition(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }
        public List<ClauseArgument> Arguments { get; set; }
    }

    public class ComplexCondition : RuleCondition
    {
        public RuleCondition FirstCondition;
        public RuleCondition SecondCondition;
        public RuleOperator Operator;
    }
}