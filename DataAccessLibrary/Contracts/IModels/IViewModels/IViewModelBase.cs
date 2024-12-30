using Bgb_DataAccessLibrary.Contracts.IServices.ICommunication.IEventAggregators;

namespace Bgb_DataAccessLibrary.Contracts
{
    public interface IViewModelBase
    {
        IServiceFactory ServiceFactory { get; set; }
        List<Delegate> EventHandlers { get; set; }
        int EventHandlersCount { get; }
        IEventAggregator EventAggregator { get; set; }
        string Name { get; }

        void Dispose();
        void LogViewModelCreation();
        void UnsubscribeEvents();
    }
}