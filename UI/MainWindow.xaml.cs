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
        private SolidColorBrush accentBrush;
        private SolidColorBrush sourceBrush;
        private SolidColorBrush outputBrush;
        private bool isRunning;

        public MainWindow()
        {
            InitializeComponent();

            InitailizeBrushes();
        }

        private void InitailizeBrushes()
        {
            accentBrush = new SolidColorBrush();
            sourceBrush = new SolidColorBrush();
            outputBrush = new SolidColorBrush();

            accentBrush.Color = AppColors.ReadyAccent;
            sourceBrush.Color = AppColors.ActiveBoxBackground;
            outputBrush.Color = AppColors.InactiveBoxBackground;

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
            ColorAnimation accentAnimation;
            ColorAnimation sourceAnimation;
            ColorAnimation outputAnimation;

            var duration = TimeSpan.FromMilliseconds(175);

            if (mode == ApplicationMode.Ready)
            {
                accentAnimation = new ColorAnimation { From = AppColors.ReadyAccent, To = AppColors.RunningAccent, Duration = duration };
                sourceAnimation = new ColorAnimation { From = AppColors.ActiveBoxBackground, To = AppColors.InactiveBoxBackground, Duration = duration };
                outputAnimation = new ColorAnimation { From = AppColors.InactiveBoxBackground, To = AppColors.ActiveBoxBackground, Duration = duration };
            }
            else
            {
                accentAnimation = new ColorAnimation { From = AppColors.RunningAccent, To = AppColors.ReadyAccent, Duration = duration };
                sourceAnimation = new ColorAnimation { From = AppColors.InactiveBoxBackground, To = AppColors.ActiveBoxBackground, Duration = duration };
                outputAnimation = new ColorAnimation { From = AppColors.ActiveBoxBackground, To = AppColors.InactiveBoxBackground, Duration = duration };
            }
            
            Storyboard.SetTargetName(accentAnimation, "accentBrush");
            Storyboard.SetTargetProperty(accentAnimation, new PropertyPath(SolidColorBrush.ColorProperty));

            Storyboard.SetTargetName(sourceAnimation, "sourceBrush");
            Storyboard.SetTargetProperty(sourceAnimation, new PropertyPath(SolidColorBrush.ColorProperty));

            Storyboard.SetTargetName(outputAnimation, "outputBrush");
            Storyboard.SetTargetProperty(outputAnimation, new PropertyPath(SolidColorBrush.ColorProperty));

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