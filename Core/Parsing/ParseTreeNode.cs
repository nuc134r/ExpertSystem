using System.Collections.Generic;

namespace Core.Parsing
{
    public class ParseTreeNode
    {
        public List<string> Tokens;

        public ParseTreeNode()
        {
            Tokens = new List<string>();
        }
    }
}