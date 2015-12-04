using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using Core;
using Core.Exceptions;
using EasterEggs;

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

        private RichSnake snakey;

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
            StartStop();

            //var test = new Test();

            //int i = 5;
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
                    AnimateModeChange(ApplicationMode.Running);
                    LaunchStopButton.Content = "  Стоп";
                    InterpreterBox.Focus();
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
                snakey?.Stop();
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
                isRunning = false;
                return isRunning;
            }

            if (context.Facts.First().Name.ToLower() == "play" &&
                context.Facts.First().Arguments.First().Name.ToLower() == "snake")
            {
                snakey = new RichSnake(OutputBox);
                isRunning = true;
                return true;
            }

            OutputBox.AppendText($"Rules: {context.Rules.Count}", Colors.DimGray);
            OutputBox.AppendText($"\nFacts: {context.Facts.Count}", Colors.DimGray);
            OutputBox.AppendText($"\nQueries: {context.Queries.Count}", Colors.DimGray);

            isRunning = true;
            return isRunning;
        }
        
        private void MainWindow_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.F5:
                    StartStop();
                    return;
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