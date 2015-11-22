using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Core.Exceptions;
using Core.Interfaces;
using Core.Language;
using static System.Char;

namespace Core
{
    /*  
    **  Kitty(X) : Soft(X) and Warm(X);
    **  
    **  Soft(Tom);
    **  Warm(Tom);
    **
    **  Tail(Tom, TomsTail);
    **  
    **  Black(TomsTail);
    **  
    **  Kitty(Tom)?
    */

    public class Parser : IParser
    {
        private string code;
        private int position;
        private ParseState state = ParseState.ClauseName;

        private string temp = "";

        public ParseResult Do(string code, RunContext context)
        {
            this.code = code;
            var stopwatch = Stopwatch.StartNew();
            Parse(context);
            stopwatch.Stop();

            return new ParseResult(stopwatch.ElapsedMilliseconds);
        }

        private void Parse(RunContext context)
        {
            var name = "";
            var arguments = new List<ClauseArgument>();

            while (position != code.Length)
            {
                if (IsWhiteSpace(code[position]))
                {
                    position++;
                    continue;
                }

                switch (state)
                {
                    case ParseState.Start:
                        break;
                    case ParseState.ClauseName:
                        if (IsLetter(code[position]))
                        {
                            temp += code[position++];
                            break;
                        }
                        if (code[position] == '(')
                        {
                            state = ParseState.OpenBracket;
                            break;
                        }
                        throw new UnexpectedTokenException(code, position);
                    case ParseState.OpenBracket:
                        name = temp;
                        temp = "";
                        arguments = new List<ClauseArgument>();
                        state = ParseState.Argument;
                        position++;
                        break;
                    case ParseState.Argument:
                        if (IsLetter(code[position]))
                        {
                            temp += code[position++];
                            break;
                        }
                        if (code[position] == ')')
                        {
                            if (temp == "") throw new ArgumentNameExpectedException(code, position);
                            state = ParseState.CloseBracket;
                            break;
                        }
                        if (code[position] == ',')
                        {
                            if (temp == "") throw new ArgumentNameExpectedException(code, position);
                            state = ParseState.Comma;
                            break;
                        }
                        throw new UnexpectedTokenException(code, position);
                    case ParseState.Comma:
                        arguments.Add(new ClauseArgument(temp));
                        temp = "";
                        state = ParseState.Argument;
                        position++;
                        break;
                    case ParseState.CloseBracket:
                        arguments.Add(new ClauseArgument(temp));
                        temp = "";
                        state = ParseState.Colon;
                        position++;
                        break;
                    case ParseState.Colon:
                        if (code[position] == ';')
                        {
                            if (arguments.Any(arg => arg.IsAtom)) throw new FactAtomException(code, position);
                            var fact = new Fact(name) {Arguments = arguments};
                            context.Facts.Add(fact);
                            state = ParseState.ClauseName;
                            position++;
                            break;
                        }
                        if (code[position] == ':')
                        {
                            var rule = new Rule(name) {Arguments = arguments};
                            context.Rules.Add(rule);
                            state = ParseState.ConditionName;
                            position++;
                            break;
                        }
                        throw new MissingSemicolonException(code, position);
                    case ParseState.ConditionName:
                        if (IsLetter(code[position]))
                        {
                            temp += code[position++];
                            break;
                        }
                        if (code[position] == '(')
                        {
                            state = ParseState.ConditionOpenBracket;
                            break;
                        }
                        throw new UnexpectedTokenException(code, position);
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
            }

            if (code.Length == 0) return;

            CheckFinishState();
        }

        private void CheckFinishState()
        {
            switch (state)
            {
                case ParseState.Colon:
                    throw new MissingSemicolonException(code, position);
                case ParseState.ClauseName:
                    if (temp != "")
                        throw new UnexpectedLineEndException(code, position);
                    break;
                default:
                    throw new UnexpectedLineEndException(code, position);
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