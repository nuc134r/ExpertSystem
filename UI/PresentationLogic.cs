using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

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

        #region Animations

        private void AnimateModeChange(ApplicationMode mode)
        {
            var isReversed = (mode == ApplicationMode.Running);

            var accentAnimation = ColorUtils.CreateColorAnimation(AppColors.ReadyAccent, AppColors.RunningAccent, "accentBrush", isReversed);
            var sourceAnimation = ColorUtils.CreateColorAnimation(AppColors.ActiveBoxBg, AppColors.InactiveBoxBg, "sourceBrush", isReversed);
            var outputAnimation = ColorUtils.CreateColorAnimation(AppColors.InactiveBoxBg, AppColors.ActiveBoxBg, "outputBrush", isReversed);

            var sb = new Storyboard();
            sb.Children.Add(accentAnimation);
            sb.Children.Add(sourceAnimation);
            sb.Children.Add(outputAnimation);

            sb.Begin(this);
        }

        private void AnimateErrorsOccurence()
        {
            var animation = ColorUtils.CreateColorAnimation(AppColors.InactiveBoxBg, Color.FromRgb(149, 0, 0), "outputBrush", reversed: true, autoreverse: true);

            var sb = new Storyboard();
            sb.Children.Add(animation);

            sb.Begin(this);
        }

        private void InitailizeAnimationBrushes()
        {
            accentBrush = new SolidColorBrush();
            sourceBrush = new SolidColorBrush();
            outputBrush = new SolidColorBrush();

            accentBrush.Color = AppColors.ReadyAccent;
            sourceBrush.Color = AppColors.ActiveBoxBg;
            outputBrush.Color = AppColors.InactiveBoxBg;

            LaunchStopBox.Background = accentBrush;
            SourceCodeWindow.Background = sourceBrush;
            OutputWindow.Background = outputBrush;

            RegisterName("accentBrush", accentBrush);
            RegisterName("sourceBrush", sourceBrush);
            RegisterName("outputBrush", outputBrush);
        }

        #endregion
    }
}