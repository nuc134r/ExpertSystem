using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Core.Exceptions;
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

    public class Parser
    {
        private readonly string code;
        private int position;

        private string name = "";
        private string temp = "";

        private List<ClauseArgument> arguments;
        private ParseState state = ParseState.Beginning;

        public Parser(string code)
        {
            this.code = code;
        }

        public ParseResult Do(RunContext context)
        {
            var stopwatch = Stopwatch.StartNew();

            Parse(context);

            stopwatch.Stop();

            return new ParseResult(stopwatch.ElapsedMilliseconds);
        }

        private void Parse(RunContext context)
        {
            if (code.Trim() == "") return;

            while (position != code.Length)
            {
                if (IsWhiteSpace(code[position]))
                {
                    position++;
                    continue;
                }

                switch (state)
                {
                    case ParseState.Beginning:
                        if (ProcessLetter())
                        {
                            state = ParseState.ClauseName;
                            break;
                        }
                        if (ProcessSymbol('/'))
                        {
                            state = ParseState.CommentBeginning;
                            break;
                        }
                        throw new UnexpectedTokenException(code, position);
                    case ParseState.ClauseName:
                        if (ProcessLetter()) break;
                        if (ProcessSymbol('('))
                        {
                            state = ParseState.OpenBracket;
                            break;
                        }
                        throw new UnexpectedTokenException(code, position);
                    case ParseState.OpenBracket:
                        PrepareForArguments();
                        state = ParseState.Argument;
                        break;
                    case ParseState.Argument:
                        if (ProcessLetter()) break;
                        if (ProcessSymbol(')'))
                        {
                            CheckForEmptyArgument();
                            state = ParseState.CloseBracket;
                            break;
                        }
                        if (ProcessSymbol(','))
                        {
                            CheckForEmptyArgument();
                            state = ParseState.Comma;
                            break;
                        }
                        throw new UnexpectedTokenException(code, position);
                    case ParseState.Comma:
                        AddArgumentAndChangeStateTo(ParseState.Argument);
                        break;
                    case ParseState.CloseBracket:
                        AddArgumentAndChangeStateTo(ParseState.Colon);
                        break;
                    case ParseState.Colon:
                        if (ProcessSymbol(';'))
                        {
                            CheckForAtoms();
                            var fact = new Fact(name) { Arguments = arguments };
                            context.Facts.Add(fact);
                            state = ParseState.Beginning;
                            break;
                        }
                        if (ProcessSymbol(':'))
                        {
                            var rule = new Rule(name) {Arguments = arguments};
                            context.Rules.Add(rule);
                            state = ParseState.ConditionName;
                            break;
                        }
                        if (ProcessSymbol('?'))
                        {
                            var query = new Query(name) {Arguments = arguments};
                            context.Queries.Add(query);
                            state = ParseState.Beginning;
                            break;
                        }
                        throw new MissingSemicolonException(code, position);
                    case ParseState.ConditionName:
                        if (ProcessLetter()) break;
                        if (ProcessSymbol('('))
                        {
                            state = ParseState.ConditionOpenBracket;
                            break;
                        }
                        throw new UnexpectedTokenException(code, position);
                    case ParseState.ConditionOpenBracket:
                        PrepareForArguments();
                        state = ParseState.ConditionArgument;
                        break;
                    case ParseState.ConditionArgument:
                        if (ProcessLetter()) break;
                        if (ProcessSymbol(')'))
                        {
                            CheckForEmptyArgument();
                            state = ParseState.ConditionCloseBracket;
                            break;
                        }
                        if (ProcessSymbol(','))
                        {
                            CheckForEmptyArgument();
                            state = ParseState.ConditionComma;
                            break;
                        }
                        throw new UnexpectedTokenException(code, position);
                    case ParseState.ConditionComma:
                        break;
                    case ParseState.ConditionCloseBracket:
                        break;
                    case ParseState.Operator:
                        break;
                    case ParseState.CommentBeginning:
                        if (ProcessSymbol('*'))
                        {
                            state = ParseState.Comment;
                            break;
                        }
                        throw new UnexpectedTokenException(code, position);
                    case ParseState.Comment:
                        if (ProcessSymbol('*'))
                        {
                            state = ParseState.CommentEnding;
                            break;
                        }
                        position++;
                        break;
                    case ParseState.CommentEnding:
                        if (ProcessSymbol('/'))
                        {
                            state = ParseState.Beginning;
                            break;
                        }
                        throw new UnexpectedTokenException(code, position); 
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            CheckFinishState();
        }

        private bool ProcessLetter()
        {
            var isCorrectSymbol = IsLetter(code[position]);

            if (!isCorrectSymbol) return false;

            temp += code[position++]; 
            return true;
        }

        private bool ProcessSymbol(char letter)
        {
            var isCorrectSymbol = (code[position] == letter);

            if (!isCorrectSymbol) return false;

            position++;
            return true;
        }

        private void PrepareForArguments()
        {
            name = temp;
            temp = "";
            arguments = new List<ClauseArgument>();
        }

        private void CheckForAtoms()
        {
            if (arguments.Any(arg => arg.IsAtom)) throw new FactAtomException(code, position);
        }

        private void AddArgumentAndChangeStateTo(ParseState nextState)
        {
            arguments.Add(new ClauseArgument(temp));
            temp = "";
            state = nextState;
        }

        private void CheckForEmptyArgument()
        {
            if (temp == "") throw new ArgumentNameExpectedException(code, position);
        }

        private void CheckFinishState()
        {
            switch (state)
            {
                case ParseState.Beginning:
                    // If we are here then everything is alright
                    break;
                case ParseState.CloseBracket:
                case ParseState.ConditionCloseBracket:
                    throw new MissingSemicolonException(code, position);
                default:
                    throw new UnexpectedLineEndException(code, position);
            }
        }
    }
}