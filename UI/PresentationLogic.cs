using System;
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

        #region Menu

        #region File

        private void FileMenuLabel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            FileMenuBox.Visibility = Visibility.Visible;
        }

        private void FileMenuInnerLabel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            FileMenuBox.Visibility = Visibility.Collapsed;
        }

        private void NewFileMenuLabel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            FileMenuBox.Visibility = Visibility.Collapsed;
            // New file
        }

        private void OpenFileMenuLabel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            FileMenuBox.Visibility = Visibility.Collapsed;
            // Open file
        }

        private void SaveFileMenuLabel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            FileMenuBox.Visibility = Visibility.Collapsed;
            // Open file
        }

        #endregion

        #region View

        private void ViewMenuLabel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ViewMenuBox.Visibility = Visibility.Visible;
        }

        private void ViewMenuInnerLabel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ViewMenuBox.Visibility = Visibility.Collapsed;
        }

        private void SimpleCodeMenuLabel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ViewMenuBox.Visibility = Visibility.Collapsed;
            // Simple code mode
        }

        #endregion

        #endregion

    }
}