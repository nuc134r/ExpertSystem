namespace Logikek.Language
{
    public class Rule : Clause
    {
        public Rule(string name) : base(name)
        {
        }

        public RuleCondition Condition { get; set; }
    }
}