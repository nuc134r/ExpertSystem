using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using Core;
using EasterEggs;
using Sprache;
using Grammar = Logikek.Grammar;

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
            //var test = new LogikekGrammar();

            //int i = 5;

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

            var lines = code.Split('\n');
            var errors = new List<Exception>();

            var counter = 0;

            foreach (var line in lines)
            {
                try
                {
                    counter++;
                    var preprocessed = line.Trim();

                    if (string.IsNullOrEmpty(preprocessed)) continue;

                    var result = Grammar.Clause.Parse(preprocessed);
                }
                catch (ParseException ex)
                {
                    errors.Add(new ParseException(ex.Message.Replace("Line 1", $"Line {counter}")));
                }
            }

            if (!errors.Any())
            {
                OutputBox.AppendText("Success!", Colors.Green);
            }
            else
            {
                foreach (var error in errors)
                {
                    OutputBox.AppendText($"{error.Message}\n", Colors.Gray);
                }
                isRunning = false;
                return isRunning;
            }

            isRunning = true;
            return isRunning;
        }

        private void MainWindow_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                // Launch or stop execution
                case Key.F5:
                    StartStop();
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