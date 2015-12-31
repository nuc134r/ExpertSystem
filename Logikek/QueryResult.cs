using System.Collections.Generic;
using Logikek.Language;

namespace Logikek
{
    public class QueryResult
    {
        public QueryResult(bool result, Query theQuery)
        {
            Result = result;
            TheQuery = theQuery;
        }

        public QueryResult(bool result, Query theQuery, List<Dictionary<string, string>> solutions)
        {
            Result = result;
            TheQuery = theQuery;
            Solutions = solutions;
        }

        public List<Dictionary<string, string>> Solutions { get; private set; }
        public Query TheQuery { get; private set; }
        public bool Result { get; private set; }
    }
}