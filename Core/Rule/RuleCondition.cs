using System.Collections.Generic;

namespace Core
{
    public class RuleCondition
    {
        public bool IsNot;
    }

    public class SimpleCondition : RuleCondition
    {
        public SimpleCondition(string name)
        {
            Name = name;
        }

        public string Name { get; }
        public List<RuleArgument> Arguments { get; set; }
    }

    public class ComplexCondition : RuleCondition
    {
        public RuleCondition FirstCondition;
        public RuleCondition SecondCondition;
        public RuleOperator Operator;
    }
}