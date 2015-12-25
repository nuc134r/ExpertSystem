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
        
        #region Animations

        private void AnimateModeChange(ApplicationMode mode)
        {
            var isReversed = (mode == ApplicationMode.Running);
            
            var sourceAnimation = ColorUtils.CreateColorAnimation(AppColors.ActiveBoxBg, AppColors.InactiveBoxBg, "sourceBrush", isReversed);
            var outputAnimation = ColorUtils.CreateColorAnimation(AppColors.InactiveBoxBg, AppColors.ActiveBoxBg, "outputBrush", isReversed);

            var sb = new Storyboard();
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
            glowBrush   = new SolidColorBrush();
            sourceBrush = new SolidColorBrush();
            outputBrush = new SolidColorBrush();

            glowBrush.Color   = AppColors.ReadyAccent;
            sourceBrush.Color = AppColors.ActiveBoxBg;
            outputBrush.Color = AppColors.InactiveBoxBg;

            LaunchButtonBox.Background = glowBrush;
            SourceCodeWindow.Background = sourceBrush;
            OutputWindow.Background = outputBrush;

            RegisterName("glowBrush", glowBrush);
            RegisterName("sourceBrush", sourceBrush);
            RegisterName("outputBrush", outputBrush);

            var glowAnimation = ColorUtils.CreateColorAnimation(AppColors.ReadyAccent, AppColors.ReadyGlowAccent,
                "glowBrush", reversed: false, autoreverse: true, repeat: true, durationMs: 1500);
            var sb = new Storyboard();
            sb.Children.Add(glowAnimation);

            sb.Begin(this);
        }

        #endregion

        private void LaunchButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Launch();
        }

        private void StopButton_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            Stop();
        }

        private void OutputBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            OutputBox.ScrollToEnd();
        }

        private void CleanOutputButton_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            OutputBox.Document.Blocks.Clear();
        }

        private void SourceCodeBox_OnPasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.FormatToApply == "Bitmap")
            {
                e.CancelCommand();
            }
        }
    }
}