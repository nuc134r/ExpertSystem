using System;

namespace Core
{
    public static class ExtensionMethods
    {
        /// http://www.codeproject.com/Tips/312312/Counting-lines-in-a-string
        /// <summary>
        ///     Returns the number of lines in a string
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static long Lines(this string s)
        {
            long count = 1;
            var position = 0;
            while ((position = s.IndexOf('\n', position)) != -1)
            {
                count++;
                position++; // Skip this occurrence!
            }
            return count;
        }

        public static string RemoveAllEntriesOf(this string s, string value)
        {
            var result = s;

            while (result.IndexOf(value, StringComparison.Ordinal) != -1)
            {
                result = result.Replace(value, "");
            }

            return result;
        }
    }
}