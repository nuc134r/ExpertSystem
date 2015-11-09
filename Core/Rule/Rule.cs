namespace Core
{
    public class Rule
    {
        public Rule(string name)
        {
            Name = name;
        }

        public string Name { get; }
        public int Arguments { get; set; }
        public RuleCondition Condition { get; set; } 
    }
}