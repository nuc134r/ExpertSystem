namespace Core
{
    public static class ExtensionMethods
    {
        /// http://www.codeproject.com/Tips/312312/Counting-lines-in-a-string
        /// 
        /// <summary>
        /// Returns the number of lines in a string
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static long Lines(this string s)
        {
            long count = 1;
            int position = 0;
            while ((position = s.IndexOf('\n', position)) != -1)
            {
                count++;
                position++;         // Skip this occurrence!
            }
            return count;
        }
    }
}