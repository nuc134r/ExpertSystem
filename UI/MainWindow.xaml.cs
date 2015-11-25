using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Core;
using Core.Exceptions;

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

            LaunchStopBox.Background = accentBrush;
            SourceCodeWindow.Background = sourceBrush;
            OutputWindow.Background = outputBrush;

            RegisterName("accentBrush", accentBrush);
            RegisterName("sourceBrush", sourceBrush);
            RegisterName("outputBrush", outputBrush);
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

            SourceCodeBox.IsReadOnly = isRunning;
            InterpreterBox.Text = "";

            if (isRunning)
            {
                AnimateModeChange(ApplicationMode.Ready);
                LaunchStopButton.Content = "  Стоп";
                InterpreterBox.Focus();

                OutputBox.Document.Blocks.Clear();
                OutputBox.Foreground = new SolidColorBrush(Colors.White);

                Run();
            }
            else
            {
                AnimateModeChange(ApplicationMode.Running);
                LaunchStopButton.Content = "Запуск";
                SourceCodeBox.Focus();
            }
        }

        private void Run()
        {
            var document = SourceCodeBox.Document;
            var code = new TextRange(document.ContentStart, document.ContentEnd).Text;

            var parser = new Parser(code);
            var context = new RunContext();

            ParseResult result = null;
            try
            {
                result = parser.Do(context);
            }
            catch (ParsingException ex)
            {
                OutputBox.AppendText("Errors occured while parsing", Colors.DimGray);
                OutputBox.AppendText($"\n{ex.Position} ", Colors.Gray);
                OutputBox.AppendText($"{ex.Message}", Colors.White);
                return;
            }
            
            OutputBox.AppendText($"({result.ElapsedTime} ms)", Colors.DimGray);
            OutputBox.AppendText($"\nRules: {context.Rules.Count}", Colors.DimGray);
            OutputBox.AppendText($"\nFacts: {context.Facts.Count}", Colors.DimGray);
            OutputBox.AppendText($"\nQueries: {context.Queries.Count}", Colors.DimGray);
        }
    }
}