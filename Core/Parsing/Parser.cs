using System;
using System.Collections.Generic;
using System.ComponentModel;
using Core.Exceptions;
using static System.Char;

namespace Core.Parsing
{
    public class Parser
    {
        private readonly string[] lines;

        private int lineCounter;

        private readonly GrammarNode grmrRoot;

        private readonly GrammarNode grmrNode;
        private readonly List<ParseTreeNode> tree;

        public Parser(string[] lines, Grammar grammar)
        {
            this.lines = lines;

            grmrNode = grmrRoot = grammar.Tree;
            tree = new List<ParseTreeNode>();
        }

        public ParseResult Do()
        {
            var errors = new List<ParsingException>();
            lineCounter = 1;

            foreach (var line in lines)
            {
                try
                {
                    Parse(line);
                }
                catch (ParsingException ex)
                {
                    ex.Line = lineCounter;
                    errors.Add(ex);
                }
                finally
                {
                    lineCounter++;
                }
            }

            return new ParseResult(tree, errors);
        }

        private void Parse(string code)
        {
            code = code.Trim();

            var position = 0;
            var temp = "";
            var skippingWhiteSpace = false;

            var node = new ParseTreeNode();

            while (position != code.Length)
            {
                if (IsWhiteSpace(code[position]))
                {
                    skippingWhiteSpace = true;
                    position++;
                    continue;
                }

                if (skippingWhiteSpace)
                {
                    skippingWhiteSpace = false;
                    ChooseNextGrammarToken(code[position]);
                }

                if (InCurrentAlphabet(code[position]))
                {
                    temp += code[position++];
                }
                else
                {
                    if (!grmrNode.Token.IsLegal(temp))
                        throw new UnknownWordException(code, position, temp);

                    node.Tokens.Add(temp);
                    temp = "";
                    ChooseNextGrammarToken(code[position]);
                }
            }

            // If we're finished parsing not in a last grammar tree node
            if (grmrNode.ChildNodes.Count != 0)
                throw new UnexpectedLineEndException(code, position);
        }

        private void ChooseNextGrammarToken(char c)
        {
            
        }

        private bool InCurrentAlphabet(char c)
        {
            return grmrNode.Token.Alphabet.IndexOf(c) != -1;
        }
    }
}