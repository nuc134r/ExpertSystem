using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Documents;
using System.Windows.Media;
using Logikek;
using Logikek.Language;
using Sprache;

namespace UI
{
    public static class SyntaxHighlighter
    {
        public static bool Run(FlowDocument document)
        {
            var code = new TextRange(document.ContentStart, document.ContentEnd).Text;
            var result = Parser.Run(code);

            if (!result.WasSuccessful)
            {
                return false;
            }

            for (var i = 0; i < document.Blocks.Count; i++)
            {
                var paragraph = document.Blocks.ElementAt(i) as Paragraph;
                if (paragraph == null) return false;
                var line = new TextRange(paragraph.ContentStart, paragraph.ContentEnd).Text.Trim();

                paragraph.Inlines.Clear();

                if (string.IsNullOrEmpty(line))
                {
                    continue;
                }

                var comment = Grammar.Comment.TryParse(line);
                if (comment.WasSuccessful)
                {
                    paragraph.Inlines.Add(NewRun(string.Format("//{0}", comment.Value), SyntaxColors.Comment));
                    continue;
                }

                var query = Grammar.Query.TryParse(line);
                if (query.WasSuccessful)
                {
                    var arguments = FormatArguments(query.Value.Arguments);

                    paragraph.Inlines.Add(NewRun(query.Value.Name, SyntaxColors.Query));
                    paragraph.Inlines.Add(NewRun("(", SyntaxColors.Bracket));
                    arguments.ForEach(arg => paragraph.Inlines.Add(arg));
                    paragraph.Inlines.Add(NewRun(")", SyntaxColors.Bracket));
                    paragraph.Inlines.Add(NewRun("?", SyntaxColors.Semicolon));

                    continue;
                }

                var fact = Grammar.Fact.TryParse(line);
                if (fact.WasSuccessful)
                {
                    var argumentString = string.Join(", ", fact.Value.Arguments.Select(_ => _.Name));

                    paragraph.Inlines.Add(NewRun(fact.Value.Name, SyntaxColors.Fact));
                    paragraph.Inlines.Add(NewRun("(", SyntaxColors.Bracket));
                    paragraph.Inlines.Add(NewRun(argumentString, SyntaxColors.Arguments));
                    paragraph.Inlines.Add(NewRun(")", SyntaxColors.Bracket));
                    paragraph.Inlines.Add(NewRun(";", SyntaxColors.Semicolon));

                    continue;
                }

                var rule = Grammar.Rule.TryParse(line);
                if (rule.WasSuccessful)
                {
                    var arguments = FormatArguments(rule.Value.Arguments);

                    paragraph.Inlines.Add(NewRun(rule.Value.Name, SyntaxColors.Rule));
                    paragraph.Inlines.Add(NewRun("(", SyntaxColors.Bracket));
                    arguments.ForEach(arg => paragraph.Inlines.Add(arg));
                    paragraph.Inlines.Add(NewRun(")", SyntaxColors.Bracket));
                    paragraph.Inlines.Add(NewRun(" : ", SyntaxColors.Semicolon));

                    var runsList = new List<Run>();
                    foreach (var condition in rule.Value.Conditions)
                    {
                        if (condition.Operator != null)
                        {
                            runsList.Add(NewRun(string.Format(" {0} ", condition.Operator.Value.GetKeyword()), SyntaxColors.Operator));
                        }

                        if (condition.Condition.IsNegated)
                        {
                            runsList.Add(NewRun(string.Format("{0} ", ConditionOperator.Not.GetKeyword()), SyntaxColors.Operator));
                        }

                        runsList.Add(NewRun(condition.Condition.Name, SyntaxColors.Fact));
                        runsList.Add(NewRun("(", SyntaxColors.Bracket));
                        var conditionArguments = FormatArguments(condition.Condition.Arguments);
                        conditionArguments.ForEach(arg => runsList.Add(arg));
                        runsList.Add(NewRun(")", SyntaxColors.Bracket));

                        runsList.ForEach(_ => paragraph.Inlines.Add(_));
                    }
                    paragraph.Inlines.Add(NewRun(";", SyntaxColors.Semicolon));
                }
            }

            return true;
        }

        private static List<Run> FormatArguments(IEnumerable<ClauseArgument> arguments)
        {
            var runsList = new List<Run>();

            foreach (var arg in arguments)
            {
                runsList.Add(NewRun(arg.Name, arg.IsAtom ? SyntaxColors.Atom : SyntaxColors.Arguments));
                runsList.Add(NewRun(", ", Colors.White));
            }

            runsList.Remove(runsList.Last());
            return runsList;
        }

        private static Run NewRun(string text, Color color)
        {
            return new Run(text) {Foreground = new SolidColorBrush(color)};
        }
    }
}