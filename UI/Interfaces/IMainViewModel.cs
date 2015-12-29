using System.Windows.Documents;

namespace UI.Interfaces
{
    public interface IMainViewModel
    {
        bool EditorHasChanges { get; }
        void Evaluate(string code);
        void Format(FlowDocument document);
        void Launch();
        void NewFile();
        void OpenFile();
        bool SaveFile();
        void StartStop();
        void Stop();
    }
}