using System;
using System.Diagnostics;
using static System.Char;

namespace Core
{
    public class Parser : IParser
    {
        private string _code;
        private RunContext _context;

        public ParseResult Do(string code, RunContext context)
        {
            var stopwatch = Stopwatch.StartNew();
            var optimizedCode = PreprocessCode(code);
            _context = context;
            _code = optimizedCode;

            // TODO Surprisingly parsing 

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

        private void Parse()
        {
            var temp = "";
            var position = 0;
            var state = ParseState.Start;

            Rule ruleTmp;
            //Fact factTmp;

            while (position != _code.Length)
            {
                switch (state)
                {
                    case ParseState.Start:
                        temp += _code[position];
                        position++;
                        state = ParseState.Name;
                        break;
                    case ParseState.Name:
                        if (IsLetterOrDigit(_code[position]))
                        {
                            temp += _code[position];
                            position++;
                        }
                        else if (_code[position] == '(')
                        {
                            state = ParseState.OpenBracket;
                            position++;
                        }
                        break;
                    case ParseState.OpenBracket:
                        ruleTmp = new Rule(name: temp);
                        break;
                    case ParseState.RuleArgumentName:
                        break;
                    case ParseState.RuleComma:
                        break;
                    case ParseState.CloseBracket:
                        break;
                    case ParseState.Semicolon:
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