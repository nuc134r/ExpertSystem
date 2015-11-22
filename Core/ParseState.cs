namespace Core
{
    public enum ParseState
    {
        Start,
        ClauseName,
        OpenBracket,
        Argument,
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