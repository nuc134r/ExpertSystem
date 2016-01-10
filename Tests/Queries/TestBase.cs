namespace Tests.Queries
{
    public class TestBase
    {
        protected string MakeLines(string[] lines)
        {
            return string.Join("\n\r", lines);
        }
    }
}