using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace UI
{
    public partial class MainWindow
    {
        private void SourceCodeBox_OnScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            LineNumbersBox.ScrollToVerticalOffset(e.VerticalOffset);
        }

        private void FileMenuLabel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            FileMenuBox.Visibility = Visibility.Visible;
        }

        private void FileMenuInnerLabel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            FileMenuBox.Visibility = Visibility.Collapsed;
        }
    }
}