using System.Collections.Generic;
using System.Linq;
using Logikek.Language;
using Sprache;

namespace Logikek
{
    public static class Parser
    {
        private static List<Fact> _facts;
        private static List<Query> _queries;
        private static List<Rule> _rules;

        public static ProcessResult Run(string code)
        {
            var errors = new List<ParseError>();

            _facts = new List<Fact>();
            _queries = new List<Query>();
            _rules = new List<Rule>();

            var lines = code.Split('\n').Select(line => line.Trim());

            var counter = 0;
            foreach (var line in lines)
            {
                counter++;
                if (string.IsNullOrEmpty(line)) continue;

                var factResult = Grammar.Fact.TryParse(line);
                if (factResult.WasSuccessful)
                {
                    if (factResult.Value.Arguments.Any(fact => fact.IsAtom))
                        errors.Add(new ParseError("факты не могут содержать атомы", counter, factResult.Remainder.Column));
                    else
                        _facts.Add(factResult.Value);

                    continue;
                }

                var queryResult = Grammar.Query.TryParse(line);
                if (queryResult.WasSuccessful)
                {
                    _queries.Add(queryResult.Value);
                    continue;
                }

                var ruleResult = Grammar.Rule.TryParse(line);
                if (ruleResult.WasSuccessful)
                {
                    _rules.Add(ruleResult.Value);
                    continue;
                }

                var commentResult = Grammar.Comment.TryParse(line);
                if (commentResult.WasSuccessful) continue;

                var expected = ruleResult.Expectations.FirstOrDefault();
                var column = ruleResult.Remainder.Column;
                errors.Add(new ParseError(ruleResult.Message + $", ожидалось {expected}", counter, column));
            }

            var queryResults = _queries.Select(ResolveQuery).ToList();

            return errors.Any() ? new ProcessResult(errors) : new ProcessResult(queryResults);
        }

        public static ProcessResult EvaluateQuery(string code)
        {
            var query = Grammar.Query.TryParse(code.Trim());

            if (query.WasSuccessful)
            {
                var queriesList = new List<QueryResult> {ResolveQuery(query.Value)};
                return new ProcessResult(queriesList);
            }

            var errorList = new List<ParseError> {new ParseError(query.Message, 0, query.Remainder.Column)};
            return new ProcessResult(errorList);
        }

        /*
        ** Friends(X, Y) : Likes(X, Y) AND Likes(Y, X);
        ** 
        ** Likes(Tom, Sandy);
        ** Likes(Sandy, Tom);
        **
        ** Friends(Tom, Sandy)?
        */

        private static QueryResult ResolveQuery(Query query)
        {
            if (!query.HasAtoms) // Если нет атомов, то запрос простой
            {
                // Ищем факт с именем запроса 
                // И таким же набором аргументов (порядок важен)
                if (_facts.Any(fact => fact.Name == query.Name && fact.Arguments.SequenceEqual(query.Arguments)))
                {
                    return new QueryResult(true, query);
                }

                var matchingRules = _rules.FindAll(rule => rule.Name == query.Name
                                                           &&
                                                           rule.Arguments.Count == query.Arguments.Count);

                // Если есть правила с именем запроса, то играем в дедукцию
                if (matchingRules.Any())
                {
                    foreach (var rule in matchingRules)
                    {
                        bool? result = null;
                        foreach (var condition in rule.Conditions)
                        {
                            var conditionArgs = ConstructArguments(rule.Arguments, condition.Condition.Arguments,
                                query.Arguments);

                            var conditionQuery = new Query(condition.Condition.Name, conditionArgs);

                            var queryResult = ResolveQuery(conditionQuery).Result;
                            if (!result.HasValue)
                            {
                                result = queryResult;
                            }
                            else if (condition.Operator == ConditionOperator.And)
                            {
                                result = result.Value & queryResult;
                            }
                            else
                            {
                                result = result.Value | queryResult;
                            }
                        }
                        if (result.HasValue && result.Value)
                        {
                            return new QueryResult(true, query);
                        }
                    }
                }
            }

            return new QueryResult(false, query);
        }

        private static List<ClauseArgument> ConstructArguments(List<ClauseArgument> ruleArgs,
            List<ClauseArgument> conditionArgs, List<ClauseArgument> queryArgs)
        {
            var argumentMappings = new Dictionary<string, ClauseArgument>();
            var counter = 0;
            foreach (var ruleArg in ruleArgs)
            {
                if (ruleArg.IsAtom)
                {
                    argumentMappings.Add(ruleArg.Name, queryArgs.ElementAt(counter));
                    counter++;
                }
            }

            var result = new List<ClauseArgument>();
            conditionArgs.ForEach(result.Add);

            for (var i = 0; i < result.Count; i++)
            {
                if (result.ElementAt(i).IsAtom && argumentMappings.ContainsKey(result.ElementAt(i).Name))
                {
                    var arg = argumentMappings[result.ElementAt(i).Name];
                    result.RemoveAt(i);
                    result.Insert(i, arg);
                }
            }

            return result;
        }
    }
}