using System.Collections.Generic;
using System.Linq;

namespace Logikek.Language
{
    public class Rule : Clause
    {
        public Rule(string name, IEnumerable<ClauseArgument> arguments,
            SimpleCondition firstCondition, IEnumerable<ComplexCondition> conditions) : base(name, arguments)
        {
            FirstCondition = firstCondition;
            Conditions = conditions;
        }

        public SimpleCondition FirstCondition { get; private set; }
        public IEnumerable<ComplexCondition> Conditions { get; private set; }
    }
}