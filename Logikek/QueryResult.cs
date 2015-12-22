using Logikek.Language;

namespace Logikek
{
    public class QueryResult
    {
        public QueryResult(Query theQuery, bool result)
        {
            TheQuery = theQuery;
            Result = result;
        }

        public Query TheQuery { get; private set; }
        public bool Result { get; private set; }
    }
}