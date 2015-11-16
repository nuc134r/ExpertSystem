using System;
using System.Diagnostics;
using Core.Interfaces;
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

        /*  
        **  Kitty(X) : Soft(X) and Warm(X);
        **  
        **  Soft(Tom);
        **  Warm(Tom);
        **
        **  Black(Tail, Tom);
        **  
        **  Kitty(Tom)?
        */

        private static void Parse(string code, RunContext context)
        {
            var temp = "";
            var position = 0;
            var state = ParseState.Name;

            while (position != code.Length)
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
                        if (IsLetter(code[position]))
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
                        var rule = new Rule(name: temp);
                        context.Rules.Add(rule);
                        temp = "";
                        state = ParseState.ArgumentName;
                        break;
                    case ParseState.ArgumentName:
                        if (IsLetter(code[position]))
                        {
                            temp += code[position++];
                            break;
                        }
                        if (code[position] == ')')
                        {
                            if (temp == "") throw new ArgumentNameExpectedException();
                            state = ParseState.CloseBracket;
                            position++;
                            break;
                        }
                        if (code[position] == ',')
                        {
                            if (temp == "") throw new ArgumentNameExpectedException();
                            state = ParseState.Comma;
                            position++;
                            break;
                        }
                        throw new UnexpectedTokenException(code, position);
                    case ParseState.Comma:
                        position++;
                        break;
                    case ParseState.CloseBracket:
                        position++;
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
                    case ParseState.QuestionMark:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            if (state != ParseState.Semicolon || state != ParseState.QuestionMark) throw new UnexpectedLineEndException();
        }

        private void ParseRule()
        {
        }

        private void ParseRuleCondition()
        {
        }
    }
}