namespace Core
{
    public enum ParseState
    {
        Start,
        ClauseName,
        OpenBracket,
        ArgumentName,
        Comma,
        CloseBracket,
        Colon,
        ConditionName,
        ConditionOpenBracket,
        ConditionArgument,
        ConditionComma,
        ConditionCloseBracket,
        Operator,
        QuestionMark
    }
}