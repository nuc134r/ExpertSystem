using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using Core;
using EasterEggs;
using Logikek;
using Logikek.Language;

namespace UI
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SolidColorBrush accentBrush;
        private bool isRunning;
        private SolidColorBrush outputBrush;

        private RichSnake snakey;
        private SolidColorBrush sourceBrush;

        public MainWindow()
        {
            InitializeComponent();
            InitailizeAnimationBrushes();
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
                    LineNumbersBox.Text += i + 1 + "\n";
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }

        private void LaunchStopButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            StartStop();
        }

        private void StartStop()
        {
            InterpreterBox.Text = "";

            if (!isRunning)
            {
                OutputBox.Document.Blocks.Clear();
                OutputBox.Foreground = new SolidColorBrush(Colors.White);

                if (Run())
                {
                    isRunning = true;
                    AnimateModeChange(ApplicationMode.Running);
                    LaunchStopButton.Content = "  Стоп";
                }
                else
                    AnimateErrorsOccurence();
            }
            else
            {
                AnimateModeChange(ApplicationMode.Ready);
                LaunchStopButton.Content = "Запуск";
                SourceCodeBox.Focus();

                isRunning = false;
                snakey?.Stop();
            }

            SourceCodeBox.IsReadOnly = isRunning;
            InterpreterBox.IsReadOnly = !isRunning;

            if (isRunning)
                InterpreterBox.Focus();
            else
                SourceCodeBox.Focus();
        }

        private bool Run()
        {
            var document = SourceCodeBox.Document;
            var code = new TextRange(document.ContentStart, document.ContentEnd).Text;

            var result = Processor.Run(code);

            if (result.Success)
            {
                foreach (var queryResult in result.Results)
                    PrintQueryResult(queryResult);
            }
            else
            {
                foreach (var error in result.Errors)
                    PrintParseError(error);
            }

            return result.Success;
        }

        private void PrintParseError(ParseError parseError)
        {
            OutputBox.AppendText($"line {parseError.Line}:{parseError.Column} ", Colors.DimGray);
            OutputBox.AppendText($"{parseError.Message}\n", Colors.LightGray);
        }

        private void PrintParseError(ParseError parseError, string code)
        {
            OutputBox.AppendText($"> {code}\n", Colors.DimGray);
            OutputBox.AppendText("Error: ", Colors.DimGray);
            OutputBox.AppendText($"{parseError.Message}\n", Colors.LightGray);
        }

        private void PrintQueryResult(QueryResult queryResult)
        {
            var args = queryResult.TheQuery.Arguments.Select(arg => arg.Name).ToArray();

            OutputBox.AppendText("> ", Colors.DimGray);
            OutputBox.AppendText($"{queryResult.TheQuery.Name}({string.Join(", ", args)})?\n", Colors.Gray);
            OutputBox.AppendText("- ", Colors.DimGray);
            OutputBox.AppendText($"{(queryResult.Result ? "Yes" : "No")}\n", Colors.White);
        }

        private void MainWindow_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                // Launch or stop execution
                case Key.F5:
                    StartStop();
                    return;

                case Key.Enter:
                    var result = Logikek.Processor.EvaluateQuery(InterpreterBox.Text);
                    if (result.Success)
                        PrintQueryResult(result.Results.First());
                    else
                        PrintParseError(result.Errors.First(), InterpreterBox.Text);
                    InterpreterBox.Text = "";
                    return;

                // You haven't seen this
                case Key.Up:
                    snakey?.SetDirection(1);
                    return;
                case Key.Right:
                    snakey?.SetDirection(2);
                    return;
                case Key.Down:
                    snakey?.SetDirection(3);
                    return;
                case Key.Left:
                    snakey?.SetDirection(4);
                    return;
            }
        }
    }
}