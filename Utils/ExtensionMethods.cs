using System;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace Utils
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

        public static void AppendText(this FlowDocument document, Color color, string text)
        {
            var tr = new TextRange(document.ContentEnd, document.ContentEnd) { Text = text };
            tr.ApplyPropertyValue(TextElement.ForegroundProperty, new SolidColorBrush(color));
        }

        public static void AppendText(this FlowDocument document, string text)
        {
            var tr = new TextRange(document.ContentEnd, document.ContentEnd) { Text = text };
            tr.ApplyPropertyValue(TextElement.ForegroundProperty, new SolidColorBrush(Colors.White));
        }
    }
}