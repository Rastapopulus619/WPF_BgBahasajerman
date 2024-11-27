namespace BgB_TeachingAssistant.ViewModels
{
    public interface IViewModelBase
    {
        int EventHandlersCount { get; }
        string Name { get; }

        void Dispose();
        void LogViewModelCreation();
        void UnsubscribeEvents();
    }
}