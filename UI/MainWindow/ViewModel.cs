using System.Linq;
using System.Windows.Documents;
using System.Windows.Media;
using Logikek;
using UI.Interfaces;

namespace UI.MainWindow
{
    public class ViewModel : IMainViewModel
    {
        private readonly IMainView view;

        private bool isRunning;

        public ViewModel(IMainView view)
        {
            this.view = view;
        }

        public void Launch()
        {
            if (!isRunning)
            {
                view.ClearOutput();

                var code = view.SourceCode;
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
        
        private void PrintParseError(ParseError parseError)
        {
            view.PrintOutput(System.Windows.Media.Colors.DimGray, $"Строка {parseError.Line}:{parseError.Column} ");
            view.PrintOutput(System.Windows.Media.Colors.White, $"{parseError.Message}\n");
        }

        private void PrintParseError(ParseError parseError, string code)
        {
            view.PrintOutput(System.Windows.Media.Colors.DimGray, "> ");
            view.PrintOutput(System.Windows.Media.Colors.LightGray, $"{code}\n");
            view.PrintOutput(System.Windows.Media.Colors.DimGray, "  Ошибка: ");
            view.PrintOutput(System.Windows.Media.Colors.White, $"{parseError.Message}\n");
        }

        private void PrintQueryResult(QueryResult queryResult)
        {
            var args = queryResult.TheQuery.Arguments.Select(arg => arg.Name).ToArray();

            view.PrintOutput(System.Windows.Media.Colors.DimGray, "> ");
            view.PrintOutput(System.Windows.Media.Colors.LightGray, $"{queryResult.TheQuery.Name}({string.Join(", ", args)})?\n");
            view.PrintOutput(System.Windows.Media.Colors.White, $"  {(queryResult.Result ? "Истина" : "Ложь")}\n");
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

        public FlowDocument Format(FlowDocument document)
        {
            var result = SyntaxHighlighter.Run(document);
            if (ReferenceEquals(result, document))
            {
                Launch();
            }

            return result;
        }
    }
}