using System;
using System.Security.AccessControl;
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

        private void LaunchStopButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            InterpreterBox.Text = "";

            if (!isRunning)
            {
                OutputBox.Document.Blocks.Clear();
                OutputBox.Foreground = new SolidColorBrush(Colors.White);

                if (Run())
                {
                    AnimateModeChange(ApplicationMode.Running);
                    LaunchStopButton.Content = "  Стоп";
                    InterpreterBox.Focus();

                    isRunning = true;
                }
                else
                {
                    AnimateErrorsOccurence();
                }
            }
            else
            {
                AnimateModeChange(ApplicationMode.Ready);
                LaunchStopButton.Content = "Запуск";
                SourceCodeBox.Focus();
                isRunning = false;
            }

            SourceCodeBox.IsReadOnly = isRunning;
            InterpreterBox.IsReadOnly = !isRunning;
        }

        private bool Run()
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
                OutputBox.AppendText("Errors occured", Colors.OrangeRed);
                OutputBox.AppendText($"\n{ex.Position} ", Colors.Gray);
                OutputBox.AppendText($"{ex.Message}", Colors.White);
                return false;
            }

            OutputBox.AppendText($"{result.ElapsedTime} ms", Colors.LawnGreen);
            OutputBox.AppendText($"\nRules: {context.Rules.Count}", Colors.DimGray);
            OutputBox.AppendText($"\nFacts: {context.Facts.Count}", Colors.DimGray);
            OutputBox.AppendText($"\nQueries: {context.Queries.Count}", Colors.DimGray);

            return true;
        }
    }
}