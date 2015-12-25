using System.Linq;
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
                var code = view.SourceCode;
                var result = Processor.Run(code);

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
                var result = Processor.EvaluateQuery(code);
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

    }
}