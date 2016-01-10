using System;

namespace Logikek.Language
{
    public enum ConditionOperator
    {
        And,
        Or,
        Not
    }

    public static class ConditionOperatorExtensionMethods
    {
        public static string GetKeyword(this ConditionOperator @operator)
        {
            switch (@operator)
            {
                case ConditionOperator.And:
                    return "»";
                case ConditionOperator.Or:
                    return "»À»";
                case ConditionOperator.Not:
                    return "Õ≈";
                default:
                    throw new ArgumentOutOfRangeException(@operator.ToString(), @operator, null);
            }
        }
    }
}