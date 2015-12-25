namespace UI.Interfaces
{
    public interface IMainViewModel
    {
        void Evaluate(string code);
        void Launch();
        void Stop();
    }
}