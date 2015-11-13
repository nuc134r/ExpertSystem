namespace Core
{
    public enum ParseState
    {
        Start,
        Name,
        OpenBracket,
        RuleArgumentName,
        RuleComma,
        CloseBracket,
        RuleSemicolon,
        FactSemicolon,
        Colon,
        ConditionName,
        ConditionOpenBracket,
        ConditionArgument,
        ConditionComma,
        ConditionCloseBracket,
        Operator
    }
}