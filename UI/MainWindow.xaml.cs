﻿using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using Core;
using EasterEggs;
using Logikek;

namespace UI
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool isRunning;

        private SolidColorBrush glowBrush;
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
                    LineNumbersBox.Text += i + 1 + "\n";
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }

        private void Launch()
        {
            if (!isRunning)
            {
                var document = OutputBox.Document;
                new TextRange(document.ContentStart, document.ContentEnd).Text = "";
                OutputBox.Foreground = new SolidColorBrush(Colors.White);

                if (Run())
                {
                    isRunning = true;
                    AnimateModeChange(ApplicationMode.Running);
                    LaunchButtonBox.Background = new SolidColorBrush(Colors.Transparent);
                    StopButtonBox.Background = new SolidColorBrush(AppColors.RunningAccent);
                    SourceCodeBox.IsReadOnly = true;
                    InterpreterBox.IsReadOnly = false;
                    InterpreterBox.Focus();
                }
                else
                    AnimateErrorsOccurence();
            }
        }

        private void Stop()
        {
            if (isRunning)
            {
                InterpreterBox.Text = "";

                AnimateModeChange(ApplicationMode.Ready);
                LaunchButtonBox.Background = glowBrush;
                StopButtonBox.Background = new SolidColorBrush(Colors.Transparent);
                InterpreterBox.IsReadOnly = true;
                SourceCodeBox.IsReadOnly = false;
                SourceCodeBox.Focus();

                isRunning = false;
            }
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
            OutputBox.AppendText(Colors.DimGray, $"Строка {parseError.Line}:{parseError.Column} ");
            OutputBox.AppendText(Colors.White, $"{parseError.Message}\n");
        }

        private void PrintParseError(ParseError parseError, string code)
        {
            OutputBox.AppendText(Colors.DimGray, "> ");
            OutputBox.AppendText(Colors.LightGray , $"{code}\n");
            OutputBox.AppendText(Colors.DimGray, "  Ошибка: ");
            OutputBox.AppendText(Colors.White, $"{parseError.Message}\n");
        }

        private void PrintQueryResult(QueryResult queryResult)
        {
            var args = queryResult.TheQuery.Arguments.Select(arg => arg.Name).ToArray();

            OutputBox.AppendText(Colors.DimGray, "> ");
            OutputBox.AppendText(Colors.LightGray, $"{queryResult.TheQuery.Name}({string.Join(", ", args)})?\n");
            OutputBox.AppendText(Colors.White, $"  {(queryResult.Result ? "Истина" : "Ложь")}\n");
        }

        private void MainWindow_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.F5:
                    if (isRunning) Stop();
                    else
                        Launch();
                    return;

                case Key.Enter:
                    if (isRunning && !string.IsNullOrEmpty(InterpreterBox.Text.Trim()))
                    {
                        var result = Processor.EvaluateQuery(InterpreterBox.Text);
                        if (result.Success)
                            PrintQueryResult(result.Results.First());
                        else
                            PrintParseError(result.Errors.First(), InterpreterBox.Text);
                        InterpreterBox.Text = "";
                    }
                    return;

                case Key.Up:
                    if (InterpreterBox.IsFocused)
                        InterpreterBox.Undo();
                    return;

                case Key.Down:
                    if (InterpreterBox.IsFocused)
                        InterpreterBox.Redo();
                    return;
            }
        }
    }
}