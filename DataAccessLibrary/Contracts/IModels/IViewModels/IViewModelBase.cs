namespace Bgb_DataAccessLibrary.Contracts
{
    public interface IViewModelBase
    {
        int EventHandlersCount { get; }
        string Name { get; }
        IEventAggregator EventAggregator { get; }

        void Dispose();
        void LogViewModelCreation();
        void UnsubscribeEvents();
    }
}