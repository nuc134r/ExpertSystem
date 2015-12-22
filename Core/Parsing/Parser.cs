using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Core.Exceptions;
using static System.Char;

namespace Core.Parsing
{
    public class Parser
    {
        /*private readonly string[] lines;

        private int lineCounter;

        private readonly GrammarNode grmrRoot;

        private GrammarNode grmrNode;
        private readonly List<ParseTreeNode> tree;

        private ParseState state = ParseState.Default;
        private List<GrammarNode> decicionPoints;

        public Parser(string[] lines, Grammar grammar)
        {
            this.lines = lines;

            grmrNode = grmrRoot = grammar.Tree;
            tree = new List<ParseTreeNode>();
            decicionPoints = new List<GrammarNode>();
        }

        public ParseResult Do()
        {
            var errors = new List<ParsingException>();
            lineCounter = 1;

            foreach (var line in lines)
            {
                try
                {
                    code = line.Trim();
                    Parse();
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

        private int position = 0;
        private string temp = "";
        private bool skippingWhiteSpace = false;
        private string code;

        private ParseTreeNode node = new ParseTreeNode();

        private void Parse()
        {
            while (position != code.Length)
            {
                if (IsWhiteSpace(code[position]))
                {
                    if (!skippingWhiteSpace)
                    {
                        if (!grmrNode.Token.IsLegal(temp))
                            throw new UnknownWordException(code, position, temp);

                        node.Tokens.Add(temp);
                        temp = "";
                        ChooseNextGrammarToken(code[position]);
                    }

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

            // If we're finished parsing not in last grammar tree node
            if (grmrNode.ChildNodes.Count != 0)
                throw new UnexpectedLineEndException(code, position);
        }

        private void ChooseNextGrammarToken(char c)
        {
            switch (state)
            {
                case ParseState.Default:
                    if (grmrNode.ChildNodes.Count == 0)
                    {
                        grmrNode = grmrRoot;
                    }
                    if (grmrNode.ChildNodes.Count == 1)
                    {
                        grmrNode = grmrNode.ChildNodes.FirstOrDefault();
                    }
                    if (grmrNode != null && grmrNode.ChildNodes.Count > 1)
                    {
                        grmrNode = grmrNode.ChildNodes.First(_ => _.Token.IsLegal(c.ToString()));
                        if (grmrNode == null)
                        {
                            throw new UnexpectedTokenException(code, position);
                        }
                    }
                    break;
                case ParseState.Optional:
                    break;
                case ParseState.Loop:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (grmrNode == null) return;

            switch (grmrNode.Type)
            {
                case GrammarNodeType.Simple:
                    state = ParseState.Default;
                    break;
                case GrammarNodeType.Optional:
                    state = ParseState.Optional;
                    decicionPoints.Add();
                    break;
                case GrammarNodeType.Loop:
                    state = ParseState.Loop;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private bool InCurrentAlphabet(char c)
        {
            return grmrNode.Token.Alphabet.IndexOf(c) != -1;
        }*/
    }

    internal enum ParseState
    {
        Default,
        Optional,
        Loop
    }
}