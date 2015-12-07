using System.Collections.Generic;

namespace Core.Parsing
{
    public class Parser
    {
        private int line;
        private int position;

        private readonly string[] code;

        private string currLine;

        private GrammarNode grammarNode;
        private List<ParseTreeNode> parseTree;

        public Parser(string[] code, Grammar grammar)
        {
            this.code = code;
            currLine = code.Length > 0 ? code[0] : null;

            grammarNode = grammar.Tree;
            parseTree = new List<ParseTreeNode>();
        }

        public ParseTreeNode Do()
        {
            var tree = new ParseTreeNode();

            return tree;
        }
    }
}
