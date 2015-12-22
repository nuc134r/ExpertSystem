using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logikek.Language;
using Sprache;

namespace Logikek
{
    public static class Processor
    {
        private static List<Fact> facts;
        private static List<Query> queries;
        private static List<Rule> rules;

        public static ProcessResult Run(string code)
        {
            var errors = new List<ParseError>();

            facts = new List<Fact>();
            queries = new List<Query>();
            rules = new List<Rule>();

            var lines = code.Split('\n').Select(line => line.Trim());

            var counter = 0;
            foreach (var line in lines)
            {
                counter++;
                if (string.IsNullOrEmpty(line)) continue;

                var factResult = Grammar.Fact.TryParse(line);
                if (factResult.WasSuccessful)
                {
                    // TODO atom check
                    facts.Add(factResult.Value);
                    continue;
                }

                var queryResult = Grammar.Query.TryParse(line);
                if (queryResult.WasSuccessful)
                {
                    queries.Add(queryResult.Value);
                    continue;
                }

                var ruleResult = Grammar.Rule.TryParse(line);
                if (ruleResult.WasSuccessful)
                {
                    rules.Add(ruleResult.Value);
                    continue;
                }

                var commentResult = Grammar.Comment.TryParse(line);
                if (commentResult.WasSuccessful) continue;

                errors.Add(new ParseError(ruleResult.Message + $", expected {ruleResult.Expectations.FirstOrDefault()}", counter, ruleResult.Remainder.Column));
            }

            var queryResults = new List<QueryResult>();

            // TODO Processing queries

            return errors.Any() ? new ProcessResult(errors) : new ProcessResult(queryResults);
        }
    }
}
