namespace Core
{
    public class RuleArgument
    {
        public RuleArgument(string name, bool isAtom)
        {
            Name = name;
            IsAtom = isAtom;
        }

        public string Name { get; }
        public bool IsAtom { get; }
    }
}