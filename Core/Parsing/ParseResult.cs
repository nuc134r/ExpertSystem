using System.Collections.Generic;
using Core.Exceptions;

namespace Core.Parsing
{
    public class ParseResult
    {
        public List<ParseTreeNode> ParseTree { get; }
        public List<ParsingException> Errors { get; }

        public ParseResult(List<ParseTreeNode> parseTree, List<ParsingException> errors)
        {
            ParseTree = parseTree;
            Errors = errors;
        }
    }
}