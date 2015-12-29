using System.Windows.Media;

namespace UI.Interfaces
{
    public interface IMainView
    {
        string SourceCodeText { get; set; }

        void IndicateErrorsOccurence();
        void ClearOutput();
        void IndicateLaunch();
        void IndicateStop();
        void InitializeComponent();
        void PrintOutput(Color color, string text);
        void UpdateFilename(string fileName);
        void HighlightSyntax(bool clearHistory);
        void ClearSourceCode(bool deep = false);
    }
}