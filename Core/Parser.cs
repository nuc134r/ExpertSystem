using System;
using System.Diagnostics;
using Core.Exceptions;
using static System.Char;

namespace Core
{
    public class Parser : IParser
    {
        public ParseResult Do(string code, RunContext context)
        {
            var stopwatch = Stopwatch.StartNew();
            var optimizedCode = PreprocessCode(code);

            Parse(optimizedCode, context);

            stopwatch.Stop();
            return new ParseResult(code, optimizedCode, stopwatch.ElapsedMilliseconds);
        }

        private static string PreprocessCode(string code)
        {
            code = code.RemoveAllEntriesOf("\n");
            code = code.RemoveAllEntriesOf("\r");
            code = code.RemoveAllEntriesOf(" ");

            return code;
        }

        private static void Parse(string code, RunContext context)
        {
            var temp = "";
            var position = 0;
            var state = ParseState.Name;

            Rule ruleTmp;
            //Fact factTmp;

            while (position != code.Length)
            {
                switch (state)
                {
                    case ParseState.Start:
                        if (IsLetterOrDigit(code[position]))
                        {
                            temp += code[position++];
                            state = ParseState.Name;
                            break;
                        }
                        throw new UnexpectedTokenException(code, position);
                    case ParseState.Name:
                        if (IsLetterOrDigit(code[position]))
                        {
                            temp += code[position++];
                            break;
                        }
                        if (code[position] == '(')
                        {
                            state = ParseState.OpenBracket;
                            position++;
                            break;
                        }
                        throw new UnexpectedTokenException(code, position);
                    case ParseState.OpenBracket:
                        ruleTmp = new Rule(name: temp);

                        position = code.Length; //Escaping 

                        break;
                    case ParseState.RuleArgumentName:
                        break;
                    case ParseState.RuleComma:
                        break;
                    case ParseState.CloseBracket:
                        break;
                    case ParseState.RuleSemicolon:
                        break;
                    case ParseState.FactSemicolon:
                        break;
                    case ParseState.Colon:
                        break;
                    case ParseState.ConditionName:
                        break;
                    case ParseState.ConditionOpenBracket:
                        break;
                    case ParseState.ConditionArgument:
                        break;
                    case ParseState.ConditionComma:
                        break;
                    case ParseState.ConditionCloseBracket:
                        break;
                    case ParseState.Operator:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private void ParseRule()
        {
        }

        private void ParseRuleCondition()
        {
        }
    }
}