using System;
using System.Collections.Generic;
using System.ComponentModel;
using Core.Exceptions;

namespace Core.Parsing
{
    public class Parser
    {
        private readonly string[] code;

        private int lineCounter;

        private readonly GrammarNode grammarRoot;

        private readonly GrammarNode grammarNode;
        private readonly List<ParseTreeNode> tree;

        public Parser(string[] code, Grammar grammar)
        {
            this.code = code;

            grammarNode = grammar.Tree;
            grammarRoot = grammar.Tree;
            tree = new List<ParseTreeNode>();
        }

        //private string currLine
        //{
        //    get { return code[line]; }
        //    set { code[line] = value; }
        //}

        public ParseResult Do()
        {
            var errors = new List<ParsingException>();

            foreach (var line in code)
            {
                try
                {
                    Parse(line, lineCounter);
                }
                catch (ParsingException ex)
                {
                    errors.Add(ex);
                }
                finally
                {
                    lineCounter++;
                }
            }

            return new ParseResult(tree, errors);
        }

        private void Parse(string code, int line)
        {
            var position = 0;

            var node = new ParseTreeNode();
        }
    }
}