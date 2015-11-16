namespace Core
{
    public class RuleArgument
    {
        public RuleArgument(string name)
        {
            Name = name;
            IsAtom = (name.Length == 1);
        }

        public string Name { get; }
        public bool IsAtom { get; }
    }
}