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
        // ReSharper disable once PossibleNullReferenceException
        private static readonly Color RunningColor = (Color) ColorConverter.ConvertFromString("#CA5100");
        private static readonly Color ReadyColor = SystemColors.MenuHighlightColor;
        private SolidColorBrush brush;
        private bool isRunning;

        public MainWindow()
        {
            InitializeComponent();

            InitailizeAnimationBrush();
        }

        private void InitailizeAnimationBrush()
        {
            brush = new SolidColorBrush();

            statusBar.Background = brush;
            LaunchStopBox.Background = brush;

            brush.Color = ReadyColor;
            RegisterName("brush", brush);
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
            var fromColor = mode == ApplicationMode.Ready ? ReadyColor : RunningColor;
            var toColor = mode == ApplicationMode.Ready ? RunningColor : ReadyColor;

            var animation = new ColorAnimation
            {
                From = fromColor,
                To = toColor,
                Duration = TimeSpan.FromMilliseconds(175)
            };

            Storyboard.SetTargetName(animation, "brush");
            Storyboard.SetTargetProperty(animation, new PropertyPath(SolidColorBrush.ColorProperty));

            var sb = new Storyboard();
            sb.Children.Add(animation);

            sb.Begin(this);
        }

        private void LaunchStopButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            isRunning = !isRunning;

            if (isRunning)
            {
                AnimateModeChange(ApplicationMode.Ready);
                LaunchStopButton.Content = "  Стоп";
                statusBarInfo.Content = "Отладка";
            }
            else
            {
                AnimateModeChange(ApplicationMode.Running);
                LaunchStopButton.Content = "Запуск";
                statusBarInfo.Content = "Готов";
            }
        }

        private void Label_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //var animation = new DoubleAnimation(0.0, 1.0, new Duration(TimeSpan.FromMilliseconds(200)));
            FileMenuBox.Visibility = Visibility.Visible;
            //FileMenuBox.BeginAnimation(OpacityProperty, animation);

            FileMenuBox.Focus();
        }

        private void Label_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            FileMenuBox.Visibility = Visibility.Collapsed;
        }
    }
}