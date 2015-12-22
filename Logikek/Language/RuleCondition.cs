using System.Collections.Generic;
using System.Linq;

namespace Logikek.Language
{
    public class RuleCondition
    {
    }

    public class SimpleCondition : RuleCondition
    {
        public SimpleCondition(string name, IEnumerable<ClauseArgument> arguments, bool isNegated)
        {
            Name = name;
            Arguments = arguments.ToList();
            IsNegated = isNegated;
        }

        public bool IsNegated { get; set; }
        public string Name { get; private set; }
        public List<ClauseArgument> Arguments { get; private set; }
    }

    public class ComplexCondition : RuleCondition
    {
        public ComplexCondition(ConditionOperator _operator, SimpleCondition condition)
        {
            Operator = _operator;
            Condition = condition;
        }

        public ConditionOperator Operator { get; private set; }
        public SimpleCondition Condition { get; private set; }
    }
}