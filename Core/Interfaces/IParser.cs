namespace Core.Interfaces
{
    public interface IParser
    {
        ParseResult Do(string code, RunContext context);
    }
}