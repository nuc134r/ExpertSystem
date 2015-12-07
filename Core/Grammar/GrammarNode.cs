using System.Collections.Generic;
using System.Diagnostics;

namespace Core
{
    [DebuggerDisplay("{Type.ToString()}, {Token?.Description}, chld:{ChildNodes.Count}")]
    public class GrammarNode
    {
        public GrammarToken Token;
        public GrammarNodeType Type;

        public List<GrammarNode> ChildNodes;
        public GrammarNode Parent;

        public GrammarNode(GrammarNode parentNode)
        {
            ChildNodes = new List<GrammarNode>();
            Parent = parentNode;

            Type = GrammarNodeType.Simple;
        }
    }
}
