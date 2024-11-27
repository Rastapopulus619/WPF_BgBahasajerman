using Bgb_DataAccessLibrary.Services.CommunicationServices.EventAggregators;
namespace Bgb_DataAccessLibrary.Models.Interfaces
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