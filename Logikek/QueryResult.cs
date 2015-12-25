using Logikek.Language;

namespace Logikek
{
    public class QueryResult
    {
        public QueryResult(bool result, Query theQuery)
        {
            TheQuery = theQuery;
            Result = result;
        }

        public Query TheQuery { get; private set; }
        public bool Result { get; private set; }
    }
}