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
        **              <<  Reference  >>
        **
        ** Friends(X, Y) : Likes(X, Y) AND Likes(Y, X);
        ** 
        ** Likes(Tom, Sandy);
        ** Likes(Sandy, Tom);
        **
        ** Friends(Tom, X)?
        */

        private static QueryResult ResolveQuery(Query query)
        {
            if (!query.HasAtoms) // Если нет атомов, то запрос простой (возвращает true или false)
            {
                // Попытка 1:
                // Ищем факт с именем запроса 
                // И таким же набором аргументов (порядок важен)
                if (_facts.Any(fact => fact.Name == query.Name && fact.Arguments.SequenceEqual(query.Arguments)))
                {
                    return new QueryResult(true, query);
                }

                // Попытка 2:
                // Найти все правила с именем запроса 
                // И аналогичным количеством аргументов
                var matchingRules = _rules.FindAll(rule => rule.Name == query.Name
                                                           &&
                                                           rule.Arguments.Count == query.Arguments.Count);

                // Если есть такие правила, то играем в дедукцию
                // Подставляем каждому правилу вместо атомов аргументы запроса 
                // И рекурсивно вычисляем каждое условие
                if (matchingRules.Any())
                {
                    foreach (var rule in matchingRules)
                    {
                        bool? finalResult = null;
                        foreach (var condition in rule.Conditions)
                        {
                            // Подставляем вместо атомов аргументы запроса
                            var conditionArgs = ConstructArguments(rule.Arguments, condition.Condition.Arguments,
                                query.Arguments);

                            // Вычисляем значение запроса
                            var conditionQuery = new Query(condition.Condition.Name, conditionArgs);
                            var queryResult = ResolveQuery(conditionQuery).Result;

                            if (condition.Condition.IsNegated)
                                queryResult = !queryResult;

                            // Применяем логический оператор
                            finalResult = ApplyLogicalOperator(finalResult, condition.Operator, queryResult);
                        }
                        if (finalResult.HasValue && finalResult.Value)
                        {
                            return new QueryResult(true, query);
                        }
                    }
                }

                // Попытка 3:
                // Не помогла дедукция -- не беда, пробуем индукцию
                // Ищем все правила, в которых наш запрос содержится в качестве условия
                // TODO

            }
            else // Атомы есть
            {
                var solutions = new List<Dictionary<string, string>>();

                // Шаг 1:
                // Найти все факты с именем запроса и нужным количеством аргументов
                var matchingFacts = _facts.FindAll(fact => fact.Name == query.Name
                                                           &&
                                                           fact.Arguments.Count == query.Arguments.Count)
                    // И взять только те, у которых идентичны аргументы, не являющиеся атомами
                    .Where(fact => CompareArgumentsIgnoringAtoms(query.Arguments, fact.Arguments));

                foreach (var fact in matchingFacts)
                {
                    solutions.Add(new Dictionary<string, string>());
                    for (var i = 0; i < query.Arguments.Count; i++)
                    {
                        var arg = query.Arguments.ElementAt(i);
                        if (arg.IsAtom)
                        {
                            var solution = fact.Arguments.ElementAt(i);
                            solutions.Last().Add(arg.Name, solution.Name);
                        }
                    }
                }

                // Шаг 2:
                // Пытаемся вычислить все правила с именем запроса
                // TODO


                return new QueryResult(solutions.Any(), query, solutions);
            }

            return new QueryResult(false, query);
        }

        private static bool CompareArgumentsIgnoringAtoms(List<ClauseArgument> original, List<ClauseArgument> another)
        {
            // Каждый элемент первого списка должен быть равен 
            // элементу второго списка (или быть атомом)
            return !original.Where((t, i) => !original.ElementAt(i).IsAtom 
                                             && 
                                             original.ElementAt(i).Name != another.ElementAt(i).Name)
                                             .Any();
        }

        private static bool ApplyLogicalOperator(bool? v1, ConditionOperator? @operator, bool v2)
        {
            if (v1 == null)
            {
                return v2;
            }

            if (@operator == ConditionOperator.And)
            {
                return v1.Value & v2;
            }
            return v1.Value | v2;
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