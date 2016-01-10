using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;

namespace Logikek.Language
{
    [DebuggerDisplay("{Name}, atom: {IsAtom}")]
    public class ClauseArgument
    {
        public ClauseArgument(string name)
        {
            Name = name;
        }

        public static IEnumerable<ClauseArgument> FromStringList(IEnumerable<string> names)
        {
            return names.Select(name => new ClauseArgument(name));
        }

        public string Name { get; private set; }
        public bool IsAtom
        {
            get { return (Name.Length == 1); }
        }

        public override bool Equals(object o)
        {
            var clauseArgument = o as ClauseArgument;
            return clauseArgument != null && (clauseArgument.Name.Equals(Name));
        }

        public override int GetHashCode()
        {
            if (Name == null) return 0;
            var hash = Name.GetHashCode();
            return hash;
        }
    }

    public class ClauseArgumentComparer : IEqualityComparer<ClauseArgument>
    {

        public bool Equals(ClauseArgument x, ClauseArgument y)
        {
            if (object.ReferenceEquals(x, y)) return true;

            return x != null && y != null && x.Name.Equals(y.Name);
        }

        public int GetHashCode(ClauseArgument obj)
        {
            var hash = obj.Name != null ? obj.Name.GetHashCode() : 0;

            return hash;
        }
    }
}