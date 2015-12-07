using System.Collections.Generic;

namespace Core.Parsing
{
    public class ParseTreeNode
    {
        public List<string> TokenList;

        public ParseTreeNode()
        {
            TokenList = new List<string>();
        }
    }
}