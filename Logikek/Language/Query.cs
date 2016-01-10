using System.Collections.Generic;
using System.Linq;

namespace Logikek.Language
{
    public class Query : Clause
    {
        public Query(string name, IEnumerable<ClauseArgument> arguments) : base(name, arguments)
        {
        }

        public bool HasAtoms
        {
            get { return Arguments.Any(arg => arg.IsAtom); }
        }

        public override bool Equals(object obj)
        {
            var anotherQuery = obj as Query;
            if (anotherQuery != null)
            {
                return Name == anotherQuery.Name
                       &&
                       Arguments.SequenceEqual(anotherQuery.Arguments); 
            }

            return false;
        }

        protected bool Equals(Query other)
        {
            return Name == other.Name
                   &&
                   Arguments.SequenceEqual(other.Arguments);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode() + Arguments.Sum(_ => _.Name.Length);
        }
    }
}