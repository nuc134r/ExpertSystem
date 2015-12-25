using System.Windows.Media;

namespace UI.Interfaces
{
    public interface IMainView
    {
        string SourceCode { get; }

        void IndicateErrorsOccurence();
        void ClearOutput();
        void IndicateLaunch();
        void IndicateStop();
        void InitializeComponent();
        void PrintOutput(Color color, string text);
    }
}