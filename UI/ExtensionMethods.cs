using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace UI
{
    public static class ExtensionMethods
    {
        public static void AppendText(this RichTextBox box, Color color, string text)
        {
            var tr = new TextRange(box.Document.ContentEnd, box.Document.ContentEnd) {Text = text};
            tr.ApplyPropertyValue(TextElement.ForegroundProperty, new SolidColorBrush(color));
        }
    }
}