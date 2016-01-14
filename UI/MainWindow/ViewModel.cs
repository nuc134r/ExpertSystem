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
        private readonly OpenFileDialog openFileDialog;
        private readonly SaveFileDialog saveFileDialog;
        private readonly IMainView view;
        private readonly Window viewWindow;

        private string fileName;
        private string fullFileName;

        private bool isRunning;
        private string originalCode;

        public ViewModel(IMainView view, Window viewWindow)
        {
            this.view = view;
            this.viewWindow = viewWindow;

            originalCode = view.SourceCodeText;

            openFileDialog = new OpenFileDialog
            {
                Filter = "База знаний Logikek (*.kek)|*.kek"
            };
            saveFileDialog = new SaveFileDialog
            {
                Filter = "База знаний Logikek (*.kek)|*.kek"
            };
        }

        /// <summary>
        /// Есть ли изменения в файле
        /// </summary>
        public bool EditorHasChanges
        {
            get { return view.SourceCodeText != originalCode; }
        }

        /// <summary>
        /// Запуск выполнения
        /// </summary>
        public void Launch()
        {
            if (!isRunning)
            {
                view.ClearOutput();

                var code = view.SourceCodeText;

                ProcessResult result;
                try
                {
                    result = Parser.Run(code);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }

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

        /// <summary>
        /// Вычислить запрос
        /// </summary>
        /// <param name="code"></param>
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

        /// <summary>
        /// Остановка выполнения
        /// </summary>
        public void Stop()
        {
            if (isRunning)
            {
                view.IndicateStop();
                isRunning = false;
            }
        }

        /// <summary>
        /// Запуск/остановка выполнения
        /// </summary>
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

        /// <summary>
        /// Форматирование кода в FlowDocument
        /// </summary>
        /// <param name="document">Объект с кодом</param>
        public void Format(FlowDocument document)
        {
            var result = SyntaxHighlighter.Run(document);

            if (result == false)
            {
                // Знаем, что есть ошибки, запускаем ещё раз, чтобы показать их
                Launch();
            }
        }

        /// <summary>
        /// Сохранение в файл
        /// </summary>
        /// <returns></returns>
        public bool SaveFile()
        {
            if (isRunning)
            {
                MessageBox.Show("Невозможно сохранить файл при запущенном интерпретаторе", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            try
            {
                if (string.IsNullOrEmpty(fullFileName))
                {
                    saveFileDialog.FileName = "";

                    var result = saveFileDialog.ShowDialog(viewWindow);
                    if (result.Value)
                    {
                        fileName = saveFileDialog.SafeFileName;
                        fullFileName = saveFileDialog.FileName;
                        view.UpdateFilename(fileName);
                    }
                    else
                    {
                        return false;
                    }
                }

                using (var writer = new StreamWriter(fullFileName, false, Encoding.Default))
                {
                    originalCode = view.SourceCodeText;
                    writer.Write(view.SourceCodeText);
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка сохранения файла");
                return false;
            }
        }

        /// <summary>
        /// Новый файл
        /// </summary>
        public void NewFile()
        {
            if (isRunning)
            {
                MessageBox.Show("Невозможно создать файл при запущенном интерпретаторе", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (PromptSavingCurrentFile())
            {
                fileName = null;
                fullFileName = null;
                view.ClearSourceCode();
                view.ClearOutput();
                view.UpdateFilename(fileName);
                originalCode = view.SourceCodeText;
            }
        }

        /// <summary>
        /// Открыть файл
        /// </summary>
        public void OpenFile()
        {
            if (isRunning)
            {
                MessageBox.Show("Невозможно открыть файл при запущенном интерпретаторе", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

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
                originalCode = view.SourceCodeText;
            }
        }

        private void PrintParseError(ParseError parseError)
        {
            view.PrintOutput(Colors.DimGray, string.Format("Строка {0}:{1} ", parseError.Line, parseError.Column));
            view.PrintOutput(Colors.White, string.Format("{0}\n", parseError.Message));
        }

        private void PrintParseError(ParseError parseError, string code)
        {
            view.PrintOutput(Colors.DimGray, "> ");
            view.PrintOutput(Colors.LightGray, string.Format("{0}\n", code));
            view.PrintOutput(Colors.DimGray, "  Ошибка: ");
            view.PrintOutput(Colors.White, string.Format("{0}\n", parseError.Message));
        }

        private void PrintQueryResult(QueryResult queryResult)
        {
            var args = queryResult.TheQuery.Arguments.Select(arg => arg.Name).ToArray();

            view.PrintOutput(Colors.DimGray, "> ");
            view.PrintOutput(Colors.LightGray,
                string.Format("{0}({1})?\n", queryResult.TheQuery.Name, string.Join(", ", args)));
            if (queryResult.Solutions != null && (queryResult.Solutions.Any() == queryResult.Result))
            {
                if (queryResult.Solutions.Any())
                {
                    var c = queryResult.Solutions.Count;
                    view.PrintOutput(Colors.DimGray,
                        string.Format("  {0} решени{1}\n", c, c == 1 ? "е" : (c < 5 ? "я" : "й")));
                    foreach (var solution in queryResult.Solutions)
                    {
                        view.PrintOutput(SyntaxColors.Atom, "  ");
                        foreach (var atom in solution.Keys)
                        {
                            var value = solution[atom];
                            view.PrintOutput(SyntaxColors.Atom, string.Format("{0}", atom));
                            view.PrintOutput(Colors.White,
                                string.Format(" = {0}{1}", value, atom == solution.Keys.Last() ? "" : ", "));
                        }
                        view.PrintOutput(Colors.White, "\n");
                    }
                }
                else
                {
                    view.PrintOutput(Colors.LightCoral, "  Решений нет\n");
                }
            }
            else
            {
                if (queryResult.Result)
                {
                    view.PrintOutput(Colors.LightGreen, "  Истина\n");
                }
                else
                {
                    view.PrintOutput(Colors.LightCoral, "  Ложь\n");
                }
            }
        }

        private bool PromptSavingCurrentFile()
        {
            if (!EditorHasChanges || string.IsNullOrEmpty(view.SourceCodeText.Trim())) return true;
            var result = MessageBox.Show(string.Format("Сохранить файл {0}?", fileName),
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