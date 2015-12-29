using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using Logikek;
using Microsoft.Win32;
using UI.Interfaces;

namespace UI.MainWindow
{
    public class ViewModel : IMainViewModel
    {
        private readonly IMainView view;
        private readonly Window viewWindow;

        private bool isRunning;

        private string fileName;
        private string fullFileName;
        private bool editorHasChanges;

        private readonly OpenFileDialog openFileDialog;
        private readonly SaveFileDialog saveFileDialog;

        public ViewModel(IMainView view, Window viewWindow)
        {
            this.view = view;
            this.viewWindow = viewWindow;

            openFileDialog = new OpenFileDialog();
            saveFileDialog = new SaveFileDialog();
        }

        public void Launch()
        {
            if (!isRunning)
            {
                view.ClearOutput();

                var code = view.SourceCodeText;
                var result = Parser.Run(code);

                if (result.WasSuccessful)
                {
                    view.IndicateLaunch();
                    foreach (var queryResult in result.Results)
                    {
                        PrintQueryResult(queryResult);
                    }

                    isRunning = true;
                }
                else
                {
                    view.IndicateErrorsOccurence();
                    foreach (var error in result.Errors)
                    {
                        PrintParseError(error);
                    }
                }
            }
        }

        public void Evaluate(string code)
        {
            code = code.Trim();

            if (isRunning && !string.IsNullOrEmpty(code))
            {
                var result = Parser.EvaluateQuery(code);
                if (result.WasSuccessful)
                {
                    PrintQueryResult(result.Results.First());
                }
                else
                {
                    PrintParseError(result.Errors.First(), code);
                }
            }
        }

        public void Stop()
        {
            if (isRunning)
            {
                view.IndicateStop();
                isRunning = false;
            }
        }

        public void NotifyTextChange()
        {
            editorHasChanges = true;
        }

        private void PrintParseError(ParseError parseError)
        {
            view.PrintOutput(Colors.DimGray, $"Строка {parseError.Line}:{parseError.Column} ");
            view.PrintOutput(Colors.White, $"{parseError.Message}\n");
        }

        private void PrintParseError(ParseError parseError, string code)
        {
            view.PrintOutput(Colors.DimGray, "> ");
            view.PrintOutput(Colors.LightGray, $"{code}\n");
            view.PrintOutput(Colors.DimGray, "  Ошибка: ");
            view.PrintOutput(Colors.White, $"{parseError.Message}\n");
        }

        private void PrintQueryResult(QueryResult queryResult)
        {
            var args = queryResult.TheQuery.Arguments.Select(arg => arg.Name).ToArray();

            view.PrintOutput(Colors.DimGray, "> ");
            view.PrintOutput(Colors.LightGray, $"{queryResult.TheQuery.Name}({string.Join(", ", args)})?\n");
            view.PrintOutput(Colors.White, $"  {(queryResult.Result ? "Истина" : "Ложь")}\n");
        }

        public void StartStop()
        {
            if (!isRunning)
            {
                Launch();
            }
            else
            {
                Stop();
            }
        }

        public void Format(FlowDocument document)
        {
            var result = SyntaxHighlighter.Run(document);

            if (result == false)
            {
                // Знаем, что есть ошибки, запускаем ещё раз, чтобы показать их
                Launch();
            }
        }

        public bool SaveFile()
        {
            try
            {
                if (string.IsNullOrEmpty(fullFileName))
                {
                    saveFileDialog.FileName = "";
                    if (saveFileDialog.ShowDialog(viewWindow).Value)
                    {
                        fileName = saveFileDialog.SafeFileName;
                        fullFileName = saveFileDialog.FileName;
                    }
                    else
                    {
                        return false;
                    }
                }

                using (var writer = new StreamWriter(fullFileName, false, Encoding.Default))
                {
                    writer.Write(view.SourceCodeText);
                }

                editorHasChanges = false;
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка сохранения файла");
                return false;
            }
        }

        public void NewFile()
        {
            if (PromptSavingCurrentFile())
            {
                fileName = null;
                fullFileName = null;
                view.ClearSourceCode();
                view.ClearOutput();
                view.UpdateFilename(fileName);
                editorHasChanges = false;
            }
        }

        public void OpenFile()
        {
            if (!PromptSavingCurrentFile()) return;

            openFileDialog.FileName = "";
            if (openFileDialog.ShowDialog(viewWindow).Value)
            {
                fileName = openFileDialog.SafeFileName;
                fullFileName = openFileDialog.FileName;
                view.SourceCodeText = "";
                view.ClearOutput();
                view.UpdateFilename(fileName);

                using (var reader = new StreamReader(openFileDialog.FileName, Encoding.Default))
                {
                    view.SourceCodeText = reader.ReadToEnd();
                }

                view.HighlightSyntax(true);
                editorHasChanges = false;
            }
        }

        private bool PromptSavingCurrentFile()
        {
            if (!editorHasChanges) return true;
            var result = MessageBox.Show($"Сохранить файл {fileName}?",
                "Сохранение",
                MessageBoxButton.YesNoCancel);

            switch (result)
            {
                case MessageBoxResult.Yes:
                    return SaveFile();
                case MessageBoxResult.Cancel:
                    return false;
                default:
                    return true;
            }
        }
    }
}