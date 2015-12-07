namespace Logikek.Language
{
    public class ClauseArgument
    {
        public ClauseArgument(string name)
        {
            Name = name;
        }

        public string Name { get; }
        public bool IsAtom => (Name.Length == 1);
    }
}