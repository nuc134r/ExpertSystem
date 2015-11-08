namespace Core
{
    public interface IParser
    {
        ParseResult Do(string code, RunContext context);
    }
}