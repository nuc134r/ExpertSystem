using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    [DebuggerDisplay("{Lexeme?.Description}, loop:{Looped}, opt:{Optional}, chld:{ChildNodes.Count}")]
    public class GrammarNode
    {
        public Lexeme Lexeme;
        public bool Looped;
        public bool Optional;

        public List<GrammarNode> ChildNodes;
        public GrammarNode Parent;

        public GrammarNode(GrammarNode parentNode)
        {
            ChildNodes = new List<GrammarNode>();
            Parent = parentNode;
        }
    }

    public class Grammar
    {
        public GrammarNode GrammarTree;

        private readonly List<GrammarNode> decicionsList;
        private GrammarNode current;

        public Grammar()
        {
            GrammarTree = new GrammarNode(null);
            current = GrammarTree;

            decicionsList = new List<GrammarNode>();
        }

        public Grammar Determine()
        {
            decicionsList.Add(current);

            return this;
        }

        public Grammar DetermineEnd()
        {
            current = decicionsList.Last();
            decicionsList.RemoveAt(decicionsList.Count - 1);

            return this;
        }

        public Grammar Way()
        {
            current.ChildNodes.Add(new GrammarNode(current));
            current = current.ChildNodes.Last();

            return this;
        }

        public Grammar WayEnd()
        {
            current = decicionsList.Last();

            return this;
        }

        public Grammar Optional()
        {
            decicionsList.Add(current);

            current.ChildNodes.Add(new GrammarNode(current));
            current = current.ChildNodes.Last();
            current.Optional = true;

            return this;
        }

        public Grammar OptionalEnd()
        {
            current = decicionsList.Last();
            decicionsList.RemoveAt(decicionsList.Count - 1);

            return this;
        }

        public Grammar Then(Lexeme lexeme)
        {
            if (current.Looped)
            {
                var node = new GrammarNode(current) {Lexeme = lexeme};
                current.ChildNodes.Add(node);

                return this;
            }

            if (current.Lexeme != null || current.Optional)
            {
                current.ChildNodes.Add(new GrammarNode(current));
                current = current.ChildNodes.Last();
            }

            current.Lexeme = lexeme;

            return this;
        }

        public Grammar Loop()
        {
            current.ChildNodes.Add(new GrammarNode(current));
            current = current.ChildNodes.Last();
            current.Looped = true;

            return this;
        }

        public Grammar LoopEnd()
        {
            current = current.Parent;

            return this;
        }
    }

    [DebuggerDisplay("{Description}")]
    public class Lexeme
    {
        public readonly bool Optional;
        public readonly string[] Options;

        public readonly bool Constant;

        public readonly string Alphabet;
        public readonly string Description;

        public Lexeme(string alphabet, string description = null, bool constant = false)
        {
            description = description ?? $"'{alphabet}'";

            Alphabet = alphabet;
            Description = description;
            Constant = constant;
        }

        public Lexeme(string[] options)
        {
            Optional = true;
            Options = options;
        }
    }
}
