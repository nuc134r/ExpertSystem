using System.Collections.Generic;
using Logikek.Language;
using Sprache;

namespace Logikek
{
    public class Grammar
    {
        public static readonly Parser<IEnumerable<char>> Whitespace =
            Parse.Char(' ').Many();

        public static readonly Parser<string> Identifier =
            Parse.Letter.AtLeastOnce().Text().Token();

        public static readonly Parser<IEnumerable<ClauseArgument>> Arguments =
            from openBracket in Parse.Char('(')
            from ws1 in Whitespace.Optional()
            from arguments in Identifier.XDelimitedBy(Parse.Char(','))
            from ws2 in Whitespace.Optional()
            from closeBracket in Parse.Char(')')
            select ClauseArgument.FromStringList(arguments);

        public static readonly Parser<string> Comment =
            from commentStart in Parse.String("//")
            from commentText in Parse.AnyChar.Many().Text().End()
            select commentText;

        public static readonly Parser<ConditionOperator> NotOperator =
            Parse.String("~")
                .Or(Parse.String("NOT"))
                .Or(Parse.String("НЕ"))
                .Return(ConditionOperator.Not);

        public static readonly Parser<SimpleCondition> RuleCondition =
            from notOperator in NotOperator.Optional()
            from ws1 in Whitespace
            from name in Identifier
            from ws2 in Whitespace.Optional()
            from arguments in Arguments
            select new SimpleCondition(name, arguments, notOperator.IsDefined);

        public static readonly Parser<ConditionOperator> AndOperator =
            Parse.String("&")
                .Or(Parse.String("AND"))
                .Or(Parse.String("И"))
                .Or(Parse.String(","))
                .Return(ConditionOperator.And);

        public static readonly Parser<ConditionOperator> OrOperator =
            Parse.String("|")
                .Or(Parse.String("OR"))
                .Or(Parse.String("ИЛИ"))
                .Return(ConditionOperator.Or);

        public static readonly Parser<ConditionOperator> LogicalOperator =
            OrOperator
                .Or(AndOperator);

        public static readonly Parser<ComplexCondition> NextRuleCondition =
            from ws1 in Whitespace.Optional()
            from _operator in LogicalOperator
            from ws2 in Whitespace.Optional()
            from condition in RuleCondition
            select new ComplexCondition(_operator, condition);

        public static readonly Parser<Rule> Rule =
            from name in Identifier
            from arguments in Arguments
            from ws1 in Whitespace.Optional()
            from colon in Parse.Char(':')
            from ws2 in Whitespace.Optional()
            from firstCondition in RuleCondition
            from nextConditions in NextRuleCondition.Many().Optional()
            from ws3 in Whitespace.Optional()
            from semicolon in Parse.Char(';').End()
            select new Rule(name, arguments, firstCondition, nextConditions.Get());

        public static readonly Parser<Fact> Fact =
            from identifier in Identifier
            from arguments in Arguments
            from ws1 in Whitespace.Optional()
            from semicolon in Parse.Char(';').End()
            select new Fact(identifier, arguments);

        public static readonly Parser<Query> Query =
            from identifier in Identifier
            from arguments in Arguments
            from ws1 in Whitespace.Optional()
            from semicolon in Parse.Char('?').End()
            select new Query(identifier, arguments);
    }
}