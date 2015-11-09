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
        Semicolon,
        Colon,
        ConditionName,
        ConditionOpenBracket,
        ConditionArgument,
        ConditionComma,
        ConditionCloseBracket,
        Operator
    }
}