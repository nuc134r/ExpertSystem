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

        CommentBeginSlash,
        CommentBeginStar,
        Comment,
        CommentEndStar,
        CommentEndSlash
    }
}