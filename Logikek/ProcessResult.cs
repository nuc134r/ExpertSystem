using System.Collections.Generic;

namespace Logikek
{
    public class ProcessResult
    {
        public ProcessResult(IEnumerable<QueryResult> results)
        {
            Results = results;
            Errors = new List<ParseError>();
            WasSuccessful = true;
        }

        public ProcessResult(IEnumerable<ParseError> errors)
        {
            Errors = errors;
            Results = new List<QueryResult>();
            WasSuccessful = false;
        }

        public bool WasSuccessful { get; private set; }

        public IEnumerable<ParseError> Errors { get; private set; }
        public IEnumerable<QueryResult> Results { get; private set; }
        
    }
}