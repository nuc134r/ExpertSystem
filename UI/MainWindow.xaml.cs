using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Core;

namespace UI
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool isRunning;

        private SolidColorBrush accentBrush;
        private SolidColorBrush outputBrush;
        private SolidColorBrush sourceBrush;

        public MainWindow()
        {
            InitializeComponent();

            InitailizeAnimationBrushes();
        }

        private void InitailizeAnimationBrushes()
        {
            accentBrush = new SolidColorBrush();
            sourceBrush = new SolidColorBrush();
            outputBrush = new SolidColorBrush();

            accentBrush.Color = AppColors.ReadyAccent;
            sourceBrush.Color = AppColors.ActiveBoxBg;
            outputBrush.Color = AppColors.InactiveBoxBg;

            StatusBar.Background = accentBrush;
            LaunchStopBox.Background = accentBrush;
            SourceCodeWindow.Background = sourceBrush;
            OutputBox.Background = outputBrush;
            InterpreterOutBox.Background = outputBrush;

            RegisterName("accentBrush", accentBrush);
            RegisterName("sourceBrush", sourceBrush);
            RegisterName("outputBrush", outputBrush);
        }

        private void SourceCodeBox_OnScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            LineNumbersBox.ScrollToVerticalOffset(e.VerticalOffset);
        }

        private void SourceCodeBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                var document = SourceCodeBox.Document;

                var code = new TextRange(document.ContentStart, document.ContentEnd).Text;
                var linesCount = code.Lines();

                if (LineNumbersBox.Text.Lines() == linesCount) return;

                LineNumbersBox.Text = "";

                for (var i = 0; i < linesCount - 1; i++)
                {
                    LineNumbersBox.Text += (i + 1) + "\n";
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }

        private void AnimateModeChange(ApplicationMode mode)
        {
            var isReversed = (mode == ApplicationMode.Ready);

            var accentAnimation = ColorUtils.CreateColorAnimation(AppColors.ReadyAccent, AppColors.RunningAccent, "accentBrush", isReversed);
            var sourceAnimation = ColorUtils.CreateColorAnimation(AppColors.ActiveBoxBg, AppColors.InactiveBoxBg, "sourceBrush", isReversed);
            var outputAnimation = ColorUtils.CreateColorAnimation(AppColors.InactiveBoxBg, AppColors.ActiveBoxBg, "outputBrush", isReversed);

            var sb = new Storyboard();
            sb.Children.Add(accentAnimation);
            sb.Children.Add(sourceAnimation);
            sb.Children.Add(outputAnimation);

            sb.Begin(this);
        }

        private void LaunchStopButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            isRunning = !isRunning;

            if (isRunning)
            {
                AnimateModeChange(ApplicationMode.Ready);
                SourceCodeBox.IsReadOnly = true;
                LaunchStopButton.Content = "  Стоп";
                statusBarInfo.Content = "Отладка";
            }
            else
            {
                AnimateModeChange(ApplicationMode.Running);
                SourceCodeBox.IsReadOnly = false;
                LaunchStopButton.Content = "Запуск";
                statusBarInfo.Content = "Готов";
            }
        }

        private void Label_MouseDown(object sender, MouseButtonEventArgs e)
        {
            FileMenuBox.Visibility = Visibility.Visible;
        }

        private void Label_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            FileMenuBox.Visibility = Visibility.Collapsed;
        }
    }
}