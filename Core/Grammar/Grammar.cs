using System.Collections.Generic;
using System.Linq;

namespace Core
{
    public class Grammar
    {
        public GrammarNode Tree;

        private readonly List<GrammarNode> decicionsList;
        private GrammarNode current;

        public Grammar()
        {
            Tree = new GrammarNode(null);
            current = Tree;

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
            current.Type = GrammarNodeType.Optional;

            return this;
        }

        public Grammar OptionalEnd()
        {
            current = decicionsList.Last();
            decicionsList.RemoveAt(decicionsList.Count - 1);

            return this;
        }

        public Grammar Then(GrammarToken grammarToken)
        {
            if (current.Type == GrammarNodeType.Loop)
            {
                var node = new GrammarNode(current) {Token = grammarToken};
                current.ChildNodes.Add(node);

                return this;
            }

            if (current.Token != null || current.Type == GrammarNodeType.Optional)
            {
                current.ChildNodes.Add(new GrammarNode(current));
                current = current.ChildNodes.Last();
            }

            current.Token = grammarToken;

            return this;
        }

        public Grammar Loop()
        {
            current.ChildNodes.Add(new GrammarNode(current));
            current = current.ChildNodes.Last();
            current.Type = GrammarNodeType.Loop;

            return this;
        }

        public Grammar LoopEnd()
        {
            current = current.Parent;

            return this;
        }
    }
}