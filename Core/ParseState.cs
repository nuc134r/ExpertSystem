namespace Core
{
    public enum ParseState
    {
        Beginning,

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

        CommentBeginning,
        Comment,
        CommentEnding
    }
}