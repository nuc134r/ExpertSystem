namespace Core
{
    public enum ParseState
    {
        Start,
        Name,
        OpenBracket,
        ArgumentName,
        Comma,
        CloseBracket,
        Semicolon,
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