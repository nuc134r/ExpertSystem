using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Core.Exceptions;
using Core.Interfaces;
using static System.Char;

namespace Core
{
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

            return new ParseResult(code, stopwatch.ElapsedMilliseconds);
        }

        private void Parse(RunContext context)
        {
            var clauseName = "";
            var clauseArguments = new List<string>();

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
                        clauseName = temp;
                        temp = "";
                        position++;
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
                        clauseArguments.Add(temp);
                        temp = "";
                        position++;
                        state = ParseState.ArgumentName;
                        break;
                    case ParseState.CloseBracket:
                        position++;
                        state = ParseState.Colon;
                        break;
                    case ParseState.Colon:
                        if (code[position] == ';')
                        {
                            //var fact = new Fact(clauseName);
                            if (clauseArguments.Any(_ => _.Length == 1))
                                throw new FactAtomException(code, position);
                            position++;
                            state = ParseState.ClauseName;
                            break;
                        }
                        if (code[position] == ':')
                        {
                            var rule = new Rule(clauseName);
                            clauseArguments.ForEach(arg => rule.Arguments.Add(new RuleArgument(arg)));
                            context.Rules.Add(rule);

                            position++;
                            state = ParseState.ConditionName;
                            break;
                        }
                        throw new MissingSemicolonException(code, position);
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