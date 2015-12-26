using System.Collections.Generic;
using System.Linq;

namespace Logikek.Language
{
    public class Rule : Clause
    {
        public Rule(string name, IEnumerable<ClauseArgument> arguments,
            SimpleCondition firstCondition, IEnumerable<ComplexCondition> conditions) : base(name, arguments)
        {
            this.firstCondition = firstCondition;
            otherConditions = conditions;
        }

        private readonly SimpleCondition firstCondition;
        private readonly IEnumerable<ComplexCondition> otherConditions;

        public IEnumerable<ComplexCondition> Conditions {
            get
            {
                var first = new List<ComplexCondition> {new ComplexCondition(null, firstCondition)};
                return first.Union(otherConditions);
            }
        }
    }
}