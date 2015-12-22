using System.Collections.Generic;
using Sprache;

namespace Logikek
{
    public class Grammar
    {
        public static readonly Parser<string> Identifier = 
            Parse.Letter.AtLeastOnce().Text().Token();

        public static readonly Parser<IEnumerable<Language.ClauseArgument>> Arguments =
            from openBracket in Parse.Char('(')
            from arguments in Identifier.XDelimitedBy(Parse.Char(','))
            from closeBracket in Parse.Char(')')
            select Language.ClauseArgument.FromStrings(arguments);

        public static readonly Parser<Language.Fact> Fact =
            from identifier in Identifier
            from arguments in Arguments
            from semicolon in Parse.Char(';')
            select new Language.Fact(identifier, arguments);
    }
}