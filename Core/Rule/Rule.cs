namespace Core
{
    public class Rule
    {
        public Rule(string name, int arguments)
        {
            Name = name;
            Arguments = arguments;
        }

        public string Name { get; }
        public int Arguments { get; }
        public RuleCondition Condition { get; set; } 
    }
}