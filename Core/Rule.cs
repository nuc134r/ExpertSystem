namespace Core
{
    public class Rule
    {
        public string Name { get; }
        public int Arguments { get; }

        public Rule(string name, int arguments)
        {
            Name = name;
            Arguments = arguments;
        }
    }
}